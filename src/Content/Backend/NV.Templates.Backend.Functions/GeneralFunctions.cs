using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

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
        public async Task<IApplicationInfo> GetInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "info")] HttpRequestData request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return _applicationInfo;
        }
    }
}
