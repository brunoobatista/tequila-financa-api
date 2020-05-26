using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
