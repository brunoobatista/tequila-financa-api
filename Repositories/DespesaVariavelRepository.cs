using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tequila.Models;

namespace Tequila.Repositories
{
    public class DespesaVariavelRepository : EFCoreRepository<DespesaVariavel, ApplicationContext>
    {
        private ApplicationContext _context;
        
        public DespesaVariavelRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        
        public List<DespesaVariavel> getListaCarteiraAtiva(long carteiraId)
        {
            return _context.DespesaVariavel
                .AsNoTracking()
                .Where(v => v.CarteiraId == carteiraId)
                .ToList();
        }
    }
}