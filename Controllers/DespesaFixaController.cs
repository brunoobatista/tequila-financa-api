using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tequila.Models;
using Tequila.Services;

public class ValorDTO
{
    public decimal valor { get; set; }
}
namespace Tequila.Controllers
{
    
    [Authorize]
    [ApiController]
    [Route("despesa-fixa")]
    public class DespesaFixaController : ControllerBase
    {
        private readonly DespesaFixaService _despesaFixaService;

        public DespesaFixaController(DespesaFixaService despesaFixaService)
        {
            _despesaFixaService = despesaFixaService;
        }
        
        [HttpGet("{id}")]
        public ActionResult<DespesaFixa> getDespesa(long id)
        {
            try
            {
                DespesaFixa despesaFixa = _despesaFixaService.getById(id);
                return Ok(despesaFixa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ativas")]
        public ActionResult<IEnumerable<DespesaFixa>> getDespesas([FromQuery] long usuarioId,
            [FromQuery] long carteiraId)
        {
            try
            {
                List<DespesaFixa> despesaFixas = _despesaFixaService.getDespesasAtivas(usuarioId, carteiraId);
                return Ok(despesaFixas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("{idDespesa}/finalizar")]
        public IActionResult finalizar(long idDespesa, [FromBody] ValorDTO valorDto)
        {
            try
            {
                bool result = _despesaFixaService.finalizarDespesa(idDespesa, valorDto.valor);
                if (result)
                    return Ok(result);
                return BadRequest("Despesa não encontrada");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}