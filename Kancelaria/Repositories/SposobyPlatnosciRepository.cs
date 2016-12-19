using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Repositories
{
    public class SposobyPlatnosciRepository : KancelariaRepository
    {
        public IQueryable<SposobPlatnosci> SposobyPlatnosci()
        {
            return (from sp in db.SposobPlatnoscis
                    select sp).AsQueryable();
        }

        public PagedSearchedQueryResult<SposobPlatnosci> SposobyPlatnosci(int page)
        {
            var result = (from sp in db.SposobPlatnoscis
                          select sp).AsQueryable();

            return new PagedSearchedQueryResult<SposobPlatnosci>(result, page);
        }

        public int GetDefaultId()
        {
            var result = (from sp in db.SposobPlatnoscis
                          where sp.CzyDomyslny == true
                          select sp).FirstOrDefault();

            if (result == null)
            {
                result = (from sp in db.SposobPlatnoscis
                          select sp).First();
            }

            if (result == null) return 0;

            return result.Id;
        }

        public void SetDefault(int id)
        {
            var OldDefault = (from sp in db.SposobPlatnoscis
                          where sp.CzyDomyslny == true
                          select sp).FirstOrDefault();

            if (OldDefault != null)
            {
                OldDefault.CzyDomyslny = false;
            }

            // musze tu zrobic save, bo update idzie kolejno po liscie, wiec jesli ustawimy domyslny jakis rekord na na true
            // jednoczesnie odznaczajac stary domyslny na false, to bez save'a to przejdzie tylko wtedy, jesli najpierw przesatwi sie ten na false 
            // a dopiero potem ten na true (a to zalezy od kolejnosci na pobranej liscie)
            Save();

            var NewDefault = (from sp in db.SposobPlatnoscis
                              where sp.Id == id
                              select sp).First();

            NewDefault.CzyDomyslny = true;
        }

        public PagedSearchedQueryResult<SposobPlatnosci> SposobyPlatnosci(int page, string search, string asc, string desc)
        {
            //var result = (from i in db.SposobPlatnoscis
            //        select i).AsQueryable();

            //return new PagedSearchedQueryResult<SposobPlatnosci>(result, page, KancelariaSettings.PageSize);

            if (search == null) search = "";

            var Query = QueryStringParser<SposobPlatnosci>.Parse(
                    (from sp in db.SposobPlatnoscis select sp).SortBy(asc, desc, "KodSposobuPlatnosci"), new SposobyPlatnosciDictionary(), ref search
                ).Where(
                    q => q.KodSposobuPlatnosci.ToLower().Contains(search.ToLower())
                        || q.OpisSposobuPlatnosci.ToLower().Contains(search.ToLower())
                        );

            return new PagedSearchedQueryResult<SposobPlatnosci>(Query, page, search);
        }

        public SposobPlatnosci SposobPlatnosci(int id)
        {
            return (from i in db.SposobPlatnoscis
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public void Dodaj(SposobPlatnosci inwestycja)
        {
            db.SposobPlatnoscis.InsertOnSubmit(inwestycja);
        }

        public void Usun(SposobPlatnosci inwestycja)
        {
            db.SposobPlatnoscis.DeleteOnSubmit(inwestycja);
        }

        public int Count()
        {
            return db.SposobPlatnoscis.Count();
        }
    }
}