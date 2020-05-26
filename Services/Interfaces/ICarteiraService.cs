using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tequila.Models;

namespace Tequila.Services.Interfaces
{
    public interface ICarteiraService
    {
        Carteira Salvar(Carteira Carteira);
        Carteira GetById(long Id);
    }
}
