using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookPublishDB
{
    public class Press
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Név megadása kötelező!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Város megadása kötelező!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Cím megadása kötelező!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Irányítószám megadása kötelező!")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Ország megadása kötelező!")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Adószám megadása kötelező!")]
        public string TaxNumber { get; set; }

        [Required(ErrorMessage = "Számlaszám megadása kötelező!")]
        public string AccountNumber { get; set; }

        public bool Active { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }
               
        public List<Pressure> Pressure { get; set; }
        

    }
}
