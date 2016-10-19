using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;

namespace BookPublish_WebApp.Models
{
    public class DepotsViewModel
    {
        public string NameSort { get; set; }

        public string CitySort { get; set; }

        public string AddressSort { get; set; }

        public string ZipSort { get; set; }

        public string TypeSort { get; set; }

        public string PartnerSort { get; set; }

        public string MarginSort { get; set; }

        public List<Depot> Depots { get; set; }

        public int AllDepotsCount { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllDepotsCount].ToList();
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
                return (int)(Math.Ceiling((float)(AllDepotsCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
    }
}