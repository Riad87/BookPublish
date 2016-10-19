using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookPublishDB
{
    public class Pressure
    {
        public int ID { get; set; }
        
        public Press Press { get; set; }
        public Books Book { get; set; }
        public DateTime OrderDate { get; set; }
        public int AfterPressure { get; set; }
        public int Quantity { get; set; }
        public int NotSold { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }


    }
}
