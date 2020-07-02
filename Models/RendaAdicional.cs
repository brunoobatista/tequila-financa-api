using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Tequila.Models.Interfaces;

namespace Tequila.Models
{
    [Table("rendaadicional")]
    public class RendaAdicional : IEntity
    {
        [Key, Column("id")]
        public long Id { get; set; }
        
        [Required(ErrorMessage = "Necessário informar o usuário"), Column("usuario_id")]
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        
        [Required(ErrorMessage = "Necessário informar a carteira"), Column("carteira_id")]
        public long CarteiraId { get; set; }
        public Carteira Carteira { get; set; }

        [Required(ErrorMessage = "Necessário descrisão"), Column("descricao")]
        public string Descricao { get; set; }
        
        [Required(ErrorMessage = "Necessário valor do acréscimo"), Column("valor")]
        public decimal Valor { get; set; }

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