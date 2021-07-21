using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tequila.Models.Interfaces;

namespace Tequila.Models
{
    [Table("endereco")]
    public class Endereco : IBase
    {

        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? Id { get; set; }
        [Column("rua")]
        public string Rua { get; set; }
        [Column("cep")]
        public string Cep { get; set; }
        [Column("numero")]
        public string Numero { get; set; }
        [Column("complemento")]
        public string Complemento { get; set; }

    }
}