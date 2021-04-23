using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("home")]
    public class HomeController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<string> GetCarteira([FromRoute] long usuarioId)
        {
            try
            {
                return Ok("Teste do Home");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}