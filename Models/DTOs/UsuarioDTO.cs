using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tequila.Models.DTOs
{
    public class UsuarioDTO
    {
        public virtual Endereco endereco { get; set; }

        public string email { get; set; }

        public string nome { get; set; }

        public string senha { get; set; }

        public string? avatar { get; set; }

        public string cpfCnpj { get; set; }

        public decimal renda { get; set; }

        public string tipoRenda { get; set; }

    }
}
