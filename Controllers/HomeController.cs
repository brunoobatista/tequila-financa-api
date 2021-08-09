using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("home")]
    public class HomeController : BaseController
    {
        [HttpGet()]
        public ActionResult<string> GetCarteira()
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