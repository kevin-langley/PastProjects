using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Reorder
    {

        //Reorder properties
        public Int32 ReorderID { get; set; }
        public bool IsPending { get; set; }
        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ReorderTotal
        {
            get { return BookReorders.Sum(bo => bo.Price); }
        }

        //Navigation property
        public User User { get; set; }
        public List<BookReorder> BookReorders { get; set; }

        public Reorder()
        {
            if (BookReorders == null)

            {
                BookReorders = new List<BookReorder>();
            }

        }
    }
}
