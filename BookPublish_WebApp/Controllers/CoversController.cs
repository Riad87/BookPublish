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
using System.Diagnostics;

namespace BookPublish_WebApp.Controllers
{
    [Authorize]
    public class CoversController : Controller
    {
        private BookContext _db = new BookContext();

        [HttpGet]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CoversViewModel coversViewModel)
        {
            //TODO: logikai törlés
            foreach (var cover in coversViewModel.Covers)
            {
                if (cover.IsDeleted == true)
                {
                    try
                    {
                        Cover c = (from x in _db.Covers
                                   where x.ID == cover.ID
                                   select x).First();
                        c.Deleted = true;
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceInformation(ex.Message);
                    }
                    
                }
            }

            var model = GetModel(coversViewModel.SortOrder, coversViewModel.CurrentFilter, null, coversViewModel.PageSize, null);

            foreach (var cover in model.Covers)
            {
                cover.IsDeleted = false;
            }

            return View(model);
        }             
        
        public CoversViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new CoversViewModel();

            model.CurrentSort = sortorder;
           
            int DefaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = DefaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;
            model.PageNumber = actualPage;

            model.CoverSort = String.IsNullOrEmpty(sortorder) ? "name_desc" : "";
            model.ActiveSort = sortorder == "active" ? "active_desc" : "active";

            if (searchString != null)
            {
                page = 1;
            }
            else
                searchString = currentFilter;

            model.CurrentFilter = searchString;

            var Covers = from c in _db.Covers
                         where c.Deleted != true
                         select c;

            model.AllCoversCount = Covers.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                Covers = Covers.Where(x => x.CoverName.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    Covers = Covers.OrderByDescending(x => x.CoverName);
                    break;
                case "active":
                    Covers = Covers.OrderBy(x => x.Active);
                    break;
                case "active_desc":
                    Covers = Covers.OrderByDescending(x => x.Active);
                    break;
                default:
                    Covers = Covers.OrderBy(x => x.CoverName);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.Covers = Covers.Skip((actualPage - 1) * DefaultPageSize).Take(DefaultPageSize).ToList();

            return model;
        }

        
        // GET: Cover/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cover cover = await _db.Covers.FindAsync(id);
            if (cover == null)
            {
                return HttpNotFound();
            }
            return View(cover);
        }

        // GET: Cover/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cover/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CoverName,Active")] CoversViewModel viewModel)
        {
            Cover cover = new Cover();
            cover.Active = viewModel.Active;
            cover.CoverName = viewModel.CoverName;
           
            if (ModelState.IsValid)
            {
                _db.Covers.Add(cover);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Cover/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cover cover = await _db.Covers.FindAsync(id);
            if (cover == null)
            {
                return HttpNotFound();
            }
            return PartialView("_partialEdit", cover);
        }

        // POST: Cover/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CoverName,Active")] Cover cover)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(cover).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cover);
        }

        // GET: Cover/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cover cover = await _db.Covers.FindAsync(id);
            if (cover == null)
            {
                return HttpNotFound();
            }
            return View(cover);
        }

        // POST: Cover/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cover cover = await _db.Covers.FindAsync(id);
            _db.Covers.Remove(cover);
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
