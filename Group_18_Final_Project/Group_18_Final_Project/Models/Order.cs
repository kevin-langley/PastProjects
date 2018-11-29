using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Order
    {
        //Order properties
        public Int32 OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsPending { get; set; }

        //Navigation properties
        public CreditCard CreditCard { get; set; }
        public User User { get; set; }
        public Coupon Coupon { get; set; }
        public List<BookOrder> BookOrders { get; set; }

    }
}
