using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BookPublish_WebApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Emlékezz a böngészőre.")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {     
        [Required]
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [Display(Name = "Emlékezz rám.")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name ="Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A jelszó {0} legalább {2} karakter hosszúnak kell lennie.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Jelszó megerősítés")]
        [Compare("Password", ErrorMessage = "A jelszó és az ellenőrző jelszó nem egyezik!")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A jelszó {0} legalább {2} karakter hosszúnak kell lennie.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Jelszó megerősítés")]
        [Compare("Password", ErrorMessage = "A jelszó és az ellenőrző jelszó nem egyezik!")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RoleViewModel
    {
        [Display(Name ="Szerepkör neve")]
        [Required(ErrorMessage = "Role nevének megadása kötelező!")]
        public string RoleName { get; set; }

        public bool isDeleted { get; set; }

        public string ID { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public int AllUserCount { get; set; }        

        public string CurrentFilter { get; internal set; }

        public string NameSort { get; internal set; }        

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
