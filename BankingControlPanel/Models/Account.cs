using System.Text.Json.Serialization;

namespace BankingControlPanel.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int ClientId { get; set; }

        [JsonIgnore]
        public Client Client { get; set; }
    }
}