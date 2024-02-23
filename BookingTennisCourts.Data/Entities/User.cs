using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTennisCourts
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
       
    }
}
