using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    //enum creditcard 
    public enum CreditCardType { Visa , AmericanExpress , Discover, MasterCard};

    public class CreditCard
    {
        //CreditCard properties
        public Int32 CreditCardID { get; set; }
        [Required(ErrorMessage = "Credit Card type is required.")]
        [Display(Name = "Credit Card Type")]
        public CreditCardType CreditType { get; set; }

        [Required(ErrorMessage = "Card Number is required.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card Number must be 16 digits long.")]
        [Display(Name = "Credit Card Number:")]
        public String CreditCardNumber { get; set; }

        //Navigation properties
        public User User { get; set; }
        public List<Order> Orders { get; set; }
    }

}
