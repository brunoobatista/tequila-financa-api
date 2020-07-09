using Tequila.Models.DTOs;
using Tequila.Repositories;
using Tequila.Services.Interfaces;

namespace Tequila.Services
{
    public class DespesaFixaService : IDespesaFixaService
    {
        private readonly DespesaFixaRepository _despesaFixaRepository;
        public DespesaFixaService(DespesaFixaRepository despesaFixaRepository)
        {
            _despesaFixaRepository = despesaFixaRepository;
        }

        public bool finalizarDespesa(long idDespesa, decimal valor)
        {
            return _despesaFixaRepository.finalizarDespesa(new DespesaFixaDTO() {Id = idDespesa, Valor = valor});
        }
    }
}