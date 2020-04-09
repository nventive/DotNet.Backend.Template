using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using NV.Templates.Backend.Core.General;

namespace NV.Templates.Backend.Functions
{
    public class GeneralFunctions
    {
        private readonly IApplicationInfo _applicationInfo;
        private readonly IOperationContext _operationContext;

        public GeneralFunctions(
            IApplicationInfo applicationInfo,
            IOperationContext operationContext)
        {
            _applicationInfo = applicationInfo;
            _operationContext = operationContext;
        }

        /// <summary>
        /// Get System Information.
        /// </summary>
        [FunctionName("GetInfo")]
        public async Task<JsonResult> GetInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "info")] HttpRequest request,
            ExecutionContext executionContext)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _operationContext.Id = executionContext.InvocationId.ToString();
            return new JsonResult(_applicationInfo) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
