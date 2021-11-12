using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tequila.Models.Interfaces;

namespace Tequila.Models
{
    public class SituacaoDespesa : IBase
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string nome { get; set; }
    }
}