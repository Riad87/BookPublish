using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDB.DBModel
{
    public class PayOff
    {
        public int ID { get; set; }

        [Required]
        [Display(Name="ISBN")]
        public string ISBN { get; set; }

        [Required]
        [Display(Name = "Név")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Mennyiség")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Ár")]
        public int Price { get; set; }
    }
}
