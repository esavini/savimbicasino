using System;
using System.Collections.Generic;
using SavimbiCasino.WebApi.Enums;

namespace SavimbiCasino.WebApi.Models
{
    public class Game
    {
        public GameStatus Status { get; set; }
        
        public string DealerConnectionId { get; set; }
        
        public string DealerAdminConnectionId { get; set; }
        
        public IList<Tuple<Player, Bet, string>> Players { get; set; }
    }
}