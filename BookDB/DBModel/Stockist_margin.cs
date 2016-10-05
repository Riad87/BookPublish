using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bookPublishDB
{
    public class Stockist_margin
    {
        public Stockist_margin()
        {
            Depot = new List<Depot>();
        }

        public int ID { get; set; }   
           
        [Required]  
        public Partner Partner { get; set; }
        public double Discount { get; set; }

        public List<Depot> Depot { get; set; }


    }
}
