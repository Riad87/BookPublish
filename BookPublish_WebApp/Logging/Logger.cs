using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookPublishDB;
using BookDB.DBModel;

namespace BookPublish_WebApp.Logging
{    
    public class Logger
    {
        private BookContext _db = new BookContext();        

        //public Logger(string cont, string user, string act)
        //{
        //    this.action = act;
        //    this.content = cont;
        //    this.user = user;
        //}

        public void AddLog(string cont, string user, string act)
        {
            Logs logrow = new Logs();

            logrow.Action = act;
            logrow.Content = cont;
            logrow.User = user;
            logrow.ModifiedDate = System.DateTime.Now;

            try
            {
                _db.Logs.Add(logrow);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}