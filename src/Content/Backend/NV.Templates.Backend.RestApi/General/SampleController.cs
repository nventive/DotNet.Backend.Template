using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NV.Templates.Backend.Core.Framework.Exceptions;

namespace NV.Templates.Backend.RestApi.General
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
        [Description("Sample Hello world")]
        public ActionResult<string> GetHello([Description("The salutation")] string name = null)
        {
            return Ok($"Hello, {name ?? "world"}");
        }

        [ApiVersion("2")]
        [HttpGet("hello")]
        [Description("Sample Hello world")]
        public ActionResult<HelloWorldResponse> GetHelloV2([Description("The salutation")] string name = null)
        {
            return Ok(new HelloWorldResponse { Message = $"Hello, {name ?? "world"}", Timestamp = DateTimeOffset.UtcNow });
        }

        [ApiVersion("1")]
        [ApiVersion("2")]
        [HttpPost("sample")]
        [Description("Sample Post action")]
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
            [Required]
            [Description("The generated message")]
            public string Message { get; set; }

            [Description("The generated timestamp")]
            public DateTimeOffset Timestamp { get; set; }
        }
    }
}
