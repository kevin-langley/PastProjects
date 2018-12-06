using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Coupon code is required.")]
        [StringLength(20, ErrorMessage = "Coupon code must be 1-20 characters long.")]
        [Display(Name = "Coupon Code")]

        public String CouponCode { get; set; }
        public Boolean CouponActive { get; set; }

        public CouponType CouponName { get; set; }

        //Navigation properites
        public List<Order> Orders { get; set; }
    }

}
