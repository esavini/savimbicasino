using System.Text.Json.Serialization;
using SavimbiCasino.WebApi.Enums;

namespace SavimbiCasino.WebApi.Dtos
{
    public class PlayerGameDto
    {
        [JsonPropertyName("status")] public GameStatus GameStatus { get; set; }
        
        [JsonPropertyName("money")] public int Money { get; set; }
        
        [JsonPropertyName("canBet")] public bool CanBet { get; set; }
        
        [JsonPropertyName("canDivide")] public bool CanDivide { get; set; }
        
        [JsonPropertyName("canDouble")] public bool CanDouble { get; set; }
    }
}