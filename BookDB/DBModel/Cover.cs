using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bookPublishDB
{
    public class Cover
    {
        public int ID { get; set; }
        public string CoverName { get; set; }
        public int Active { get; set; }
        
        //public Books Books { get; set; }

    }
}