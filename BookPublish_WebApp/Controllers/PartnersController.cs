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

namespace BookPublish_WebApp.Controllers
{
    [Authorize]
    public class PartnersController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Partner
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        // GET: Partner/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = await _db.Partners.FindAsync(id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        [HttpPost]
        public ActionResult Index(PartnersViewModel partnersViewModel)
        {
            foreach (var partner in partnersViewModel.Partners)
            {
                if (partner.IsDeleted == true)
                {
                    Partner p = (from x in _db.Partners
                                 where x.Name == partner.Name
                                 select x).First();
                    p.Deleted = true;
                    _db.SaveChanges();
                }
            }

            var model = GetModel(partnersViewModel.SortOrder, partnersViewModel.CurrentFilter, null, partnersViewModel.PageSize, null);

            foreach (var partner in model.Partners)
            {
                partner.IsDeleted = false;
            }

            return View(model);
        }

        public PartnersViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new PartnersViewModel();

            model.SortOrder = sortorder;

            //TODO: egyszerűsíteni
            int defaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = defaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;
            model.PageNumber = actualPage;

            model.NameSort = String.IsNullOrEmpty(model.SortOrder) ? "name_desc" : "";
            model.ActiveSort = model.SortOrder == "active" ? "act_desc" : "active";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;


            model.CurrentFilter = searchString;

            var partners = from p in _db.Partners
                           where p.Deleted != true
                           select p;

            model.AllPartnerCount = partners.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                partners = partners.Where(s => s.Name.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    partners = partners.OrderByDescending(s => s.Name);
                    break;
                case "active":
                    partners = partners.OrderBy(s => s.Active);
                    break;
                case "act_desc":
                    partners = partners.OrderByDescending(s => s.Active);
                    break;
                default:
                    partners = partners.OrderBy(s => s.Name);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.Partners = partners.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }

        // GET: Partner/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(_db.AccountTypes, "ID", "Details");
            return View();
        }

        // POST: Partner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Active")] PartnersViewModel viewModel)
        {
            Partner partner = new Partner();
            partner.Name = viewModel.Name;
            partner.Active = viewModel.Active;

            if (ModelState.IsValid)
            {
                _db.Partners.Add(partner);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ID = new SelectList(_db.AccountTypes, "ID", "Details", viewModel.ID);
            return View(viewModel);
        }

        // GET: Partner/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = await _db.Partners.FindAsync(id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            return PartialView("_partialEdit", partner);
        }

        // POST: Partner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Active")] Partner partner)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(partner).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        // GET: Partner/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = await _db.Partners.FindAsync(id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        // POST: Partner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Partner partner = await _db.Partners.FindAsync(id);
            _db.Partners.Remove(partner);
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
