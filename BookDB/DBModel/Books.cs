using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace bookPublishDB
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Books
    {
        public Books()
        {
            //Author = new List<Author>();
            //Cover = new List<Cover>();
            //Theme = new List<Theme>();
            //Depot = new List<Depot>();
        }
        public int ID { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public int ItemNumber { get; set; }
        public int NetValue { get; set; }
        public int Vat { get; set; }
        public int GrossValue { get; set; }
        
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Deleted { get; set; }   
        
        [NotMapped]     
        public int IN_Stock { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }

        [Required]
        public Author Author { get; set; }
                
        public Cover Cover { get; set; }
        
        public Theme Theme { get; set; }
        

    }
}
