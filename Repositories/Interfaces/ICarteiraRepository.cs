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
        Carteira GetCarteiraLazy(long id);
        Carteira GetCarteiraAtiva(long IdUsuario);
        ICollection<Carteira> GetAllCarteirasByUsuario(long IdUsuario);
    }
}
