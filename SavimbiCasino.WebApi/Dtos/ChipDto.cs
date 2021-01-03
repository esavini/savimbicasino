using System.Text.Json.Serialization;

namespace SavimbiCasino.WebApi.Dtos
{
    public class ChipDto
    {
        [JsonPropertyName("value")] public int Value { get; set; }
    }
}