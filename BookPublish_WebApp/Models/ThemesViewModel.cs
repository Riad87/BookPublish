using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;
using System.ComponentModel.DataAnnotations;

namespace BookPublish_WebApp.Models
{
    public class ThemesViewModel
    {
        public bool Active { get; set; }

        [Required(ErrorMessage ="Név megadása kötelező!")]
        public string ThemeName { get; set; }

        public string ActiveSort { get; internal set; }

        public string NameSort { get; internal set; }

        public List<Theme> Themes { get; set; }

        public int AllThemeCount { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllThemeCount].ToList();
            }
        }

        public string CurrentFilter { get; internal set; }        

        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllThemeCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
    }
}