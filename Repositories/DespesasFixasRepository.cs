﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Tequila.Core;
using Tequila.Models;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;

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

        public PagedResult<DespesasFixas> getDespesasFixasByUsuario(long idUsuario, QueryParams parameters)
        {
            return _context.DespesasFixas
                .Where(d => d.UsuarioId == idUsuario && d.Ativo == 1 && d.StatusId == (int)STATUS.ABERTO)
                .AsNoTracking()
                .GetPaged(parameters);
        }
        
        public PagedResult<DespesasFixas> getDespesasFixasHistorico(long idUsuario, QueryParams parameters)
        {
            return _context.DespesasFixas
                .Where(d => d.UsuarioId == idUsuario && d.Ativo == 1)
                .AsNoTracking()
                .GetPaged(parameters);
        }

        public DespesasFixas criarDespesasFixas(DespesasFixasDTO despesasFixasDto)
        {
            Carteira carteira = _carteiraRepository.GetCarteiraAtivaByUsuario(despesasFixasDto.UsuarioId);
            if (carteira == null)
                throw new NullReferenceException("Não foi encontrado nenhuma carteira ativa!");

            var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            conn.Open();
            using (var cmd = new NpgsqlCommand("CALL inserirdespesasfixas(@user_id, @cart_id, @descri, @valor_final, @data_venc, @tipo_id, @total_parc, @id_despesa)", conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Bigint){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesasFixasDto.UsuarioId
                });
                cmd.Parameters.Add(new NpgsqlParameter("cart_id", NpgsqlDbType.Bigint){
                    Direction = ParameterDirection.InputOutput,
                    Value = carteira.Id
                });
                cmd.Parameters.Add(new NpgsqlParameter("descri",NpgsqlDbType.Varchar){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesasFixasDto.Descricao
                });
                cmd.Parameters.Add(new NpgsqlParameter("valor_final", NpgsqlDbType.Numeric){
                    Direction = ParameterDirection.InputOutput,
                    Value = despesasFixasDto.Valor
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
                    Value = (despesasFixasDto.Id != null) ? despesasFixasDto.Id : (object)DBNull.Value,
                    IsNullable = true
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