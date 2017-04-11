using BookPublish_WebApp.Models;
using bookPublishDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;

//using System.Drawing;

namespace BookPublish_WebApp.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private BookContext _db = new BookContext();

        [HttpGet]
        public ActionResult Index(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = GetModel(sortorder, currentFilter, searchString, pagesize, page);

            return View(model);
        }

        [HttpGet]
        public ActionResult OrderDetail(string id = null)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var pressure = new PressureMasterDetailsModel();
            pressure.PressureOrders = _db.Pressure
                          .Where(x => x.Book.ID.ToString() == id).ToList();
                                
            return PartialView("_partialPressures", pressure);
        }

        public void PDFGenerate(string id)
        {

            try
            {

                Phrase phrase = null;
                PdfPCell headerCell = null;
                PdfPTable maintabl = null;
                PdfPTable suppliertabl = null;
                PdfPTable customertabl = null;
                PdfPTable datetable = null;
                PdfPTable middletabl = null;
                PdfPTable summatabl = null;
                //Color color = null;

                var pressures = _db.Pressure
                                .Where(x => x.ID.ToString() == id)
                                .Include(p => p.Book)
                                .Include("Press")
                                .FirstOrDefault();

                var book = _db.Books
                          .Where(b => b.ID == pressures.Book.ID)
                          .Include("Author").FirstOrDefault();

                var document = new Document();
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, Response.OutputStream);
                document.Open();

                maintabl = new PdfPTable(2);
                maintabl.TotalWidth = 500f;
                maintabl.LockedWidth = true;
                maintabl.SetWidths(new float[] { 1f, 1f });

                datetable = new PdfPTable(4);
                datetable.TotalWidth = 500f;
                datetable.LockedWidth = true;
                datetable.SetWidths(new float[] { 0.2f, 0.3f, 0.2f, 0.3f });

                middletabl = new PdfPTable(7);
                middletabl.TotalWidth = 500f;
                middletabl.LockedWidth = true;
                middletabl.SetWidths(new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f });

                summatabl = new PdfPTable(7);
                summatabl.TotalWidth = 500f;
                summatabl.LockedWidth = true;
                summatabl.SetWidths(new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f });

                suppliertabl = new PdfPTable(2);
                suppliertabl.TotalWidth = 250f;
                //table2.LockedWidth = true;
                suppliertabl.SetWidths(new float[] { 1f, 1f });

                customertabl = new PdfPTable(2);
                customertabl.TotalWidth = 250f;
                //table2.LockedWidth = true;
                customertabl.SetWidths(new float[] { 0.4f, 0.6f });


                string ArialUni_TTF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");

                BaseFont bf = BaseFont.CreateFont(ArialUni_TTF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font bigf = new Font(bf, 20, iTextSharp.text.Font.BOLD);
                Font lowf = new Font(bf, 10, Font.NORMAL);
                Font lowb = new Font(bf, 10, Font.BOLD);


                //Header
                headerCell = new PdfPCell(new Phrase("Megrendelőlap", bigf));
                headerCell.Colspan = 2;
                headerCell.FixedHeight = 50f;
                headerCell.HorizontalAlignment = 1; //0=bal, 1=közép, 2=jobbra rendez
                maintabl.AddCell(headerCell);

                //Supplier
                PdfPCell supplierheader = new PdfPCell(new Phrase("Szállító: ", lowf));
                supplierheader.HorizontalAlignment = 0;
                supplierheader.Border = 0;

                PdfPCell supplierdata = new PdfPCell(new Phrase(pressures.Press.Name.ToString(), lowb));
                supplierdata.HorizontalAlignment = 0;
                supplierdata.Border = 0;

                PdfPCell supplieraddress = new PdfPCell(new Phrase("Cím: ", lowf));
                supplieraddress.HorizontalAlignment = 0;
                supplieraddress.Border = 0;

                PdfPCell supplieradrdata = new PdfPCell(new Phrase(pressures.Press.Address.ToString(), lowb));
                supplieradrdata.HorizontalAlignment = 0;
                supplieradrdata.Border = 0;

                PdfPCell supplierspace = new PdfPCell(new Phrase("", lowf));
                supplierspace.HorizontalAlignment = 0;
                supplierspace.Border = 0;

                PdfPCell suppliercity = new PdfPCell(new Phrase(pressures.Press.City.ToString(), lowb));
                suppliercity.HorizontalAlignment = 0;
                suppliercity.Border = 0;

                PdfPCell supplierzip = new PdfPCell(new Phrase(pressures.Press.Zip.ToString(), lowb));
                supplierzip.HorizontalAlignment = 0;
                supplierzip.Border = 0;

                PdfPCell suppliercountry = new PdfPCell(new Phrase(pressures.Press.Country.ToString(), lowb));
                suppliercountry.HorizontalAlignment = 0;
                suppliercountry.Border = 0;

                PdfPCell suppliertaxnumber = new PdfPCell(new Phrase("Adószám: ", lowf));
                suppliertaxnumber.HorizontalAlignment = 0;
                suppliertaxnumber.Border = 0;

                PdfPCell suppliertaxnumberd = new PdfPCell(new Phrase(pressures.Press.TaxNumber.ToString(), lowb));
                suppliertaxnumberd.HorizontalAlignment = 0;
                suppliertaxnumberd.Border = 0;

                PdfPCell supplieraccountnum = new PdfPCell(new Phrase("Számlaszám: ", lowf));
                supplieraccountnum.HorizontalAlignment = 0;
                supplieraccountnum.Border = 0;

                PdfPCell supplieraccountnumd = new PdfPCell(new Phrase(pressures.Press.AccountNumber.ToString(), lowb));
                supplieraccountnumd.HorizontalAlignment = 0;
                supplieraccountnumd.Border = 0;


                suppliertabl.AddCell(supplierheader);
                suppliertabl.AddCell(supplierdata);
                suppliertabl.AddCell(supplieraddress);
                suppliertabl.AddCell(supplieradrdata);
                suppliertabl.AddCell(supplierspace); // üres mező
                suppliertabl.AddCell(suppliercity);
                suppliertabl.AddCell(supplierspace); // ürese mező
                suppliertabl.AddCell(supplierzip);
                suppliertabl.AddCell(supplierspace); // üres mező
                suppliertabl.AddCell(suppliercountry);
                suppliertabl.AddCell(suppliertaxnumber);
                suppliertabl.AddCell(suppliertaxnumberd);
                suppliertabl.AddCell(supplieraccountnum);
                suppliertabl.AddCell(supplieraccountnumd);

                //Customer

                PdfPCell customerheader = new PdfPCell(new Phrase("Vevő: ", lowf));
                customerheader.HorizontalAlignment = 0;
                customerheader.Border = 0;

                PdfPCell customerdata = new PdfPCell(new Phrase("Northwind Kft.", lowb));
                customerdata.HorizontalAlignment = 0;
                customerdata.Border = 0;

                PdfPCell customeraddr = new PdfPCell(new Phrase("Cím: ", lowf));
                customeraddr.HorizontalAlignment = 0;
                customeraddr.Border = 0;

                PdfPCell customeraddrData = new PdfPCell(new Phrase("1117 Budapest, Deseffy út 12.", lowb));
                customeraddrData.HorizontalAlignment = 0;
                customeraddrData.Border = 0;

                customertabl.AddCell(customerheader);
                customertabl.AddCell(customerdata);
                customertabl.AddCell(customeraddr);
                customertabl.AddCell(customeraddrData);

                //Date & comment

                Random rnd = new Random();
                int i = rnd.Next(1, 100);

                PdfPCell number = new PdfPCell(new Phrase("SORSZ:", lowf));
                number.HorizontalAlignment = 0;
                number.Border = 0;

                PdfPCell numberd = new PdfPCell(new Phrase(DateTime.Now.ToString("s", DateTimeFormatInfo.InvariantInfo) + "/" + i.ToString(), lowb));
                numberd.HorizontalAlignment = 0;
                numberd.Border = 0;

                PdfPCell comment = new PdfPCell(new Phrase("Megjegyzés:", lowf));
                comment.HorizontalAlignment = 0;
                comment.Border = 0;

                PdfPCell commentcell = new PdfPCell(new Phrase("", lowf));
                comment.HorizontalAlignment = 0;
                comment.Border = 1;

                datetable.AddCell(number);
                datetable.AddCell(numberd);
                datetable.AddCell(comment);
                datetable.AddCell(commentcell);

                //Data

                PdfPCell DataAuthor = new PdfPCell(new Phrase("Szerző", lowf));
                DataAuthor.HorizontalAlignment = 0;
                DataAuthor.Border = 1;

                PdfPCell DataAuthorD = new PdfPCell(new Phrase(book.Author.AuthorName.ToString(), lowb));
                DataAuthorD.HorizontalAlignment = 0;
                DataAuthorD.Border = 1;

                PdfPCell DataName = new PdfPCell(new Phrase("Könyv címe", lowf));
                DataName.HorizontalAlignment = 0;
                DataName.Border = 1;

                PdfPCell DataNameD = new PdfPCell(new Phrase(pressures.Book.Name.ToString(), lowb));
                DataNameD.HorizontalAlignment = 0;
                DataNameD.Border = 1;

                PdfPCell DataQuantity = new PdfPCell(new Phrase("Mennyiség", lowf));
                DataQuantity.HorizontalAlignment = 0;
                DataQuantity.Border = 1;

                PdfPCell DataQuantityD = new PdfPCell(new Phrase(pressures.Quantity.ToString(), lowb));
                DataQuantityD.HorizontalAlignment = 0;
                DataQuantityD.Border = 1;

                PdfPCell DataNet = new PdfPCell(new Phrase("Nettó egységár", lowf));
                DataNet.HorizontalAlignment = 0;
                DataNet.Border = 1;

                PdfPCell DataNetD = new PdfPCell(new Phrase(pressures.Book.NetValue.ToString(), lowf));
                DataNetD.HorizontalAlignment = 0;
                DataNetD.Border = 1;

                PdfPCell DataVat = new PdfPCell(new Phrase("Áfaérték", lowf));
                DataVat.HorizontalAlignment = 0;
                DataVat.Border = 1;

                PdfPCell DataVatD = new PdfPCell(new Phrase(pressures.Book.Vat.ToString(), lowf));
                DataVatD.HorizontalAlignment = 0;
                DataVatD.Border = 1;

                PdfPCell DataNetp = new PdfPCell(new Phrase("Nettó összérték", lowf));
                DataNetp.HorizontalAlignment = 0;
                DataNetp.Border = 1;

                PdfPCell DataNetpD = new PdfPCell(new Phrase((pressures.Quantity * pressures.Book.NetValue).ToString(), lowf));
                DataNetpD.HorizontalAlignment = 0;
                DataNetpD.Border = 1;

                PdfPCell DataGross = new PdfPCell(new Phrase("Bruttó összérték", lowf));
                DataGross.HorizontalAlignment = 0;
                DataGross.Border = 1;

                PdfPCell DataGrossD = new PdfPCell(new Phrase((pressures.Quantity * pressures.Book.GrossValue).ToString(), lowb));
                DataGrossD.HorizontalAlignment = 0;
                DataGrossD.Border = 1;

                middletabl.AddCell(DataAuthor);
                middletabl.AddCell(DataName);
                middletabl.AddCell(DataQuantity);
                middletabl.AddCell(DataNet);
                middletabl.AddCell(DataVat);
                middletabl.AddCell(DataNetp);
                middletabl.AddCell(DataGross);
                middletabl.AddCell(DataAuthorD);
                middletabl.AddCell(DataNameD);
                middletabl.AddCell(DataQuantityD);
                middletabl.AddCell(DataNetD);
                middletabl.AddCell(DataVatD);
                middletabl.AddCell(DataNetpD);
                middletabl.AddCell(DataGrossD);

                //Summa

                PdfPCell Space = new PdfPCell(new Phrase("", lowf));
                Space.HorizontalAlignment = 0;
                Space.Colspan = 4;
                Space.Border = 0;

                PdfPCell NetSumma = new PdfPCell(new Phrase("Nettó végösszeg", lowf));
                NetSumma.HorizontalAlignment = 0;
                NetSumma.Border = 0;

                PdfPCell VatSumma = new PdfPCell(new Phrase("Áfaérték", lowf));
                VatSumma.HorizontalAlignment = 0;
                VatSumma.Border = 0;

                PdfPCell GrossSumma = new PdfPCell(new Phrase("Bruttó végösszeg", lowf));
                GrossSumma.HorizontalAlignment = 0;
                GrossSumma.Border = 0;

                PdfPCell NetSummaD = new PdfPCell(new Phrase((pressures.Quantity * pressures.Book.NetValue).ToString(), lowf));
                NetSummaD.HorizontalAlignment = 0;
                NetSummaD.Border = 0;

                PdfPCell VatSummaD = new PdfPCell(new Phrase(((pressures.Quantity * pressures.Book.GrossValue) - (pressures.Quantity * pressures.Book.NetValue)).ToString(), lowf));
                VatSummaD.HorizontalAlignment = 0;
                VatSummaD.Border = 0;

                PdfPCell GrossSummaD = new PdfPCell(new Phrase((pressures.Quantity * pressures.Book.GrossValue).ToString(), lowb));
                GrossSummaD.HorizontalAlignment = 0;
                GrossSummaD.Border = 0;

                summatabl.AddCell(Space);
                summatabl.AddCell(NetSumma);
                summatabl.AddCell(VatSumma);
                summatabl.AddCell(GrossSumma);
                summatabl.AddCell(Space);
                summatabl.AddCell(NetSummaD);
                summatabl.AddCell(VatSummaD);
                summatabl.AddCell(GrossSummaD);

                maintabl.AddCell(suppliertabl);
                maintabl.AddCell(customertabl);
                document.Add(maintabl);
                document.Add(new Paragraph(25, "\u00a0"));
                document.Add(datetable);
                document.Add(new Paragraph(25, "\u00a0"));
                document.Add(middletabl);
                document.Add(new Paragraph(25, "\u00a0"));
                document.Add(summatabl);


                pdfWriter.CloseStream = false;
                document.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachemnt;filename=" + book.Name.ToString()+"_megrendelo.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(document);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        public BooksViewModel GetModel(string sortorder, string currentFilter, string searchString, int? pagesize, int? page)
        {
            var model = new BooksViewModel();

            model.CurrentSort = sortorder;

            int DefaultPageSize = pagesize.HasValue ? pagesize.Value : 10;

            model.PageSize = DefaultPageSize;

            int actualPage = page.HasValue ? page.Value : 1;

            //model.CurrentPageSize
            model.NameSort = String.IsNullOrEmpty(sortorder) ? "name_desc" : "";
            model.ISBNSort = sortorder == "isbn" ? "isbn_desc" : "isbn";
            model.ItemNumSort = sortorder == "item" ? "item_desc" : "item";
            model.NetValueSort = sortorder == "netV" ? "netV_desc" : "netV";
            model.VatSort = sortorder == "Vat" ? "Vat_desc" : "Vat";
            model.GrossValueSort = sortorder == "GrossValue" ? "GrossValue_desc" : "GrossValue";
            model.ValidFromSort = sortorder == "ValidFrom" ? "ValidFrom_desc" : "ValidFrom";
            model.ValidToSort = sortorder == "ValidTo" ? "ValidTo_desc" : "ValidTo";
            model.CoverTypeSort = sortorder == "Cover_asc" ? "Cover_desc" : "Cover_asc";
            model.ThemeTypeSort = sortorder == "Theme_asc" ? "Theme_desc" : "Theme_asc";
            model.AuthorNameSort = sortorder == "Author_asc" ? "Author_desc" : "Author_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;


            model.CurrentFilter = searchString;

            var books = _db.Books
                        .Include(a => a.Author)
                        .Include(c => c.Cover)
                        .Include(t => t.Theme)
                        .Where(b => b.Deleted != true);

            model.AllBooksCount = books.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Name.Contains(searchString));
            }

            switch (sortorder)
            {
                case "name_desc":
                    books = books.OrderByDescending(s => s.Name);
                    break;
                case "isbn":
                    books = books.OrderBy(s => s.ISBN);
                    break;
                case "isbn_desc":
                    books = books.OrderByDescending(s => s.ISBN);
                    break;
                case "item":
                    books = books.OrderBy(s => s.ItemNumber);
                    break;
                case "item_desc":
                    books = books.OrderByDescending(s => s.ItemNumber);
                    break;
                case "netV":
                    books = books.OrderBy(s => s.NetValue);
                    break;
                case "netV_desc":
                    books = books.OrderByDescending(s => s.NetValue);
                    break;
                case "Vat":
                    books = books.OrderBy(s => s.Vat);
                    break;
                case "Vat_desc":
                    books = books.OrderByDescending(s => s.Vat);
                    break;
                case "GrossValue":
                    books = books.OrderBy(s => s.GrossValue);
                    break;
                case "GrossValue_desc":
                    books = books.OrderByDescending(s => s.GrossValue);
                    break;
                case "ValidFrom":
                    books = books.OrderBy(s => s.ValidFrom);
                    break;
                case "ValidFrom_desc":
                    books = books.OrderByDescending(s => s.ValidFrom);
                    break;
                case "ValidTo":
                    books = books.OrderBy(s => s.ValidTo);
                    break;
                case "ValidTo_desc":
                    books = books.OrderByDescending(s => s.ValidTo);
                    break;
                case "Cover_asc":
                    books = books.OrderBy(s => s.Cover.CoverName);
                    break;
                case "Cover_desc":
                    books = books.OrderByDescending(s => s.Cover.CoverName);
                    break;
                case "Theme_asc":
                    books = books.OrderBy(s => s.Theme.ThemeName);
                    break;
                case "Theme_desc":
                    books = books.OrderByDescending(s => s.Theme.ThemeName);
                    break;
                case "Author_asc":
                    books = books.OrderBy(s => s.Author.AuthorName);
                    break;
                case "Author_desc":
                    books = books.OrderByDescending(s => s.Author.AuthorName);
                    break;
                default:
                    books = books.OrderBy(s => s.Name);
                    break;
            }

            int pageNumber = (page ?? 1);

            model.PageNumber = pageNumber;

            model.Books = books.Skip((actualPage - 1) * DefaultPageSize).Take(DefaultPageSize).ToList();

            return model;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Könyvelést támogató alkalmazás.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Elérhetőségek:";

            return View();
        }
    }
}