using System.ComponentModel.DataAnnotations;

namespace bookPublishDB
{
    public class Depot
    {
        public int ID { get; set; }
            
        [Required]
        public Depot_type Type { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
    }
}