using Tequila.Models;
using Tequila.Models.DTOs;

namespace Tequila.Services.Interfaces
{
    public interface IRendaAdicionalService
    {
        void remover(long id);
        RendaAdicional nova(RendaAdicionalDTO rendaAdicionalDto);
        RendaAdicional editar(RendaAdicionalDTO rendaAdicionalDto);
    }
}