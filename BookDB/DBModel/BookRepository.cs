using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace bookPublishDB
{
    //Továbbfejlesztési lehetőség Repo kialakítása
    public class BookRepository
    {
        public IEnumerable<Books> GetBooks()
        {
            using (var ctx = new BookContext())
            {
                return ctx.Books
                        .Include(a => a.Author)
                        .Include(c => c.Cover)
                        .Include(t => t.Theme)
                        .Where(b => b.Deleted != true).ToList();
            }

        }
        public Books GetBook(int? id)
        {
            using (var ctx = new BookContext())
            {
                if (id != null)
                    return ctx.Books.Find(id);
                else
                    return null;
            }
        }

        public void SaveBooks(Books book)
        {
            using (var Context = new BookContext())
            {
                Context.Books.Add(book);
                Context.SaveChanges();
            }
        }

        public void SaveBookAfterEdit(Books book)
        {
            using (var Context = new BookContext())
            {
                Context.Entry(book).State = EntityState.Modified;
                Context.SaveChanges();
            }
        }

        public void DeleteBook(Books book)
        {
            using (var ctx = new BookContext())
            {
                try
                {
                    var b = ctx.Books
                       .Where(x => x.ID == book.ID)
                       .Include(a => a.Author).First();
                    b.Deleted = true;
                    ctx.SaveChanges();
                }

                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        public Author GetAuthor(int id)
        {
            using (var Context = new BookContext())
            {
                return Context.Authors.Find(id);
            }
        }
        public bool BookIsExists(string isbn)
        {
            using (var Context = new BookContext())
            {
                var Book = Context.Books.FirstOrDefault(x => x.ISBN == isbn);

                if (Book == null)
                    return true;
                else
                    return false;
            }
        }
    }
}
