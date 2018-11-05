using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class User
    {
        //User properties
        public Int32 UserID { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public Int32 ZipCode { get; set; }
        public Int32 PhoneNumber { get; set; }
        public Boolean ActiveUser { get; set; }

        //Navigation properties
        public List<CreditCard> CreditCards { get; set; }
        public List<Order> Orders { get; set; }
        public List<Reorder> Reorders { get; set; }
        public List <Review> ReviewsWritten { get; set; }
        public List <Review> ReviewsApproved { get; set; }

    }
}
