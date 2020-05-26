using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tequila.Models;

namespace Tequila.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Usuario ValidarLoginUsuario(AuthenticationDTO authentication);
        dynamic GetById(long Id);
    }
}
