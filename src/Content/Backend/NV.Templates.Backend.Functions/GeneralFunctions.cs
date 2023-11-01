using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace NV.Templates.Backend.Functions
{
    public class GeneralFunctions
    {
        private readonly IApplicationInfo _applicationInfo;

        public GeneralFunctions(
            IApplicationInfo applicationInfo)
        {
            _applicationInfo = applicationInfo;
        }

        /// <summary>
        /// Get System Information.
        /// </summary>
        [Function("GetInfo")]
        public async Task<IActionResult> GetInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "info")] HttpRequest request, FunctionContext context)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new JsonResult(_applicationInfo);
        }
    }
}
