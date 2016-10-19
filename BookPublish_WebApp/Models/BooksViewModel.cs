using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;

namespace BookPublish_WebApp.Models
{
    public class BooksViewModel
    {
        public string ActiveSort { get; internal set; }

        public List<Books> Books { get; set; }

        public int AllBooksCount { get; set; }

        public int Quantity { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllBooksCount].ToList();
            }
        }

        public string CurrentFilter { get; internal set; }

        public string NameSort { get; internal set; }

        public string ISBN { get; set; }

        public string ItemNum { get; set; }

        public string NetValue { get; set; }

        public string Vat { get; set; }

        public string GrossValue { get; set; }

        public string ValidFrom { get; set; }

        public string ValidTo { get; set; }

        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllBooksCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
    }
}