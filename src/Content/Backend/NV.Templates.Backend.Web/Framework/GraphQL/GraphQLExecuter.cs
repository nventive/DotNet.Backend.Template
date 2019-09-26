using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Instrumentation;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Types;
using GraphQL.Validation;
using HelpDeskId;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NV.Templates.Backend.Core.General;

namespace NV.Templates.Backend.Web.Framework.GraphQL
{
    /// <summary>
    /// Custom <see cref="IGraphQLExecuter"/> that adds extensions related to <see cref="IOperationContext"/>.
    /// </summary>
    /// <typeparam name="TSchema">The GraphQL Schema type.</typeparam>
    internal class GraphQLExecuter<TSchema> : DefaultGraphQLExecuter<TSchema>
        where TSchema : ISchema
    {
        private readonly IOperationContext _operationContext;
        private readonly IHelpDeskIdGenerator _helpDeskIdGenerator;
        private readonly ILogger _logger;

        public GraphQLExecuter(
            TSchema schema,
            IDocumentExecuter documentExecuter,
            IOptions<GraphQLOptions> options,
            IEnumerable<IDocumentExecutionListener> listeners,
            IEnumerable<IValidationRule> validationRules,
            IOperationContext operationContext,
            IHelpDeskIdGenerator helpDeskIdGenerator,
            ILogger<GraphQLExecuter<TSchema>> logger)
            : base(schema, documentExecuter, options, listeners, validationRules)
        {
            _operationContext = operationContext ?? throw new ArgumentNullException(nameof(operationContext));
            _helpDeskIdGenerator = helpDeskIdGenerator ?? throw new ArgumentNullException(nameof(helpDeskIdGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public override async Task<ExecutionResult> ExecuteAsync(string operationName, string query, Inputs variables, object context, CancellationToken cancellationToken = default)
        {
            LogQuery(operationName, query, variables);
            var result = await base.ExecuteAsync(operationName, query, variables, context, cancellationToken);

            if (result.Extensions == null)
            {
                result.Extensions = new Dictionary<string, object>();
            }

            if (result.Errors != null && result.Errors.Any())
            {
                var helpDeskId = _helpDeskIdGenerator.GenerateReadableId();
                result.Extensions["helpDeskId"] = helpDeskId;
                _logger.LogError("HelpDeskId: {HelpDeskId}", helpDeskId);
            }

            result.Extensions["operationContext"] = new
            {
                operationId = _operationContext.OperationId,
                timestamp = _operationContext.Timestamp.ToUnixTimeMilliseconds(),
            };

            LogResult(result);

            return result;
        }

        /// <inheritdoc />
        protected override ExecutionOptions GetOptions(string operationName, string query, Inputs variables, object context, CancellationToken cancellationToken)
        {
            var options = base.GetOptions(operationName, query, variables, context, cancellationToken);
            options.FieldMiddleware.Use<ErrorFieldMiddleware>();

            return options;
        }

        private void LogQuery(string operationName, string query, Inputs variables)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(
                    @"
{{
""operationName"": {OperationName}
""query"": {Query}
""variables"": {Variables}
}}",
                    operationName,
                    query,
                    JsonConvert.SerializeObject(variables, Formatting.Indented));
            }
        }

        private void LogResult(ExecutionResult result)
        {
            var logLevel = result.Errors != null && result.Errors.Any()
                ? LogLevel.Error
                : LogLevel.Trace;

            if (_logger.IsEnabled(logLevel))
            {
                const string MessageFormat = @"
{{
""data"": {Data},
""errors"": {Errors},
""extensions"": {Extensions}
}}";
                _logger.Log(
                    logLevel,
                    MessageFormat,
                    JsonConvert.SerializeObject(result.Data, Formatting.Indented),
                    JsonConvert.SerializeObject(result.Errors ?? new ExecutionErrors(), Formatting.Indented),
                    JsonConvert.SerializeObject(result.Extensions, Formatting.Indented));
            }
        }
    }
}
