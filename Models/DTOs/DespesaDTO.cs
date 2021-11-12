using System;

namespace Tequila.Models.DTOs
{
    public class DespesaDTO
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public long? CarteiraId { get; set; }
        public long? UsuarioId { get; set; }
        public long? DespesasFixasId { get; set; }
        public int? TipoId { get; set; }
        public int? SituacaoDespesaId { get; set; }

        public decimal Valor { get; set; }

        public int? ParcelaAtual { get; set; }
        
        public int? TotalParcelas { get; set; }
        
        public DateTime? DataVencimento { get; set; }
    }
}