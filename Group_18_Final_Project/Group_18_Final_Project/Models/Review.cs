using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Review
    {
        public Int32 ReviewID { get; set; }
        public String ReviewText { get; set; }
        public Boolean Approval { get; set; }
        public Int32 Rating { get; set; }
        public String Author { get; set; }
        public String Approver { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
