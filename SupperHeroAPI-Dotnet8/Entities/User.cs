using System.ComponentModel.DataAnnotations;

namespace SupperHeroAPI_Dotnet8.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public required string userName {get; set;}
        public required string passWordHash{get; set;}
        public required string fullName {get; set;}
        public required string email {get; set;}
        public required string phoneNumber {get; set;}
        public RoleName Role {get; set;} = RoleName.Customer;
        public DateTime CreateAt { get; set;} = DateTime.Now;

      public  ICollection<Booking>? bookings { get; set;}
        
    }
   public enum RoleName
    {
        Admin, Staff, Customer
    }
}
