using System;

namespace Tequila.Models.DTOs
{
    public class CarteiraDTO
    {
        public long Id { get; set; }
        public long usuarioId { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }
        public decimal Renda { get; set; }
        public decimal? RendaExtra { get; set; }
        public decimal Despesa { get; set; }
        public DateTime? CriadoEm { get; set; }
        public DateTime? AlteradoEm { get; set; }
    }
}