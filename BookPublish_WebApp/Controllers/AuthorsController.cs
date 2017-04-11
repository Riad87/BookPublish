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
using BookPublish_WebApp.Models;
using BookPublish_WebApp.Logging;


namespace BookPublish_WebApp.Controllers
{
    [Authorize(Roles ="Admin,User,PowerUser")]
    public class AuthorsController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Authors   
        [HttpGet]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AuthorsViewModel authorsViewModel)
        {
            foreach (var author in authorsViewModel.Authors)
            {
                if (author.IsDeleted == true)
                {
                    Author a = (from x in _db.Authors
                                where x.ID == author.ID
                                select x).First();
                    a.Delete = true;
                    _db.SaveChanges();
                }
            }

            var model = GetModel(authorsViewModel.SortOrder, authorsViewModel.CurrentFilter, null, authorsViewModel.PageSize, null);

            foreach (var author in model.Authors)
            {
                author.IsDeleted = false;
            }

            return View(model);
        }

        public AuthorsViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new AuthorsViewModel();

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

            var authors = from a in _db.Authors
                          where a.Delete != true
                          select a;

            model.AllAuthorCount = authors.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                authors = authors.Where(s => s.AuthorName.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    authors = authors.OrderByDescending(s => s.AuthorName);
                    break;
                case "active":
                    authors = authors.OrderBy(s => s.Active);
                    break;
                case "act_desc":
                    authors = authors.OrderByDescending(s => s.Active);
                    break;
                default:
                    authors = authors.OrderBy(s => s.AuthorName);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.Authors = authors.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }

        // GET: Authors/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = await _db.Authors.FindAsync(id);
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
        public async Task<ActionResult> Create([Bind(Include = "ID,AuthorName,Active")] Author author)
        {
            if (ModelState.IsValid)
            {
                _db.Authors.Add(author);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: Authors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = await _db.Authors.FindAsync(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return PartialView("_partialEdit", author);
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
                _db.Entry(author).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        // GET: Authors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = await _db.Authors.FindAsync(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult DeleteConfirmed(PagedList<Author> m)
        {
            var forDelete = m
                .OrderByDescending(x => x.AuthorName)
                .ToList()
                .Where(x => x.IsDeleted == true);

            _db.Authors.RemoveRange(forDelete);
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
