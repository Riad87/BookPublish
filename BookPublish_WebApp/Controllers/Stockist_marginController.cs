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
    public class Stockist_marginController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Stockist_margin
        [HttpGet]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(StockistMarginViewModel stockistMarginViewModel)
        {
            foreach (var stMargin in stockistMarginViewModel.StMargins)
            {
                if (stMargin.IsDeleted == true)
                {
                    Stockist_margin st = _db.Stockist_margins
                                         .Where(x => x.ID == stMargin.ID)
                                         .Include(p => p.Partner).First();
                                        
                    st.Deleted = true;
                    _db.SaveChanges();
                }
            }

            var model = GetModel(stockistMarginViewModel.SortOrder, stockistMarginViewModel.CurrentFilter, null, stockistMarginViewModel.PageSize, null);

            foreach (var stMargin in stockistMarginViewModel.StMargins)
            {
                stMargin.IsDeleted = false;
            }

            return View(model);
        }

        public StockistMarginViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new StockistMarginViewModel();

            model.SortOrder = sortorder;

            int defaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = defaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;
            model.PageNumber = actualPage;

            model.DiscountSort = String.IsNullOrEmpty(model.SortOrder) ? "discount_desc" : "";
            model.ActiveSort = model.SortOrder == "active" ? "act_desc" : "active";
            model.PartnerSort = model.SortOrder == "partner_asc" ? "partner_desc" : "partner_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;


            model.CurrentFilter = searchString;

            var stockistMargin = _db.Stockist_margins
                          .Where(st => st.Deleted != true)
                          .Include(p => p.Partner);
               

            model.AllStMarginCount = stockistMargin.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                stockistMargin = stockistMargin.Where(s => s.Partner.Name.Contains(searchString));
            }

            switch (sortorder)
            {
                case "discount_desc":
                    stockistMargin = stockistMargin.OrderByDescending(s => s.Discount);
                    break;
                case "active":
                    stockistMargin = stockistMargin.OrderBy(s => s.Active);
                    break;
                case "act_desc":
                    stockistMargin = stockistMargin.OrderByDescending(s => s.Active);
                    break;
                case "partner_asc":
                    stockistMargin = stockistMargin.OrderBy(s => s.Partner.Name);
                    break;
                case "partner_desc":
                    stockistMargin = stockistMargin.OrderByDescending(s => s.Partner.Name);
                    break;
                default:
                    stockistMargin = stockistMargin.OrderBy(s => s.Discount);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.StMargins = stockistMargin.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }

        // GET: Stockist_margin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stockist_margin stockist_margin = await _db.Stockist_margins.FindAsync(id);
            if (stockist_margin == null)
            {
                return HttpNotFound();
            }
            return View(stockist_margin);
        }

        // GET: Stockist_margin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stockist_margin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Discount")] Stockist_margin stockist_margin)
        {
            if (ModelState.IsValid)
            {
                _db.Stockist_margins.Add(stockist_margin);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(stockist_margin);
        }

        // GET: Stockist_margin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stockist_margin stockist_margin = await _db.Stockist_margins.FindAsync(id);
            if (stockist_margin == null)
            {
                return HttpNotFound();
            }
            return View(stockist_margin);
        }

        // POST: Stockist_margin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Discount")] Stockist_margin stockist_margin)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(stockist_margin).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(stockist_margin);
        }

        // GET: Stockist_margin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stockist_margin stockist_margin = await _db.Stockist_margins.FindAsync(id);
            if (stockist_margin == null)
            {
                return HttpNotFound();
            }
            return View(stockist_margin);
        }

        // POST: Stockist_margin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Stockist_margin stockist_margin = await _db.Stockist_margins.FindAsync(id);
            _db.Stockist_margins.Remove(stockist_margin);
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
