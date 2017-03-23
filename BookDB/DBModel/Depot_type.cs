using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookPublishDB
{
    public class Depot_type
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Raktár típus megadása kötelező!")]
        public string Type { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }
    }
}
