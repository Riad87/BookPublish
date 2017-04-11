using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;
using System.ComponentModel.DataAnnotations;

namespace BookPublish_WebApp.Models
{
    public class PayOffViewModel
    {
        public string ISBNSort { get; internal set; }

        public List<PayOff> PayOffs { get; set; }

        public int AllPayoffsCount { get; set; }
            
        public List<object> PagerList
        {
            get
            {
                return new object[AllPayoffsCount].ToList();
            }
        }

        public string CurrentFilter { get; internal set; }

        public string NameSort { get; internal set; }

        public string QuantitySort { get; set; }

        public string PriceSort { get; set; }

        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllPayoffsCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
    }
}