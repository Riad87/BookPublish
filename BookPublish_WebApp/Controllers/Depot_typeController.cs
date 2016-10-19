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
    public class Depot_typeController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Depot_type
        [HttpGet]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DepotTypeViewModel depotypeViewModel)
        {
            foreach (var depot_t in depotypeViewModel.Depot_type)
            {
                if (depot_t.IsDeleted == true)
                {
                    Depot_type d = (from x in _db.Depot_types
                                where x.Type == depot_t.Type
                                select x).First();
                    d.Deleted = true;
                    _db.SaveChanges();
                }
            }

            var model = GetModel(depotypeViewModel.SortOrder, depotypeViewModel.CurrentFilter, null, depotypeViewModel.PageSize, null);

            foreach (var depot_t in model.Depot_type)
            {
                depot_t.IsDeleted = false;
            }

            return View(model);
        }

        public DepotTypeViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new DepotTypeViewModel();

            model.SortOrder = sortorder;
                        
            int defaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = defaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;
            model.PageNumber = actualPage;

            model.TypeSort = String.IsNullOrEmpty(model.SortOrder) ? "type_desc" : "";
            model.IDSort = model.SortOrder == "id_asc" ? "id_desc" : "id_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            model.CurrentFilter = searchString;

            var depot_t = from d in _db.Depot_types
                          where d.Deleted != true
                          select d;

            model.AllDepotTypesCount = depot_t.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                depot_t = depot_t.Where(s => s.Type.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    depot_t = depot_t.OrderByDescending(s => s.Type);
                    break;
                case "asc":
                    depot_t = depot_t.OrderBy(s => s.ID);
                    break;
                case "act_desc":
                    depot_t = depot_t.OrderByDescending(s => s.ID);
                    break;
                default:
                    depot_t = depot_t.OrderBy(s => s.Type);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.Depot_type = depot_t.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }


        // GET: Depot_type/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot_type depot_type = await _db.Depot_types.FindAsync(id);
            if (depot_type == null)
            {
                return HttpNotFound();
            }
            return View(depot_type);
        }

        // GET: Depot_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Depot_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Type")] Depot_type depot_type)
        {
            if (ModelState.IsValid)
            {
                _db.Depot_types.Add(depot_type);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(depot_type);
        }

        // GET: Depot_type/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot_type depot_type = await _db.Depot_types.FindAsync(id);
            if (depot_type == null)
            {
                return HttpNotFound();
            }
            return View(depot_type);
        }

        // POST: Depot_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Type")] Depot_type depot_type)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(depot_type).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(depot_type);
        }

        // GET: Depot_type/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot_type depot_type = await _db.Depot_types.FindAsync(id);
            if (depot_type == null)
            {
                return HttpNotFound();
            }
            return View(depot_type);
        }

        // POST: Depot_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Depot_type depot_type = await _db.Depot_types.FindAsync(id);
            _db.Depot_types.Remove(depot_type);
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
    }
}
