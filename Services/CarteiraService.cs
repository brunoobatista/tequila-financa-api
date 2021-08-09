using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Tequila.Core;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;
using Tequila.Repositories;
using Tequila.Services.Interfaces;

namespace Tequila.Services
{
    public class CarteiraService : ICarteiraService
    {
        private readonly CarteiraRepository _carteiraRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly DespesaRepository _despesaFixaRepository;

        public CarteiraService(CarteiraRepository carteiraRepository, UsuarioRepository usuarioRepository, DespesaRepository despesaFixaRepository)
        {
            _carteiraRepository = carteiraRepository;
            _usuarioRepository = usuarioRepository;
            _despesaFixaRepository = despesaFixaRepository;
        }

        public Carteira GetById(long Id)
        {
            return _carteiraRepository.GetCarteira(Id);
        }

        public CarteiraDTO GetCarteiraAtivaByUsuario(long usuarioId)
        {
            Carteira carteira = _carteiraRepository.GetCarteiraAtivaByUsuario(usuarioId);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Carteira, CarteiraDTO>());
            var mapper = config.CreateMapper();
            
            CarteiraDTO carteiraDto = mapper.Map<CarteiraDTO>(carteira);
            // carteiraDto.Status = STATUS[carteiraDto.StatusId];
            return carteiraDto;
        }

        public Carteira Salvar(CarteiraDTO carteiraDTO)
        {
            if (_carteiraRepository.hasCarteiraAtiva(carteiraDTO.usuarioId))
                throw new DuplicateWaitObjectException("Já existe uma carteira aberta");
            
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CarteiraDTO, Carteira>());
            var mapper = config.CreateMapper();

            Usuario usuario = _usuarioRepository.Get(carteiraDTO.usuarioId);
            Carteira carteira = mapper.Map<Carteira>(carteiraDTO);
            carteira.Renda = usuario.Renda;
            
            _carteiraRepository.Add(carteira);

            return carteira;
        }
        
        public void finalizarCarteira(CarteiraDTO carteiraDto)
        {
            Carteira carteira = _carteiraRepository.GetCarteira(carteiraDto.Id);
            if (carteira.StatusId != (int) STATUS.ABERTO)
                throw new VerificationException("Carteira não está aberta");

            List<Despesa> despesas = _despesaFixaRepository.getDespesaContinuaPorCarteira(carteira.Id);

            foreach (var despesa in despesas)
                if (despesa.StatusId == (int)STATUS.ABERTO)
                    throw new VerificationException("Carteira possui despesa(s) contínua(s) sem finalizar com valor final");

            carteira.StatusId = (int) STATUS.FINALIZADO;
            _carteiraRepository.Update(carteira);
        }
        
        /*
         * @TODO
         * Aplicar regras de negócio para o cancelamento
         */
        public void cancelarCarteira(long userId, CarteiraDTO carteiraDto)
        {
            // throw new NotImplementedException("Feature não implementada");
            Carteira carteira = _carteiraRepository.GetCarteira(carteiraDto.Id);
            int totalDespesas = _despesaFixaRepository.hasDespesasOnCarteira(userId, carteira.Id);
            if ((carteira.StatusId == (int)STATUS.FINALIZADO || carteira.StatusId == (int)STATUS.ABERTO) && totalDespesas > 0)
                throw new VerificationException("Carteira já possui despesas vinculadas");
            carteira.StatusId = (int) STATUS.CANCELADO;
            _carteiraRepository.Update(carteira);
        }

        public Carteira reativarCarteira(CarteiraDTO carteiraDto)
        {
            Carteira ultimaCarteira = _carteiraRepository.getUltimaCarteira();
            if (carteiraDto.Id == ultimaCarteira.Id && 
                (ultimaCarteira.StatusId == (int)STATUS.CANCELADO ||
                 ultimaCarteira.StatusId == (int)STATUS.FINALIZADO))
            {
                DateTime now = DateTime.Now;

                if (ultimaCarteira.CriadoEm?.Month == now.Month &&
                    ultimaCarteira.CriadoEm?.Year == now.Year)
                {
                    ultimaCarteira.StatusId = (int)STATUS.ABERTO;
                    _carteiraRepository.Update(ultimaCarteira);
                    return ultimaCarteira;
                }
                throw new ArgumentException(paramName: "Carteira", message: "A carteira precisa ser do mesmo mês e ano da data de reativação");
            }
            throw new ArgumentException(paramName: "Carteira", message: "A carteira enviada não foi a última criada ou não foi finalizada");
        }

        public PagedResult<Carteira> getCarteirasByUsuario(QueryParams parameters, long usuarioId)
        {
            return _carteiraRepository.getCarteirasByUsuario(parameters, usuarioId);
        }
    }
}
