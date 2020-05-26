using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Tequila.Controllers
{
    [ApiController]
    [Route("errors")]
    public class ErrorController : ControllerBase
    {

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(int code)
        {
            return await Task.Run(() =>
            {
                return StatusCode(code, new ProblemDetails()
                {
                    Detail = "See the errors property for details.",
                    Instance = HttpContext.Request.Path,
                    Status = code,
                    Title = ((HttpStatusCode)code).ToString(),
                    Type = "https://my.api.com/response"
                });
            });
        }
    }
}
