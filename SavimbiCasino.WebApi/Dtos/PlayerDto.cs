using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SavimbiCasino.WebApi.Dtos
{
    public class PlayerDto
    {
        [JsonPropertyName("id")] public Guid Id => Guid.NewGuid();
        
        [JsonPropertyName("name")] public string Username { get; set; }
        
        [JsonPropertyName("chips")] public IEnumerable<int> Chips { get; set; }
        
        [JsonPropertyName("isScratched")] public bool IsScratched { get; set; }
     }
}