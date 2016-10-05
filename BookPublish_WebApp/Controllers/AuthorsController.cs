using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using bookPublishDB;

namespace BookPublish_WebApp.Controllers
{
    public class AuthorsController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Authors
        [Authorize]
        public ViewResult Index(string sortorder, string currentFilter, string searchString,int? pagesize, int? page)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.PageSize = pagesize;
            int? DefaultPageSize = 10;
            if (pagesize != null)
            {
                DefaultPageSize = pagesize;
            }
            ViewBag.NameSort = String.IsNullOrEmpty(sortorder) ? "name_desc" : "";
            ViewBag.ActiveSort = sortorder == "active" ? "act_desc" : "active";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            
            ViewBag.CurrentFilter = searchString;

            var Authors = from a in db.Authors
                          select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                Authors = Authors.Where(s => s.AuthorName.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    Authors = Authors.OrderByDescending(s => s.AuthorName);
                    break;
                case "active":
                    Authors = Authors.OrderBy(s => s.Active);
                    break;
                case "act_desc":
                    Authors = Authors.OrderByDescending(s => s.Active);
                    break;
                default:
                    Authors = Authors.OrderBy(s => s.AuthorName);
                    break;
            }

            int pageNumber = (page ?? 1);

            return View(Authors.ToPagedList(pageNumber,(int)DefaultPageSize));
        }

        // GET: Authors/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = await db.Authors.FindAsync(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,AuthorName,Acitve")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = await db.Authors.FindAsync(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,AuthorName,Active")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = await db.Authors.FindAsync(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult DeleteConfirmed()
        {
            var forDelete = db.Authors
                .OrderByDescending(x => x.AuthorName)
                .ToList()
                .Where(x => x.IsDeleted == true);             

            db.Authors.RemoveRange(forDelete);
            db.SaveChanges();

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
