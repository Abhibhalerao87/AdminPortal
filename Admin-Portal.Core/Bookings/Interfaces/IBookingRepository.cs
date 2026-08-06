using Admin_Portal.Core.Bookings.Entities;
using Admin_Portal.Core.Bookings.Enums;

namespace Admin_Portal.Core.Bookings.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid bookingId);

        Task<List<Booking>> GetByUserIdAsync(Guid userId);

        Task<List<Booking>> GetByAdminIdAsync(Guid adminId);

        Task<List<Booking>> GetByStatusAsync(BookingStatus status);

        Task<List<Booking>> GetAllAsync();

        Task<List<Booking>> GetUserBookingsByStatusAsync(Guid userId, BookingStatus status);

        Task<List<Booking>> GetAdminBookingsByStatusAsync(Guid adminId, BookingStatus status);

        Task<Booking> CreateAsync(Booking booking);

        Task<Booking> UpdateAsync(Booking booking);

        Task<bool> DeleteAsync(Guid bookingId);

        Task<List<Booking>> GetPendingBookingsAsync();

        Task<List<Booking>> GetAcceptedBookingsAsync();
    }
}
