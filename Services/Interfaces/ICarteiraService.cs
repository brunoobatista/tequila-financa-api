using Tequila.Models;
using Tequila.Models.DTOs;

namespace Tequila.Services.Interfaces
{
    public interface ICarteiraService
    {
        Carteira Salvar(CarteiraDTO Carteira);
        Carteira GetById(long Id);

        CarteiraDTO GetCarteiraAtivaByUsuario(long usuarioId);
        void finalizarCarteira(CarteiraDTO carteiraDto);

        void cancelarCarteira(CarteiraDTO carteiraDto);

        Carteira reativarCarteira(CarteiraDTO carteiraDto);
    }
}
