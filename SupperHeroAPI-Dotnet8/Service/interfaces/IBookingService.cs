using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.DTO.Booking;
using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Service.interfaces
{
    public interface IBookingService
    {
        Task<ApiResponse<IEnumerable<Booking>>> getListBooking();
        Task<ApiResponse<IEnumerable<Booking>>> CheckBookingAvailable(string checkIn,string checkOut);
        Task<ApiResponse<Booking>> CreateBooking(BookingInsert bookingInsert);
        Task<ApiResponse<bool>> CancelBooking();
    }
}
