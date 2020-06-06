using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Services;

namespace Tequila.Repositories
{
    public class UsuarioRepository : EFCoreRepository<Usuario, ApplicationContext>
    {

        private readonly ApplicationContext context;

        public UsuarioRepository(ApplicationContext ctx) : base(ctx)
        {
            context = ctx;
        }

        public Usuario atualizar(UsuarioDTO usuarioDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UsuarioDTO, Usuario>());
            var mapper = config.CreateMapper();
            
            Usuario usuario = mapper.Map<Usuario>(usuarioDto);

            try
            {
                if (usuarioDto.endereco != null)
                {
                    Endereco endereco = usuarioDto.endereco;
                    if (endereco.Id == null)
                        context.Endereco.Add(endereco);
                    else
                        context.Endereco.Update(endereco); 
                    context.SaveChanges();
                    usuario.EnderecoId = endereco.Id;
                }

                usuario.AlteradoEm = DateTime.UtcNow;
                context.Entry(usuario).State = EntityState.Modified;
                
                // Type type = typeof(Usuario);
                // PropertyInfo[] properties = type.GetProperties();
                // foreach (PropertyInfo property in properties)
                // {
                //     if (property.PropertyType == typeof(Endereco))
                //         continue;
                //     
                //     if (property.GetValue(usuario, null) == null)
                //     {
                //         context.Entry(usuario).Property(property.Name).IsModified = false;
                //     }
                // }
                context.SaveChanges();
                
                return usuario;
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Usuario salvar(UsuarioDTO usuarioDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UsuarioDTO, Usuario>());
            var mapper = config.CreateMapper();

            Usuario usuario = mapper.Map<Usuario>(usuarioDto);

            if (usuario.Senha == null)
                throw new ArgumentNullException(paramName: "Senha", message: "Senha náo pode estar vazia");
            
            try
            {
                // if (usuarioDto.endereco != null)
                // {
                //     Endereco endereco = usuarioDto.endereco;
                //     context.Endereco.Add(endereco);
                //     context.SaveChanges();
                //     usuario.EnderecoId = endereco.Id;
                // }

                usuario.Senha = HashService.GenerateHash(usuario.Senha);

                Add(usuario);

                return usuario;
            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Usuario getDetalhe(long id)
        {
            var usuario = context.Usuario
                            .Include(u => u.Endereco)
                            .AsNoTracking()
                            .SingleOrDefault(u => u.Ativo == 1 && u.Id == id);

            return usuario;
        }

        public void alterarSenha(long id, AlterarSenhaDTO alterarSenhaDto)
        {
            if (id == alterarSenhaDto.id)
            {
                Usuario usuario = Get(id);
                if (HashService.VerifyHash(alterarSenhaDto.senhaAtual, usuario.Senha))
                {
                    if (alterarSenhaDto.novaSenha != alterarSenhaDto.confirmacaoSenha)
                        throw new Exception("Nova senha e confirmação incorretas");
                    var novaSenha = HashService.GenerateHash(alterarSenhaDto.novaSenha);
                    usuario.Senha = novaSenha;
                    Update(usuario);
                }
                else
                    throw new Exception("Senha atual incorreta");
            }
        }

        public void inativar(long id)
        {
            Usuario usuario = Get(id);
            if (usuario != null)
            {
                usuario.Ativo = 0;
                Update(usuario);
            }
            else
                throw new Exception("Usuario não existe");
        }

        public Usuario ValidarLoginUsuario(AuthenticationDTO authentication)
        {
            var senhaHash = HashService.GenerateHash(authentication.Senha);
            return context.Usuario
                .AsNoTracking()
                .SingleOrDefault(e => e.Email == authentication.Email && e.Senha == senhaHash && e.Ativo == 1);
        }

    }
}
