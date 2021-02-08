using System.Collections.Generic;
using IRO.Mvc.Core.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ConsimpleTestTask.WebApp.Controllers
{
    /// <summary>
    /// This class edded only for swagger documentation.
    /// </summary>
    [ApiController]
    [Route("")]
    public class LogsController : ControllerBase
    {

        [HttpGet("logsMongo")]
        [SwaggerResponse(200, type: typeof(HttpContextInfo), Description = "If id passed.")]
        [SwaggerResponse(201, type: typeof(List<HttpContextInfo>))]
        [SwaggerOperation("Works only in dev mode. Default types: 'http_req', 'message'.")]
        public void Get([FromQuery] string type, [FromQuery] int id)
        {
        }

        [HttpGet("requestsLogs")]
        [SwaggerResponse(200, type: typeof(HttpContextInfo), Description = "If id passed.")]
        [SwaggerResponse(201, type: typeof(List<HttpContextInfo>))]
        [SwaggerOperation("Inmemory http log. Works only in dev mode.")]
        public void Get([FromQuery] int id)
        {
        }
    }
}