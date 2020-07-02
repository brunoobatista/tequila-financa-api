using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Tequila.Models;
using Tequila.Models.DTOs;

namespace Tequila.Repositories
{
    public class DespesasFixasRepository : EFCoreRepository<DespesasFixas, ApplicationContext>
    {
        private readonly ApplicationContext _context;
        private readonly CarteiraRepository _carteiraRepository;
        public DespesasFixasRepository(ApplicationContext context, CarteiraRepository carteiraRepository) : base(context)
        {
            _context = context;
            _carteiraRepository = carteiraRepository;
        }

        public List<DespesasFixas> getDespesasFixasByUsuario(long idUsuario)
        {
            return _context.DespesasFixas
                .Where(d => d.UsuarioId == idUsuario && d.Ativo == 1)
                .ToList();
        }

        public DespesasFixas criarDespesasFixas(DespesasFixasDTO despesasFixasDto)
        {
            long idDespesa = -1;

            Carteira carteira = _carteiraRepository.GetCarteiraAtivaByUsuario(despesasFixasDto.UsuarioId);
            
            NpgsqlConnection con = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString);
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("usuario_id", despesasFixasDto.UsuarioId));
            cmd.Parameters.Add(new NpgsqlParameter("carteira_id", carteira.Id));
            cmd.Parameters.Add(new NpgsqlParameter("descri", despesasFixasDto.Descricao));
            cmd.Parameters.Add(new NpgsqlParameter("valor_prev", despesasFixasDto.ValorPrevisto));
            cmd.Parameters.Add(new NpgsqlParameter("data_venc", despesasFixasDto.DataVencimento));
            cmd.Parameters.Add(new NpgsqlParameter("tipo_id", despesasFixasDto.TipoId));
            cmd.Parameters.Add(new NpgsqlParameter("total_parc", despesasFixasDto.TotalParcelas));
            cmd.Parameters.Add(new NpgsqlParameter("id", idDespesa));

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();
            return null;
        }
    }
}