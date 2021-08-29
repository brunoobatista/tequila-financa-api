using Tequila.Models;
using Tequila.Models.DTOs;

namespace Tequila.Services.Interfaces
{
    public interface ICarteiraService
    {
        Carteira Salvar(CarteiraDTO Carteira);
        Carteira GetById(long Id);

        CarteiraDTO GetCarteiraAtivaByUsuario(long usuarioId);
        Carteira finalizarCarteira(CarteiraDTO carteiraDto);

        void cancelarCarteira(long userId,CarteiraDTO carteiraDto);

        Carteira reativarCarteira(CarteiraDTO carteiraDto);
    }
}
