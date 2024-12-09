using Microsoft.EntityFrameworkCore;
using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;
using SupperHeroAPI_Dotnet8.Service.interfaces;
using SupperHeroAPI_Dotnet8.UnitOfWork;

namespace SupperHeroAPI_Dotnet8.Service.implement
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _dataContext;
        public RoomService(IUnitOfWork unitOfWork, DataContext dataContext)
        {
            _unitOfWork= unitOfWork;
            _dataContext= dataContext;
        }

        public async Task<ApiResponse<bool>> ChangeStatusRoom(int idRoom, string status)
        {
            var room = await _dataContext.Rooms.Where(x => x.IdRoom == idRoom).FirstOrDefaultAsync();

            if (room == null)
            {
                return new ApiResponse<bool>()
                {
                    stautsCode = 404,
                    message = "Not found Room",
                    data = false
                };
            }
            if (!Enum.IsDefined(typeof(Status),status))
            {
                return new ApiResponse<bool>()
                {
                    stautsCode = 404,
                    message = "Status not valid ",
                    data = false
                };
            }
            room.status = Enum.TryParse(status, true, out Status result) ? result : Status.Available; ;
             await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<bool>()
            {
                stautsCode = 200,
                message = "Change status success",
                data = false
            };
        }

        public async Task<ApiResponse<bool>> DeleteRoom(int idRoom)
        {
            var room = await _dataContext.Rooms.Where(x => x.IdRoom == idRoom).FirstOrDefaultAsync();

            if (room == null)
            {
                return new ApiResponse<bool>()
                {
                    stautsCode = 404,
                    message = "Not found Room",
                    data = false
                };
            }
            room.isActive = false;
            await _unitOfWork.SaveChangesAsync();
            return new ApiResponse<bool>()
            {
                stautsCode = 200,
                message = "Delete Room Success",
                data = true
            };
        }

        public async Task<ApiResponse<IEnumerable<Room>>> getRoomAll(string? search)
        {
            var listRoom= await _unitOfWork.Repository<Room>().GetAllAsync(room=>room.isActive==true ,x=>x.RoomType,x=>x.Images);
            if (listRoom.Count()==0)
            {
                return new ApiResponse<IEnumerable<Room>>()
                {
                    stautsCode = 404,
                    message = "Not found list room",
                    data = null
                };
            }
            return new ApiResponse<IEnumerable<Room>>()
            {
                stautsCode = 200,
                message = "List Room",
                data=listRoom
            };

        }

        public async Task<ApiResponse<Room>> getRoomById(int id)
        {
            var room= _dataContext.Rooms
                .Include(x=>x.RoomType)
                .Include(x=>x.Images)
               .Where(x=>x.IdRoom==id).FirstOrDefault();
            if(room == null)
            {
                return new ApiResponse<Room>()
                {
                    stautsCode = 404,
                    message = "Not found Room",
                    data = null
                };
            }
            return new ApiResponse<Room> ()
            {
                stautsCode = 200,
                message = "Get room success",
                data = room
            };
        }

        public async Task<ApiResponse<Room>> InsertRoom(RoomRequestDTO roomRequest)
        {
            //insert roomType
            var roomType = await _unitOfWork.Repository<RoomType>().GetByIdAsync(roomRequest.IdRoomType);
            if (roomType == null)
            {
                return new ApiResponse<Room>()
                {
                    stautsCode = 404,
                    message = "Not found room Type",
                    data = null
                };
            }

            var roomBDExitRoomNumber = await _unitOfWork.Repository<Room>().GetAllAsync(x=>(x.RoomNumber==roomRequest.NumberRoom && x.isActive==true));
            var rooms = _dataContext.Rooms.Where(x => (x.RoomNumber == roomRequest.NumberRoom && x.isActive == true));
            if (roomBDExitRoomNumber.Count()>0) {
                return new ApiResponse<Room>()
                {
                    stautsCode = 400,
                    message = "Room number already exists",
                    data = null
                };
            }

            var roomNew = new Room();
            roomNew.RoomNumber=roomRequest.NumberRoom;
            roomNew.RoomTypeID = roomRequest.IdRoomType;
          
            var roomSucces= await _unitOfWork.Repository<Room>().AddAsync(roomNew);
            await _unitOfWork.SaveChangesAsync();
            
            //insert image
            //Xử lí ảnh nếu có
            if (roomRequest.FileImages != null)
            {
                //thư mục lưu ảnh : wwwroot/Images/Room
                var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Hotel");
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }
                var listImageSave= new List<Image>();

                foreach (var itemImage in roomRequest.FileImages)
                {
                    //Tạo tên file duy nhất
                    var fileName = $"{Guid.NewGuid()}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}_{Path.GetFileName(itemImage.FileName)}";
                    var filePath = Path.Combine(imageDirectory, fileName);
                    //using dùng để giải phóng dung lượng
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        //lưu ảnh
                        await itemImage.CopyToAsync(stream);
                    }
                    //luu image trong database
                    var imageFileNew= new Image();
                    imageFileNew.RoomID = roomSucces.IdRoom;
                    imageFileNew.dateTime = DateTime.Now;
                    imageFileNew.NameImage = "/Images/Hotel/" + fileName;
                    listImageSave.Add(imageFileNew);
                }
                await _unitOfWork.Repository<Image>().AddRangeAsync(listImageSave);
                await _unitOfWork.SaveChangesAsync();

            }
            return new ApiResponse<Room>()
            {
                stautsCode = 200,
                message = "add room success",
                data =  roomSucces
            };
        }

        public async Task<ApiResponse<Room>> UpdateRoom(int idRoom, RoomRequestDTO roomRequest)
        {
            var room = await _dataContext.Rooms.Where(x => x.IdRoom == idRoom).FirstOrDefaultAsync();

            if (room == null)
            {
                return  new ApiResponse<Room>()
                {
                    stautsCode = 404,
                    message = "Not found Room",
                    data = null
                };
            }

            var roomType = await _unitOfWork.Repository<RoomType>().GetByIdAsync(roomRequest.IdRoomType);
            if (roomType == null)
            {
                return new ApiResponse<Room>()
                {
                    stautsCode = 404,
                    message = "Not found room Type",
                    data = null
                };
            }
            room.RoomTypeID=roomRequest.IdRoomType;
            room.RoomNumber = roomRequest.NumberRoom;
            // xóa ảnh cũ
            if (roomRequest.FileImages != null)
            {
                var ListImageRoom = await _dataContext.images.Where(x => x.RoomID == room.IdRoom).ToListAsync();
                _dataContext.images.RemoveRange(ListImageRoom);

                //thư mục lưu ảnh : wwwroot/Images/Room
                var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Hotel");
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }
                var listImageSave = new List<Image>();

                foreach (var itemImage in roomRequest.FileImages)
                {
                    //Tạo tên file duy nhất
                    var fileName = $"{Guid.NewGuid()}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}_{Path.GetFileName(itemImage.FileName)}";
                    var filePath = Path.Combine(imageDirectory, fileName);
                    //using dùng để giải phóng dung lượng
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        //lưu ảnh
                        await itemImage.CopyToAsync(stream);
                    }
                    //luu image trong database
                    var imageFileNew = new Image();
                    imageFileNew.RoomID = room.IdRoom;
                    imageFileNew.dateTime = DateTime.Now;
                    imageFileNew.NameImage = "/Images/Hotel/" + fileName;
                    listImageSave.Add(imageFileNew);
                }
                await _unitOfWork.Repository<Image>().AddRangeAsync(listImageSave);
                await _unitOfWork.SaveChangesAsync();
                //
            }
            return new ApiResponse<Room>()
            {
                stautsCode = 200,
                message = "Update Room Success",
                data = room
            };
        }
    }
}
