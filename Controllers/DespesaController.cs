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
         * Despesas Geral
         */
        [HttpPut("{id}/editar")]
        public ActionResult<Despesa> atualizar([FromRoute] long id, [FromBody] DespesaDTO despesaFixaDto)
        {
            despesaFixaDto.Id = id;
            try
            {
                Despesa despesaFixa = _despesaService.atualizar(GetUserId(), despesaFixaDto);
                return Ok(despesaFixa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}/cancelar")]
        public ActionResult<Despesa> cancelar([FromRoute] long id)
        {
            try
            {
                Despesa despesa = _despesaService.cancelarDespesa(id);
                return Ok(despesa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}/reativar")]
        public ActionResult<Despesa> reativar([FromRoute] long id)
        {
            try
            {
                Despesa despesa = _despesaService.reativarespesa(id);
                return Ok(despesa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult<PagedResult<Despesa>> getDespesasAll(
            [FromQuery] QueryParams parameters, string tipos, bool? ativo)
        {
            try
            {
                PagedResult<Despesa> despesas = _despesaService.getDespesasAll(parameters, this.GetUserId(), tipos, ativo.HasValue ? ativo.Value : true);
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
                PagedResult<Despesa> despesaFixas = _despesaService.getDespesas(parameters, this.GetUserId(), null, (int)SITUACAODESPESA.TODOS);
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
                PagedResult<Despesa> despesa = _despesaService.getDespesas(parameters, this.GetUserId(), carteiraId, (int)SITUACAODESPESA.TODOS);
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
        [HttpPost("fixa/salvar")]
        public ActionResult salvarDespesaFixa([FromBody] DespesasFixasDTO despesasFixasDto)
        {
            try
            {
                Despesa despesa = _despesaService.salvarDespesaFixa(this.GetUserId(), despesasFixasDto);
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
         * Despesas Avulsa
         */
        [HttpGet("avulsa/ativas")]
        public ActionResult<PagedResult<Despesa>> getDespesasAvulsas([FromQuery] QueryParams parameters, long? carteiraId)
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

        /**
         * Despesa
         * remover
         */
        [HttpDelete("{id}/remover")]
        public ActionResult removerDespesaAvulsa(long id)
        {
            try
            {
               if (!_despesaService.inativarDespesaAvulsa(id)) 
                   return BadRequest("Despesa não encontrada");
               return Ok("Despesa removida");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}