using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;
using SupperHeroAPI_Dotnet8.Service.implement;
using SupperHeroAPI_Dotnet8.Service.interfaces;
using SupperHeroAPI_Dotnet8.UnitOfWork;

namespace SupperHeroAPI_Dotnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _dataContext;
        public PaymentsController(IPaymentService paymentsService, IUnitOfWork unitOfWork, DataContext dataContext)
        {
            _dataContext = dataContext;
            _paymentsService = paymentsService;
            _unitOfWork = unitOfWork;
            StripeConfiguration.ApiKey = "sk_test_51QMVSAGEYdWIH8EBjJYkifesynojWXNveCleMeGbGdMYkv6hMvVJHX5lhEYoE4ojaE1RBwplkD70IWAKlCWmXPa900NWwEP8BW";
        }
        [HttpGet]
        public async Task<IActionResult> GetPaymentList()
        {
            var result = await _paymentsService.getListPayment();
            return StatusCode(result.stautsCode, result);
        }
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession(string paymentId)
        {
            var payment= await _dataContext.Payments.Include(x=>x.Booking).ThenInclude(b=>b.BookingRooms).ThenInclude(br=>br.Room).ThenInclude(rt=>rt.RoomType).Where(z=>z.Id==new Guid(paymentId)).FirstOrDefaultAsync();
            if(payment == null)
            {
                return NotFound(new ApiResponse<Payment>()
                {
                    stautsCode = 404,
                    message="Payment Not Found",
                    data=null
                });
            }
            string nameDescript = "";

            // get name room 
            foreach(var bookingRoom in payment.Booking.BookingRooms)
            {
                nameDescript += bookingRoom.Room.RoomType.Name+"\n";
                
            }

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
        {
          new SessionLineItemOptions
          {
            PriceData = new SessionLineItemPriceDataOptions
            {
              UnitAmount = (long?)payment.Amount,
              Currency = "vnd",
              ProductData = new SessionLineItemPriceDataProductDataOptions
              {
                Name = nameDescript,
              },
            },
            Quantity = payment.Booking.BookingRooms.Count(),
          },
        },
                Mode = "payment",
                SuccessUrl = "http://localhost:4242/success",
                CancelUrl = "http://localhost:4242/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

      
            var link = session.Url;
            return  Ok(new ApiResponse<string>() { stautsCode=200,message="Success get link",data=link});
        }

    }
}
