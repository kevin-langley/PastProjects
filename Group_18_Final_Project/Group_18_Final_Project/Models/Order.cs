using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Order
    {
        //Order properties
        public Int32 OrderID { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        public bool IsPending { get; set; }


        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Shipping Price")]
        public Decimal TotalShippingPrice { get; set; }

        [Display(Name = "Order Subtotal")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderSubtotal
        {
            get { return BookOrders.Sum(bo => bo.ExtendedPrice); }
        }

        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderTotal
        {
            get { return OrderSubtotal + TotalShippingPrice; }
        }

        //Navigation properties
        public CreditCard CreditCard { get; set; }
        public User User { get; set; }
        public Coupon Coupon { get; set; }
        public List<BookOrder> BookOrders { get; set; }


        public Order()
        {
            if (BookOrders == null)

            {
                BookOrders = new List<BookOrder>();
            }

        }
    }
}
