using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tequila.Models.DTOs;
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

        [Route("{idDespesa}/finalizar")]
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