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
    public class PressesController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Press
        public async Task<ActionResult> Index()
        {
            return View(await db.Presses.ToListAsync());
        }

        // GET: Press/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Press press = await db.Presses.FindAsync(id);
            if (press == null)
            {
                return HttpNotFound();
            }
            return View(press);
        }

        // GET: Press/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Press/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,City,Address,Zip,Country,Active")] Press press)
        {
            if (ModelState.IsValid)
            {
                db.Presses.Add(press);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(press);
        }

        // GET: Press/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Press press = await db.Presses.FindAsync(id);
            if (press == null)
            {
                return HttpNotFound();
            }
            return View(press);
        }

        // POST: Press/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,City,Address,Zip,Country,Active")] Press press)
        {
            if (ModelState.IsValid)
            {
                db.Entry(press).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(press);
        }

        // GET: Press/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Press press = await db.Presses.FindAsync(id);
            if (press == null)
            {
                return HttpNotFound();
            }
            return View(press);
        }

        // POST: Press/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Press press = await db.Presses.FindAsync(id);
            db.Presses.Remove(press);
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
