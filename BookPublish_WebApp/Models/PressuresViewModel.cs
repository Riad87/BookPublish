using System;
using System.Collections.Generic;
using System.Linq;
using bookPublishDB;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookPublish_WebApp.Models
{
    public class PressuresViewModel
    {
        public SelectList AllBooks { get; set; }

        public int ID { get; set; }

        [Display(Name = "Könyv neve")]
        [Required(ErrorMessage = "Könyv kiválasztása kötelező!")]
        public int SelectedBookID { get; set; }
       
        public SelectList AllPresses { get; set; }

        [Display(Name = "Nyomda")]
        [Required(ErrorMessage = "Nyomda kiválasztása kötelező!")]
        public int SelectedPressID { get; set; }
        
        public SelectList AllDepots { get; set; }

        [Display(Name = "Raktár")]
        [Required(ErrorMessage = "Raktár megadása kötelező!")]
        public int SelecetdDepotID { get; set; }

        public int AfterPressure { get; set; }

        [Display(Name = "Megrendelés dátuma")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Rendelés dátumának megadása kötelező!")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Mennyiség")]
        [Required(ErrorMessage ="Mennyiség megadása kötelező!")]
        public int Quantity { get; set; }

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