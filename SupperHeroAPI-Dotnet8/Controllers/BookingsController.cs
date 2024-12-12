using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupperHeroAPI_Dotnet8.DTO.Booking;
using SupperHeroAPI_Dotnet8.Service.interfaces;

namespace SupperHeroAPI_Dotnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingsService;
        public BookingsController(IBookingService bookingsService)
        {
            _bookingsService = bookingsService;
        }
        [HttpPost]
        public async  Task<IActionResult> InsertBooking(BookingInsert bookingInsert)
        {
          var result=  await _bookingsService.CreateBooking(bookingInsert);
            return StatusCode(result.stautsCode,result);
        }
        [HttpGet]
        public async Task<IActionResult> GetBookingList()
        {
            var result = await _bookingsService.getListBooking();
            return StatusCode(result.stautsCode, result);
        }
        [HttpGet("CheckBookingAvailable")]
        public async Task<IActionResult> CheckBookingAvailable ( [FromQuery] string checkIn, [FromQuery] string checkOut)
        {
            var result = await _bookingsService.CheckBookingAvailable(checkIn, checkOut);
            return StatusCode(result.stautsCode, result);
        }
    }
}
