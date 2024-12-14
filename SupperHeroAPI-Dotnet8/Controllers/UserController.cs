using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SupperHeroAPI_Dotnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly AppSettings _appSettings;

        public UserController(DataContext dataContext, IOptionsMonitor<AppSettings> appSettings)
        {
            _dataContext = dataContext;

            _appSettings = appSettings.CurrentValue;

        }

        [HttpPost]
        public IActionResult Validate(LoginModel loginModel)
        {
            var user =_dataContext.Users.SingleOrDefault(p=>p.userName
            ==loginModel.Username && p.passWordHash == loginModel.Password);
            if(user == null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Invaild username/password"
                });
            }
            //cấp token

            return Ok(new ApiResponse<object>
            {
                stautsCode = 200,
                message = "Authen success",
                data = null
            });
        }
        private string GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes=Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,user.fullName),
                    new Claim(ClaimTypes.Email,user.email),
                    new Claim("UserName",user.userName),
                    new Claim("ID",user.UserId.ToString()),

                    //roles
                    new Claim("TokenID",Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
