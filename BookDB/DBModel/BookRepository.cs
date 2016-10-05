using bookPublishDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookDB.DBModel
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
    }
}
