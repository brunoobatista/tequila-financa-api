using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tequila.Models;
using Tequila.Repositories;
using Tequila.Repositories.Interfaces;
using Tequila.Services;
using Tequila.Services.Interfaces;

namespace Tequila.Controllers
{

    [Authorize]
    [ApiController]
    [Route("token")]
    public class TokenController : ControllerBase
    {

        private readonly TokenService _tokenService;
        private readonly UsuarioRepository _usuarioRepository;

        public TokenController(TokenService tokenService, UsuarioRepository usuarioRepository)
        {
            _tokenService = tokenService;
            _usuarioRepository = usuarioRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult Login([FromBody] AuthenticationDTO authentication)
        {
            Usuario usuario = _usuarioRepository.ValidarLoginUsuario(authentication);
            if (usuario == null)
            {
                return Unauthorized("E-mail e/ou senha incorreto(s)!");
            }

            var token = _tokenService.GerarToken(usuario);

            //return Ok(new { usuario });
            return Ok(new AuthenticatedDTO(usuario, token));
        }
    }
}