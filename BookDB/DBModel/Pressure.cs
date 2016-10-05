using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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


    }
}
