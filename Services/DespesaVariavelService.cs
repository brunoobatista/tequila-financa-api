using System.Security;
using AutoMapper;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Repositories;
using Tequila.Services.Interfaces;

namespace Tequila.Services
{
    public class DespesaVariavelService : IDespesaVariavelService
    {
        private DespesaVariavelRepository _despesaVariavelRepository;
        public DespesaVariavelService(DespesaVariavelRepository despesaVariavelRepository)
        {
            _despesaVariavelRepository = despesaVariavelRepository;
        }

        public DespesaVariavel salvar(DespesaVariavelDTO despesaVariavelDto)
        {
            DespesaVariavel despesaVariavel = mapper(despesaVariavelDto);
            despesaVariavel = _despesaVariavelRepository.Add(despesaVariavel);
            return despesaVariavel;
        }

        public DespesaVariavel atualizar(DespesaVariavelDTO despesaVariavelDto)
        {
            DespesaVariavel dv = _despesaVariavelRepository.Get(despesaVariavelDto.Id);
            if (despesaVariavelDto.CarteiraId != dv.CarteiraId)
                throw new VerificationException("Carteira/Usuário não podem sem modificados");
            DespesaVariavel despesaVariavel = mapper(despesaVariavelDto);
            despesaVariavel = _despesaVariavelRepository.Update(despesaVariavel);
            return despesaVariavel;
        }

        /*
         * @TODO
         * Aplicar as regras
         * a. Ao salvar nova, desenvolver trigger que altera valor da despesa na carteira
         * b. Ao update, efetuar alteracao do valor na carteira tbm caso seja diferente
         * c. Ao remover(update do campo ativo para 0), remover o valor da despesa na carteira
         */
        public void remover(long idDespesaVariavel)
        {
            DespesaVariavel despesaVariavel = _despesaVariavelRepository.Get(idDespesaVariavel);
            if (despesaVariavel != null)
            {
                despesaVariavel.Ativo = 0;
                _despesaVariavelRepository.Update(despesaVariavel);
            }
        }
        
        private DespesaVariavel mapper(DespesaVariavelDTO despesaVariavelDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DespesaVariavelDTO, DespesaVariavel>());
            var map = config.CreateMapper();

            return map.Map<DespesaVariavel>(despesaVariavelDto);
        }
    }
}