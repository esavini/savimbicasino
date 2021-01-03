using System.Text.Json.Serialization;

namespace SavimbiCasino.WebApi.Dtos
{
    public class TokenDto
    {
        [JsonPropertyName("token")] public string Token { get; set; }
    }
}