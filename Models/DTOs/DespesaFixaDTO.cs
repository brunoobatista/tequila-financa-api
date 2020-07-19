using System;

namespace Tequila.Models.DTOs
{
    public class DespesaFixaDTO
    {
        public long Id { get; set; }
        public long CarteiraId { get; set; }
        public long DespesasFixasId { get; set; }
        public int TipoId { get; set; }
        public string Descricao { get; set; }

        public decimal? Valor { get; set; }

        public decimal? ValorPrevisto { get; set; }

        public int? ParcelaAtual { get; set; }
        
        public int? TotalParcelas { get; set; }
        
        public DateTime? DataVencimento { get; set; }
    }
}