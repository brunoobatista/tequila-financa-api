using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tequila.Core;
using Tequila.Models;

namespace Tequila.Controllers
{
    [ApiController]
    [Route("enderecos")]
    public class EnderecosController : BaseController
    {
        private readonly ApplicationContext _context;
        
        public EnderecosController(ApplicationContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public virtual PagedResult<Endereco> Get(QueryParams parameters)
        {
            var enderecos = _context.Endereco.GetPaged(parameters);
            return enderecos;
        }
    }
}