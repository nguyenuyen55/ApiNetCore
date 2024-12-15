using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;
using SupperHeroAPI_Dotnet8.Service.interfaces;
using SupperHeroAPI_Dotnet8.UnitOfWork;

namespace SupperHeroAPI_Dotnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        public IUnitOfWork? _unitOfWork;
        public IRoomService? _roomService;
        public RoomController(IUnitOfWork? unitOfWork, IRoomService? roomService)
        {
            _unitOfWork = unitOfWork;
            _roomService = roomService;
        }
        [HttpGet("getAll")]
        [Authorize]
        public async Task<IActionResult> getAll(string? search)
        {
            var rooms = await _roomService.getRoomAll(search);
            return StatusCode(rooms.stautsCode, rooms);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomDetail(int id)
        {
            var room = await _roomService.getRoomById(id);
            return StatusCode(room.stautsCode, room);
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id,[FromBody] string statusInput)
        {
            var room = await _roomService.ChangeStatusRoom(id, statusInput);
            return StatusCode(room.stautsCode, room);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromForm] RoomRequestDTO roomRequestDTO)
        {
            var result = await _roomService!.InsertRoom(roomRequestDTO);
            return StatusCode(result.stautsCode,result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRoom([FromQuery] int idRoom,[FromForm] RoomRequestDTO roomRequestDTO)
        {
            var result = await _roomService!.UpdateRoom(idRoom,roomRequestDTO);
            return StatusCode(result.stautsCode, result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveRoom([FromQuery] int idRoom)
        {
            var result = await _roomService!.DeleteRoom(idRoom);
            return StatusCode(result.stautsCode, result);
        }
    }
}
