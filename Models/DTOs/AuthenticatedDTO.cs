namespace Tequila.Models
{
    public class AuthenticatedDTO
    {
        public long id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
        public string access_token { get; set; }

        public AuthenticatedDTO(Usuario usuario, string token)
        {
            id = usuario.Id;
            nome = usuario.Nome;
            email = usuario.Email;
            avatar = usuario.Avatar;
            access_token = token;
        }
    }
}
