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

            model.AllPartner = new SelectList(_db.Partners.Where(p => p.Deleted != true && p.Active != false), "ID", "Name", 0);

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
        public async Task<ActionResult> Create([Bind(Include = "Discount,Active,SelectedPartnerID")] StockistMarginViewModel viewModel)
        {
            Stockist_margin stockist_margin = new Stockist_margin();
            stockist_margin.Active = viewModel.Active;
            stockist_margin.Discount = Convert.ToDouble(viewModel.Discount);
            stockist_margin.Partner = _db.Partners.Find(viewModel.SelectedPartnerID);

            if (ModelState.IsValid)
            {
                _db.Stockist_margins.Add(stockist_margin);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(stockist_margin);
        }

        // GET: Stockist_margin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stockist_margin stockist_margin = _db.Stockist_margins
                                                .Where(s=>s.ID == id)
                                                .Include(p=>p.Partner).FirstOrDefault();

            int SelectedParterID = stockist_margin.Partner == null ? 0 : stockist_margin.Partner.ID;

            StockistMarginViewModel viewModel = new StockistMarginViewModel();
            viewModel.Active = stockist_margin.Active;
            viewModel.Discount = stockist_margin.Discount;
            viewModel.SelectedPartnerID = SelectedParterID;
            viewModel.AllPartner = new SelectList(_db.Partners, "ID", "Name", SelectedParterID);

            if (stockist_margin == null)
            {
                return HttpNotFound();
            }

            return PartialView("_partialEdit", viewModel);
        }

        // POST: Stockist_margin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Discount,Active,SelectedPartnerID")] StockistMarginViewModel viewModel)
        {
            var stockistfromdb = _db.Stockist_margins.Find(viewModel.ID);

            stockistfromdb.Active = viewModel.Active;
            stockistfromdb.Discount = viewModel.Discount;
            stockistfromdb.Partner = _db.Partners.Find(viewModel.SelectedPartnerID);

            if (ModelState.IsValid)
            {
                //_db.Entry(stockistfromdb).State = EntityState.Modified;

                _db.Configuration.AutoDetectChangesEnabled = true;

                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);
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

        // A megkapott model state-ből kiveszi a a hibákat és egy listába beteszi és azt adja vissza.
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
