namespace Tequila.Models.DTOs
{
    public class DespesaAvulsaDTO
    {
        public long? Id { get; set; }
        public long CarteiraId { get; set; }
        public int TipoId { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }
}