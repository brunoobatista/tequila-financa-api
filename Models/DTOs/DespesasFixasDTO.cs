using System;

namespace Tequila.Models.DTOs
{
    public class DespesasFixasDTO
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public int TipoId { get; set; }
        
        public int? StatusId { get; set; }
        
        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public int? ParcelaAtual { get; set; }
        
        public int? TotalParcelas { get; set; }
        
        public DateTime? DataVencimento { get; set; }
    }
}