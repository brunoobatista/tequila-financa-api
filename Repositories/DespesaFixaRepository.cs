using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Tequila.Models;
using Tequila.Models.DTOs;

namespace Tequila.Repositories
{
    public class DespesaFixaRepository : EFCoreRepository<DespesaVariavel, ApplicationContext>
    {
        private readonly ApplicationContext _context;
        
        public DespesaFixaRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public bool finalizarDespesa(DespesaFixaDTO despesaFixaDto)
        {
            int result = 0;
            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            conn.Open();
            using (var cmd = new NpgsqlCommand("CALL finalizardespesacontinua(@id_despesa, @valor_final, @result)", conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("id_despesa", NpgsqlDbType.Bigint){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesaFixaDto.Id
                });
                cmd.Parameters.Add(new NpgsqlParameter("valor_final", NpgsqlDbType.Numeric){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesaFixaDto.Valor
                });
                
                cmd.Parameters.Add(new NpgsqlParameter("result", NpgsqlDbType.Integer){
                    Direction = ParameterDirection.InputOutput,
                    Value = 0
                });

                cmd.ExecuteNonQuery();
                result = (int) cmd.Parameters[2].Value;
            }
            conn.Close();
            return result > 0;
        }
    }
}