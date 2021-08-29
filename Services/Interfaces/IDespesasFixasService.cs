using Tequila.Models;
using Tequila.Models.DTOs;

namespace Tequila.Services.Interfaces
{
    public interface IDespesasFixasService
    {
        DespesasFixas salvar(DespesasFixasDTO despesasFixasDto);
        DespesasFixas atualizar(DespesasFixasDTO despesasFixasDto);
        
        DespesasFixas finalizar(long idDespesasFixas);
        
        void remover(long idDespesasFixas);
    }
}