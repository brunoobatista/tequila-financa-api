using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Tequila.Core;
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

        public PagedResult<DespesasFixas> getDespesasFixasByUsuario(long idUsuario, int page, int pageSize)
        {
            return _context.DespesasFixas
                .Where(d => d.UsuarioId == idUsuario && d.Ativo == 1)
                .AsNoTracking()
                .GetPaged<DespesasFixas>(page, pageSize);
        }

        public DespesasFixas criarDespesasFixas(DespesasFixasDTO despesasFixasDto)
        {
            Carteira carteira = _carteiraRepository.GetCarteiraAtivaByUsuario(despesasFixasDto.UsuarioId);
            if (carteira == null)
                throw new NullReferenceException("Não foi encontrado nenhuma carteira ativa!");

            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            conn.Open();
            using (var cmd = new NpgsqlCommand("CALL inserirdespesasfixas(@usuario_id, @carteira_id, @descri, @valor_prev, @data_venc, @tipo_id, @total_parc, @id_despesa)", conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("usuario_id", NpgsqlDbType.Bigint){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesasFixasDto.UsuarioId
                });
                cmd.Parameters.Add(new NpgsqlParameter("carteira_id", NpgsqlDbType.Bigint){
                    Direction = ParameterDirection.InputOutput,
                    Value = carteira.Id
                });
                cmd.Parameters.Add(new NpgsqlParameter("descri",NpgsqlDbType.Varchar){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesasFixasDto.Descricao
                });
                cmd.Parameters.Add(new NpgsqlParameter("valor_prev", NpgsqlDbType.Numeric){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesasFixasDto.ValorPrevisto
                });
                cmd.Parameters.Add(new NpgsqlParameter("data_venc", NpgsqlDbType.Timestamp){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesasFixasDto.DataVencimento
                });
                cmd.Parameters.Add(new NpgsqlParameter("tipo_id",NpgsqlDbType.Integer){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesasFixasDto.TipoId
                });
                cmd.Parameters.Add(new NpgsqlParameter("total_parc", NpgsqlDbType.Integer){
                    Direction = ParameterDirection.InputOutput,
                    Value = (despesasFixasDto.TotalParcelas != null) ? despesasFixasDto.TotalParcelas : (object)DBNull.Value,
                    IsNullable = true
                });
                cmd.Parameters.Add(new NpgsqlParameter("id_despesa", NpgsqlDbType.Bigint)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = despesasFixasDto.Id
                });
                cmd.ExecuteNonQuery();
                despesasFixasDto.Id = (long)cmd.Parameters[7].Value;
            }
            conn.Close();

            DespesasFixas despesasFixas = _context.DespesasFixas.Find(despesasFixasDto.Id);
            return despesasFixas;
        }
    }
}