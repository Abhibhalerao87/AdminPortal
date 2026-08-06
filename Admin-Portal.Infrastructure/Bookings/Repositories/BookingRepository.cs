using Admin_Portal.Core.Bookings.Entities;
using Admin_Portal.Core.Bookings.Enums;
using Admin_Portal.Core.Bookings.Interfaces;
using Admin_Portal.Infrastructure.Admin.Data;
using Microsoft.EntityFrameworkCore;

namespace Admin_Portal.Infrastructure.Bookings.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AdminPortalDbContext _context;

        public BookingRepository(AdminPortalDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(Guid bookingId)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(a => a.Id == bookingId);
        }

        public async Task<List<Booking>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Bookings
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByAdminIdAsync(Guid adminId)
        {
            return await _context.Bookings
                .Where(a => a.AdminId == adminId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByStatusAsync(BookingStatus status)
        {
            return await _context.Bookings
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetUserBookingsByStatusAsync(Guid userId, BookingStatus status)
        {
            return await _context.Bookings
                .Where(a => a.UserId == userId && a.Status == status)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetAdminBookingsByStatusAsync(Guid adminId, BookingStatus status)
        {
            return await _context.Bookings
                .Where(a => a.AdminId == adminId && a.Status == status)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> UpdateAsync(Booking booking)
        {
            booking.UpdatedAt = DateTime.UtcNow;
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> DeleteAsync(Guid bookingId)
        {
            var booking = await GetByIdAsync(bookingId);
            if (booking is null)
                return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Booking>> GetPendingBookingsAsync()
        {
            return await _context.Bookings
                .Where(a => a.Status == BookingStatus.Pending)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetAcceptedBookingsAsync()
        {
            return await _context.Bookings
                .Where(a => a.Status == BookingStatus.Accepted)
                .OrderByDescending(a => a.AcceptedAt)
                .ToListAsync();
        }
    }
}
