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
    }
}