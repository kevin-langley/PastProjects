using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Coupon
    {
        public Int32 CouponID { get; set; }
        public enum CouponType { get; set; }
        public List<Order> Orders { get; set; }
    }
}
