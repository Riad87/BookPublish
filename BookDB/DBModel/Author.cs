using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookPublishDB
{
    public class Author
    {
        public Author()
        {
            Book = new List<Books>();
        }
        public int ID { get; set; }

        [Display(Name = "Szerző neve")]
        [Required(ErrorMessage = "A név megadása kötelező!")]
        public string AuthorName { get; set; }

        [Display(Name = "Aktív-e")]
        public bool Active { get; set; }

        public bool Delete { get; set; }

        [Required]
        public List<Books> Book { get; set; }

        [NotMapped]
        public bool IsDeleted {get; set;}
    }
}