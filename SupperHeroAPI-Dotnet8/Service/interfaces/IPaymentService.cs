
using SupperHeroAPI_Dotnet8.DTO.Booking;
using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Service.interfaces
{
    public interface IPaymentService
    {
        Task<ApiResponse<IEnumerable<Payment>>> getListPayment();
        //Task<ApiResponse<IEnumerable<Room>>> CheckBookingAvailable(string checkIn, string checkOut);
        //Task<ApiResponse<Booking>> CreateBooking(BookingInsert bookingInsert);
    }
}
