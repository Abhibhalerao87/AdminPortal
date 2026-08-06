using Admin_Portal.Application.Bookings.Interfaces;
using Admin_Portal.Contracts.Bookings.Responses;
using Admin_Portal.Core.Bookings.Entities;
using Admin_Portal.Core.Bookings.Enums;
using Admin_Portal.Core.Bookings.Interfaces;

namespace Admin_Portal.Application.Bookings.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingServiceResult> CreateBookingAsync(Guid userId, string CompanyName, DateTime startTime, DateTime endTime, string hrMail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CompanyName))
                    return new BookingServiceResult { IsSuccess = false, Message = "Complain Name is required" };

                if (string.IsNullOrWhiteSpace(hrMail))
                    return new BookingServiceResult { IsSuccess = false, Message = "HR Mail is required" };

                if (startTime >= endTime)
                    return new BookingServiceResult { IsSuccess = false, Message = "Start time must be before end time" };

                if (startTime < DateTime.UtcNow)
                    return new BookingServiceResult { IsSuccess = false, Message = "Cannot create booking in the past" };

                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CompanyName = CompanyName.Trim(),
                    StartTime = startTime,
                    EndTime = endTime,
                    HrMail = hrMail?.Trim() ?? string.Empty,
                    Status = BookingStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdBooking = await _bookingRepository.CreateAsync(booking);
                var dto = MapToDto(createdBooking);

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Booking created successfully",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error creating booking: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> UpdateBookingAsync(Guid bookingId, Guid userId, string? CompanyName, DateTime? startTime, DateTime? endTime, string? hrMail)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking is null)
                    return new BookingServiceResult { IsSuccess = false, Message = "Booking not found" };

                if (booking.UserId != userId)
                    return new BookingServiceResult { IsSuccess = false, Message = "You can only update your own bookings" };

                if (booking.Status != BookingStatus.Pending)
                    return new BookingServiceResult { IsSuccess = false, Message = "Can only update pending bookings" };

                if (!string.IsNullOrWhiteSpace(CompanyName))
                    booking.CompanyName = CompanyName.Trim();

                if (startTime.HasValue)
                    booking.StartTime = startTime.Value;

                if (endTime.HasValue)
                    booking.EndTime = endTime.Value;

                if (!string.IsNullOrWhiteSpace(hrMail))
                    booking.HrMail = hrMail.Trim();

                if (booking.StartTime >= booking.EndTime)
                    return new BookingServiceResult { IsSuccess = false, Message = "Start time must be before end time" };

                if (booking.StartTime < DateTime.UtcNow)
                    return new BookingServiceResult { IsSuccess = false, Message = "Cannot set booking in the past" };

                booking.UpdatedAt = DateTime.UtcNow;
                var updatedBooking = await _bookingRepository.UpdateAsync(booking);
                var dto = MapToDto(updatedBooking);

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Booking updated successfully",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error updating booking: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> GetBookingByIdAsync(Guid bookingId)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking is null)
                    return new BookingServiceResult { IsSuccess = false, Message = "Booking not found" };

                var dto = MapToDto(booking);
                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Booking retrieved successfully",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error retrieving booking: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> GetUserBookingsAsync(Guid userId)
        {
            try
            {
                var bookings = await _bookingRepository.GetByUserIdAsync(userId);
                var dtos = bookings.Select(a => MapToDto(a)).ToList();

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Bookings retrieved successfully",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error retrieving bookings: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> GetUserPendingBookingsAsync(Guid userId)
        {
            try
            {
                var bookings = await _bookingRepository.GetUserBookingsByStatusAsync(userId, BookingStatus.Pending);
                var dtos = bookings.Select(a => MapToDto(a)).ToList();

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Pending bookings retrieved successfully",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error retrieving bookings: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> GetUserAcceptedBookingsAsync(Guid userId)
        {
            try
            {
                var bookings = await _bookingRepository.GetUserBookingsByStatusAsync(userId, BookingStatus.Accepted);
                var dtos = bookings.Select(a => MapToDto(a)).ToList();

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Accepted bookings retrieved successfully",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error retrieving bookings: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> GetAdminBookingsAsync(Guid adminId)
        {
            try
            {
                var bookings = await _bookingRepository.GetByAdminIdAsync(adminId);
                var dtos = bookings.Select(a => MapToDto(a)).ToList();

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Bookings retrieved successfully",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error retrieving bookings: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> GetAdminPendingBookingsAsync(Guid adminId)
        {
            try
            {
                var bookings = await _bookingRepository.GetAdminBookingsByStatusAsync(adminId, BookingStatus.Pending);
                var dtos = bookings.Select(a => MapToDto(a)).ToList();

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Pending bookings retrieved successfully",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error retrieving bookings: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> GetAdminAcceptedBookingsAsync(Guid adminId)
        {
            try
            {
                var bookings = await _bookingRepository.GetAdminBookingsByStatusAsync(adminId, BookingStatus.Accepted);
                var dtos = bookings.Select(a => MapToDto(a)).ToList();

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Accepted bookings retrieved successfully",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error retrieving bookings: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> AcceptBookingAsync(Guid bookingId, Guid adminId)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking is null)
                    return new BookingServiceResult { IsSuccess = false, Message = "Booking not found" };

                if (booking.Status != BookingStatus.Pending)
                    return new BookingServiceResult { IsSuccess = false, Message = "Only pending bookings can be accepted" };

                booking.Status = BookingStatus.Accepted;
                booking.AdminId = adminId;
                booking.AcceptedAt = DateTime.UtcNow;
                booking.UpdatedAt = DateTime.UtcNow;

                var updatedBooking = await _bookingRepository.UpdateAsync(booking);
                var dto = MapToDto(updatedBooking);

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Booking accepted successfully",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error accepting booking: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> RejectBookingAsync(Guid bookingId, Guid adminId, string rejectionReason)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking is null)
                    return new BookingServiceResult { IsSuccess = false, Message = "Booking not found" };

                if (booking.Status != BookingStatus.Pending)
                    return new BookingServiceResult { IsSuccess = false, Message = "Only pending bookings can be rejected" };

                if (string.IsNullOrWhiteSpace(rejectionReason))
                    return new BookingServiceResult { IsSuccess = false, Message = "Rejection reason is required" };

                booking.Status = BookingStatus.Rejected;
                booking.AdminId = adminId;
                booking.RejectionReason = rejectionReason.Trim();
                booking.UpdatedAt = DateTime.UtcNow;

                var updatedBooking = await _bookingRepository.UpdateAsync(booking);
                var dto = MapToDto(updatedBooking);

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Booking rejected successfully",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error rejecting booking: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> DeleteBookingAsync(Guid bookingId, Guid userId)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking is null)
                    return new BookingServiceResult { IsSuccess = false, Message = "Booking not found" };

                if (booking.UserId != userId)
                    return new BookingServiceResult { IsSuccess = false, Message = "You can only delete your own bookings" };

                if (booking.Status != BookingStatus.Pending)
                    return new BookingServiceResult { IsSuccess = false, Message = "Can only delete pending bookings" };

                await _bookingRepository.DeleteAsync(bookingId);

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Booking deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error deleting booking: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> DeleteBookingByAdminAsync(Guid bookingId, Guid adminId)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);
                if (booking is null)
                    return new BookingServiceResult { IsSuccess = false, Message = "Booking not found" };

                await _bookingRepository.DeleteAsync(bookingId);

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Booking deleted successfully by admin"
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error deleting booking: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> GetAllPendingBookingsAsync()
        {
            try
            {
                var bookings = await _bookingRepository.GetPendingBookingsAsync();
                var dtos = bookings.Select(a => MapToDto(a)).ToList();

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Pending bookings retrieved successfully",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error retrieving bookings: {ex.Message}" };
            }
        }

        public async Task<BookingServiceResult> GetAllAcceptedBookingsAsync()
        {
            try
            {
                var bookings = await _bookingRepository.GetAcceptedBookingsAsync();
                var dtos = bookings.Select(a => MapToDto(a)).ToList();

                return new BookingServiceResult
                {
                    IsSuccess = true,
                    Message = "Accepted bookings retrieved successfully",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new BookingServiceResult { IsSuccess = false, Message = $"Error retrieving bookings: {ex.Message}" };
            }
        }

        private BookingDto MapToDto(Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                AdminId = booking.AdminId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                CompanyName = booking.CompanyName,
                HrMail = booking.HrMail,
                Status = booking.Status.ToString(),
                RejectionReason = booking.RejectionReason,
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt,
                AcceptedAt = booking.AcceptedAt
            };
        }
    }
}
