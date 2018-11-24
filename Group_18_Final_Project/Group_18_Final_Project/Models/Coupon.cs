using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    //enum coupontype
    public enum CouponType { placeholder1, placeholder2, placeholder3 };

    public class Coupon
    {

        //Coupon properties
        public Int32 CouponID { get; set; }
        public CouponType CouponName { get; set; }

        //Navigation properites
        public List<Order> Orders { get; set; }
    }

}
