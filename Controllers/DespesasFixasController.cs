using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Repositories;
using Tequila.Services;

namespace Tequila.Controllers
{
    [Authorize]
    [ApiController]
    [Route("despesas-fixas")]
    public class DespesasFixasController : BaseController
    {
        private readonly DespesasFixasService _despesasFixasService;
        private readonly DespesasFixasRepository _despesasFixasRepository;

        public DespesasFixasController(DespesasFixasService despesasFixasService, DespesasFixasRepository despesasFixasRepository)
        {
            _despesasFixasService = despesasFixasService;
            _despesasFixasRepository = despesasFixasRepository;
        }
    
        [HttpGet("usuario/{idUsuario}")]
        public ActionResult<IEnumerable<DespesasFixas>> getDespesasFixasByUsuario(long idUsuario)
        {
            try
            {
                List<DespesasFixas> despesasFixas = _despesasFixasRepository.getDespesasFixasByUsuario(idUsuario);
                if (despesasFixas.Count == 0)
                    return NotFound();

                return despesasFixas;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("{id}")]
        public ActionResult<DespesasFixas> getDespesasFixas(long id)
        {
            try
            {
                DespesasFixas despesasFixas = _despesasFixasRepository.Get(id);
                if (despesasFixas == null)
                    return NotFound();

                return despesasFixas;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("novo")]
        public ActionResult<DespesasFixas> criarNova(DespesasFixasDTO despesasFixasDto)
        {
            try
            {
                despesasFixasDto.UsuarioId = this.GetUserId();
                DespesasFixas despesasFixas = _despesasFixasService.salvar(despesasFixasDto);
                return Ok(despesasFixas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("editar")]
        public ActionResult<DespesasFixas> atualizar(DespesasFixasDTO despesasFixasDto)
        {
            try
            {
                DespesasFixas despesasFixas = _despesasFixasService.atualizar(despesasFixasDto);
                return Ok(despesasFixas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("finalizar")]
        public ActionResult<DespesasFixas> finalizar(DespesasFixasDTO despesasFixasDto)
        {
            try
            {
                DespesasFixas despesasFixas = _despesasFixasService.finalizar(despesasFixasDto);
                return Ok(despesasFixas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}