using System;
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
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioController(ApplicationContext context, UsuarioRepository usuarioRepository)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet("{id}")]
        public ActionResult Get(long id)
        {
            var usuario = _usuarioRepository.Get(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }
        
        [HttpGet("{id}/detalhe")]
        public ActionResult getDetalhe(long id)
        {
            var usuario = _usuarioRepository.getDetalhe(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<Usuario> salvar([FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                Usuario usuario = _usuarioRepository.salvar(usuarioDto);
                return Ok(usuario);
            } catch (Exception e)
            {
                return BadRequest("Falha ao salvar usuario. " + e.Message);
            }
        }
        
        [HttpPut("{id}")]
        public ActionResult<Usuario> atualizar(long id, [FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                if (id != usuarioDto.id)
                    return BadRequest("Usuario não confere");
                
                Usuario usuario = _usuarioRepository.atualizar(usuarioDto);
                return Ok(usuario);
            } catch (Exception e)
            {
                return BadRequest("Falha ao salvar usuario." + e.Message);
            }
        }

        [HttpPut("{id}/alterar_senha")]
        public ActionResult alterarSenha(long id, AlterarSenhaDTO alterarSenhaDto)
        {
            try
            {
                _usuarioRepository.alterarSenha(id, alterarSenhaDto);
                return NoContent();
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult inativarUsuario(long id)
        {
            try
            {
                _usuarioRepository.inativar(id);
                return NoContent();
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}