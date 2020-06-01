namespace Tequila.Models.DTOs
{
    public class AlterarSenhaDTO
    {
        public long id { get; set; }
        public string senhaAtual { get; set; }
        public string novaSenha { get; set; }
        public string confirmacaoSenha { get; set; }

    }
}