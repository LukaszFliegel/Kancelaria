using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Repositories
{
    public class TypyInwestycjiRepository : KancelariaRepository
    {
        public IQueryable<TypInwestycji> TypyInwestycji()
        {
            return (from ti in db.TypInwestycjis
                    select ti).AsQueryable();
        }

        public PagedSearchedQueryResult<TypInwestycji> TypyInwestycji(int page)
        {
            var result = (from ti in db.TypInwestycjis
                          select ti).AsQueryable();

            return new PagedSearchedQueryResult<TypInwestycji>(result, page);
        }

        public TypInwestycji TypInwestycji(int id)
        {
            return (from i in db.TypInwestycjis
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public void SetDefault(int id)
        {
            var OldDefault = (from i in db.TypInwestycjis
                              where i.CzyDomyslny == true
                              select i).FirstOrDefault();

            if (OldDefault != null)
            {
                OldDefault.CzyDomyslny = false;
            }

            // musze tu zrobic save, bo update idzie kolejno po liscie, wiec jesli ustawimy domyslny jakis rekord na na true
            // jednoczesnie odznaczajac stary domyslny na false, to bez save'a to przejdzie tylko wtedy, jesli najpierw przesatwi sie ten na false 
            // a dopiero potem ten na true (a to zalezy od kolejnosci na pobranej liscie)
            Save();

            var NewDefault = (from i in db.TypInwestycjis
                              where i.Id == id
                              select i).First();

            NewDefault.CzyDomyslny = true;
        }

        public int? GetDefaultId()
        {
            var result = (from sp in db.TypInwestycjis
                          where sp.CzyDomyslny == true
                          select sp).FirstOrDefault();

            if (result == null)
            {
                result = (from sp in db.TypInwestycjis
                          select sp).FirstOrDefault();
            }

            if (result == null) return null;

            return result.Id;
        }

        public void Dodaj(TypInwestycji inwestycja)
        {
            db.TypInwestycjis.InsertOnSubmit(inwestycja);
        }

        public void Usun(TypInwestycji inwestycja)
        {
            db.TypInwestycjis.DeleteOnSubmit(inwestycja);
        }

        public int Count()
        {
            return db.TypInwestycjis.Count();
        }
    }
}