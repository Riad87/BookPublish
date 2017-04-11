using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BookDB.DBModel;
using bookPublishDB;
using BookPublish_WebApp.Models;

namespace BookPublish_WebApp.Controllers
{
    public class PayOffController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: PayOff
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        public PayOffViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new PayOffViewModel();

            model.SortOrder = sortorder;

            //TODO: egyszerűsíteni
            int defaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = defaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;
            model.PageNumber = actualPage;

            model.NameSort = String.IsNullOrEmpty(model.SortOrder) ? "name_desc" : "";
            model.ISBNSort = sortorder == "isbn" ? "isbn_desc" : "isbn";
            model.QuantitySort = model.SortOrder == "quantity_asc" ? "quantity_desc" : "quantity_asc";
            model.PriceSort = sortorder == "Price" ? "Price_desc" : "Price";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;


            model.CurrentFilter = searchString;

            var payoffs = from a in _db.PayOffs
                          select a;

            model.AllPayoffsCount = payoffs.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                payoffs = payoffs.Where(s => s.Name.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    payoffs = payoffs.OrderByDescending(s => s.Name);
                    break;
                case "isbn":
                    payoffs = payoffs.OrderBy(s => s.ISBN);
                    break;
                case "isbn_desc":
                    payoffs = payoffs.OrderByDescending(s => s.ISBN);
                    break;
                case "quantity_asc":
                    payoffs = payoffs.OrderBy(s => s.Quantity);
                    break;
                case "quantity_desc":
                    payoffs = payoffs.OrderByDescending(s => s.Quantity);
                    break;
                case "Price":
                    payoffs = payoffs.OrderBy(s => s.Price);
                    break;
                case "Price_desc":
                    payoffs = payoffs.OrderByDescending(s => s.Price);
                    break;
                default:
                    payoffs = payoffs.OrderBy(s => s.Name);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.PayOffs = payoffs.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }

        // GET: PayOff/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayOff payOff = _db.PayOffs.Find(id);
            if (payOff == null)
            {
                return HttpNotFound();
            }
            return View(payOff);
        }

        // GET: PayOff/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PayOff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ISBN,Name,Quantity,Price")] PayOff payOff)
        {
            if (ModelState.IsValid)
            {
                _db.PayOffs.Add(payOff);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payOff);
        }

        // GET: PayOff/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayOff payOff = _db.PayOffs.Find(id);
            if (payOff == null)
            {
                return HttpNotFound();
            }
            return View(payOff);
        }

        // POST: PayOff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ISBN,Name,Quantity,Price")] PayOff payOff)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(payOff).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payOff);
        }

        // GET: PayOff/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayOff payOff = _db.PayOffs.Find(id);
            if (payOff == null)
            {
                return HttpNotFound();
            }
            return View(payOff);
        }

        // POST: PayOff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PayOff payOff = _db.PayOffs.Find(id);
            _db.PayOffs.Remove(payOff);
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
