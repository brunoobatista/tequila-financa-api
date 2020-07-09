namespace Tequila.Services.Interfaces
{
    public interface IDespesaFixaService
    {
        bool finalizarDespesa(long idDespesa, decimal valor);
    }
}