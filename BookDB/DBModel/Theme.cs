using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookPublishDB
{
    public class Theme
    {
        public int ID { get; set; }
        public string ThemeName { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }



    }
}