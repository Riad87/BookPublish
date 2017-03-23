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
    public class ThemesController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Themes
        [HttpGet]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ThemesViewModel themesViewModel)
        {
            foreach (var theme in themesViewModel.Themes)
            {
                if (theme.IsDeleted == true)
                {
                    Theme a = (from x in _db.Themes
                                where x.ID == theme.ID
                                select x).First();
                    a.Deleted = true;
                    _db.SaveChanges();
                }
            }

            var model = GetModel(themesViewModel.SortOrder, themesViewModel.CurrentFilter, null, themesViewModel.PageSize, null);

            foreach (var theme in model.Themes)
            {
                theme.IsDeleted = false;
            }

            return View(model);
        }

        public ThemesViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new ThemesViewModel();

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

            var themes = from a in _db.Themes
                          where a.Deleted != true
                          select a;

            model.AllThemeCount = themes.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                themes = themes.Where(s => s.ThemeName.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    themes = themes.OrderByDescending(s => s.ThemeName);
                    break;
                case "active":
                    themes = themes.OrderBy(s => s.Active);
                    break;
                case "act_desc":
                    themes = themes.OrderByDescending(s => s.Active);
                    break;
                default:
                    themes = themes.OrderBy(s => s.ThemeName);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.Themes = themes.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }

        // GET: Themes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme theme = await _db.Themes.FindAsync(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            return View(theme);
        }

        // GET: Themes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Themes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ThemeName,Active")] ThemesViewModel viewModel)
        {
            Theme theme = new Theme();
            theme.Active = viewModel.Active;
            theme.ThemeName = viewModel.ThemeName;

            if (ModelState.IsValid)
            {
                _db.Themes.Add(theme);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Themes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme theme = await _db.Themes.FindAsync(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            return PartialView("_partialEdit", theme);
        }

        // POST: Themes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ThemeName,Active")] Theme theme)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(theme).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }
        // GET: Themes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme theme = await _db.Themes.FindAsync(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            return View(theme);
        }

        // POST: Themes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Theme theme = await _db.Themes.FindAsync(id);
            _db.Themes.Remove(theme);
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
