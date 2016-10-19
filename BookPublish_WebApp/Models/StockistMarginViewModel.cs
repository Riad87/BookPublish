using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;

namespace BookPublish_WebApp.Models
{
    public class StockistMarginViewModel
    {
        public string ActiveSort { get; internal set; }

        public string DiscountSort { get; internal set; }

        public string PartnerSort { get; internal set; }

        public List<Stockist_margin> StMargins { get; set; }

        public int AllStMarginCount { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllStMarginCount].ToList();
            }
        }

        public string CurrentFilter { get; internal set; }        

        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllStMarginCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
    }
}