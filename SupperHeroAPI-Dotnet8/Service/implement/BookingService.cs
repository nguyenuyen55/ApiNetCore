using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.DTO.Booking;
using SupperHeroAPI_Dotnet8.Entities;
using SupperHeroAPI_Dotnet8.Service.interfaces;
using SupperHeroAPI_Dotnet8.UnitOfWork;

namespace SupperHeroAPI_Dotnet8.Service.implement
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _dataContext;
        public BookingService(IUnitOfWork unitOfWork, DataContext dataContext) {
        _unitOfWork = unitOfWork;
          _dataContext = dataContext;
        }
        public Task<ApiResponse<bool>> CancelBooking()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IEnumerable<Booking>>> CheckBookingAvailable(string checkIn, string checkOut)
        {
            //var listBookingAvailable=_dataContext.bookings.Where()

            throw new NotImplementedException();
        }

        public async Task<ApiResponse<Booking>> CreateBooking(BookingInsert bookingInsert)
        {
            try
            {
                var user = await _unitOfWork.Repository<User>().GetByIdAsync<Guid, User>(bookingInsert.UserId);
                if (user == null)
                {
                    return new ApiResponse<Booking>()
                    {
                        stautsCode = 404,
                        message = "User not found",
                        data = null
                    };

                }
                foreach (var bookingRoomInsert in bookingInsert.BookingRooms)
                {
                    var roomInvaild = await _unitOfWork.Repository<Room>().GetByIdAsync<int, Room>(bookingRoomInsert.IdRoom);
                    if(roomInvaild == null)
                    {
                        return new ApiResponse<Booking>()
                        {
                            stautsCode = 404,
                            message = "Room not found with "+ bookingRoomInsert.IdRoom,
                            data = null
                        };
                    }
                
                }
                 Booking booking = new Booking();
                booking.UserId = bookingInsert.UserId;
                booking.CheckInDate = bookingInsert.CheckInDate;
                booking.CheckOutDate = bookingInsert.CheckOutDate;
                booking.status = statusBooking.Pending;

                await _unitOfWork.Repository<Booking>().AddAsync(booking);
                await _unitOfWork.SaveChangesAsync();

                List<BookingRoom> bookingRoomList = new List<BookingRoom>();

                foreach (var bookingRoomInsert in bookingInsert.BookingRooms)
                {
                    var bookingRoom = new BookingRoom();
                    bookingRoom.IdRoom = bookingRoomInsert.IdRoom;
                    bookingRoom.IdBooking = booking.Id;
                    bookingRoom.TypeBed = bookingRoomInsert.TypeBed;
                    bookingRoomList.Add(bookingRoom);
                }
                await _unitOfWork.Repository<BookingRoom>().AddRangeAsync(bookingRoomList);

                // add payment hàng đợi 
                Payment payment = new Payment();
                payment.Amount = bookingInsert.Amount;
                payment.paymentMethod = PaymentMethod.Unpaid;
                payment.BookingID = booking.Id;
                await _unitOfWork.Repository<Payment>().AddAsync(payment);
                await _unitOfWork.SaveChangesAsync();
                return new ApiResponse<Booking>()
                {
                    stautsCode = 200,
                    message = "add booking success",
                    data = booking
                };
            }catch(Exception ex)
            {
                return new ApiResponse<Booking>()
                {
                    stautsCode = 500,
                    message = ex.Message,
                    data = null
                };
            }
           
        }

        public async Task<ApiResponse<IEnumerable<Booking>>> getListBooking()
        {
            var bookings = await _unitOfWork.Repository<Booking>().GetAllAsync();
            if (bookings.Count() == 0)
            {
                return new ApiResponse<IEnumerable<Booking>>()
                {
                    stautsCode = 404,
                    message = "not found list booking",
                    data = null
                };
            }
            return new ApiResponse<IEnumerable<Booking>>()
            {
                stautsCode = 200,
                message = "List booking",
                data = bookings
            };

        }
    }
}
