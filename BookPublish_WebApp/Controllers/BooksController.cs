using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using bookPublishDB;

namespace BookPublish_WebApp.Controllers
{
    public class BooksController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Books
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
            
            //ViewBag.CurrentPageSize
            ViewBag.Name = String.IsNullOrEmpty(sortorder) ? "name_desc" : "";
            ViewBag.ISBN = sortorder == "isbn" ? "isbn_desc" : "isbn";
            ViewBag.ItemNum = sortorder == "item" ? "item_desc" : "item";
            ViewBag.NetValue = sortorder == "netV" ? "netV_desc" : "netV";
            ViewBag.Vat = sortorder == "Vat" ? "Vat_desc" : "Vat";
            ViewBag.GrossValue = sortorder == "GrossValue" ? "GrossValue_desc" : "GrossValue";
            ViewBag.ValidFrom = sortorder == "ValidFrom" ? "ValidFrom_desc" : "ValidFrom";
            ViewBag.ValidTo = sortorder == "ValidTo" ? "ValidTo_desc" : "ValidTo";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;


            ViewBag.CurrentFilter = searchString;

            var Books = from b in db.Books
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                Books = Books.Where(s => s.Name.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    Books = Books.OrderByDescending(s => s.Name);
                    break;
                case "isbn":
                    Books = Books.OrderBy(s => s.ISBN);
                    break;
                case "isbn_desc":
                    Books = Books.OrderByDescending(s => s.ISBN);
                    break;
                case "item":
                    Books = Books.OrderBy(s => s.ItemNumber);
                    break;
                case "item_desc":
                    Books = Books.OrderByDescending(s => s.ItemNumber);
                    break;
                case "netV":
                    Books = Books.OrderBy(s => s.NetValue);
                    break;
                case "netV_desc":
                    Books = Books.OrderByDescending(s => s.NetValue);
                    break;
                case "Vat":
                    Books = Books.OrderBy(s => s.Vat);
                    break;
                case "Vat_desc":
                    Books = Books.OrderByDescending(s => s.Vat);
                    break;
                case "GrossValue":
                    Books = Books.OrderBy(s => s.GrossValue);
                    break;
                case "GrossValue_desc":
                    Books = Books.OrderByDescending(s => s.GrossValue);
                    break;
                case "ValidFrom":
                    Books = Books.OrderBy(s => s.ValidFrom);
                    break;
                case "ValidFrom_desc":
                    Books = Books.OrderByDescending(s => s.ValidFrom);
                    break;
                case "ValidTo":
                    Books = Books.OrderBy(s => s.ValidTo);
                    break;
                case "ValidTo_desc":
                    Books = Books.OrderByDescending(s => s.ValidTo);
                    break;
                default:
                    Books = Books.OrderBy(s => s.Name);
                    break;
            }

            int pageNumber = (page ?? 1);
            
            return View(Books.ToPagedList(pageNumber, (int)DefaultPageSize));
        }

        // GET: Books/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = (Books) db.Books.Include(n => n.Author);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ISBN,Name,ItemNumber,NetValue,Vat,GrossValue,ValidFrom,ValidTo")] Books books)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(books);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(books);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ISBN,Name,ItemNumber,NetValue,Vat,GrossValue,ValidFrom,ValidTo")] Books books)
        {
            if (ModelState.IsValid)
            {
                db.Entry(books).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(books);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Books books = db.Books.Find(id);
            db.Books.Remove(books);
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
