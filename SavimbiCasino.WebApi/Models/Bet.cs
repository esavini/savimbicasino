namespace SavimbiCasino.WebApi.Models
{
    public class Bet
    {
        public int? Amount { get; set; }
        
        public bool IsSplitted { get; set; }
        
        public bool IsDouble { get; set; }
    }
}