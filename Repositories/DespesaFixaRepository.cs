﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;

namespace Tequila.Repositories
{
    public class DespesaFixaRepository : EFCoreRepository<DespesaFixa, ApplicationContext>
    {
        private readonly ApplicationContext _context;
        
        public DespesaFixaRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public List<DespesaFixa> getListaCarteiraAtiva(long carteiraId)
        {
            return _context.DespesaFixa
                .AsNoTracking()
                .Where(d => d.CarteiraId == carteiraId)
                .ToList();
        }

        public List<DespesaFixa> getDespesaFixaContinuaPorCarteira(long idCarteira)
        {
            return _context.DespesaFixa.AsNoTracking()
                .Where(e => e.CarteiraId == idCarteira && e.TipoId == (int)TIPO.CONTINUO)
                .ToList();
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