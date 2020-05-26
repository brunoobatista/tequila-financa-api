using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tequila.Models;

namespace Tequila.Controllers
{
    [ApiController]
    [Route("enderecos")]
    public class EnderecosController : ControllerBase
    {
        private readonly ApplicationContext _context;
        
        public EnderecosController(ApplicationContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public virtual IEnumerable<Endereco> Get()
        {
            var enderecos = _context.Endereco.ToList();
            return enderecos;
        }
    }
}