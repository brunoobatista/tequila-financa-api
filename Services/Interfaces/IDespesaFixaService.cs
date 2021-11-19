using Tequila.Models;

namespace Tequila.Services.Interfaces
{
    public interface IDespesaFixaService
    {
        Despesa finalizarDespesaFixa(long idDespesa, decimal valor);
    }
}