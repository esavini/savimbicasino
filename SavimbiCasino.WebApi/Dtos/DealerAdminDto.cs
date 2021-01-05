using System.Collections.Generic;
using System.Text.Json.Serialization;
using SavimbiCasino.WebApi.Enums;

namespace SavimbiCasino.WebApi.Dtos
{
    public class DealerAdminDto
    {
        [JsonPropertyName("status")] public GameStatus Status { get; set; }

        [JsonPropertyName("players")] public IEnumerable<BetDto> Players { get; set; }
    }
}