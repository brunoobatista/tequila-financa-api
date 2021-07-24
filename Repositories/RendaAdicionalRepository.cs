using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tequila.Models;

namespace Tequila.Repositories
{
    public class RendaAdicionalRepository : EFCoreRepository<RendaAdicional, ApplicationContext>
    {
        private ApplicationContext _context;
        public RendaAdicionalRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        
        public List<RendaAdicional> GetAll(long usuarioId)
        {
            var query = _context.Set<RendaAdicional>().AsQueryable();

            return query.Where(e => e.UsuarioId == usuarioId && e.Ativo == 1).AsNoTracking().ToList();
        }
    }
}