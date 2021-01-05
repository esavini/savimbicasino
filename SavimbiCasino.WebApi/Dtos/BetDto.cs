using System;
using System.Text.Json.Serialization;

namespace SavimbiCasino.WebApi.Dtos
{
    public class BetDto
    {
        [JsonPropertyName("id")] public Guid Id => Guid.NewGuid();

        [JsonPropertyName("userId")] public Guid UserId { get; set; }

        [JsonPropertyName("username")] public string Username { get; set; }
        
        [JsonPropertyName("bet")] public int Bet { get; set; }
        
        [JsonPropertyName("isDivided")] public bool IsDivided { get; set; }
        
        [JsonPropertyName("isDouble")] public bool IsDouble { get; set; }
    }
}