using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public async Task<IActionResult> Validate(LoginModel loginModel)
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
                data = await GenerateToken(user)
            });
        }
        private async Task<TokenModel> GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes=Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,user.fullName),
                    new Claim(JwtRegisteredClaimNames.Email,user.email),
                    new Claim(JwtRegisteredClaimNames.Sub,user.email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("UserName",user.userName),
                    new Claim("ID",user.UserId.ToString()),

                    //roles
                    new Claim("TokenID",Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken= jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //lưu database
            var refreshTokenEntity = new TokenRefresh
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                token = refreshToken,
                IsUsed = false,
                UserId=user.UserId,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1),

            };
           await _dataContext.TokenRefreshes.AddAsync(refreshTokenEntity);
          await  _dataContext.SaveChangesAsync();
            return  new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
          var random= new byte[32];
            using (var randomgenerator = RandomNumberGenerator.Create())
            {
                randomgenerator.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        [HttpPost("renewToken")]
        public async Task<IActionResult> RenewToken(TokenModel tokeninput)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,
               
                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokeninput.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return Ok(new ApiResponse<object>
                        {
                            stautsCode = 400,
                            message = "Invalid token"
                        });
                    }
                }

                //check 3: Check accessToken expire?
                var a = tokenInVerification.Claims;
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == "exp").Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new ApiResponse<object>
                    {
                        stautsCode = 400,
                            message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _dataContext.TokenRefreshes.FirstOrDefault(x => x.token == tokeninput.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        stautsCode = 400,
                        message = "Refresh token does not exist"
                    });
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return Ok(new ApiResponse<object>
                    {
                        stautsCode = 400,
                        message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse<object>
                    {
                        stautsCode = 400,
                        message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                //var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                //if (storedToken.JwtId != jti)
                //{
                //    return Ok(new ApiResponse<object>
                //    {
                //        stautsCode = 400,
                //        message = "Token doesn't match"
                //    });
                //}

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _dataContext.Update(storedToken);
                await _dataContext.SaveChangesAsync();

                //create new token
                var user = await _dataContext.Users.SingleOrDefaultAsync(nd => nd.UserId == storedToken.UserId);
                var token = await GenerateToken(user);

                return Ok(new ApiResponse<object>
                {
                    stautsCode = 200,
                    message = "Renew token success",
                    data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    stautsCode = 500,
                    message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExprieDate)
        {
           var dateTimeInterval= new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExprieDate).ToUniversalTime();
            return dateTimeInterval;
        }
    }
}
