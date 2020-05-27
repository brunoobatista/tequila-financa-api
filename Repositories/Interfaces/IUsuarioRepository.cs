using Tequila.Models;

namespace Tequila.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Usuario ValidarLoginUsuario(AuthenticationDTO authentication);
        Usuario getDetail(long Id);
        Usuario getById(long Id);
        Usuario salvar(Tequila.Models.DTOs.UsuarioDTO usuarioDto);

    }
}
