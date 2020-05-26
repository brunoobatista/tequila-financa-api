using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tequila.Repositories.Interfaces;

using Tequila.Models;
using System;
using Tequila.Services.Interfaces;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("carteiras")]
    public class CarteiraController : ControllerBase
    {
        private readonly ICarteiraService carteiraService;
        private readonly ICarteiraRepository carteiraRepository;

        public CarteiraController(ICarteiraService carteiraService, ICarteiraRepository carteiraRepository)
        {
            this.carteiraService = carteiraService;
            this.carteiraRepository = carteiraRepository;
        }

        [HttpPost("nova")]
        public IActionResult CriarNova([FromBody] Carteira carteira)
        {
            try 
            {
                Carteira carteiraSalva = this.carteiraService.Salvar(carteira);
                return Ok(carteiraSalva);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Carteira> GetCarteiraById([FromRoute] long Id)
        {
            try
            {
                Carteira carteira = this.carteiraService.GetById(Id);
                return Ok(carteira);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/detail")]
        public ActionResult<Carteira> GetCarteiraByIdLazy([FromRoute] long Id)
        {
            try
            {
                //using (var app = new ApplicationContext())
                //{

                //}


                Carteira carteia = this.carteiraRepository.GetCarteiraLazy(Id);
                return Ok(carteia);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
