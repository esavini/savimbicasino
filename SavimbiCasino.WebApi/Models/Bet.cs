using System;

namespace SavimbiCasino.WebApi.Models
{
    public class Bet
    {
        public int? Amount { get; set; }

        public Player Player { get; set; }

        public Guid PlayerId { get; set; }
        
        public string ConnectionId { get; set; }

        public Room Room { get; set; }

        public Guid RoomId { get; set; }

        public bool IsSplitted { get; set; }
    }
}