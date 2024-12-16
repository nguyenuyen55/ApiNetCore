using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SupperHeroAPI_Dotnet8.Entities
{
    public class User
    {
        public User()
        {
            
        }
        [Key]
        public Guid UserId { get; set; }
        public  string userName {get; set;}
        public  string passWordHash{get; set;}
        public  string passWordSalt{get; set;}
        public  string fullName {get; set;}
        public  string email {get; set;}
        public  string phoneNumber {get; set;}
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoleName Role {get; set;} = RoleName.Customer;
        public DateTime? CreateAt { get; set;} = DateTime.Now;

      public  ICollection<Booking>? bookings { get; set;}
        
    }
   public enum RoleName
    {
        Admin, Staff, Customer
    }
}
