using System;
using System.Collections.Generic;
using System.Linq;
using bookPublishDB;
namespace BookPublish_WebApp.Models
{
    public class PartnersViewModel
    {
        public string ActiveSort { get; internal set; }

        public List<Partner> Partners { get; set; }

        public int AllPartnerCount { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllPartnerCount].ToList();
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
                return (int)(Math.Ceiling((float)(AllPartnerCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
                
    }
}