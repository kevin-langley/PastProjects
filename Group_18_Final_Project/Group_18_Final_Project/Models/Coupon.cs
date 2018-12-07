using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    //enum coupontype
    public enum CouponType { FreeShippingForX, XOffOrder };

    public class Coupon
    {
        //Coupon properties
        public Int32 CouponID { get; set; }

        [Required(ErrorMessage = "Coupon code is required.")]
        [StringLength(20, ErrorMessage = "Coupon code must be 1-20 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        [Display(Name = "Coupon Code")]
        public String CouponCode { get; set; }

        public Boolean CouponActive { get; set; }

        public CouponType CouponType { get; set; }

        [Required(ErrorMessage = "Coupon needs promotion value.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        [Display(Name = "Coupon Value")]
        public Decimal CouponValue { get; set; }

        //Navigation properites
        public List<Order> Orders { get; set; }
    }

}
