using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookPublish_WebApp.Models;
using bookPublishDB;
using PagedList;

namespace BookPublish_WebApp.Controllers
{
    public class Authors2Controller : Controller
    {
        private BookContext _db = new BookContext();

        // GET: Authors2
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AuthorsViewModel authorsViewModel)
        {
            //TODO: logikai törlés
            foreach (var author in authorsViewModel.Authors)
            {
                if (author.IsDeleted == true)
                {
                    Author a = (from x in _db.Authors
                                where x.AuthorName == author.AuthorName
                                select x).First();
                    a.Delete = true;
                    _db.SaveChanges();
                }
            }

            var model = GetModel(authorsViewModel.SortOrder, authorsViewModel.CurrentFilter, null, authorsViewModel.PageSize, null);                       

            return View(model);
        }

        private AuthorsViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new AuthorsViewModel();          

            model.SortOrder = sortorder;
            
            //TODO: egyszerűsíteni
            int defaultPageSize = 2;

            if (pagesize != null)
            {
                defaultPageSize = pagesize.Value;
            }
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
    }
}