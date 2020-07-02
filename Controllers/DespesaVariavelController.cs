using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Services;
using Tequila.Services.Interfaces;

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
        
        [Route("nova")]
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