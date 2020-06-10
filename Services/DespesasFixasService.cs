using System.Security;
using AutoMapper;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;
using Tequila.Repositories;
using Tequila.Services.Interfaces;

namespace Tequila.Services
{
    public class DespesasFixasService : IDespesasFixasService
    {
        private readonly DespesasFixasRepository despesasFixasRepository;

        public DespesasFixasService(DespesasFixasRepository despesasFixasRepository)
        {
            this.despesasFixasRepository = despesasFixasRepository;
        }
        
        public DespesasFixas salvar(DespesasFixasDTO despesasFixasDto)
        {
            DespesasFixas despesasFixas = mapper(despesasFixasDto);

            if (despesasFixas.TotalParcelas != null)
            {
                despesasFixas.ParcelaAtual = 1;
            }

            despesasFixasRepository.Add(despesasFixas);
            return despesasFixas;
        }

        public DespesasFixas atualizar(DespesasFixasDTO despesasFixasDto)
        {
            DespesasFixas despesasFixas = mapper(despesasFixasDto);

            return despesasFixasRepository.Update(despesasFixas);
        }
        
        public DespesasFixas finalizar(DespesasFixasDTO despesasFixasDto)
        {
            DespesasFixas despesasFixas = despesasFixasRepository.Get(despesasFixasDto.Id);
            if (despesasFixas.StatusId != (int) STATUS.ABERTO)
                throw new VerificationException("Despesa Fixa não está aberta");
            despesasFixas.StatusId = (int) STATUS.FINALIZADO;
            despesasFixasRepository.Update(despesasFixas);
            return despesasFixas;
        }
        

        /*
         * @TODO
         * Aplicar regra de só remover quando as despesas fixas não tiverem sido usadas
         */
        public void remover(long idDespesasFixas)
        {
            despesasFixasRepository.Inactive(idDespesasFixas);
        }

        private DespesasFixas mapper(DespesasFixasDTO despesasFixasDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DespesasFixasDTO, DespesasFixas>());
            var map = config.CreateMapper();

            return map.Map<DespesasFixas>(despesasFixasDto);
        }
    }
}