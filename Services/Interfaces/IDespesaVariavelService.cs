using Tequila.Models;
using Tequila.Models.DTOs;

namespace Tequila.Services.Interfaces
{
    public interface IDespesaVariavelService
    {
        DespesaVariavel salvar(DespesaVariavelDTO despesaVariavelDto);
        DespesaVariavel atualizar(DespesaVariavelDTO despesaVariavelDto);
        void remover(long idDespesaVariavel);
    }
}