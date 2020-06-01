using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tequila.Models;

namespace Tequila.Repositories.Interfaces
{
    public interface ICarteiraRepository
    {
        Carteira Salvar(Carteira Carteira);
        Carteira GetCarteira(long Id);
        Carteira GetCarteiraAtiva(long IdUsuario);
        IEnumerable<Carteira> GetAllCarteirasByUsuario(long IdUsuario);
    }
}
