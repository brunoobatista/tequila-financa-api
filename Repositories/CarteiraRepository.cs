using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Tequila.Models;
using Tequila.Models.Enum;

namespace Tequila.Repositories
{
    public class CarteiraRepository : EFCoreRepository<Carteira, ApplicationContext>
    {
        private readonly ApplicationContext _context;

        public CarteiraRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Carteira> GetAllCarteirasByUsuario(long IdUsuario)
        {
            return _context.Carteira
                        .AsNoTracking()
                        .Where(c => c.UsuarioId == IdUsuario && c.StatusId == (int) STATUS.ABERTO)
                        .ToList();

        }

        public bool hasCarteiraAtiva(long usuarioId)
        {
            return _context.Carteira
                .AsNoTracking()
                .Any(c => c.UsuarioId == usuarioId && c.StatusId == (int)STATUS.ABERTO);
        }
        
        public Carteira GetCarteira(long Id)
        { 
            return Get(Id);
        }

        public Carteira GetCarteiraAtivaByUsuario(long IdUsuario)
        {
            Carteira carteira = _context.Carteira
                                    // .Include(c => c.Status)
                                    .AsNoTracking()
                                    .Where(c => c.UsuarioId == IdUsuario && c.StatusId == (int) STATUS.ABERTO)
                                    .OrderByDescending(c => c.Id)
                                    .FirstOrDefault();
            return carteira;
        }

        public ICollection<Carteira> getCarteirasByUsuario(long usuarioId)
        {
            ICollection<Carteira> carteiras = _context.Carteira
                .AsNoTracking()
                .Where(c => c.UsuarioId == usuarioId && c.Ativo == 1)
                .OrderByDescending(c => c.CriadoEm)
                .ToList();
            return carteiras;
        }

        public Carteira getUltimaCarteira()
        {
            return _context.Carteira
                .AsNoTracking()
                .OrderByDescending(c => c.Id)
                .FirstOrDefault();
        }
    }
}
