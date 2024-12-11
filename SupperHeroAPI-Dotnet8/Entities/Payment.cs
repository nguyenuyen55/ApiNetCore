using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Principal;
using System;

namespace SupperHeroAPI_Dotnet8.Entities
{
    public class Payment
    {


        public Guid Id { get; set; }
     
        public Guid BookingID { get; set; }
        public Booking? Booking { get; set; }

        public double? Amount { get; set; }
        public DateTime PaymentDate { get; set; }=DateTime.Now;

        public PaymentMethod paymentMethod { get; set; }

    }
    public enum PaymentMethod
    {Unpaid,
        CreditCard, 
        Cash, 
        BankTransfer
    }
}
