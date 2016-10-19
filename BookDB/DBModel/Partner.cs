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
        public string Name { get; set; }
        public List<Depot> Depot { get; set; }
        public int Active { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }

        public AccountType AccountType { get; set; }
    }
}
