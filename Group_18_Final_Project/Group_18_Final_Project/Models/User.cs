using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Group_18_Final_Project.Models
{
    public class User : IdentityUser
    {
        //User properties
        public String Password { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public String Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public String City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public String State { get; set; }

        [Required(ErrorMessage = "Zip Code is required.")]
        public String ZipCode { get; set; }

        public Boolean ActiveUser { get; set; }
        public String UserType { get; set; }

        //Navigation properties
        public List<CreditCard> CreditCards { get; set; }
        public List<Order> Orders { get; set; }
        public List<Reorder> Reorders { get; set; }
        //Tags help EF identify what the relationships actually map to
        [InverseProperty("Author")]
        public List <Review> ReviewsWritten { get; set; }
        [InverseProperty("Approver")]
        public List <Review> ReviewsApproved { get; set; }

    }
}
