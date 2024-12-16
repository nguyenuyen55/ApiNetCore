using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;
using SupperHeroAPI_Dotnet8.Service.interfaces;
using SupperHeroAPI_Dotnet8.UnitOfWork;

namespace SupperHeroAPI_Dotnet8.Service.implement
{
    public class PaymentService : IPaymentService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _dataContext;
        public PaymentService(IUnitOfWork unitOfWork, DataContext dataContext)
        {
            _unitOfWork = unitOfWork;
            _dataContext = dataContext;
        }
        public async Task<ApiResponse<IEnumerable<Payment>>> getListPayment()
        {
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            if (payments.Count() == 0)
            {
                return new ApiResponse<IEnumerable<Payment>>()
                {
                    stautsCode = 404,
                    message = "Not found list Payment",
                    data = null
                };
            }
            return new ApiResponse<IEnumerable<Payment>>()
            {
                stautsCode = 200,
                message = "List Payment",
                data = payments
            };
        }

       
        
    }
}
