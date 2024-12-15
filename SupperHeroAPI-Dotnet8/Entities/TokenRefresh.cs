using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupperHeroAPI_Dotnet8.Entities
{
    public class TokenRefresh

    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; } //đã đc sử dụng chưa
        public bool IsRevoked { get; set; } ///được thu hồi chưa
        public DateTime IssuedAt { get; set; }

        public DateTime ExpiredAt { get; set; }
    }
}
