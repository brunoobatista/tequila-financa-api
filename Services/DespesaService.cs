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
        private readonly DespesaRepository _despesaRepository;
        private readonly CarteiraRepository _carteiraRepository;
        public DespesaService(DespesaRepository despesaRepository, CarteiraRepository carteiraRepository)
        {
            _despesaRepository = despesaRepository;
            _carteiraRepository = carteiraRepository;
        }

        public Despesa getById(long id)
        {
            return _despesaRepository.Get(id);
        }

        public Despesa salvarDespesaAvulsa(long userId, DespesaAvulsaDTO despesaAvulsaDto)
        {
            Carteira carteira = _carteiraRepository.GetCarteiraAtivaByUsuario(userId);
            Despesa despesa = Despesa.mapperDespesaAvulsa(userId, carteira.Id, despesaAvulsaDto);
            if (despesaAvulsaDto.Id == null)
            {
                _despesaRepository.Add(despesa);
            }
            else
            {
                _despesaRepository.Update(despesa);
            }

            return despesa;
        }

        public Despesa getDespesaAvulsa(long userId, long despesaId)
        {
            Despesa despesa = _despesaRepository.getDespesaAvulsaByUsuario(userId, despesaId);
            return despesa;
        }
        
        public Despesa atualizar(long userId, DespesaDTO despesaDto)
        {
            Despesa despesaOld = _despesaRepository.Get(despesaDto.Id);

            if (despesaOld.TipoId == (int)TIPO.PARCELADO)
            {
                throw new VerificationException("Despesa Parcelada não pode ser alterada");
            }

            Despesa despesaAtualizada = despesaOld.atualizarDados(despesaDto);
            despesaAtualizada.DespesasFixasId = despesaOld.DespesasFixasId;
            despesaAtualizada.CarteiraId = despesaOld.CarteiraId;
            despesaAtualizada.UsuarioId = userId;
            return _despesaRepository.Update(despesaAtualizada);
        }
        
        public PagedResult<Despesa> getDespesasAll(QueryParams parameters, long usuarioId, string tipos = "", bool ativo = true)
        {
            long? cartId = null;
            if (ativo)
            {
                Carteira aberta = _carteiraRepository.GetCarteiraAtivaByUsuario(usuarioId);
                if (aberta == null)
                {
                    throw new ValidationException("Não há carteira aberta, crie ou informe uma carteira");
                }

                cartId = aberta.Id;
            }
            return _despesaRepository.getDespesaAll(parameters, usuarioId, tipos, cartId);
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
                return _despesaRepository.getListaCarteiraAtiva(parameters,usuarioId, carteira.Id, tipo);
            }

            return new PagedResult<Despesa>();
        }

        public bool finalizarDespesaFixa(long idDespesa, decimal valor)
        {
            return _despesaRepository.finalizarDespesa(new DespesaDTO() {Id = idDespesa, Valor = valor});
        }

        public bool inativarDespesaAvulsa(long idDespesa)
        {
            Despesa despesa = _despesaRepository.Get(idDespesa);
            if (despesa.TipoId != (int)TIPO.AVULSA)
                throw new ValidationException("Somente despesa avulsa pode ser removida");
            return _despesaRepository.Inactive(idDespesa);
        }
        
        public Despesa cancelarDespesa(long idDespesa)
        {
            Despesa despesa = _despesaRepository.Get(idDespesa);
            if (despesa.TipoId == (int)TIPO.PARCELADO)
                throw new ValidationException("Despesa parcelada não pode ser cancelada");
            despesa.StatusId = 0;
            return _despesaRepository.Update(despesa);
        }
        
        public Despesa reativarespesa(long idDespesa)
        {
            Despesa despesa = _despesaRepository.Get(idDespesa);
            if (despesa.TipoId == (int)TIPO.PARCELADO)
                throw new ValidationException("Despesa parcelada não pode ser reativada");
            if (despesa.TipoId == (int) TIPO.AVULSA)
                despesa.StatusId = (int) STATUSDESPESA.FIXADO;
            else
                despesa.StatusId = (int) STATUSDESPESA.ABERTO;
            return _despesaRepository.Update(despesa);
        }
        
        private Despesa mapper(DespesaDTO despesaFixaDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Despesa, DespesaDTO>());
            var map = config.CreateMapper();
            var mapper = new Mapper(config);
 

            return mapper.Map<Despesa>(despesaFixaDto);
        }
        
    }
}