using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{

    public class Review
    {

        //Review properties
        public Int32 ReviewID { get; set; }

        [Display(Name = "Review Text")]
        [StringLength(100)]
        public String ReviewText { get; set; }

        public Boolean Approval { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        public Int32 Rating { get; set; }

        public Boolean IsPending { get; set; }

        //Navigation properties
        //Author
        public User Author { get; set; }
        //Approver
        public User Approver { get; set; }
        public Book Book { get; set; }
    }
}
