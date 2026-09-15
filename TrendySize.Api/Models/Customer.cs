using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace TrendySize.Api.Models
{
    //Creaye a new class called Customer that will be used to store customer information in the database. 
    //This class will have the following properties:
    public class Customer
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Gender Gender { get; set; } 
        public string PhoneNumber { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum Gender
    {
        Male ,
        Female 
    }


}
