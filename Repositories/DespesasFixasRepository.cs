using System.Collections.Generic;
using System.Linq;
using Tequila.Models;

namespace Tequila.Repositories
{
    public class DespesasFixasRepository : EFCoreRepository<DespesasFixas, ApplicationContext>
    {
        private readonly ApplicationContext _context;

        public DespesasFixasRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public List<DespesasFixas> getDespesasFixasByUsuario(long idUsuario)
        {
            return _context.DespesasFixas
                .Where(d => d.UsuarioId == idUsuario && d.Ativo == 1)
                .ToList();
        }
    }
}