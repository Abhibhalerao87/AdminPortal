using Admin_Portal.Application.Bookings.Interfaces;
using Admin_Portal.Contracts.Bookings.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Admin_Web_API.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminBookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public AdminBookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Get all pending bookings (Admin only)
        /// </summary>
        [HttpGet("pending")]
        public async Task<IActionResult> GetAllPendingBookings()
        {
            var result = await _bookingService.GetAllPendingBookingsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get all accepted bookings (Admin only)
        /// </summary>
        [HttpGet("accepted")]
        public async Task<IActionResult> GetAllAcceptedBookings()
        {
            var result = await _bookingService.GetAllAcceptedBookingsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get booking details by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Get all bookings for a specific admin
        /// </summary>
        [HttpGet("my/all")]
        public async Task<IActionResult> GetAdminBookings()
        {
            // TODO: Get adminId from authenticated admin user
            var adminId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Placeholder

            var result = await _bookingService.GetAdminBookingsAsync(adminId);
            return Ok(result);
        }

        /// <summary>
        /// Get pending bookings for current admin
        /// </summary>
        [HttpGet("my/pending")]
        public async Task<IActionResult> GetAdminPendingBookings()
        {
            // TODO: Get adminId from authenticated admin user
            var adminId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Placeholder

            var result = await _bookingService.GetAdminPendingBookingsAsync(adminId);
            return Ok(result);
        }

        /// <summary>
        /// Get accepted bookings for current admin
        /// </summary>
        [HttpGet("my/accepted")]
        public async Task<IActionResult> GetAdminAcceptedBookings()
        {
            // TODO: Get adminId from authenticated admin user
            var adminId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Placeholder

            var result = await _bookingService.GetAdminAcceptedBookingsAsync(adminId);
            return Ok(result);
        }

        /// <summary>
        /// Accept booking (Admin only)
        /// </summary>
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptBooking(Guid id, [FromBody] AcceptBookingRequest request)
        {
            // TODO: Get adminId from authenticated admin user
            var adminId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Placeholder

            var result = await _bookingService.AcceptBookingAsync(id, adminId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Reject booking (Admin only)
        /// </summary>
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectBooking(Guid id, [FromBody] RejectBookingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // TODO: Get adminId from authenticated admin user
            var adminId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Placeholder

            var result = await _bookingService.RejectBookingAsync(id, adminId, request.RejectionReason);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Delete booking (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            // TODO: Get adminId from authenticated admin user
            var adminId = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Placeholder

            var result = await _bookingService.DeleteBookingByAdminAsync(id, adminId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
