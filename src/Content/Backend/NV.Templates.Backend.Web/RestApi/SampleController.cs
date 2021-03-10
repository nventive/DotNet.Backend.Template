using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NV.Templates.Backend.Core.Framework.Continuation;
using NV.Templates.Backend.Core.Framework.Entities;
using NV.Templates.Backend.Web.Framework.Models;

namespace NV.Templates.Backend.Web.RestApi
{
    /// <summary>
    /// This sample controller demonstrate usage of API versioning and Rest API Conventions applications.
    /// </summary>
    [Route("api/v{version:apiVersion}/samples")]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        [Description("Query samples")]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        public async Task<ContinuationEnumerableModel<SampleModel>> FindSamples([FromQuery] SampleQuery query)
        {
            return Enumerable.Empty<SampleModel>()
                .ToContinuationEnumerable(query)
                .ToModel();
        }

        [HttpGet("{id}")]
        [Description("Get a sample")]
        [ApiVersion("1.0")]
        public async Task<SampleModel> GetSample(string id)
        {
            return new SampleModel
            {
                Id = id,
                Name = "Foo",
            };
        }

        [HttpGet("{id}")]
        [Description("Get a sample - v2")]
        [ApiVersion("2.0")]
        public async Task<SampleModel> GetSampleV2(string id)
        {
            return new SampleModel
            {
                Id = id,
                Name = "Foo",
            };
        }

        [HttpPost]
        [Description("Create a sample")]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        public async Task<ActionResult<SampleModel>> CreateSample([FromBody] SampleModelAttributes model)
        {
            var createdModel = new SampleModel
            {
                Id = IdGenerator.Generate(),
                Name = model.Name,
            };

            return CreatedAtAction(nameof(GetSample), new { version = "v1", id = createdModel.Id }, createdModel);
        }

        [HttpPut("{id}")]
        [Description("Update a sample")]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        public async Task<SampleModel> UpdateSample(string id, [FromBody] SampleModelAttributes model)
        {
            var updatedModel = new SampleModel
            {
                Id = id,
                Name = model.Name,
            };

            return updatedModel;
        }

        [HttpPatch("{id}")]
        [Description("Update a sample")]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        public async Task<SampleModel> UpdateSample(string id, [FromBody] JsonPatchDocument<SampleModelAttributes> patch)
        {
            var existingModel = new SampleModel
            {
                Id = id,
            };

            patch.ApplyTo(existingModel);

            return existingModel;
        }

        [HttpDelete("{id}")]
        [Description("Delete a sample")]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        public async Task<IActionResult> DeleteSample(string id)
        {
            return NoContent();
        }

#pragma warning disable CA1034 // Nested types should not be visible
        [Description("The description for the sample model.")]
        public class SampleModelAttributes
        {
            [Required]
            public string? Name { get; set; }
        }

        [Description("The description for the sample model.")]
        public class SampleModel : SampleModelAttributes
        {
            [Required]
            public string? Id { get; set; }
        }

        public class SampleQuery : ContinuationQuery
        {
            [Description("The name of the sample to query.")]
            public string? Name { get; set; }
        }
    }
}
