using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    [Route("despesas")]
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
        
        [HttpGet]
        public ActionResult<PagedResult<Despesa>> getDespesasAll(
            [FromQuery] QueryParams parameters, string? tipos, bool? ativo)
        {
            try
            {
                PagedResult<Despesa> despesas = _despesaService.getDespesasAll(parameters, this.GetUserId(), tipos, ativo);
                return Ok(despesas);
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
        
        [HttpGet("carteira/{carteiraId}")]
        public ActionResult<PagedResult<Despesa>> getDespesasByCarteira(
            long carteiraId, [FromQuery] QueryParams parameters)
        {
            try
            {
                PagedResult<Despesa> despesa = _despesaService.getDespesas(parameters, this.GetUserId(), carteiraId, (int)STATUSDESPESA.TODOS);
                return Ok(despesa);
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
        public ActionResult finalizar(long despesaId, [FromBody] ValorDTO valorDto)
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
        [HttpPost("avulsa/salvar")]
        public ActionResult salvarDespesaAvulsa([FromBody] DespesaAvulsaDTO despesaAvulsaDto)
        {
            try
            {
                Despesa despesa = _despesaService.salvarDespesaAvulsa(this.GetUserId(), despesaAvulsaDto);
                return Ok(despesa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /**
         * Despesas Avulsa
         */
        [HttpGet("avulsa/{id}")]
        public ActionResult salvarDespesaAvulsa([FromQuery] long id)
        {
            try
            {
                Despesa despesa = _despesaService.getDespesaAvulsa(this.GetUserId(), id);
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
        [HttpGet("avulsa/ativas")]
        public ActionResult<PagedResult<Despesa>> getDespesasVariaveis([FromQuery] QueryParams parameters, long? carteiraId)
        {
            try
            {
                PagedResult<Despesa> despesaAvulsas = _despesaService.getDespesas(parameters, this.GetUserId(), carteiraId, (int)TIPO.AVULSA);
                return Ok(despesaAvulsas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}