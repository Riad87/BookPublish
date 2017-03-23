using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookPublish_WebApp.Models
{
    public class StockistMarginViewModel
    {
        public int ID { get; set; }

        public SelectList AllPartner { get; set; }

        [Required(ErrorMessage ="Partner megadása kötelező!")]
        public int SelectedPartnerID { get; set; }

        [Range(typeof(double),"0","0,75",ErrorMessage ="Nem adható meg 0-nál kisebb és 0,75 nagyobb érték!")]
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage ="Kedvezmény megadása kötelező!")]
        public double Discount { get; set; }

        public bool Active { get; set; }

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