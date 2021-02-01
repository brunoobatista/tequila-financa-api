using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Services;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("renda-adicional")]
    public class RendaAdicionalController : ControllerBase
    {
        private readonly RendaAdicionalService _rendaAdicionalService;

        public RendaAdicionalController(RendaAdicionalService rendaAdicionalService)
        {
            _rendaAdicionalService = rendaAdicionalService;
        }

        [Route("nova")]
        public ActionResult<RendaAdicional> criarNova([FromBody] RendaAdicionalDTO rendaAdicionalDto)
        {
            try
            {
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
                RendaAdicional rendaAdicional = _rendaAdicionalService.editar(rendaAdicionalDto);
                return Ok(rendaAdicional);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("{id}/remover")]
        public ActionResult<RendaAdicional> remover(long id)
        {
            try
            {
                _rendaAdicionalService.remover(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}