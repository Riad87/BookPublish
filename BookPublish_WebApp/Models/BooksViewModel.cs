using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookPublish_WebApp.Models
{
    public class BooksViewModel
    {
        public string ActiveSort { get; internal set; }

        public List<Books> Books { get; set; }

        [Required(ErrorMessage ="Szerző kiválasztása kötelező!")]
        public int SelectedAuthorID { get; set; }

        [Required(ErrorMessage ="Borító kiválasztása kötelező!")]
        public int SelectedCoverID { get; set; }

        [Required(ErrorMessage ="Téma kiválasztása kötelező!")]
        public int SelectedThemeID { get; set; }

        public SelectList AuthorsName { get; set; }

        public SelectList CoverNames { get; set; }

        public SelectList ThemeNames { get; set; }

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

        [Required(ErrorMessage ="Könyv címének kitöltése kötelező!")]
        public string Name { get; set; }

        public bool Deleted { get; set; }

        public int ID { get; set; }

        public string ISBNSort { get; set; }

        [Required(ErrorMessage ="ISBN megadása kötelező!")]
        public string ISBN { get; set; }

        public string ItemNumSort { get; set; }

        public int ItemNumber { get; set; }

        public string NetValueSort { get; set; }

        public int NetValue { get; set; }

        public string VatSort { get; set; }

        public int Vat { get; set; }

        public string GrossValueSort { get; set; }

        public int GrossValue { get; set; }

        public string CoverTypeSort { get; set; }

        public string CoverType { get; set; }

        public string ThemeTypeSort { get; set; }

        public string ThemeType { get; set; }

        public string AuthorNameSort { get; set; }

        public string AuthorName { get; set; }

        public string ValidFromSort { get; set; }

        [Required(ErrorMessage ="Érvényesség kezdete kötelező!")]
        [DataType(DataType.Date)]
        public DateTime ValidFrom { get; set; }
       
        public string ValidToSort { get; set; }

        [Required(ErrorMessage ="Érvényesség vége kötelező!")]
        [DataType(DataType.Date)]
        public DateTime ValidTo { get; set; }

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