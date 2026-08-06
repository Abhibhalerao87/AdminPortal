namespace Admin_Portal.Contracts.Bookings.Requests
{
    public class CreateBookingRequest
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string HrMail { get; set; } = string.Empty;
    }
}

