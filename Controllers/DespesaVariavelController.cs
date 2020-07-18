using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Services;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("despesa-variavel")]
    public class DespesaVariavelController : ControllerBase
    {

        private readonly DespesaVariavelService _despesaVariavelService;

        public DespesaVariavelController(DespesaVariavelService despesaVariavelService)
        {
            _despesaVariavelService = despesaVariavelService;
        }
        
        [HttpGet("{id}")]
        public ActionResult<DespesaVariavel> getDespesa(long id)
        {
            try
            {
                DespesaVariavel despesaVariavel = _despesaVariavelService.getById(id);
                return Ok(despesaVariavel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ativas")]
        public ActionResult<IEnumerable<DespesaVariavel>> getDespesas([FromQuery] long usuarioId,
            [FromQuery] long carteiraId)
        {
            try
            {
                List<DespesaVariavel> despesaVariavels = _despesaVariavelService.getDespesasAtivas(usuarioId, carteiraId);
                return Ok(despesaVariavels);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("nova")]
        public ActionResult<DespesaVariavel> criarNova([FromBody] DespesaVariavelDTO despesaVariavelDto)
        {
            try
            {
                DespesaVariavel despesaVariavel = _despesaVariavelService.salvar(despesaVariavelDto);
                return Ok(despesaVariavel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("editar")]
        public ActionResult<DespesaVariavel> editar([FromBody] DespesaVariavelDTO despesaVariavelDto)
        {
            try
            {
                DespesaVariavel despesaVariavel = _despesaVariavelService.atualizar(despesaVariavelDto);
                return Ok(despesaVariavel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("remover/{id}")]
        public ActionResult<DespesaVariavel> remover(long id)
        {
            try
            {
                _despesaVariavelService.remover(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}