using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookPublishDB
{
    public class Cover
    {
        public int ID { get; set; }
        public string CoverName { get; set; }
        public int Active { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }
        
        //public Books Books { get; set; }

    }
}