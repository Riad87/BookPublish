using bookPublishDB;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BookDB.DBModel
{
    // Interface loose coupling-hez (Unit teszeléshez)
    //public interface IBookPublishContext
    //{
    //    IQueryable<Books> Books { get; }
    //    IQueryable<Author> Authors { get; }
    //    IQueryable<AccountType> AccountType { get; }
    //    IQueryable<Cover> Covers { get; }
    //    IQueryable<Depot> Depots { get; }
    //    IQueryable<Depot_type> Depot_types { get; }
    //    IQueryable<Partner> Partners { get; }
    //    IQueryable<Press> Presses { get; }
    //    IQueryable<Pressure> Pressure { get; }
    //    IQueryable<Stockist_margin> Stockist_margin { get; }
    //    IQueryable<Theme> Themes { get; }

    //    int SaveChanges();
    //    T Add<T>(T entity) where T : class;
    //    T Delete<T>(T entity) where T : class;

    //    Books FindBookById(int ID);
    //    Author FindAuthorById(int ID);
    //    AccountType FindAccountTypeById(int ID);
    //    Cover FindCoverById(int ID);
    //    Depot FindDepotById(int ID);
    //    Depot_type FindDepot_typeById(int ID);
    //    Partner FindPartnerById(int ID);
    //    Press FindPressById(int ID);
    //    Pressure FindPressureById(int ID);
    //    Stockist_margin FindStockist_marginById(int ID);
    //    Theme FindThemeById(int ID);

    //}
}
