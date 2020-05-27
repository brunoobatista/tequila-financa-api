using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Tequila.Models;
using Tequila.Models.DTOs;
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

        public Usuario salvar(UsuarioDTO usuarioDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UsuarioDTO, Usuario>());
            var mapper = config.CreateMapper();

            Usuario usuario = mapper.Map<Usuario>(usuarioDto);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (usuarioDto.endereco != null)
                    {
                        Endereco endereco = usuarioDto.endereco;
                        context.Endereco.Add(endereco);
                        context.SaveChanges();
                        usuario.EnderecoId = endereco.Id;
                    }

                    usuario.Senha = HashService.GenerateHash(usuario.Senha);

                    context.Usuario.Add(usuario);
                    context.SaveChanges();


                    transaction.Commit();

                    return usuario;
                } catch(Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public Usuario getById(long id)
        {
            return context.Usuario.Where(u => u.Ativo == 1 && u.Id == id).SingleOrDefault();
        }

        public Usuario getDetail(long id)
        {
            var usuario = context.Usuario.Include(u => u.Endereco)
                    .AsNoTracking()
                    .Where(u => u.Ativo == 1 && u.Id == id)
                    .SingleOrDefault();

            return usuario;

        }

        public Usuario ValidarLoginUsuario(AuthenticationDTO authentication)
        {
            var senhaHash = HashService.GenerateHash(authentication.Senha);
            return context.Usuario
                .AsNoTracking()
                .Where(e => e.Email == authentication.Email && e.Senha == senhaHash && e.Ativo == 1)
                .FirstOrDefault();
        }

    }
}
