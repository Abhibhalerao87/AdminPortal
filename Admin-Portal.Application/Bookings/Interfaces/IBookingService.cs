namespace Admin_Portal.Application.Bookings.Interfaces
{
    public interface IBookingService
    {
        Task<BookingServiceResult> CreateBookingAsync(Guid userId, string CompanyName, DateTime startTime, DateTime endTime, string hrMail);

        Task<BookingServiceResult> UpdateBookingAsync(Guid bookingId, Guid userId, string? CompanyName, DateTime? startTime, DateTime? endTime, string? hrMail);

        Task<BookingServiceResult> GetBookingByIdAsync(Guid bookingId);

        Task<BookingServiceResult> GetUserBookingsAsync(Guid userId);

        Task<BookingServiceResult> GetUserPendingBookingsAsync(Guid userId);

        Task<BookingServiceResult> GetUserAcceptedBookingsAsync(Guid userId);

        Task<BookingServiceResult> GetAdminBookingsAsync(Guid adminId);

        Task<BookingServiceResult> GetAdminPendingBookingsAsync(Guid adminId);

        Task<BookingServiceResult> GetAdminAcceptedBookingsAsync(Guid adminId);

        Task<BookingServiceResult> AcceptBookingAsync(Guid bookingId, Guid adminId);

        Task<BookingServiceResult> RejectBookingAsync(Guid bookingId, Guid adminId, string rejectionReason);

        Task<BookingServiceResult> DeleteBookingAsync(Guid bookingId, Guid userId);

        Task<BookingServiceResult> GetAllPendingBookingsAsync();

        Task<BookingServiceResult> GetAllAcceptedBookingsAsync();

        Task<BookingServiceResult> DeleteBookingByAdminAsync(Guid bookingId, Guid adminId);
    }

    public class BookingServiceResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public object? Data { get; set; }
    }
}
