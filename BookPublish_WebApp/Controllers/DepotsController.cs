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
    public class DepotsController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Depots
        [HttpGet]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }


        [HttpPost]
        public ActionResult Index(DepotsViewModel depotsViewModel)
        {
            foreach (var depot in depotsViewModel.Depots)
            {
                if (depot.IsDeleted == true)
                {
                    Depot d = _db.Depots
                               .Where(x => x.ID == depot.ID)
                               .Include(t => t.Type).First();

                    d.Deleted = true;
                    _db.SaveChanges();
                }
            }

            var model = GetModel(depotsViewModel.SortOrder, depotsViewModel.CurrentFilter, null, depotsViewModel.PageSize, null);

            foreach (var depot in model.Depots)
            {
                depot.IsDeleted = false;
            }

            return View(model);
        }

        public DepotsViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new DepotsViewModel();

            model.SortOrder = sortorder;

            //TODO: egyszerűsíteni
            int defaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = defaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;
            model.PageNumber = actualPage;

            model.NameSort = String.IsNullOrEmpty(model.SortOrder) ? "name_desc" : "";
            model.CitySort = model.SortOrder == "city_asc" ? "city_desc" : "city_asc";
            model.AddressSort = model.AddressSort == "addr_asc" ? "addr_desc" : "addr_asc";
            model.ZipSort = model.ZipSort == "zip_asc" ? "zip_desc" : "zip_asc";
            model.TypeSort = model.TypeSort == "type_asc" ? "type_desc" : "type_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;


            model.CurrentFilter = searchString;

            var depots = _db.Depots
                         .Where(d => d.Deleted != true)
                         .Include(t => t.Type)
                         .Include(p => p.Partner);

            model.AllDepotsCount = depots.Count();

            model.AllPartner = new SelectList(_db.Partners.Where(p => p.Deleted != true), "ID", "Name", 0);

            model.AllTypes = new SelectList(_db.Depot_types.Where(d => d.Deleted != true), "ID", "Type", 0);


            if (!String.IsNullOrEmpty(searchString))
            {
                depots = depots.Where(s => s.Depot_Name.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    depots = depots.OrderByDescending(s => s.Depot_Name);
                    break;
                case "city_asc":
                    depots = depots.OrderBy(s => s.City);
                    break;
                case "city_desc":
                    depots = depots.OrderByDescending(s => s.City);
                    break;
                case "addr_asc":
                    depots = depots.OrderBy(s => s.Address);
                    break;
                case "addr_desc":
                    depots = depots.OrderByDescending(s => s.Address);
                    break;
                case "zip_asc":
                    depots = depots.OrderBy(s => s.Zip);
                    break;
                case "zip_desc":
                    depots = depots.OrderByDescending(s => s.Zip);
                    break;
                case "type_asc":
                    depots = depots.OrderBy(s => s.Type.Type);
                    break;
                case "type_desc":
                    depots = depots.OrderByDescending(s => s.Type.Type);
                    break;
                default:
                    depots = depots.OrderBy(s => s.Depot_Name);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.Depots = depots.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }

        // GET: Depots/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot depot = await _db.Depots.FindAsync(id);
            if (depot == null)
            {
                return HttpNotFound();
            }
            return View(depot);
        }

        // GET: Depots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Depots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepotName,SelectedPartnerID,SelectedTypeID,City,Address,Zip")] DepotsViewModel viewModel)
        {
            Depot depot = new Depot();

            depot.Address = viewModel.Address;
            depot.City = viewModel.City;
            depot.Depot_Name = viewModel.DepotName;
            depot.Zip = viewModel.Zip.ToString();
            depot.Partner = _db.Partners.Find(viewModel.SelectedPartnerID);
            depot.Type = _db.Depot_types.Find(viewModel.SelectedTypeID);

            if (ModelState.IsValid)
            {
                _db.Depots.Add(depot);
                _db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.DenyGet);
        }

        // GET: Depots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var depot = _db.Depots
                          .Where(d => d.ID == id)
                          .Include(t => t.Type)
                          .Include(p => p.Partner).FirstOrDefault();

            int selectedTypeId = depot.Type == null ? 0 : depot.Type.ID;
            int selectedPartnerId = depot.Partner == null ? 0 : depot.Partner.ID;

            var depotview = new DepotsViewModel();

            depotview.ID = depot.ID;
            depotview.City = depot.City;
            depotview.DepotName = depot.Depot_Name;
            depotview.Address = depot.Address;
            depotview.Zip = int.Parse(depot.Zip);
            depotview.SelectedTypeID = selectedTypeId;
            depotview.SelectedPartnerID = selectedPartnerId;

            depotview.AllTypes = new SelectList(_db.Depot_types, "ID", "Type", selectedTypeId);

            depotview.AllPartner = new SelectList(_db.Partners, "ID", "Name", selectedPartnerId);

            if (depot == null)
            {
                return HttpNotFound();
            }
            return PartialView("_partialEdit", depotview);
        }

        // POST: Depots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,City,DepotName,Address,Zip,SelectedTypeID, SelectedPartnerID")] DepotsViewModel viewModel)
        {
            Depot depotfromdb = _db.Depots.Find(viewModel.ID); // lekérdezem az eredeti objektumot az adatbázisból, ezzen fogja érzékelni az EF a navigációs property változását.

            depotfromdb.ID = viewModel.ID;
            depotfromdb.Depot_Name = viewModel.DepotName;
            depotfromdb.Address = viewModel.Address;
            depotfromdb.City = viewModel.City;
            depotfromdb.Zip = viewModel.Zip.ToString();
            depotfromdb.Partner = _db.Partners.Find(viewModel.SelectedPartnerID);
            depotfromdb.Type = _db.Depot_types.Find(viewModel.SelectedTypeID);

            if (ModelState.IsValid)
            {
                //_db.Entry(depotfromdb).State = EntityState.Modified; // nem alkalmas navigation property mentésére, csak skaláris értékek mentésére.
                
                _db.Configuration.AutoDetectChangesEnabled = true; // ezért használom ezt a megoldást, hogy a befrissítse az EF a változó navigációs property-t
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = GetModelStateErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        // GET: Depots/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot depot = await _db.Depots.FindAsync(id);
            if (depot == null)
            {
                return HttpNotFound();
            }
            return View(depot);
        }

        // POST: Depots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Depot depot = await _db.Depots.FindAsync(id);
            _db.Depots.Remove(depot);
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
