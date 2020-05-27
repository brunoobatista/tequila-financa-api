using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tequila.Models;
using Tequila.Repositories.Interfaces;

namespace Tequila.Repositories
{
    public class CarteiraRepository : ICarteiraRepository
    {
        private readonly ApplicationContext _context;

        public CarteiraRepository(ApplicationContext context)
        {
            _context = context;
        }

        public ICollection<Carteira> GetAllCarteirasByUsuario(long IdUsuario)
        {
            throw new NotImplementedException();
        }

        public Carteira GetCarteira(long Id)
        {
            return _context.Carteira.Find(Id);
        }

        public Carteira GetCarteiraLazy(long Id)
        {


            return _context.Carteira.Include("Usuario").Where(c => c.Id == Id).FirstOrDefault();
        }

        public Carteira GetCarteiraAtiva(long IdUsuario)
        {
            throw new NotImplementedException();
        }

        public Carteira Salvar(Carteira carteira)
        {
            _context.Carteira.Add(carteira);
            _context.SaveChanges();

            return carteira;
        }
    }
}
