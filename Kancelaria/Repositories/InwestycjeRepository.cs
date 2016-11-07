using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Repositories
{
    public class InwestycjeRepository : KancelariaRepository
    {
        public IQueryable<Inwestycja> Inwestycje(int idFirmy)
        {
            return (from i in db.Inwestycjas
                    where i.IdFirmy == idFirmy
                    select i).AsQueryable();
        }

        public PagedSearchedQueryResult<Inwestycja> Inwestycje(int idFirmy, int page, string search)
        {
            var Query = (from i in db.Inwestycjas
                          where i.IdFirmy == idFirmy
                          select i).AsQueryable();

            if (search != null)
            {
                Query = Query.Where(
                    o => o.NumerInwestycji.ToLower().Contains(search.ToLower())
                    || o.NumerUmowy.ToLower().Contains(search.ToLower())
                );
            }

            return new PagedSearchedQueryResult<Inwestycja>(Query, page);
        }

        public int GetDefaultId(int idFirmy)
        {
            var result = (from i in db.Inwestycjas
                          where i.CzyDomyslny == true
                          && i.IdFirmy == idFirmy
                          select i).FirstOrDefault();

            if (result == null)
            {
                result = (from i in db.Inwestycjas
                          where i.IdFirmy == idFirmy
                          select i).FirstOrDefault();
            }

            if (result == null) return 0;

            return result.Id;
        }

        public void SetDefault(int idFirmy, int id)
        {
            var OldDefault = (from i in db.Inwestycjas
                              where i.CzyDomyslny == true
                              && i.IdFirmy == idFirmy
                              select i).FirstOrDefault();

            if (OldDefault != null)
            {
                OldDefault.CzyDomyslny = false;
            }

            // musze tu zrobic save, bo update idzie kolejno po liscie, wiec jesli ustawimy domyslny jakis rekord na na true
            // jednoczesnie odznaczajac stary domyslny na false, to bez save'a to przejdzie tylko wtedy, jesli najpierw przesatwi sie ten na false 
            // a dopiero potem ten na true (a to zalezy od kolejnosci na pobranej liscie)
            Save();

            var NewDefault = (from i in db.Inwestycjas
                              where i.Id == id
                              && i.IdFirmy == idFirmy
                              select i).First();

            NewDefault.CzyDomyslny = true;
        }

        public PagedSearchedQueryResult<Inwestycja> Inwestycje(int idFirmy, int page, string search, string asc, string desc, int pageSize = KancelariaSettings.PageSize)
        {
            //var result = (from i in db.Inwestycjas
            //        select i).AsQueryable();

            //return new PagedSearchedQueryResult<Inwestycja>(result, page, KancelariaSettings.PageSize);

            if (search == null) search = "";

            var Query = QueryStringParser<Inwestycja>.Parse(
                    (from i in db.Inwestycjas where i.IdFirmy == idFirmy select i).SortBy(asc, desc, "NumerInwestycji"), new InwestycjeDictionary(), ref search
                ).Where(
                    q => q.NumerInwestycji.ToLower().Contains(search.ToLower())
                        || q.NumerUmowy.ToLower().Contains(search.ToLower())
                        );

            return new PagedSearchedQueryResult<Inwestycja>(Query, page, pageSize, search);
        }

        public Inwestycja Inwestycja(int id)
        {
            return (from i in db.Inwestycjas
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public void Dodaj(Inwestycja inwestycja)
        {
            db.Inwestycjas.InsertOnSubmit(inwestycja);
        }

        public void Usun(Inwestycja inwestycja)
        {
            db.Inwestycjas.DeleteOnSubmit(inwestycja);
        }

        public int Count(int idFirmy)
        {
            return db.Inwestycjas.Where(p => p.IdFirmy == idFirmy).Count();
        }
    }
}