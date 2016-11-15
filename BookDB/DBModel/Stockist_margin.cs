using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookPublishDB
{
    public class Stockist_margin
    {
        public Stockist_margin()
        {
          
        }

        public int ID { get; set; }   
           
        [Required]  
        public Partner Partner { get; set; }
        public double Discount { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
       

        [NotMapped]
        public bool IsDeleted { get; set; }


    }
}
