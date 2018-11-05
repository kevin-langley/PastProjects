using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class CreditCard
    {
        //CreditCard properties
        public Int32 CreditCardID { get; set; }
        public enum CreditCardType { get; set; }
        public Int32 CreditCardNumber { get; set; }

        //Navigation properties
        public User User { get; set; }
        public List<Order> Orders { get; set; }
    }
}
