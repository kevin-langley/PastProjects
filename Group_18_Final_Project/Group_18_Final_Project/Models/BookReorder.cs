using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class BookReorder
    {
        public Int32 BookReorderID { get; set; }
        public Int32 ReorderQuantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Price { get; set; }

        //Nav properties
        public Book Book { get; set; }
        public Reorder Reorder { get; set; }
        

    }
}
