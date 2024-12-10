using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupperHeroAPI_Dotnet8.DTO.Booking;

namespace SupperHeroAPI_Dotnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        [HttpGet]
        public async  Task<IActionResult> InsertBooking(BookingInsert bookingInsert)
        {
            return Ok("");
        }
    }
}
