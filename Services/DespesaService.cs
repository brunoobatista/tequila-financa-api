﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security;
using AutoMapper;
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

        public Despesa salvarDespesaVariavel(long userId, DespesaVariavelDTO despesaVariavelDto)
        {
            Carteira carteira = _carteiraRepository.GetCarteiraAtivaByUsuario(userId);
            Despesa despesa = Despesa.mapperDespesaVariavel(userId, carteira.Id, despesaVariavelDto);
            if (despesaVariavelDto.Id == null)
            {
                _despesaFixaRepository.Add(despesa);
            }
            else
            {
                _despesaFixaRepository.Update(despesa);
            }

            return despesa;
        }

        public Despesa getDespesaVariavel(long userId, long despesaId)
        {
            Despesa despesa = _despesaFixaRepository.getDespesaVariavelByUsuario(userId, despesaId);
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

        public List<Despesa> getDespesas(long usuarioId, long? carteiraId, int tipo)
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
                return _despesaFixaRepository.getListaCarteiraAtiva(usuarioId, carteira.Id, tipo);
            }

            return new List<Despesa>();
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