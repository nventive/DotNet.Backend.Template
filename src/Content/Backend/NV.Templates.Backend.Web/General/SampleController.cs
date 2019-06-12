using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using NV.Templates.Backend.Core.Framework.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace NV.Templates.Backend.Web.General
{
    /// <summary>
    /// This sample controller illustrate some conventions
    /// and API versioning feature of the template.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SampleController : ControllerBase
    {
        [ApiVersion("1")]
        [HttpGet("hello")]
        [SwaggerOperation(Summary = "Sample Hello world")]
        public ActionResult<string> GetHello(string name = null)
        {
            return Ok($"Hello, {name ?? "world"}");
        }

        [ApiVersion("2")]
        [HttpGet("hello")]
        [SwaggerOperation(Summary = "Sample Hello world")]
        public ActionResult<HelloWorldResponse> GetHelloV2([FromServices] IClock clock, string name = null)
        {
            return Ok(new HelloWorldResponse { Message = $"Hello, {name ?? "world"}", Timestamp = clock.GetCurrentInstant() });
        }

        [ApiVersion("1")]
        [ApiVersion("2")]
        [HttpPost("sample")]
        [SwaggerOperation(Summary = "Sample Post action")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public IActionResult PostSample(bool return404 = false)
        {
            if (return404)
            {
                throw new NotFoundException(HttpContext.Request.Path);
            }

            return NoContent();
        }

        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Sample code to be deleted.")]
        public class HelloWorldResponse
        {
            public string Message { get; set; }

            public Instant Timestamp { get; set; }
        }
    }
}
