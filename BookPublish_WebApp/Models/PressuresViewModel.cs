using System;
using System.Collections.Generic;
using System.Linq;
using bookPublishDB;

namespace BookPublish_WebApp.Models
{
    public class PressuresViewModel
    {
        public string OrderSort { get; internal set; }

        public string AfterPressureSort { get; internal set; }

        public string QuantitySort { get; set; }

        public string BookSort { get; set; }

        public string PressSort { get; set; }

        public string IDSort { get; set; }

        public List<Pressure> Pressures { get; set; }

        public int AllPressureCount { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllPressureCount].ToList();
            }
        }

        public string CurrentFilter { get; internal set; }
        
        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllPressureCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }

    }
}