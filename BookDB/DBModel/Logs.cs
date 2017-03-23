using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDB.DBModel
{
    public class Logs
    {
        public int ID { get; set; }

        public string Content { get; set; }

        public string User { get; set; }

        public string Action { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
