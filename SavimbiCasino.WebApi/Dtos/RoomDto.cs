using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SavimbiCasino.WebApi.Dtos
{
    public class RoomDto
    {
        [JsonPropertyName("players")]
        public IEnumerable<PlayerDto> Players { get; set; }
    }
}