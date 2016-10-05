using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bookPublishDB
{
    public class Press
    {
        public int ID { get; set; }        
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public int Active { get; set; }
        
        [Required]
        public List<Pressure> Pressure { get; set; }
        

    }
}
