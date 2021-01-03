using System;

namespace SavimbiCasino.WebApi.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public Player Dealer { get; set; }
        
        public Guid DealerId { get; set; }
    }
}