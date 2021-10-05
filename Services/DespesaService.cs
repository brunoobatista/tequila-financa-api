using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security;
using AutoMapper;
using Tequila.Core;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;
using Tequila.Repositories;
using Tequila.Services.Interfaces;

namespace Tequila.Services
{
    public class DespesaService : IDespesaFixaService
    {
        private readonly DespesaRepository _despesaFixaRepository;
        private readonly CarteiraRepository _carteiraRepository;
        public DespesaService(DespesaRepository despesaFixaRepository, CarteiraRepository carteiraRepository)
        {
            _despesaFixaRepository = despesaFixaRepository;
            _carteiraRepository = carteiraRepository;
        }

        public Despesa getById(long id)
        {
            return _despesaFixaRepository.Get(id);
        }

        public Despesa salvarDespesaAvulsa(long userId, DespesaAvulsaDTO despesaAvulsaDto)
        {
            Carteira carteira = _carteiraRepository.GetCarteiraAtivaByUsuario(userId);
            Despesa despesa = Despesa.mapperDespesaAvulsa(userId, carteira.Id, despesaAvulsaDto);
            if (despesaAvulsaDto.Id == null)
            {
                _despesaFixaRepository.Add(despesa);
            }
            else
            {
                _despesaFixaRepository.Update(despesa);
            }

            return despesa;
        }

        public Despesa getDespesaAvulsa(long userId, long despesaId)
        {
            Despesa despesa = _despesaFixaRepository.getDespesaAvulsaByUsuario(userId, despesaId);
            return despesa;
        }
        
        public Despesa atualizar(DespesaFixaDTO despesaFixaDto)
        {
            Despesa despesaOld = _despesaFixaRepository.Get(despesaFixaDto.Id);

            if (despesaOld.TipoId == (int)TIPO.PARCELADO)
            {
                throw new VerificationException("Despesa Parcelada não pode ser alterada");
            }
            Despesa despesaFixa = mapper(despesaFixaDto);
            despesaFixa.DespesasFixasId = despesaOld.DespesasFixasId;
            despesaFixa.CarteiraId = despesaOld.CarteiraId;

            return _despesaFixaRepository.Update(despesaFixa);
        }
        
        public PagedResult<Despesa> getDespesasAll(QueryParams parameters, long usuarioId, string? tipos, bool? ativo = true)
        {
            long? cartId = null;
            if (ativo.HasValue && ativo.Value)
            {
                Carteira aberta = _carteiraRepository.GetCarteiraAtivaByUsuario(usuarioId);
                if (aberta == null)
                {
                    throw new ValidationException("Não há carteira aberta, crie ou informe uma carteira");
                }

                cartId = aberta.Id;
            }
            return _despesaFixaRepository.getDespesaAll(parameters, usuarioId, tipos, cartId);
        }

        public PagedResult<Despesa> getDespesas(QueryParams parameters, long usuarioId, long? carteiraId, int tipo)
        {
            long cartId = 0;
            if (carteiraId == null)
            {
                Carteira aberta = _carteiraRepository.GetCarteiraAtivaByUsuario(usuarioId);
                if (aberta == null)
                {
                    throw new ValidationException("Não há carteira aberta, crie ou informe uma carteira");
                }

                cartId = aberta.Id;
            }
            else
            {
                cartId = carteiraId.Value;
            }
            Carteira carteira = _carteiraRepository.Get(cartId);
            if (carteira.UsuarioId == usuarioId)
            {
                return _despesaFixaRepository.getListaCarteiraAtiva(parameters,usuarioId, carteira.Id, tipo);
            }

            return new PagedResult<Despesa>();
        }

        public bool finalizarDespesaFixa(long idDespesa, decimal valor)
        {
            return _despesaFixaRepository.finalizarDespesa(new DespesaFixaDTO() {Id = idDespesa, Valor = valor});
        }
        
        private Despesa mapper(DespesaFixaDTO despesaFixaDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DespesaFixaDTO, Despesa>());
            var map = config.CreateMapper();

            return map.Map<Despesa>(despesaFixaDto);
        }
        
    }
}