using Tequila.Models;
using Tequila.Models.DTOs;

namespace Tequila.Services.Interfaces
{
    public interface IDespesasFixasService
    {
        DespesasFixas salvar(DespesasFixasDTO despesasFixasDto);
        DespesasFixas atualizar(DespesasFixasDTO despesasFixasDto);
        
        DespesasFixas finalizar(DespesasFixasDTO despesasFixasDto);
        
        void remover(long idDespesasFixas);
    }
}