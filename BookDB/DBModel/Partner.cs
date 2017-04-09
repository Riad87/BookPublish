using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookPublishDB
{
    public class Partner
    {
        public Partner()
        {
            Depot = new List<bookPublishDB.Depot>();
        }

        public int ID { get; set; }

        [Display(Name = "Partner neve")]
        [Required(ErrorMessage = "Név megadása kötelező")]
        public string Name { get; set; }

        public List<Depot> Depot { get; set; }

        [Display(Name = "Aktív-e")]
        public bool Active { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }

        public AccountType AccountType { get; set; }
    }
}
