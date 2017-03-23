using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookPublishDB
{
    public class Depot
    {
        public int ID { get; set; }            
        
        [Required]
        public Depot_type Type { get; set; }        
        public Partner Partner { get; set; }

        [Required(ErrorMessage ="Raktár nevének kitöltése kötelező!")]
        public string Depot_Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }

    }
}