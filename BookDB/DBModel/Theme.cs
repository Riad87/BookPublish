using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookPublishDB
{
    public class Theme
    {
        public int ID { get; set; }

        [Display(Name = "Téma neve")]
        public string ThemeName { get; set; }

        [Display(Name = "Aktív-e")]
        public bool Active { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }



    }
}