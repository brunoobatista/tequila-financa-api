using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tequila.Models;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly CarteiraService _carteiraService;

        public CarteiraController(CarteiraService carteiraService, CarteiraRepository carteiraRepository)
        {
            _carteiraService = carteiraService;
        }

        [HttpPost("usuario/{usuarioId}/nova")]
        public ActionResult<Carteira> nova([FromRoute] long usuarioId)
        {
            CarteiraDTO carteiraDto = new CarteiraDTO();
            carteiraDto.usuarioId = usuarioId;
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
        
        [HttpGet("usuario/{usuarioId}/aberta")]
        public ActionResult<CarteiraDTO> GetCarteira([FromRoute] long usuarioId)
        {
            CarteiraDTO carteiraDto = new CarteiraDTO();
            carteiraDto.usuarioId = usuarioId;
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
        
        [HttpGet("usuario/{usuarioId}")]
        public ActionResult<IEnumerable<Carteira>> GetCarteiraDoUsuarioById([FromRoute] long usuarioId)
        {
            CarteiraDTO carteiraDto = new CarteiraDTO();
            carteiraDto.usuarioId = usuarioId;
            try
            {
                ICollection<Carteira> carteiras = _carteiraService.getCarteirasByUsuario(carteiraDto.usuarioId);
                return Ok(carteiras);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
