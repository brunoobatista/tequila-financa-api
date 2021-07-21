using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace Tequila.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected long GetUserId() {

            return long.Parse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}