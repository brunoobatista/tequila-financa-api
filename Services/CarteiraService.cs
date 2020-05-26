using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tequila.Models;
using Tequila.Models.Enum;
using Tequila.Repositories.Interfaces;
using Tequila.Services.Interfaces;

namespace Tequila.Services
{
    public class CarteiraService : ICarteiraService
    {
        private readonly ICarteiraRepository carteiraRepository;
        private readonly IUsuarioRepository usuarioRepository;

        public CarteiraService(ICarteiraRepository carteiraRepository, IUsuarioRepository usuarioRepository)
        {
            this.carteiraRepository = carteiraRepository;
            this.usuarioRepository = usuarioRepository;
        }

        public Carteira GetById(long Id)
        {
            return this.carteiraRepository.GetCarteira(Id);
        }

        public Carteira Salvar(Carteira carteira)
        {
            Usuario usuario = this.usuarioRepository.GetById(carteira.UsuarioId);
            carteira.Usuario = usuario;
            carteira.Renda = usuario.Renda;
            carteira.StatusId = (int)STATUS.ABERTO;

            this.carteiraRepository.Salvar(carteira);
            //Carteira carteiraSalva = this.carteiraRepository.GetCarteira(carteira.Id)   ;

            return carteira;
        }
    }
}
