using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tequila.Repositories.Interfaces;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Repositories;

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

        [HttpGet("{id}")]
        public IActionResult get(long id)
        {
            var usuario = _usuarioRepository.getById(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [HttpGet("{id}/detalhe")]
        public IActionResult getDetail(long Id)
        {
            var usuario = _usuarioRepository.getDetail(Id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [AllowAnonymous]
        [HttpPost("/cadastro")]
        public ActionResult<Usuario> salvar([FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                Usuario usuario = _usuarioRepository.salvar(usuarioDto);
                return Ok(usuario);
            } catch
            {
                return BadRequest("Falha ao salvar usuário");
            }
        }

    }
}