using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Review
    {

        //Review properties
        public Int32 ReviewID { get; set; }
        public String ReviewText { get; set; }
        public Boolean Approval { get; set; }
        public Int32 Rating { get; set; }

        //Navigation properties
        //Author
        public User Author { get; set; }
        //Approver
        public User Approver { get; set; }
        public Book Book { get; set; }
    }
}
