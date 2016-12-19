using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Repositories
{
    public class JednostkiMiaryRepository : KancelariaRepository
    {
        public IQueryable<JednostkaMiary> SposobyPlatnosci()
        {
            return (from sp in db.JednostkaMiaries
                    select sp).AsQueryable();
        }

        public PagedSearchedQueryResult<JednostkaMiary> SposobyPlatnosci(int page)
        {
            var result = (from sp in db.JednostkaMiaries
                          select sp).AsQueryable();

            return new PagedSearchedQueryResult<JednostkaMiary>(result, page);
        }

        public int GetDefaultId()
        {
            var result = (from sp in db.JednostkaMiaries
                          where sp.CzyDomyslna == true
                          select sp).FirstOrDefault();

            if (result == null)
            {
                result = (from sp in db.JednostkaMiaries
                          select sp).First();
            }

            if (result == null) return 0;

            return result.Id;
        }

        public void SetDefault(int id)
        {
            var OldDefault = (from sp in db.JednostkaMiaries
                              where sp.CzyDomyslna == true
                              select sp).FirstOrDefault();

            if (OldDefault != null)
            {
                OldDefault.CzyDomyslna = false;
            }

            // musze tu zrobic save, bo update idzie kolejno po liscie, wiec jesli ustawimy domyslny jakis rekord na na true
            // jednoczesnie odznaczajac stary domyslny na false, to bez save'a to przejdzie tylko wtedy, jesli najpierw przesatwi sie ten na false 
            // a dopiero potem ten na true (a to zalezy od kolejnosci na pobranej liscie)
            Save();

            var NewDefault = (from sp in db.JednostkaMiaries
                              where sp.Id == id
                              select sp).First();

            NewDefault.CzyDomyslna = true;
        }

        public PagedSearchedQueryResult<JednostkaMiary> SposobyPlatnosci(int page, string search, string asc, string desc)
        {
            //var result = (from i in db.JednostkaMiaries
            //        select i).AsQueryable();

            //return new PagedSearchedQueryResult<JednostkaMiary>(result, page, KancelariaSettings.PageSize);

            if (search == null) search = "";

            var Query = QueryStringParser<JednostkaMiary>.Parse(
                    (from sp in db.JednostkaMiaries select sp).SortBy(asc, desc, "KodJednostkiMiary"), new JednostkiMiaryDictionary(), ref search
                ).Where(
                    q => q.KodJednostkiMiary.ToLower().Contains(search.ToLower())
                        || q.OpisJednostkiMiary.ToLower().Contains(search.ToLower())
                        );

            return new PagedSearchedQueryResult<JednostkaMiary>(Query, page, search);
        }

        public JednostkaMiary JednostkaMiary(int id)
        {
            return (from i in db.JednostkaMiaries
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public void Dodaj(JednostkaMiary inwestycja)
        {
            db.JednostkaMiaries.InsertOnSubmit(inwestycja);
        }

        public void Usun(JednostkaMiary inwestycja)
        {
            db.JednostkaMiaries.DeleteOnSubmit(inwestycja);
        }

        public int Count()
        {
            return db.JednostkaMiaries.Count();
        }
    }
}