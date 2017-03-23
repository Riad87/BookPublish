using bookPublishDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookDB.DBModel
{
    // Repository loose coupling-hez (Unit teszeléshez)
    //class BookPublishRepository : BookContext, IBookPublishContext
    //{
    //    IQueryable<AccountType> IBookPublishContext.AccountType
    //    {
    //        get { return AccountTypes; }
    //    }

    //    IQueryable<Press> IBookPublishContext.Presses
    //    {
    //        get { return Press; }
    //    }

    //    IQueryable<Stockist_margin> IBookPublishContext.Stockist_margin
    //    {
    //        get { return Stockist_margins; }
    //    }

    //    IQueryable<Author> IBookPublishContext.Authors
    //    {
    //        get { return Authors; }
    //    }

    //    IQueryable<Books> IBookPublishContext.Books
    //    {
    //        get { return Books; }
    //    }

    //    IQueryable<Cover> IBookPublishContext.Covers
    //    {
    //        get { return Covers; }
    //    }

    //    IQueryable<Depot> IBookPublishContext.Depots
    //    {
    //        get { return Depots; }
    //    }

    //    IQueryable<Depot_type> IBookPublishContext.Depot_types
    //    {
    //        get { return Depot_types; }
    //    }

    //    IQueryable<Partner> IBookPublishContext.Partners
    //    {
    //        get { return Partners; }
    //    }

    //    IQueryable<Pressure> IBookPublishContext.Pressure
    //    {
    //        get { return Pressure; }
    //    }

    //    IQueryable<Theme> IBookPublishContext.Themes
    //    {
    //        get { return Themes; }
    //    }

    //    T IBookPublishContext.Add<T>(T entity)
    //    {
    //        return Set<T>().Add(entity);
    //    }

    //    T IBookPublishContext.Delete<T>(T entity)
    //    {
    //        return Set<T>().Remove(entity);
    //    }

    //    public AccountType FindAccountTypeById(int ID)
    //    {
    //        return Set<AccountType>().Find(ID);
    //    }

    //    public Author FindAuthorById(int ID)
    //    {
    //        return Set<Author>().Find(ID);
    //    }

    //    public Books FindBookById(int ID)
    //    {
    //        return Set<Books>().Find(ID);
    //    }

    //    public Cover FindCoverById(int ID)
    //    {
    //        return Set<Cover>().Find(ID);
    //    }

    //    public Depot FindDepotById(int ID)
    //    {
    //        return Set<Depot>().Find(ID);
    //    }

    //    public Depot_type FindDepot_typeById(int ID)
    //    {
    //        return Set<Depot_type>().Find(ID);
    //    }

    //    public Partner FindPartnerById(int ID)
    //    {
    //        return Set<Partner>().Find(ID);
    //    }

    //    public Press FindPressById(int ID)
    //    {
    //        return Set<Press>().Find(ID);
    //    }

    //    public Pressure FindPressureById(int ID)
    //    {
    //        return Set<Pressure>().Find(ID);
    //    }

    //    public Stockist_margin FindStockist_marginById(int ID)
    //    {
    //        return Set<Stockist_margin>().Find(ID);
    //    }

    //    public Theme FindThemeById(int ID)
    //    {
    //        return Set<Theme>().Find(ID);
    //    }
    //}
}
