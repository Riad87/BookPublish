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

        [Display(Name = "Nyomda neve")]
        [Required(ErrorMessage = "Név megadása kötelező!")]
        public string Name { get; set; }

        [Display(Name = "Város")]
        [Required(ErrorMessage = "Város megadása kötelező!")]
        public string City { get; set; }

        [Display(Name = "Cím")]
        [Required(ErrorMessage = "Cím megadása kötelező!")]
        public string Address { get; set; }

        [Display(Name = "Irsz")]
        [Required(ErrorMessage = "Irányítószám megadása kötelező!")]
        public string Zip { get; set; }

        [Display(Name = "Ország")]
        [Required(ErrorMessage = "Ország megadása kötelező!")]
        public string Country { get; set; }

        [Display(Name = "Adószám")]
        [Required(ErrorMessage = "Adószám megadása kötelező!")]
        public string TaxNumber { get; set; }

        [Display(Name = "Szla szám")]
        [Required(ErrorMessage = "Számlaszám megadása kötelező!")]
        public string AccountNumber { get; set; }

        [Display(Name = "Aktív-e")]
        public bool Active { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }
               
        public List<Pressure> Pressure { get; set; }
        

    }
}
