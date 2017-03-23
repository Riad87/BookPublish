using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;
using System.ComponentModel.DataAnnotations;

namespace BookPublish_WebApp.Models
{
    public class PressesViewModel
    {
        [Required(ErrorMessage ="Név megadása kötelező!")]
        public string PressName { get; set; }

        [Required(ErrorMessage ="Város megadása kötelező!")]
        public string City { get; set; }

        [Required(ErrorMessage ="Cím megadása kötelező!")]
        public string Address { get; set; }

        [Required(ErrorMessage ="Irányítószám megadása kötelező!")]
        public string Zip { get; set; }

        [Required(ErrorMessage ="Ország megadása kötelező!")]
        public string Country { get; set; }

        [Required(ErrorMessage ="Adószám megadása kötelező!")]
        public string TaxNumber { get; set; }

        [Required(ErrorMessage ="Számlaszám megadása kötelező!")]
        public string AccountNumber { get; set; }

        public bool Active { get; set; }

        public string ActiveSort { get; internal set; }

        public List<Press> Presses { get; set; }

        public string NameSort { get; internal set; }

        public string CitySort { get; set; }

        public string AddressSort { get; set; }

        public string ZipSort { get; set; }

        public string CountrySort { get; set; }

        public int AllPressCount { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllPressCount].ToList();
            }
        }

        public string CurrentFilter { get; internal set; }        

        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllPressCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
    }
}