namespace BankingControlPanel.DTOs
{
    public class ClientParameters
    {
        private const int maxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        public string? SortBy { get; set; } // Property to specify sorting field
        public string? SortOrder { get; set; } // Property to specify sorting order ("asc" or "desc")

        public string? FirstName { get; set; } // Filter by first name
        public string? LastName { get; set; } // Filter by last name
        public string? Email { get; set; } // Filter by email
    }
}