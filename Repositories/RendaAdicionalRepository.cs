using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tequila.Core;
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
        
        public PagedResult<RendaAdicional> GetAll(QueryParams parameters, long usuarioId)
        {
           return _context.Set<RendaAdicional>().AsNoTracking().Where(e => e.UsuarioId == usuarioId && e.Ativo == 1).GetPaged(parameters);
        }
    }
}