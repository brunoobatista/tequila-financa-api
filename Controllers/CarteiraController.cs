using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tequila.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Tequila.Core;
using Tequila.Models.DTOs;
using Tequila.Repositories;
using Tequila.Services;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("carteiras")]
    public class CarteiraController : BaseController
    {
        private readonly CarteiraService _carteiraService;

        public CarteiraController(CarteiraService carteiraService, CarteiraRepository carteiraRepository)
        {
            _carteiraService = carteiraService;
        }

        [HttpPost("usuario/nova")]
        public ActionResult<Carteira> nova()
        {
            CarteiraDTO carteiraDto = new CarteiraDTO();
            carteiraDto.usuarioId = this.GetUserId();
            try 
            {
                Carteira carteiraSalva = _carteiraService.Salvar(carteiraDto);
                return Ok(carteiraSalva);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("usuario/aberta")]
        public ActionResult<CarteiraDTO> GetCarteira()
        {
            CarteiraDTO carteiraDto = new CarteiraDTO();
            carteiraDto.usuarioId = this.GetUserId();
            try
            {
                CarteiraDTO carteira = _carteiraService.GetCarteiraAtivaByUsuario(carteiraDto.usuarioId);
                return Ok(carteira);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}/finalizar")]
        public ActionResult finalizarCarteira([FromRoute] long id)
        {
            CarteiraDTO carteiraDto = new CarteiraDTO();
            carteiraDto.Id = id;
            try
            {
                _carteiraService.finalizarCarteira(carteiraDto);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}/cancelar")]
        public ActionResult cancelarCarteira([FromBody] CarteiraDTO carteiraDto)
        {
            try
            {
                throw new NotSupportedException("Função não permitida");
                // _carteiraService.cancelarCarteira(carteiraDto);
                // return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}/reativar")]
        public ActionResult reativarCarteira([FromRoute] long id)
        {
            CarteiraDTO carteiraDto = new CarteiraDTO();
            carteiraDto.Id = id;
            try
            {
                Carteira carteira = _carteiraService.reativarCarteira(carteiraDto);
                return Ok(carteira);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Carteira> GetCarteiraById([FromRoute] long Id)
        {
            try
            {
                Carteira carteira = _carteiraService.GetById(Id);
                return Ok(carteira);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("usuario")]
        public ActionResult<PagedResult<Carteira>> GetCarteiraDoUsuarioById([FromQuery] QueryParams parameters)
        {
            CarteiraDTO carteiraDto = new CarteiraDTO();
            carteiraDto.usuarioId = this.GetUserId();
            try
            {
                PagedResult<Carteira> carteiras = _carteiraService.getCarteirasByUsuario(parameters, carteiraDto.usuarioId);
                return Ok(carteiras);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
