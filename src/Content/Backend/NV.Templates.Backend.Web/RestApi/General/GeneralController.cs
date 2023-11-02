using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NV.Templates.Backend.Web.RestApi.General
{
    [Route("api")]
    public class GeneralController : ControllerBase
    {
        private readonly IApplicationInfo _applicationInfo;
        private readonly IStringLocalizerEx _localizer;

        public GeneralController(IApplicationInfo applicationInfo, IStringLocalizerEx localizer)
        {
            _applicationInfo = applicationInfo;
            _localizer = localizer;
        }

        [ApiVersionNeutral]
        [AllowAnonymous]
        [HttpGet("info")]
        [Description("Get application information")]
        public ActionResult<ApplicationInfoModel> GetInfo()
        {
            return Ok(new ApplicationInfoModel(_applicationInfo));
        }
#if DEBUG
        [ApiVersionNeutral]
        [AllowAnonymous]
        [HttpGet("localization-test")]
        [Description("Allows to test localization.")]
        public ActionResult LocalizationTest(string? key = null)
        {
            key ??= "Hello, world!";

            // Just to ensure there are enough arguments for string.Format.
            var arguments = Enumerable.Range(0, 100).Select(i => $"{i}").ToArray();
            return Ok(new Dictionary<string, object>
            {
                { "current key", key! },
                { "request culture", CultureInfo.CurrentUICulture.Name },
                { "localized value", _localizer[key, arguments] },
                { "all supported cultures for the given key", _localizer.GetStrings(key, arguments: arguments) },
                { "all strings of the app in the request culture", _localizer.GetAllStrings(true, arguments) },
            });
        }
#endif
    }
}
