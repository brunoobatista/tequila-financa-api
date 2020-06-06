using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tequila.Models;
using System;
using Tequila.Models.DTOs;
using Tequila.Repositories;
using Tequila.Services;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("carteiras")]
    public class CarteiraController : ControllerBase
    {
        private readonly CarteiraService carteiraService;

        public CarteiraController(CarteiraService carteiraService, CarteiraRepository carteiraRepository)
        {
            this.carteiraService = carteiraService;
        }

        [HttpPost("nova")]
        public ActionResult<Carteira> CriarNova([FromBody] CarteiraDTO carteiraDTO)
        {
            try 
            {
                Carteira carteiraSalva = this.carteiraService.Salvar(carteiraDTO);
                return Ok(carteiraSalva);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("aberta")]
        public ActionResult<CarteiraDTO> GetCarteira([FromBody] CarteiraDTO carteiraDto)
        {
            try
            {
                CarteiraDTO carteira = this.carteiraService.GetCarteiraAtivaByUsuario(carteiraDto.usuarioId);
                return Ok(carteira);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("finalizar")]
        public ActionResult finalizarCarteira([FromBody] CarteiraDTO carteiraDto)
        {
            try
            {
                this.carteiraService.finalizarCarteira(carteiraDto);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("cancelar")]
        public ActionResult cancelarCarteira([FromBody] CarteiraDTO carteiraDto)
        {
            try
            {
                throw new NotSupportedException("Função não permitida");
                this.carteiraService.cancelarCarteira(carteiraDto);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("reativar")]
        public ActionResult reativarCarteira([FromBody] CarteiraDTO carteiraDto)
        {
            try
            {
                Carteira carteira = this.carteiraService.reativarCarteira(carteiraDto);
                return Ok(carteira);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // [HttpGet("{id}")]
        // public ActionResult<Carteira> GetCarteiraById([FromRoute] long Id)
        // {
        //     try
        //     {
        //         Carteira carteira = this.carteiraService.GetById(Id);
        //         return Ok(carteira);
        //     }
        //     catch (Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }

    }
}
