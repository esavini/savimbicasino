using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SavimbiCasino.WebApi.Dtos
{
    public class PlayerDto
    {
        [JsonPropertyName("id")] public Guid Id { get; set; }
        
        [JsonPropertyName("name")] public string Username { get; set; }
        
        [JsonPropertyName("chips")] public IEnumerable<ChipDto> Chips { get; set; }
     }
}