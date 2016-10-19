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
using BookPublish_WebApp.Models;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

namespace BookPublish_WebApp.Controllers
{
    public class BooksController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Books
        [Authorize]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(BooksViewModel booksViewModel)
        {
            //TODO: logikai törlés
            foreach (var book in booksViewModel.Books)
            {
                if (book.IsDeleted == true)
                {
                    try
                    {
                        Books b = _db.Books
                                   .Where(x => x.ID == book.ID)
                                   .Include(a => a.Author).First();
                        b.Deleted = true;
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

            var model = GetModel(booksViewModel.SortOrder, booksViewModel.CurrentFilter, null, booksViewModel.PageSize, null);

            foreach (var books in model.Books)
            {
                books.IsDeleted = false;
            }

            return View(model);
        }
        public BooksViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new BooksViewModel();

            model.CurrentSort = sortorder;

            int DefaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = DefaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;

            //model.CurrentPageSize
            model.NameSort = String.IsNullOrEmpty(sortorder) ? "name_desc" : "";
            model.ISBN = sortorder == "isbn" ? "isbn_desc" : "isbn";
            model.ItemNum = sortorder == "item" ? "item_desc" : "item";
            model.NetValue = sortorder == "netV" ? "netV_desc" : "netV";
            model.Vat = sortorder == "Vat" ? "Vat_desc" : "Vat";
            model.GrossValue = sortorder == "GrossValue" ? "GrossValue_desc" : "GrossValue";
            model.ValidFrom = sortorder == "ValidFrom" ? "ValidFrom_desc" : "ValidFrom";
            model.ValidTo = sortorder == "ValidTo" ? "ValidTo_desc" : "ValidTo";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;


            model.CurrentFilter = searchString;

            //var books = _db.Books
            //            .Join(_db.Pressure, b => b.ID, p => p.ID, (b, p) => new { Books = b, Pressure = p })
            //            .Where(bp => bp.Books.ID == bp.Pressure.Book.ID)
            //            .Include(a => a.Author);

            var books = _db.Books
                        .Include(a => a.Author)
                        .Where(b => b.Deleted != true);
                        

            model.AllBooksCount = books.Count();            

            //model.Quantity = _db.Database.ExecuteSqlCommand("GetBookQuantity",new SqlParameter("@BookID",4));

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Name.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    books = books.OrderByDescending(s => s.Name);
                    break;
                case "isbn":
                    books = books.OrderBy(s => s.ISBN);
                    break;
                case "isbn_desc":
                    books = books.OrderByDescending(s => s.ISBN);
                    break;
                case "item":
                    books = books.OrderBy(s => s.ItemNumber);
                    break;
                case "item_desc":
                    books = books.OrderByDescending(s => s.ItemNumber);
                    break;
                case "netV":
                    books = books.OrderBy(s => s.NetValue);
                    break;
                case "netV_desc":
                    books = books.OrderByDescending(s => s.NetValue);
                    break;
                case "Vat":
                    books = books.OrderBy(s => s.Vat);
                    break;
                case "Vat_desc":
                    books = books.OrderByDescending(s => s.Vat);
                    break;
                case "GrossValue":
                    books = books.OrderBy(s => s.GrossValue);
                    break;
                case "GrossValue_desc":
                    books = books.OrderByDescending(s => s.GrossValue);
                    break;
                case "ValidFrom":
                    books = books.OrderBy(s => s.ValidFrom);
                    break;
                case "ValidFrom_desc":
                    books = books.OrderByDescending(s => s.ValidFrom);
                    break;
                case "ValidTo":
                    books = books.OrderBy(s => s.ValidTo);
                    break;
                case "ValidTo_desc":
                    books = books.OrderByDescending(s => s.ValidTo);
                    break;
                default:
                    books = books.OrderBy(s => s.Name);
                    break;
            }

            int pageNumber = (page ?? 1);

            model.PageNumber = pageNumber;

            model.Books = books.Skip((actualPage - 1) * DefaultPageSize).Take(DefaultPageSize).ToList();

            return model;
        }

        // GET: Books/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = (Books)_db.Books.Include(n => n.Author);
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
                _db.Books.Add(books);
                _db.SaveChanges();
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
            Books books = _db.Books.Find(id);
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
                _db.Entry(books).State = EntityState.Modified;
                _db.SaveChanges();
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
            Books books = _db.Books.Find(id);
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
            Books books = _db.Books.Find(id);
            _db.Books.Remove(books);
            _db.SaveChanges();
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
