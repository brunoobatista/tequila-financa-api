using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tequila.Repositories.Interfaces;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(ApplicationContext context, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
        }

        //[HttpGet]
        //public virtual IEnumerable<Usuario> Get()
        //{
        //    var usuarios = _context.Usuario.ToList();
        //    return usuarios;
        //}

        [HttpGet("{id}")]
        public IActionResult Get(long Id)
        {
            var usuario = _context.Usuario.Find(Id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [HttpGet("{id}/detail")]
        public IActionResult GetDetail(long Id)
        {
            var usuario = _usuarioRepository.GetById(Id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

    }
}