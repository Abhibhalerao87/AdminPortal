namespace Admin_Portal.Contracts.Bookings.Responses
{
    public class BookingDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid? AdminId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string HrMail { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? AcceptedAt { get; set; }
    }
}

