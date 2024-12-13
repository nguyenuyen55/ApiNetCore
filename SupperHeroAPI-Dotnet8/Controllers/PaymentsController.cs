using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupperHeroAPI_Dotnet8.Service.implement;
using SupperHeroAPI_Dotnet8.Service.interfaces;

namespace SupperHeroAPI_Dotnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentsService;
        public PaymentsController(IPaymentService paymentsService)
        {
            _paymentsService = paymentsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPaymentList()
        {
            var result = await _paymentsService.getListPayment();
            return StatusCode(result.stautsCode, result);
        }

    }
}
