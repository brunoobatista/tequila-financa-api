using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tequila.Models;
using Tequila.Repositories.Interfaces;
using Tequila.Services;

namespace Tequila.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly ApplicationContext context;

        public UsuarioRepository(ApplicationContext ctx)
        {
            context = ctx;
        }

        public dynamic GetById(long Id)
        {

            //var data = (from s in _context.Usuario
            //            join e
            //            where s.Id == Id
            //            select s).FirstOrDefault();

            //var usuario = context.Usuario
            //        .Join(
            //                context.Endereco,
            //                usuario => usuario.Id,
            //                endereco => endereco.UsuarioId,
            //                (usuario, endereco) => new
            //                {
            //                    usuario.Endereco(endereco)
            //                }
            //            ).Where(u => u.usuario.Id == Id).FirstOrDefault();


            var usuario = context.Usuario.Include(u => u.Endereco).FirstOrDefault(u => u.Id == Id);

            return usuario;

        }

        public Usuario ValidarLoginUsuario(AuthenticationDTO authentication)
        {
            var senhaHash = HashService.GenerateHash(authentication.Senha);
            return context.Usuario.Where(e => e.Email == authentication.Email && e.Senha == senhaHash && e.Ativo == 1).FirstOrDefault();
        }

    }
}
