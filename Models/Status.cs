using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tequila.Models
{
    [Table("status")]
    public class Status
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string nome { get; set; }
    }
}
