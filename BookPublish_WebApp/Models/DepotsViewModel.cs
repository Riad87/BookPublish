using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookPublish_WebApp.Models
{
    public class DepotsViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="A raktár nevének megadása kötelező!")]
        public string DepotName { get; set; }

        [Required(ErrorMessage ="A város megadása kötelező!")]
        public string City { get; set; }

        [Required(ErrorMessage ="A cím megadása kötelező!")]
        public string Address { get; set; }

        [Required(ErrorMessage ="Az irányítószám megadása kötelező!")]
        public int Zip { get; set; }
        
        public SelectList AllTypes { get; set; }

        [Required(ErrorMessage ="Típus kiválasztása kötelező!")]
        public int SelectedTypeID { get; set; }

        public SelectList AllPartner { get; set; }

        [Required(ErrorMessage ="Partner kiválasztása kötelező!")]
        public int SelectedPartnerID { get; set; }

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