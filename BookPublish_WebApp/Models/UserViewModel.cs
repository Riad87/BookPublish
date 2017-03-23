using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BookPublish_WebApp.Models;
using System.Web.Mvc;

namespace BookPublish_WebApp.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "E-mail cím megadása kötelező!")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Nem megfelelő az e-mail cím formátuma.")]
        [Display(Name ="E-mail cím")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Felhasználó név megadása kötelező!")]
        [Display(Name ="Felhasználó név")]
        public string UserName { get; set; }                  

        [Required(ErrorMessage = "Jelszó megadása kötelező!")]
        [StringLength(100, ErrorMessage = "A jelszónak legalább {2} karakter hosszúnak kell lennie.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Jelszó megerősítés")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "A jelszó és az ellenőrző jelszó nem egyezik!")]
        public string ConfirmPassword { get; set; }

        public string ID { get; set; }

        public SelectList AllRoles { get; set; }

        [Required(ErrorMessage = "Szerepkör megadása kötelező")]
        [Display(Name = "Szerepkör")]
        public string SelectedRoleID { get; set; }

        public Dictionary<string,string> RoleMappingForUsers { get; set; }      

        public List<ApplicationUser> Users { get; set; }

        public int AllUserCount { get; set; }        

        public string CurrentFilter { get; internal set; }

        public string NameSort { get; internal set; }

        public string EmailSort { get; set; }
        
        public int PageSize { get; set; }

        public string SortOrder { get; set; }

        public List<object> PagerList
        {
            get
            {
                return new object[AllUserCount].ToList();
            }
        }

        public int PageCount
        {
            get
            {
                return (int)(Math.Ceiling((float)(AllUserCount) / (float)PageSize));
            }
        }

        public int PageNumber { get; set; }

        public string CurrentSort { get; set; }
    }
}