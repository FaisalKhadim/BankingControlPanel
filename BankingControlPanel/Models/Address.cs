using System.Text.Json.Serialization;

namespace BankingControlPanel.Models
{
    public class Address
    {
        public int Id { get; set; } // Primary Key
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public int ClientId { get; set; }

        [JsonIgnore]
        public Client Client { get; set; }
    }
}