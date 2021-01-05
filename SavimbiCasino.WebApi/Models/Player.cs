using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace SavimbiCasino.WebApi.Models
{
    public class Player
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string HashedPassword { get; set; }
        
        public string SessionToken { get; set; }
        
        public int Money { get; set; }

        public bool IsAdmin { get; set; }
        
        public bool IsScratched { get; set; }

        public virtual IEnumerable<Room> DealerRooms { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}