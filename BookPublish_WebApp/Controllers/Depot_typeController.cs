﻿using System;
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
    public class Depot_typeController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Depot_type
        public async Task<ActionResult> Index()
        {
            return View(await db.Depot_types.ToListAsync());
        }

        // GET: Depot_type/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot_type depot_type = await db.Depot_types.FindAsync(id);
            if (depot_type == null)
            {
                return HttpNotFound();
            }
            return View(depot_type);
        }

        // GET: Depot_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Depot_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Type")] Depot_type depot_type)
        {
            if (ModelState.IsValid)
            {
                db.Depot_types.Add(depot_type);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(depot_type);
        }

        // GET: Depot_type/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot_type depot_type = await db.Depot_types.FindAsync(id);
            if (depot_type == null)
            {
                return HttpNotFound();
            }
            return View(depot_type);
        }

        // POST: Depot_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Type")] Depot_type depot_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depot_type).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(depot_type);
        }

        // GET: Depot_type/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot_type depot_type = await db.Depot_types.FindAsync(id);
            if (depot_type == null)
            {
                return HttpNotFound();
            }
            return View(depot_type);
        }

        // POST: Depot_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Depot_type depot_type = await db.Depot_types.FindAsync(id);
            db.Depot_types.Remove(depot_type);
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