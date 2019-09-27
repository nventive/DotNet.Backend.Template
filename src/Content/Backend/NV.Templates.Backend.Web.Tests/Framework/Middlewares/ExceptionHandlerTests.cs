using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentValidation.Results;
using HelpDeskId;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NV.Templates.Backend.Core.Framework;
using NV.Templates.Backend.Core.Framework.Exceptions;
using NV.Templates.Backend.Core.General;
using NV.Templates.Backend.Web.Framework.Middlewares;
using Xunit;

namespace NV.Templates.Backend.Web.Tests.Framework.Middlewares
{
    public class ExceptionHandlerTests
    {
        public static IEnumerable<object?[]> GenerateDataForItShouldHandleExceptions()
        {
            var faker = new Faker();
            yield return new object?[]
            {
                null,
                new Action<Exception, ProblemDetails>((ex, result) =>
                {
                    result.Status.Should().Be(StatusCodes.Status500InternalServerError);
                }),
            };

            yield return new object[]
            {
                new Exception(faker.Random.Words()),
                new Action<Exception, ProblemDetails>((ex, result) =>
                {
                    result.Status.Should().Be(StatusCodes.Status500InternalServerError);
                    result.Detail.Should().Be(ex.Message);
                }),
            };

            yield return new object[]
            {
                new DependencyException(faker.Random.Words(), faker.Random.Words()),
                new Action<Exception, ProblemDetails>((ex, result) =>
                {
                    result.Status.Should().Be(StatusCodes.Status502BadGateway);
                    result.Detail.Should().Be(ex.Message);
                    result.Instance.Should().Be(((DependencyException)ex).DependencyName);
                }),
            };

            yield return new object[]
            {
                new NotFoundException(faker.Random.Words(), faker.Random.Words()),
                new Action<Exception, ProblemDetails>((ex, result) =>
                {
                    result.Status.Should().Be(StatusCodes.Status404NotFound);
                    result.Detail.Should().Be(ex.Message);
                    result.Instance.Should().Be(((NotFoundException)ex).Target);
                }),
            };

            yield return new object[]
            {
                new ConcurrencyException(faker.Random.Words(), faker.Random.Words()),
                new Action<Exception, ProblemDetails>((ex, result) =>
                {
                    result.Status.Should().Be(StatusCodes.Status409Conflict);
                    result.Detail.Should().Be(ex.Message);
                    result.Instance.Should().Be(((ConcurrencyException)ex).Target);
                }),
            };

            yield return new object[]
            {
                new FluentValidation.ValidationException(faker.Random.Words()),
                new Action<Exception, ProblemDetails>((ex, result) =>
                {
                    result.Status.Should().Be(StatusCodes.Status400BadRequest);
                    result.Detail.Should().Be(ex.Message);
                }),
            };

            yield return new object[]
            {
                new FluentValidation.ValidationException(new[] { new ValidationFailure(faker.Random.Word(), faker.Random.Words()) }),
                new Action<Exception, ProblemDetails>((ex, result) =>
                {
                    result.Status.Should().Be(StatusCodes.Status400BadRequest);
                    var validationFailure = ((FluentValidation.ValidationException)ex).Errors.First();
                    var errors = result.Extensions["errors"] as JObject;
                    errors?[validationFailure.PropertyName]?.First()?.ToString().Should().Be(validationFailure.ErrorMessage);
                }),
            };
        }

        [Theory]
        [MemberData(nameof(GenerateDataForItShouldHandleExceptions))]
        public async Task ItShouldHandleExceptions(Exception exception, Action<Exception, ProblemDetails> assert)
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProviderMock.Object,
            };
            httpContext.Response.Body = new MemoryStream();
            var jsonOptions = new JsonOptions();
            var environmentMock = new Mock<IHostEnvironment>();

            var operationId = IdGenerator.Generate();
            var operationContextMock = new Mock<IOperationContext>();
            operationContextMock.SetupGet(x => x.OperationId).Returns(operationId);
            serviceProviderMock.Setup(x => x.GetService(typeof(IOperationContext))).Returns(operationContextMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IHelpDeskIdGenerator))).Returns(new HelpDeskIdGenerator());

            var exceptionHandlerFeatureMock = new Mock<IExceptionHandlerFeature>();
            exceptionHandlerFeatureMock.SetupGet(x => x.Error).Returns(exception);
            httpContext.Features.Set(exceptionHandlerFeatureMock.Object);

            await ExceptionHandler.HandleException(httpContext, jsonOptions, environmentMock.Object, NullLogger.Instance);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(httpContext.Response.Body))
            {
                var result = JsonConvert.DeserializeObject<ProblemDetails>(streamReader.ReadToEnd());
                result.Extensions[ExceptionHandler.OperationIdProperty].Should().Be(operationId);
                result.Extensions[ExceptionHandler.HelpDeskIdProperty].Should().NotBeNull();
                assert(exception, result);
            }
        }
    }
}
