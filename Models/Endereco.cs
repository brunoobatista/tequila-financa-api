using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tequila.Models
{
    [Table("endereco")]
    public class Endereco
    {

        [Column("id")]
        public long EnderecoId { get; set; }
        [Column("rua")]
        public string Rua { get; set; }
        [Column("cep")]
        public string Cep { get; set; }
        [Column("numero")]
        public string Numero { get; set; }
        [Column("complemento")]
        public string Complemento { get; set; }

        [Column("usuario_id")]
        public long UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

    }
}