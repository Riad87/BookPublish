using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookPublish_WebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace BookPublish_WebApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserManagerController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext ApplicationDb;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager //RoleManager beszerzése OWIN-tól
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public ApplicationDbContext _db
        {
            get
            {
                return ApplicationDb ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }

            set
            {
                ApplicationDb = value;
            }
        }

        [HttpGet]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        public UserViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new UserViewModel();


            model.SortOrder = sortorder;

            int defaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = defaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;
            model.PageNumber = actualPage;

            model.NameSort = String.IsNullOrEmpty(model.SortOrder) ? "name_desc" : "";
            model.EmailSort = model.SortOrder == "email_asc" ? "email_desc" : "email_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;


            model.CurrentFilter = searchString;

            var users = UserManager.Users.ToList();

            model.AllUserCount = users.Count();

            model.AllRoles = new SelectList(RoleManager.Roles, "ID", "Name", 0);


            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.Contains(searchString)).ToList();
            }

            switch (sortorder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.UserName).ToList();
                    break;
                case "email_asc":
                    users = users.OrderBy(s => s.Email).ToList();
                    break;
                case "email_desc":
                    users = users.OrderByDescending(s => s.Email).ToList();
                    break;
                default:
                    users = users.OrderBy(s => s.UserName).ToList();
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.Users = users.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);

            var roles = UserManager.GetRoles(id).FirstOrDefault();

            var roleSelectedID = RoleManager.FindByName(roles.ToString());

            var userview = new UserViewModel();

            userview.ID = user.Id;
            userview.UserName = user.UserName;
            userview.Email = user.Email;
            userview.Password = "000000_A"; // nem lesz elmentve csak a validáció miatt kap értéket
            userview.ConfirmPassword = "000000_A";  // nem lesz elmentve csak a validáció miatt kap értéket

            userview.AllRoles = new SelectList(RoleManager.Roles, "Id", "Name", roleSelectedID.Id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return PartialView("_partialEdit", userview);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserName,Email,Password,ConfirmPassword,SelectedRoleID")] UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userfromDb = _db.Users.Find(viewModel.ID); // lekérdezem az eredeti objektumot az adatbázisból, ezzen fogja érzékelni az EF a navigációs property változását.

                userfromDb.Email = viewModel.Email;
                userfromDb.UserName = viewModel.UserName;

                var role = await RoleManager.FindByIdAsync(viewModel.SelectedRoleID);

                if (role != null)
                {

                    var deleterole = await UserManager.RemoveFromRoleAsync(viewModel.ID, role.Name);
                    var roleresult = await UserManager.AddToRoleAsync(viewModel.ID, role.Name);
                    if (roleresult.Succeeded && deleterole.Succeeded)
                    {
                        var userresult = await UserManager.UpdateAsync(userfromDb);
                        await _db.SaveChangesAsync();

                        if (userresult.Succeeded)
                        {
                            return Json(new { success = true });
                        }
                        AddErrors(userresult);
                        // Ha nem sikerült frissíteni a user adatait.
                        return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);

                    }
                    //Ha nem sikerült a rolehoz rendelés akkor dobja ezt a hibát.
                    AddErrors(roleresult);
                    return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);
                }
                // Ha nem létezik az adott role akkor dobja ezt.               
                return Json(new { success = false, errors = "Nincs ilyen role" }, JsonRequestBehavior.AllowGet);
            }

            // Ha nem valid a modelState, akkor kidobja a validációs hibákat
            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.DenyGet);

        }

        public ActionResult ChangePassword(string id)
        {
            var userviewModel = new UserViewModel();

            var user = UserManager.FindById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            userviewModel.ID = user.Id;
            userviewModel.UserName = user.UserName;
            userviewModel.Email = user.Email;
            userviewModel.SelectedRoleID = "0";


            return PartialView("_partialChangePassword", userviewModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Include = "ID, UserName,Email,SelectedRoleID, Password, ConfirmPassword")]UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(viewModel.ID);

                if (user != null)
                {
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(viewModel.Password);
                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        AddErrors(result);
                        return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return HttpNotFound();
            }

            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult CreateUser()
        {
            var viewModel = new UserViewModel();

            viewModel.AllRoles = new SelectList(RoleManager.Roles, "Id", "Name");

            return PartialView("_partialCreate", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser([Bind(Include = "UserName,Email,SelectedRoleID,Password,ConfirmPassword")] UserViewModel viewModel) //Aszinkron metódus hívásnál nem adódik vissza modelnek defaultban a hibaüzenet, ezért Ajax hívással adom vissza a viewnak és listázom ki az esetleges hibákat!!! Scripts lásd scripts.js
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.UserName, Email = viewModel.Email };
                var result = await UserManager.CreateAsync(user, viewModel.Password);
                var getuserid = await UserManager.FindByNameAsync(viewModel.UserName);
                var role = await RoleManager.FindByIdAsync(viewModel.SelectedRoleID);

                if (result.Succeeded)
                {
                    var roleresult = await UserManager.AddToRoleAsync(getuserid.Id, role.Name);
                    if (roleresult.Succeeded)
                    {
                        return Json(new { success = true });
                    }
                    AddErrors(roleresult);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IdentityResult userResult;
            var user = UserManager.FindById(Id);

            userResult = UserManager.Delete(user);
            if (userResult.Succeeded)
            {
                return RedirectToAction("Index");
            }

            AddErrors(userResult);


            return RedirectToAction("Index", ModelState);


        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

        }

        public List<string> GetModelStateErrors(ModelStateDictionary ModelState)
        {
            List<string> errorMessages = new List<string>();

            var validationErrors = ModelState.Values.Select(x => x.Errors);
            validationErrors.ToList().ForEach(ve =>
            {
                var errorStrings = ve.Select(x => x.ErrorMessage);
                errorStrings.ToList().ForEach(em =>
                {
                    errorMessages.Add(em);
                });
            });

            return errorMessages;
        }

        #endregion
    }
}