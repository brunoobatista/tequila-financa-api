using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tequila.Core;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;
using Tequila.Services;

public class ValorDTO
{
    public decimal valor { get; set; }
}
namespace Tequila.Controllers
{
    
    [Authorize]
    [ApiController]
    [Route("despesa")]
    public class DespesaFixaController : BaseController
    {
        private readonly DespesaService _despesaService;

        public DespesaFixaController(DespesaService despesaFixaService)
        {
            _despesaService = despesaFixaService;
        }
        
        [HttpGet("{id}")]
        public ActionResult<Despesa> getDespesa(long id)
        {
            try
            {
                Despesa despesaFixa = _despesaService.getById(id);
                return Ok(despesaFixa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /**
         * Despesas Fixas
         */
        [HttpPut("{id}/editar")]
        public ActionResult<Despesa> atualizar([FromRoute] long id, [FromBody] DespesaFixaDTO despesaFixaDto)
        {
            despesaFixaDto.Id = id;
            try
            {
                Despesa despesaFixa = _despesaService.atualizar(despesaFixaDto);
                return Ok(despesaFixa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /**
         * Despesas
         */
        [HttpGet("ativas")]
        public ActionResult<PagedResult<Despesa>> getDespesas(
            [FromQuery] QueryParams parameters)
        {
            try
            {
                PagedResult<Despesa> despesaFixas = _despesaService.getDespesas(parameters, this.GetUserId(), null, (int)STATUSDESPESA.TODOS);
                return Ok(despesaFixas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /**
         * Despesas Fixas
         */
        [HttpPost("{despesaId}/finalizar")]
        public IActionResult finalizar(long despesaId, [FromBody] ValorDTO valorDto)
        {
            try
            {
                bool result = _despesaService.finalizarDespesaFixa(despesaId, valorDto.valor);
                if (result)
                    return Ok(result);
                return BadRequest("Despesa não encontrada");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /**
         * Despesas Variável
         */
        [HttpPost("variavel/salvar")]
        public IActionResult salvarDespesaVariavel([FromBody] DespesaVariavelDTO despesaVariavelDto)
        {
            try
            {
                Despesa despesa = _despesaService.salvarDespesaVariavel(this.GetUserId(), despesaVariavelDto);
                return Ok(despesa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /**
         * Despesas Variável
         */
        [HttpGet("variavel/{id}")]
        public IActionResult salvarDespesaVariavel([FromQuery] long id)
        {
            try
            {
                Despesa despesa = _despesaService.getDespesaVariavel(this.GetUserId(), id);
                return Ok(despesa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /**
         * Despesas Variável
         */
        [HttpGet("variavel/ativas")]
        public ActionResult<PagedResult<Despesa>> getDespesasVariaveis([FromQuery] QueryParams parameters, long? carteiraId)
        {
            try
            {
                PagedResult<Despesa> despesaVariaveis = _despesaService.getDespesas(parameters, this.GetUserId(), carteiraId, (int)TIPO.VARIAVEL);
                return Ok(despesaVariaveis);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}