using System;
using System.Security;
using AutoMapper;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;
using Tequila.Repositories;
using Tequila.Services.Interfaces;

namespace Tequila.Services
{
    public class CarteiraService : ICarteiraService
    {
        private readonly CarteiraRepository carteiraRepository;
        private readonly UsuarioRepository usuarioRepository;

        public CarteiraService(CarteiraRepository carteiraRepository, UsuarioRepository usuarioRepository)
        {
            this.carteiraRepository = carteiraRepository;
            this.usuarioRepository = usuarioRepository;
        }

        public Carteira GetById(long Id)
        {
            return this.carteiraRepository.GetCarteira(Id);
        }

        public CarteiraDTO GetCarteiraAtivaByUsuario(long usuarioId)
        {
            Carteira carteira = carteiraRepository.GetCarteiraAtivaByUsuario(usuarioId);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Carteira, CarteiraDTO>());
            var mapper = config.CreateMapper();
            
            CarteiraDTO carteiraDto = mapper.Map<CarteiraDTO>(carteira);
            // carteiraDto.Status = STATUS[carteiraDto.StatusId];
            return carteiraDto;
        }

        public Carteira Salvar(CarteiraDTO carteiraDTO)
        {
            if (carteiraRepository.hasCarteiraAtiva(carteiraDTO.usuarioId))
                throw new DuplicateWaitObjectException("Já existe uma carteira aberta");
            
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CarteiraDTO, Carteira>());
            var mapper = config.CreateMapper();

            Usuario usuario = this.usuarioRepository.Get(carteiraDTO.usuarioId);
            Carteira carteira = mapper.Map<Carteira>(carteiraDTO);
            carteira.Renda = usuario.Renda;
            
            this.carteiraRepository.Add(carteira);

            return carteira;
        }
        
        public void finalizarCarteira(CarteiraDTO carteiraDto)
        {
            Carteira carteira = carteiraRepository.GetCarteira(carteiraDto.Id);
            if (carteira.StatusId != (int) STATUS.ABERTO)
                throw new VerificationException("Carteira não está aberta");
            carteira.StatusId = (int) STATUS.FINALIZADO;
            carteiraRepository.Update(carteira);
        }
        
        /*
         * @TODO
         * Aplicar regras de negócio para o cancelamento
         */
        public void cancelarCarteira(CarteiraDTO carteiraDto)
        {
            Carteira carteira = carteiraRepository.GetCarteira(carteiraDto.Id);
            if (carteira.StatusId == (int) STATUS.ABERTO)
                return;
            carteira.StatusId = (int) STATUS.CANCELADO;
            carteiraRepository.Update(carteira);
        }

        public Carteira reativarCarteira(CarteiraDTO carteiraDto)
        {
            Carteira ultimaCarteira = carteiraRepository.getUltimaCarteira();
            if (carteiraDto.Id == ultimaCarteira.Id && 
                (ultimaCarteira.StatusId == (int)STATUS.CANCELADO ||
                 ultimaCarteira.StatusId == (int)STATUS.FINALIZADO))
            {
                DateTime now = DateTime.Now;

                if (ultimaCarteira.CriadoEm.Month == now.Month &&
                    ultimaCarteira.CriadoEm.Year == now.Year)
                {
                    ultimaCarteira.StatusId = (int)STATUS.ABERTO;

                    return ultimaCarteira;
                }
                throw new ArgumentException(paramName: "Carteira", message: "A carteira precisa ser do mesmo mês e ano da data de reativação");
            }
            throw new ArgumentException(paramName: "Carteira", message: "A carteira enviada não foi a última criada ou não foi finalizada");
        }
    }
}
