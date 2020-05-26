
using Tequila.Models;

namespace Tequila.Services.Interfaces
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
