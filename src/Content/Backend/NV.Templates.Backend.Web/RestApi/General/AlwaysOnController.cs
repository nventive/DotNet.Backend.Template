using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NV.Templates.Backend.Web.RestApi.General
{
    /// <summary>
    /// A controller used to respond with a 200 Status for the "Always On" configuration setting.
    /// The "AlwaysOn" feature of Azure AppService will send requests to HTTP GET/ every 5 minutes.
    /// </summary>
    [Route("ping")]
    public class AlwaysOnController : ControllerBase
    {
        /// <summary>
        /// Simple action to return a 200 status response.
        /// </summary>
        /// <returns>Returns a 200 to avoid 404 responses.</returns>
        [HttpGet]
        [ApiVersionNeutral]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlwaysOn()
        {
            return Ok();
        }
    }
}
