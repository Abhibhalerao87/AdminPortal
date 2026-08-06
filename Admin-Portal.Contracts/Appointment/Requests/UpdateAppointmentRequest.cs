namespace Admin_Portal.Contracts.Bookings.Requests
{
    public class UpdateBookingRequest
    {
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? CompanyName { get; set; }

        public string? HrMail { get; set; }
    }
}

