using System;

namespace Tequila.Models.Interfaces
{
    public interface IEntity
    {
        long Id { get; set; }
        int Ativo { get; set; }

        public DateTime? AlteradoEm { get; set; }
    }
}