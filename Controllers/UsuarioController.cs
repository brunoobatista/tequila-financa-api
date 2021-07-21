using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Repositories;
using Tequila.Controllers;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : BaseController
    {
        private readonly ApplicationContext _context;
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioController(ApplicationContext context, UsuarioRepository usuarioRepository)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var usuario = _usuarioRepository.Get(this.GetUserId());
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }
        
        [HttpGet("detalhe")]
        public ActionResult getDetalhe()
        {
            var usuario = _usuarioRepository.getDetalhe(this.GetUserId());
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
        
        [HttpPut]
        public ActionResult<Usuario> atualizar([FromBody] UsuarioDTO usuarioDto)
        {
            try
            {
                usuarioDto.id = this.GetUserId();
                Usuario usuario = _usuarioRepository.atualizar(usuarioDto);
                return Ok(usuario);
            } catch (Exception e)
            {
                return BadRequest("Falha ao salvar usuario." + e.Message);
            }
        }

        [HttpPut("alterar_senha")]
        public ActionResult alterarSenha(AlterarSenhaDTO alterarSenhaDto)
        {
            try
            {
                _usuarioRepository.alterarSenha(this.GetUserId(), alterarSenhaDto);
                return Ok("Senha alterada com sucesso!");
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public ActionResult inativarUsuario()
        {
            try
            {
                _usuarioRepository.inativar(this.GetUserId());
                return NoContent();
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}