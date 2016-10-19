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
using BookPublish_WebApp.Models;

namespace BookPublish_WebApp.Controllers
{
    public class PressureController : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Press
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PressuresViewModel pressuresViewModel)
        {
            foreach (var pressure in pressuresViewModel.Pressures)
            {
                if (pressure.IsDeleted == true)
                {
                    Pressure p = _db.Pressure
                                .Where(x => x.ID == pressure.ID)
                                .Include(b => b.Book)
                                .Include(pr => pr.Press).First();
                                
                    p.Deleted = true;
                    _db.SaveChanges();
                }
            }

            var model = GetModel(pressuresViewModel.SortOrder, pressuresViewModel.CurrentFilter, null, pressuresViewModel.PageSize, null);

            foreach (var pressure in model.Pressures)
            {
                pressure.IsDeleted = false;
            }

            return View(model);
        }

        public PressuresViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new PressuresViewModel();

            model.SortOrder = sortorder;
           
            int defaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = defaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;
            model.PageNumber = actualPage;

            model.OrderSort = String.IsNullOrEmpty(model.SortOrder) ? "order_desc" : "";
            model.AfterPressureSort = model.SortOrder == "afterpress_asc" ? "afterpress_desc" : "afterpress_asc";
            model.QuantitySort = model.SortOrder == "quantity_asc" ? "quantity_desc" : "quantity_asc";
            model.BookSort = model.SortOrder == "book_asc" ? "book_desc" : "book_asc";
            model.PressSort = model.SortOrder == "press_asc" ? "press_desc" : "press_asc";
            model.IDSort = model.SortOrder == "id_asc" ? "id_desc" : "id_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;


            model.CurrentFilter = searchString;

            var pressures = _db.Pressure
                            .Where(x=>x.Deleted != true)
                            .Include(p => p.Book)
                            .Include(t => t.Press);

            model.AllPressureCount = pressures.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                pressures = pressures.Where(s => s.Book.Name.Contains(searchString));
            }

            switch (sortorder)
            {
                case "order_desc":
                    pressures = pressures.OrderByDescending(s => s.OrderDate);
                    break;
                case "afterpress_asc":
                    pressures = pressures.OrderBy(s => s.AfterPressure);
                    break;
                case "afterpress_desc":
                    pressures = pressures.OrderByDescending(s => s.AfterPressure);
                    break;
                case "quantity_asc":
                    pressures = pressures.OrderBy(s => s.Quantity);
                    break;
                case "quantity_desc":
                    pressures = pressures.OrderByDescending(s => s.Quantity);
                    break;
                case "book_asc":
                    pressures = pressures.OrderBy(s => s.Book.Name);
                    break;
                case "book_desc":
                    pressures = pressures.OrderByDescending(s => s.Book.Name);
                    break;
                case "press_asc":
                    pressures = pressures.OrderBy(s => s.Press.Name);
                    break;
                case "press_desc":
                    pressures = pressures.OrderByDescending(s => s.Press.Name);
                    break;
                case "id_asc":
                    pressures = pressures.OrderBy(s => s.ID);
                    break;
                case "id_desc":
                    pressures = pressures.OrderByDescending(s => s.ID);
                    break;

                default:
                    pressures = pressures.OrderBy(s => s.OrderDate);
                    break;
            }

            int pageNumber = (page ?? 1);
            model.PageNumber = pageNumber;

            model.Pressures = pressures.Skip((actualPage - 1) * defaultPageSize).Take(defaultPageSize).ToList();

            return model;
        }
        // GET: Pressure/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pressure pressure = await _db.Pressure.FindAsync(id);
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
                _db.Pressure.Add(pressure);
                await _db.SaveChangesAsync();
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
            Pressure pressure = await _db.Pressure.FindAsync(id);
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
                _db.Entry(pressure).State = EntityState.Modified;
                await _db.SaveChangesAsync();
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
            Pressure pressure = await _db.Pressure.FindAsync(id);
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
            Pressure pressure = await _db.Pressure.FindAsync(id);
            _db.Pressure.Remove(pressure);
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
