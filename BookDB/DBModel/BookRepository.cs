using bookPublishDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace bookPublishDB
{
    public class BookRepository
    {
        public List<Books> GetBooksWithAuthor()
        {
            using (var Context = new BookContext())
            {
                //return Context.Books.ToList();
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
