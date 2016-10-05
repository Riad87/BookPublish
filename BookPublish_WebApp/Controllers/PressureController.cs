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

namespace BookPublish_WebApp.Controllers
{
    public class PressureController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Pressure
        public async Task<ActionResult> Index()
        {
            return View(await db.Pressure.ToListAsync());
        }

        // GET: Pressure/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pressure pressure = await db.Pressure.FindAsync(id);
            if (pressure == null)
            {
                return HttpNotFound();
            }
            return View(pressure);
        }

        // GET: Pressure/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pressure/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,OrderDate,AfterPressure,Quantity")] Pressure pressure)
        {
            if (ModelState.IsValid)
            {
                db.Pressure.Add(pressure);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pressure);
        }

        // GET: Pressure/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pressure pressure = await db.Pressure.FindAsync(id);
            if (pressure == null)
            {
                return HttpNotFound();
            }
            return View(pressure);
        }

        // POST: Pressure/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,OrderDate,AfterPressure,Quantity")] Pressure pressure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pressure).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pressure);
        }

        // GET: Pressure/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pressure pressure = await db.Pressure.FindAsync(id);
            if (pressure == null)
            {
                return HttpNotFound();
            }
            return View(pressure);
        }

        // POST: Pressure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pressure pressure = await db.Pressure.FindAsync(id);
            db.Pressure.Remove(pressure);
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
