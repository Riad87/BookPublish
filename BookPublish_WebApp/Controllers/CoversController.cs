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
using PagedList;

namespace BookPublish_WebApp.Controllers
{
    public class CoversController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Cover
        [Authorize]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.PageSize = pagesize;
            int? DefaultPageSize = 10;

            if (pagesize != null)
            {
                DefaultPageSize = pagesize;
            }

            ViewBag.CoverName = String.IsNullOrEmpty(sortorder) ? "name_desc" : "";
            ViewBag.Active = sortorder == "active" ? "active_desc" : "active";

            if (searchString != null)
            {
                page = 1;
            }
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var Covers = from c in db.Covers
                         select c;

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

            int pagenumber = (page ?? 1);

            return View(Covers.ToPagedList(pagenumber,(int)DefaultPageSize));
        }

        // GET: Cover/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cover cover = await db.Covers.FindAsync(id);
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
        public async Task<ActionResult> Create([Bind(Include = "ID,CoverName,Active")] Cover cover)
        {
            if (ModelState.IsValid)
            {
                db.Covers.Add(cover);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cover);
        }

        // GET: Cover/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cover cover = await db.Covers.FindAsync(id);
            if (cover == null)
            {
                return HttpNotFound();
            }
            return View(cover);
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
                db.Entry(cover).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
            Cover cover = await db.Covers.FindAsync(id);
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
            Cover cover = await db.Covers.FindAsync(id);
            db.Covers.Remove(cover);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
