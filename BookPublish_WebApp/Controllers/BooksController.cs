using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using bookPublishDB;
using BookPublish_WebApp.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Collections.Generic;

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

            model.NameSort = String.IsNullOrEmpty(sortorder) ? "name_desc" : "";
            model.ISBNSort = sortorder == "isbn" ? "isbn_desc" : "isbn";
            model.ItemNumSort = sortorder == "item" ? "item_desc" : "item";
            model.NetValueSort = sortorder == "netV" ? "netV_desc" : "netV";
            model.VatSort = sortorder == "Vat" ? "Vat_desc" : "Vat";
            model.GrossValueSort = sortorder == "GrossValue" ? "GrossValue_desc" : "GrossValue";
            model.ValidFromSort = sortorder == "ValidFrom" ? "ValidFrom_desc" : "ValidFrom";
            model.ValidToSort = sortorder == "ValidTo" ? "ValidTo_desc" : "ValidTo";
            model.CoverTypeSort = sortorder == "Cover_asc" ? "Cover_desc" : "Cover_asc";
            model.ThemeTypeSort = sortorder == "Theme_asc" ? "Theme_desc" : "Theme_asc";
            model.AuthorNameSort = sortorder == "Author_asc" ? "Author_desc" : "Author_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            model.CurrentFilter = searchString;

            //IEnumerable<SelectedListItem> authors = _db.Authors
            //              .Where(a => a.Delete != true)
            //              .Select(a=> new SelectedListItem { Text = a.AuthorName.ToString(), Value = a.ID }).ToList();

            model.AuthorsName = new SelectList(_db.Authors.Where(a => a.Delete != true), "ID", "AuthorName", 0);

            model.CoverNames = new SelectList(_db.Covers.Where(c => c.Deleted != true), "ID", "CoverName", 0);

            model.ThemeNames = new SelectList(_db.Themes.Where(c => c.Deleted != true), "ID", "ThemeName", 0);

            var books = _db.Books
                        .Include(a => a.Author)
                        .Include(c => c.Cover)
                        .Include(t => t.Theme)
                        .Where(b => b.Deleted != true);


            model.AllBooksCount = books.Count();

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
                case "Cover_asc":
                    books = books.OrderBy(s => s.Cover.CoverName);
                    break;
                case "Cover_desc":
                    books = books.OrderByDescending(s => s.Cover.CoverName);
                    break;
                case "Theme_asc":
                    books = books.OrderBy(s => s.Theme.ThemeName);
                    break;
                case "Theme_desc":
                    books = books.OrderByDescending(s => s.Theme.ThemeName);
                    break;
                case "Author_asc":
                    books = books.OrderBy(s => s.Author.AuthorName);
                    break;
                case "Author_desc":
                    books = books.OrderByDescending(s => s.Author.AuthorName);
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
            Books books = _db.Books.Find(id);
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
        public ActionResult Create([Bind(Include = "ISBN,Name,SelectedAuthorID,SelectedCoverID,SelectedThemeID,ItemNumber,NetValue,Vat,GrossValue,ValidFrom,ValidTo")] BooksViewModel viewmodel)
        {
            Books book = new Books();

            book.ISBN = viewmodel.ISBN.ToString();
            book.Name = viewmodel.Name;
            book.Author = _db.Authors.Find(viewmodel.SelectedAuthorID);
            book.Cover = _db.Covers.Find(viewmodel.SelectedCoverID);
            book.Theme = _db.Themes.Find(viewmodel.SelectedThemeID);
            book.ItemNumber = viewmodel.ItemNumber;
            book.NetValue = viewmodel.NetValue;
            book.Vat = viewmodel.Vat;
            book.GrossValue = viewmodel.GrossValue;
            book.ValidFrom = (DateTime)viewmodel.ValidFrom;
            book.ValidTo = (DateTime)viewmodel.ValidTo;

            if (ModelState.IsValid)
            {
                _db.Books.Add(book);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var book = _db.Books.Where(b => b.ID == id)
                        .Include(a => a.Author)
                        .Include(c => c.Cover)
                        .Include(t => t.Theme).FirstOrDefault();

            if (book == null)
            {
                return HttpNotFound();
            }
            int selectedAuthorID = book.Author == null ? 0 : book.Author.ID;
            int selectedCoverID = book.Cover == null ? 0 : book.Cover.ID;
            int selectedThemeID = book.Theme == null ? 0 : book.Theme.ID;

            var booksview = new BooksViewModel();
            booksview.Name = book.Name;
            booksview.ISBN = book.ISBN;
            booksview.ItemNumber = book.ItemNumber;
            booksview.NetValue = book.NetValue;
            booksview.Vat = book.Vat;
            booksview.GrossValue = book.GrossValue;
            booksview.ValidFrom = book.ValidFrom;
            booksview.ValidTo = book.ValidTo;
            booksview.Deleted = book.Deleted;
            booksview.SelectedAuthorID = selectedAuthorID;
            booksview.SelectedCoverID = selectedCoverID;
            booksview.SelectedThemeID = selectedThemeID;

            booksview.AuthorsName = new SelectList(_db.Authors, "ID", "AuthorName", selectedAuthorID);

            booksview.CoverNames = new SelectList(_db.Covers, "ID", "CoverName", selectedCoverID);

            booksview.ThemeNames = new SelectList(_db.Themes, "ID", "ThemeName", selectedThemeID);

            return PartialView("_partialEdit", booksview);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ISBN,Name,ItemNumber,NetValue,Vat,ValidFrom,ValidTo,Deleted,SelectedAuthorID,SelectedCoverID,SelectedThemeID")] BooksViewModel viewmodel)
        {
            var bookfromdb = _db.Books.Find(viewmodel.ID); // lekérdezem az eredeti objektumot az adatbázisból, ezzen fogja érzékelni az EF a navigációs property változását.

            bookfromdb.ID = viewmodel.ID;
            bookfromdb.ISBN = viewmodel.ISBN;
            bookfromdb.Name = viewmodel.Name;
            bookfromdb.Author = _db.Authors.Find(viewmodel.SelectedAuthorID);
            bookfromdb.Cover = _db.Covers.Find(viewmodel.SelectedCoverID);
            bookfromdb.Theme = _db.Themes.Find(viewmodel.SelectedThemeID);
            bookfromdb.ItemNumber = viewmodel.ItemNumber;
            bookfromdb.NetValue = viewmodel.NetValue;
            bookfromdb.Vat = viewmodel.Vat;
            bookfromdb.GrossValue = 0;
            bookfromdb.ValidFrom = (DateTime)viewmodel.ValidFrom;
            bookfromdb.ValidTo = (DateTime)viewmodel.ValidTo;
            bookfromdb.Deleted = viewmodel.Deleted;

            if (ModelState.IsValid)
            {
                //_db.Entry(bookfromdb).State = EntityState.Modified; // nem alkalmas navigation property mentésére, csak skaláris értékek mentésére.

                _db.Configuration.AutoDetectChangesEnabled = true; // ezért használom ezt a megoldást, hogy a befrissítse az EF a változó navigációs property-t

                _db.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books book = _db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
        #region Helpers

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
