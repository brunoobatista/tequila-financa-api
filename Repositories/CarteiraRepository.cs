using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using Tequila.Models;

namespace Tequila.Repositories
{
    public class CarteiraRepository : EFCoreRepository<Carteira, ApplicationContext>
    {
        private readonly ApplicationContext _context;

        public CarteiraRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable GetAllCarteirasByUsuario(long IdUsuario)
        {
            throw new NotImplementedException();
        }

        public Carteira GetCarteira(long Id)
        {
            return _context.Carteira.Find(Id);
        }

        public Carteira GetCarteiraAtiva(long IdUsuario)
        {
            throw new NotImplementedException();
        }

        public Carteira Salvar(Carteira carteira)
        {
            Add(carteira);
            return carteira;
        }
    }
}
