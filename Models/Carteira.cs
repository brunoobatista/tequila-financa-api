using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tequila.Models.Interfaces;

namespace Tequila.Models
{
    [Table("carteira")]
    public class Carteira : IEntity
    {
        [Key, Column("id")]
        public long Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("status_id")]
        public int StatusId { get; set; }
        
        [ForeignKey("StatusId")]
        public Status Status { get; set; }

        [Required(ErrorMessage = "Necessário informar usuário"), Column("usuario_id")]
        public long UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

        [Column("renda")]
        [Required(ErrorMessage = "Necessário informar renda inicial")]
        public decimal Renda { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("renda_extra")]
        public decimal? RendaExtra { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("despesa")]
        public decimal Despesa { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("criado_em")]
        public DateTime? CriadoEm { get; set; }

        [Column("alterado_em")] public DateTime? AlteradoEm { get; set; }

        [Column("finalizado_em")]
        public DateTime? FinalizadoEm { get; set; }

        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ativo")]
        public int? Ativo { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("can_reativar")]
        public bool? CanReativar { get; set; }
    }
}