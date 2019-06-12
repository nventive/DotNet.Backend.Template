using Microsoft.AspNetCore.Mvc;
using NV.Templates.Backend.Core.General;
using Swashbuckle.AspNetCore.Annotations;

namespace NV.Templates.Backend.Web.General
{
    [ApiVersionNeutral]
    public class GeneralController : ControllerBase
    {
        private readonly IApplicationInfo _applicationInfo;

        public GeneralController(IApplicationInfo applicationInfo)
        {
            _applicationInfo = applicationInfo;
        }

        [HttpGet("info")]
        [SwaggerOperation(Summary = "Get application information")]
        public ActionResult<ApplicationInfoModel> GetInfo()
        {
            return Ok(new ApplicationInfoModel(_applicationInfo));
        }
    }
}
