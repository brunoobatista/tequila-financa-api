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
    }
}