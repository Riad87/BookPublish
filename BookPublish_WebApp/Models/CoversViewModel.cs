using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;
using System.ComponentModel.DataAnnotations;

namespace BookPublish_WebApp.Models
{
    public class CoversViewModel
    {
        [Required(ErrorMessage ="Név megadása kötelező!")]
        public string CoverName { get; set; }

        [Required]
        public bool Active { get; set; }

        public string ActiveSort { get; internal set; }

        public List<Cover> Covers { get; set; }

        public int AllCoversCount { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllCoversCount].ToList();
            }
        }

        public string CurrentFilter { get; internal set; }

        public string CoverSort { get; internal set; }

        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllCoversCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
    }
}