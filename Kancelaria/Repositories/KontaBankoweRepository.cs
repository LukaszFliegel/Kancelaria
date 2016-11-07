using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kancelaria.Repositories.Interfaces;

namespace Kancelaria.Repositories
{
    public class KontaBankoweRepository : KancelariaRepository, IKontaBankoweRepository
    {
        public IQueryable<KontoBankowe> KontaBankowe(int idFirmy)
        {
            return (from kb in db.KontoBankowes
                    where kb.IdFirmy == idFirmy
                    select kb).AsQueryable();
        }

        public PagedSearchedQueryResult<KontoBankowe> KontaBankowe(int idFirmy, int page)
        {
            var result = (from kb in db.KontoBankowes
                          where kb.IdFirmy == idFirmy
                          select kb).AsQueryable();

            return new PagedSearchedQueryResult<KontoBankowe>(result, page);
        }

        public int GetDefaultId(int idFirmy)
        {
            var result = (from kb in db.KontoBankowes
                          where kb.CzyDomyslny == true
                            && kb.IdFirmy == idFirmy
                          select kb).FirstOrDefault();

            if (result == null)
            {
                result = (from kb in db.KontoBankowes
                          select kb).First();
            }

            if (result == null) return 0;

            return result.Id;
        }

        public void SetDefault(int idFirmy, int id)
        {
            var OldDefault = (from kb in db.KontoBankowes
                              where kb.CzyDomyslny == true
                              && kb.IdFirmy == idFirmy
                              select kb).FirstOrDefault();

            if (OldDefault != null)
            {
                OldDefault.CzyDomyslny = false;
            }

            // musze tu zrobic save, bo update idzie kolejno po liscie, wiec jesli ustawimy domyslny jakis rekord na na true
            // jednoczesnie odznaczajac stary domyslny na false, to bez save'a to przejdzie tylko wtedy, jesli najpierw przesatwi sie ten na false 
            // a dopiero potem ten na true (a to zalezy od kolejnosci na pobranej liscie)
            Save();

            var NewDefault = (from kb in db.KontoBankowes
                              where kb.Id == id
                              select kb).First();

            NewDefault.CzyDomyslny = true;
        }

        public PagedSearchedQueryResult<KontoBankowe> KontaBankowe(int idFirmy, int page, string search, string asc, string desc)
        {
            if (search == null) search = "";

            var Query = QueryStringParser<KontoBankowe>.Parse(
                    (from kb in db.KontoBankowes where kb.IdFirmy == idFirmy select kb).SortBy(asc, desc, "Id"), new KontaBankoweDictionary(), ref search
                ).Where(
                    q => q.NumerKonta.ToLower().Contains(search.ToLower())
                        );

            return new PagedSearchedQueryResult<KontoBankowe>(Query, page, search);
        }

        public KontoBankowe KontoBankowe(int id)
        {
            return (from i in db.KontoBankowes
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public void Dodaj(KontoBankowe kontoBankowe)
        {
            db.KontoBankowes.InsertOnSubmit(kontoBankowe);
        }

        public void Usun(KontoBankowe kontoBankowe)
        {
            db.KontoBankowes.DeleteOnSubmit(kontoBankowe);
        }

        public int Count(int idFirmy)
        {
            return db.KontoBankowes.Where(p => p.IdFirmy == idFirmy).Count();
        }
    }
}