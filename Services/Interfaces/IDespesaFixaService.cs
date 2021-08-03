namespace Tequila.Services.Interfaces
{
    public interface IDespesaFixaService
    {
        bool finalizarDespesaFixa(long idDespesa, decimal valor);
    }
}