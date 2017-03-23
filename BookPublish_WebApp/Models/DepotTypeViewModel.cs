using System;
using System.Collections.Generic;
using System.Linq;
using bookPublishDB;
using System.ComponentModel.DataAnnotations;

namespace BookPublish_WebApp.Models
{
    public class DepotTypeViewModel
    {
        public string TypeSort { get; internal set; }

        [Required(ErrorMessage ="Raktár típus megadása kötelező!")]
        public string Type { get; set; }

        public bool Active { get; set; }

        public string IDSort { get; set; }

        public List<Depot_type> Depot_type { get; set; }

        public int AllDepotTypesCount { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllDepotTypesCount].ToList();
            }
        }

        public string CurrentFilter { get; internal set; }

        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllDepotTypesCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
    }
}