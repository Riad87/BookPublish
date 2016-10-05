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
    public class Stockist_marginController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Stockist_margin
        public async Task<ActionResult> Index()
        {
            return View(await db.Stockist_margins.ToListAsync());
        }

        // GET: Stockist_margin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stockist_margin stockist_margin = await db.Stockist_margins.FindAsync(id);
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
                db.Stockist_margins.Add(stockist_margin);
                await db.SaveChangesAsync();
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
            Stockist_margin stockist_margin = await db.Stockist_margins.FindAsync(id);
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
                db.Entry(stockist_margin).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
            Stockist_margin stockist_margin = await db.Stockist_margins.FindAsync(id);
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
            Stockist_margin stockist_margin = await db.Stockist_margins.FindAsync(id);
            db.Stockist_margins.Remove(stockist_margin);
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
