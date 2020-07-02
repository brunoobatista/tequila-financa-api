using System.Security;
using AutoMapper;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Repositories;
using Tequila.Services.Interfaces;

namespace Tequila.Services
{
    public class RendaAdicionalService : IRendaAdicionalService
    {
        private readonly RendaAdicionalRepository _rendaAdicionalRepository;
        private readonly CarteiraRepository _carteiraRepository;
        public RendaAdicionalService(
            RendaAdicionalRepository rendaAdicionalRepository,
            CarteiraRepository carteiraRepository
            )
        {
            _rendaAdicionalRepository = rendaAdicionalRepository;
            _carteiraRepository = carteiraRepository;
        }

        public void remover(long id)
        {
            // Carteira carteira = _carteiraRepository.Get(rendaAdicionalDto.CarteiraId);
            RendaAdicional rendaAdicional = _rendaAdicionalRepository.Get(id);
            if (/*carteira != null &&*/ rendaAdicional != null)
            {
                // carteira.RendaExtra -= rendaAdicional.Valor;
                rendaAdicional.Ativo = 0;
                
                // _carteiraRepository.Update(carteira);
                _rendaAdicionalRepository.Update(rendaAdicional);
            }
        }

        /*
         * @TODO
         * Aplicar as regras
         * a. Ao salvar nova, desenvolver trigger que altera valor da rendaextra na carteira
         * b. Ao update, efetuar alteracao do valor na carteira tbm caso seja diferente
         * c. Ao remover(update do campo ativo para 0), remover o valor da rendaextra na carteira
         */
        public RendaAdicional nova(RendaAdicionalDTO rendaAdicionalDto)
        {
            RendaAdicional rendaAdicional = mapper(rendaAdicionalDto);
            rendaAdicional = _rendaAdicionalRepository.Add(rendaAdicional);
            return rendaAdicional;
        }

        public RendaAdicional editar(RendaAdicionalDTO rendaAdicionalDto)
        {
            RendaAdicional renda = _rendaAdicionalRepository.Get(rendaAdicionalDto.Id);
            if (rendaAdicionalDto.UsuarioId != renda.UsuarioId || rendaAdicionalDto.CarteiraId != renda.CarteiraId)
                throw new VerificationException("Carteira/Usuário não podem sem modificados");
            RendaAdicional rendaAdicional = mapper(rendaAdicionalDto);
            rendaAdicional = _rendaAdicionalRepository.Update(rendaAdicional);
            return rendaAdicional;
        }
        
        private RendaAdicional mapper(RendaAdicionalDTO rendaAdicionalDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RendaAdicionalDTO, RendaAdicional>());
            var map = config.CreateMapper();

            return map.Map<RendaAdicional>(rendaAdicionalDto);
        }
    }
}