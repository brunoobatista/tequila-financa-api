using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tequila.Models
{
    [Table("carteira")]
    public class Carteira
    {
        [Key, Column("id")]
        public long Id { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }

        public Status Status { get; set; }

        [Required, Column("usuario_id")]
        public long UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

        [Column("renda")]
        [Required]
        public decimal Renda { get; set; }

        [Column("renda_extra")]
        public decimal? RendaExtra { get; set; }

        [Column("despesa")]
        public decimal? Despesa { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("criado_em")]
        public DateTime? CriadoEm { get; set; }

        [Column("alterado_em")]
        public DateTime? AlteradoEm { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        [Column("ativo")]
        public int Ativo { get; set; }
    }
}