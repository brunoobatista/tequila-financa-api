using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Tequila.Core;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;

namespace Tequila.Repositories
{
    public class DespesaRepository : EFCoreRepository<Despesa, ApplicationContext>
    {
        private readonly ApplicationContext _context;
        
        public DespesaRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public int hasDespesasOnCarteira(long userId, long carteiraId)
        {
            return _context.Despesa
                .AsNoTracking()
                .Count(d => d.UsuarioId == userId && d.CarteiraId == carteiraId && d.Ativo == 1);
        }

        public PagedResult<Despesa> getDespesaAll(QueryParams parameters, long userId, string tipos, long? cartId)
        {
            IQueryable<Despesa> query = _context.Despesa.AsNoTracking().Where(d => d.UsuarioId == userId);

            if (tipos != null)
            {
                Collection<int> tiposD = new Collection<int>();
                foreach (var tipo in tipos.Split(','))
                {
                    tiposD.Add(Int16.Parse(tipo));
                }

                query = query.Where(d => tiposD.Contains(d.TipoId));
            }

            if (cartId != null)
            {
                query = query.Where(d => d.CarteiraId == cartId);
            }

            return query.GetPaged(parameters: parameters);
        }

        public PagedResult<Despesa> getListaCarteiraAtiva(QueryParams parameters, long userId, long carteiraId, int? tipo)
        {
            IQueryable<Despesa> query = _context.Despesa.AsNoTracking();
            
            PagedResult<Despesa> despesas;
            if (tipo == null || tipo == (int)TIPO.TODOS)
            {
                despesas = query
                    .Where(d => d.UsuarioId == userId && d.CarteiraId == carteiraId && d.Ativo == 1)
                    .OrderBy(d => d.Id)
                    .GetPaged(parameters);
            }
            else
            {
                despesas = query
                    .Where(
                        d => d.UsuarioId == userId && 
                             d.CarteiraId == carteiraId && 
                             d.Ativo == 1 && 
                             d.TipoId == tipo
                             )
                    .GetPaged(parameters);
            }
            
            return despesas;
        }

        public Despesa getDespesaVariavelByUsuario(long userId, long despesaId)
        {
            return _context.Despesa
                .AsNoTracking()
                .SingleOrDefault(
                    d => d.UsuarioId == userId && 
                         d.Id == despesaId && 
                         d.TipoId == (int)TIPO.VARIAVEL && 
                         d.Ativo == 1
                    );
        }

        public List<Despesa> getDespesaContinuaPorCarteira(long carteiraId)
        {
            return _context.Despesa.AsNoTracking()
                .Where(e => e.CarteiraId == carteiraId && e.TipoId == (int)TIPO.CONTINUO)
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