using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using bookPublishDB;
using BookPublish_WebApp.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace BookPublish_WebApp.Controllers
{
    public class PressesController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Press
        [HttpGet]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PressesViewModel pressesViewModel)
        {
            foreach (var press in pressesViewModel.Presses)
            {
                if (press.IsDeleted == true)
                {
                    try
                    {
                        Press p = _db.Press
                                   .Where(x => x.ID == press.ID)
                                  .Include(pr => pr.Pressure).First();
                        p.Deleted = true;
                        _db.SaveChanges();
                    }

                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                                        validationError.PropertyName,
                                                        validationError.ErrorMessage);
                            }
                        }
                    }
                }
            }

            var model = GetModel(pressesViewModel.SortOrder, pressesViewModel.CurrentFilter, null, pressesViewModel.PageSize, null);

            foreach (var press in model.Presses)
            {
                press.IsDeleted = false;
            }

            return View(model);
        }

        public PressesViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new PressesViewModel();

            model.SortOrder = sortorder;

            //TODO: egyszerűsíteni
            int defaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = defaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;
            model.PageNumber = actualPage;

            model.NameSort = String.IsNullOrEmpty(model.SortOrder) ? "name_desc" : "";
            model.ActiveSort = model.SortOrder == "active" ? "act_desc" : "active";
            model.CitySort = model.SortOrder == "city_asc" ? "city_desc" : "city_asc";
            model.AddressSort = model.SortOrder == "address_asc" ? "address_desc" : "address_asc";
            model.ZipSort = model.SortOrder == "zip_asc" ? "zip_desc" : "zip_asc";
            model.CountrySort = model.SortOrder == "country_asc" ? "country_desc" : "country_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            model.CurrentFilter = searchString;

            var presses = from p in _db.Press
                          where p.Deleted != true
                          select p;

            model.AllPressCount = presses.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                presses = presses.Where(s => s.Name.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    presses = presses.OrderByDescending(s => s.Name);
                    break;
                case "active":
                    presses = presses.OrderBy(s => s.Active);
                    break;
                case "act_desc":
                    presses = presses.OrderByDescending(s => s.Active);
                    break;
                case "city_asc":
                    presses = presses.OrderBy(s => s.City);
                    break;
                case "city_desc":
                    presses = presses.OrderByDescending(s => s.City);
                    break;
                case "address_asc":
                    presses = presses.OrderBy(s => s.Address);
                    break;
                case "address_desc":
                    presses = presses.OrderByDescending(s => s.Address);
                    break;
                case "zip_asc":
                    presses = presses.OrderBy(s => s.Zip);
                    break;
                case "zip_desc":
                    presses = presses.OrderByDescending(s => s.Zip);
                    break;
                case "country_asc":
                    presses = presses.OrderBy(s => s.Country);
                    break;
                case "country_desc":
                    presses = presses.OrderByDescending(s => s.Country);
                    break;
                default:
                    presses = presses.OrderBy(s => s.Name);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.Presses = presses.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }
        // GET: Press/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Press press = await _db.Press.FindAsync(id);
            if (press == null)
            {
                return HttpNotFound();
            }
            return View(press);
        }

        // GET: Press/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Press/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PressName,City,Address,Zip,Country,TaxNumber,AccountNumber")] PressesViewModel viewModel)
        {
            Press press = new Press();

            press.AccountNumber = viewModel.AccountNumber;
            press.Address = viewModel.Address;
            press.City = viewModel.City;
            press.Country = viewModel.Country;
            press.Name = viewModel.PressName;
            press.Zip = viewModel.Zip;
            press.TaxNumber = viewModel.TaxNumber;

            if (ModelState.IsValid)
            {
                _db.Press.Add(press);

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Press/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Press press = await _db.Press.FindAsync(id);
            if (press == null)
            {
                return HttpNotFound();
            }
            return PartialView("_partialEdit", press);
        }

        // POST: Press/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,City,Address,Zip,Country,TaxNumber,AccountNumber,Active")] Press press)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(press).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        // GET: Press/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Press press = await _db.Press.FindAsync(id);
            if (press == null)
            {
                return HttpNotFound();
            }
            return View(press);
        }

        // POST: Press/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Press press = await _db.Press.FindAsync(id);
            _db.Press.Remove(press);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
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
    }
}
