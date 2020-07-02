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

        [HttpPost("nova")]
        public ActionResult<Carteira> nova([FromBody] CarteiraDTO carteiraDTO)
        {
            try 
            {
                Carteira carteiraSalva = _carteiraService.Salvar(carteiraDTO);
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
                CarteiraDTO carteira = _carteiraService.GetCarteiraAtivaByUsuario(carteiraDto.usuarioId);
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
                _carteiraService.finalizarCarteira(carteiraDto);
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
                // _carteiraService.cancelarCarteira(carteiraDto);
                // return NoContent();
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
        
        [HttpPost]
        public ActionResult<IEnumerable<Carteira>> GetCarteiraById([FromBody] CarteiraDTO carteiraDto)
        {
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
