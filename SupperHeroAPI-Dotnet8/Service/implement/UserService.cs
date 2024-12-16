using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;
using SupperHeroAPI_Dotnet8.Service.interfaces;
using SupperHeroAPI_Dotnet8.UnitOfWork;
using System.Security.Cryptography;

namespace SupperHeroAPI_Dotnet8.Service.implement
{
    public class UserService : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<User>> InsertRoom(UserInsert userInsert)
        {
            var user= new User();
            user.phoneNumber = userInsert.phoneNumber;
            user.email = userInsert.email;
            user.userName = userInsert.userName;
            user.fullName = userInsert.fullName;

            //chuyển hóa password và lưu salt
            var (passwordHash, salt) = HashPassword(userInsert.passWord);
            user.passWordHash = passwordHash;
            user.passWordSalt = Convert.ToBase64String(salt);

          await  _unitOfWork.Repository<User>().AddAsync(user);
          await  _unitOfWork.SaveChangesAsync();
            return new ApiResponse<User>()
            {
                stautsCode = 200,
                message = "Create user success",
                data = user
            };
        }
        private (string hashedPassword, byte[] hashedPasswordSalt) HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return (hashed, salt);
        }

    }
}
