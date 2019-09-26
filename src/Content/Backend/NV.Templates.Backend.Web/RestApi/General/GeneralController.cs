using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NV.Templates.Backend.Core.General;

namespace NV.Templates.Backend.Web.RestApi.General
{
    [Route("api")]
    public class GeneralController : ControllerBase
    {
        private readonly IApplicationInfo _applicationInfo;

        public GeneralController(IApplicationInfo applicationInfo)
        {
            _applicationInfo = applicationInfo;
        }

        [ApiVersionNeutral]
        [AllowAnonymous]
        [HttpGet("info")]
        [Description("Get application information")]
        public ActionResult<ApplicationInfoModel> GetInfo()
        {
            return Ok(new ApplicationInfoModel(_applicationInfo));
        }
    }
}
