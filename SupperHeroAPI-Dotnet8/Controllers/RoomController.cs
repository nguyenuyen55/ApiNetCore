using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupperHeroAPI_Dotnet8.Entities;
using SupperHeroAPI_Dotnet8.UnitOfWork;

namespace SupperHeroAPI_Dotnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase 
    {
        public IUnitOfWork? _unitOfWork;
        public RoomController(IUnitOfWork? unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var room = await _unitOfWork!.Repository<Room>().GetAllAsync();
            return Ok(room);
        }
    }
}
