namespace Tequila.Models.DTOs
{
    public class DespesaVariavelDTO
    {
        public long Id { get; set; }
        public long CarteiraId { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }
}