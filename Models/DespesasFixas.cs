using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using Tequila.Models.Interfaces;

namespace Tequila.Models
{
    [Table("despesasfixas")]
    public class DespesasFixas : IEntity
    {
        [Key, Column("id")]
        public long Id { get; set; }

        [Required, Column("usuario_id")]
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("status_id")]
        public int? StatusId { get; set; }
        public Status Status { get; set; }
        
        [Required, Column("tipo_id")]
        public int TipoId { get; set; }
        
        public ICollection<DespesaFixa> ListaDespesasFixas { get; set; }

        [Required, Column("descricao")]
        public string Descricao { get; set; }
        
        [Column("valor_previsto")]
        public decimal? ValorPrevisto { get; set; }
        
        [Column("parcela_atual")]
        public int? ParcelaAtual { get; set; }
        
        [Column("total_parcelas")]
        public int? TotalParcelas { get; set; }
        
        [Column("data_vencimento")]
        public DateTime? DataVencimento { get; set; }

        [JsonIgnore]
        [Column("ativo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Ativo { get; set; }
        
        [Column("alterado_em")]
        public DateTime? AlteradoEm { get; set; }
        
        [Column("criado_em")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CriadoEm { get; set; }
    }
}