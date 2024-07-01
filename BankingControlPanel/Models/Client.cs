using System.Net;

namespace BankingControlPanel.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public string ProfilePhoto { get; set; }
        public string MobileNumber { get; set; }
        public string Sex { get; set; }
        public Address Address { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
