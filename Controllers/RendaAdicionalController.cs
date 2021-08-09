using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tequila.Core;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Repositories;
using Tequila.Services;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("renda-adicional")]
    public class RendaAdicionalController : BaseController
    {
        private readonly RendaAdicionalService _rendaAdicionalService;
        private readonly RendaAdicionalRepository _rendaAdicionalRepository;

        public RendaAdicionalController(RendaAdicionalService rendaAdicionalService, RendaAdicionalRepository rendaAdicionalRepository)
        {
            _rendaAdicionalService = rendaAdicionalService;
            _rendaAdicionalRepository = rendaAdicionalRepository;
        }

        [Route("")]
        public ActionResult<PagedResult<RendaAdicional>> getAllRendas([FromQuery] QueryParams parameters)
        {
            try
            {
                PagedResult<RendaAdicional> rendasAdicionals = _rendaAdicionalRepository.GetAll(parameters, this.GetUserId());
                return Ok(rendasAdicionals);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("nova")]
        public ActionResult<RendaAdicional> criarNova([FromBody] RendaAdicionalDTO rendaAdicionalDto)
        {
            try
            {
                rendaAdicionalDto.UsuarioId = this.GetUserId();
                RendaAdicional rendaAdicional = _rendaAdicionalService.nova(rendaAdicionalDto);
                return Ok(rendaAdicional);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("editar")]
        public ActionResult<RendaAdicional> editar([FromBody] RendaAdicionalDTO rendaAdicionalDto)
        {
            try
            {
                rendaAdicionalDto.UsuarioId = this.GetUserId();
                RendaAdicional rendaAdicional = _rendaAdicionalService.editar(rendaAdicionalDto);
                return Ok(rendaAdicional);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("{id}/remover")]
        public ActionResult<RendaAdicional> remover(long id)
        {
            try
            {
                _rendaAdicionalService.remover(id, this.GetUserId());
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}