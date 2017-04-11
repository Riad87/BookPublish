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
              
        public void AddLog(string cont, string user, string act)
        {
            using (var _db = new BookContext())
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
}