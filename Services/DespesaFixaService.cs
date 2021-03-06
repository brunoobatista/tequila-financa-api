﻿using System.Collections.Generic;
using AutoMapper;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;
using Tequila.Repositories;
using Tequila.Services.Interfaces;

namespace Tequila.Services
{
    public class DespesaFixaService : IDespesaFixaService
    {
        private readonly DespesaFixaRepository _despesaFixaRepository;
        private readonly CarteiraRepository _carteiraRepository;
        public DespesaFixaService(DespesaFixaRepository despesaFixaRepository, CarteiraRepository carteiraRepository)
        {
            _despesaFixaRepository = despesaFixaRepository;
            _carteiraRepository = carteiraRepository;
        }

        public DespesaFixa getById(long id)
        {
            return _despesaFixaRepository.Get(id);
        }
        
        public DespesaFixa atualizar(DespesaFixaDTO despesaFixaDto)
        {
            DespesaFixa despesaFixa = mapper(despesaFixaDto);

            return _despesaFixaRepository.Update(despesaFixa);
        }

        public List<DespesaFixa> getDespesasAtivas(long usuarioId, long carteiraId)
        {
            Carteira carteira = _carteiraRepository.Get(carteiraId);
            if (carteira.UsuarioId == usuarioId && carteira.StatusId == (int)STATUS.ABERTO)
            {
                return _despesaFixaRepository.getListaCarteiraAtiva(carteira.Id);
            }

            return new List<DespesaFixa>();
        }

        public bool finalizarDespesa(long idDespesa, decimal valor)
        {
            return _despesaFixaRepository.finalizarDespesa(new DespesaFixaDTO() {Id = idDespesa, Valor = valor});
        }
        
        private DespesaFixa mapper(DespesaFixaDTO despesaFixaDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DespesaFixaDTO, DespesaFixa>());
            var map = config.CreateMapper();

            return map.Map<DespesaFixa>(despesaFixaDto);
        }
    }
}