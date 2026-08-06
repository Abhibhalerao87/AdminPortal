using Admin_Portal.Application.Bookings.Interfaces;
using Admin_Portal.Contracts.Bookings.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Admin_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Create a new booking request
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // TODO: Get userId from authenticated user (JWT token)
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Placeholder

            var result = await _bookingService.CreateBookingAsync(
                userId,
                request.CompanyName,
                request.StartTime,
                request.EndTime,
                request.HrMail);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetBookingById), new { id = ((dynamic)result.Data!).Id }, result);
        }

        /// <summary>
        /// Get booking by ID
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
        /// Get all bookings for current user
        /// </summary>
        [HttpGet("user/all")]
        public async Task<IActionResult> GetUserBookings()
        {
            // TODO: Get userId from authenticated user
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Placeholder

            var result = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Get pending bookings for current user
        /// </summary>
        [HttpGet("user/pending")]
        public async Task<IActionResult> GetUserPendingBookings()
        {
            // TODO: Get userId from authenticated user
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Placeholder

            var result = await _bookingService.GetUserPendingBookingsAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Get accepted bookings for current user
        /// </summary>
        [HttpGet("user/accepted")]
        public async Task<IActionResult> GetUserAcceptedBookings()
        {
            // TODO: Get userId from authenticated user
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Placeholder

            var result = await _bookingService.GetUserAcceptedBookingsAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Update booking (user can only update their pending bookings)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] UpdateBookingRequest request)
        {
            // TODO: Get userId from authenticated user
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Placeholder

            var result = await _bookingService.UpdateBookingAsync(
                id,
                userId,
                request.CompanyName,
                request.StartTime,
                request.EndTime,
                request.HrMail);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Delete booking (user can only delete their pending bookings)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            // TODO: Get userId from authenticated user
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Placeholder

            var result = await _bookingService.DeleteBookingAsync(id, userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
