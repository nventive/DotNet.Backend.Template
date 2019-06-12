using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using NV.Templates.Backend.Core.General;

namespace NV.Templates.Backend.Functions
{
    public class SampleFunction
    {
        private readonly IApplicationInfo _applicationInfo;
        private readonly IOperationContext _operationContext;

        public SampleFunction(
            IApplicationInfo applicationInfo,
            IOperationContext operationContext)
        {
            _applicationInfo = applicationInfo ?? throw new ArgumentNullException(nameof(applicationInfo));
            _operationContext = operationContext ?? throw new ArgumentNullException(nameof(operationContext));
        }

        /// <summary>
        /// This is a sample function that illustrates how pieces fit together.
        /// </summary>
        [FunctionName("SampleFunction")]
        public async Task<JsonResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "info")] HttpRequest request,
            ExecutionContext executionContext)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _operationContext.OperationId = executionContext.InvocationId.ToString();
            return new JsonResult(_applicationInfo, SerializerSettings.JsonSerializerSettings) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
