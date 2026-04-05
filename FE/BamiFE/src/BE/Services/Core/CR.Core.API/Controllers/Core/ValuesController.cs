using CR.Common.Filters;
using CR.DtoBase;
using CR.Utils.Net.MimeTypes;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Core
{
    [Authorize]
    [Route("api/core/value")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ValuesController : ApiControllerBase
    {
        public ValuesController(
            ILogger<ValuesController> logger
        )
            : base(logger)
        {
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        [PermissionFilter]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("getpdf")]
        public IActionResult Test([FromBody] string base64)
        {
            byte[] byteArray = Convert.FromBase64String(base64);
            MemoryStream stream = new MemoryStream(byteArray);
            return File(stream, MimeTypeNames.ApplicationPdf, "1232");
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }

        public class Data1
        {
            public string? Name { get; set; }
            public string? Desc { get; set; }
        }

        [HttpGet("test-result")]
        public ApiResponse TestResult()
        {
            return OkResult(Result.Success());
        }
    }
}
