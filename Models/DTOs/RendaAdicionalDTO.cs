using System;

namespace Tequila.Models.DTOs
{
    public class RendaAdicionalDTO
    {
        public long Id { get; set; }
        
        public long UsuarioId { get; set; }
        public long CarteiraId { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }
}