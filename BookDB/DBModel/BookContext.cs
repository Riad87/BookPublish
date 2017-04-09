using BookDB.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace bookPublishDB
{
    public class BookContext : DbContext
    {
        public BookContext() : base("name=BookDBConnectionString")
        {
        }
        public DbSet<Books> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Cover> Covers { get; set; }
        public DbSet<Depot> Depots { get; set; }
        public DbSet<Depot_type> Depot_types { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Press> Press { get; set; }
        public DbSet<Pressure> Pressure { get; set; }
        public DbSet<Stockist_margin> Stockist_margins { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<PayOff> PayOffs { get; set; }


    }
}
