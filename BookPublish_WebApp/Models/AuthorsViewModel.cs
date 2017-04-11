using bookPublishDB;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookPublish_WebApp.Models
{
    public class AuthorsViewModel
    {
        public string ActiveSort { get; internal set; }

        public List<Author> Authors { get; set; }

        public int AllAuthorCount { get; set; }

        [Display(Name = "Szerző neve")]
        [Required(ErrorMessage ="A név megadása kötelező!")]
        public string AuthorName { get; set; }

        [Display(Name = "Aktív-e")]
        public bool Active { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllAuthorCount].ToList();
            }
        }

        public string CurrentFilter { get; internal set; }

        public string NameSort { get; internal set; }

        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllAuthorCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }

        public AuthorsViewModel()
        {

        }
    }
}