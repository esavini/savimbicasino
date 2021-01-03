using System.Text.Json.Serialization;

namespace SavimbiCasino.WebApi.Dtos
{
    public class CredentialsDto
    {
        [JsonPropertyName("username")] public string Username { get; set; }
        
        [JsonPropertyName("password")] public string Password { get; set; }
    }
}