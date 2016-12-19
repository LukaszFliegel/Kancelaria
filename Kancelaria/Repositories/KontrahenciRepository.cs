using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Repositories
{
    public class KontrahenciRepository : KancelariaRepository
    {
        public IQueryable<Kontrahent> Kontrahenci(int idFirmy)
        {
            return (from k in db.Kontrahents
                    where k.IdFirmy == idFirmy
                    select k).AsQueryable();
        }

        public PagedSearchedQueryResult<Kontrahent> Kontrahenci(int idFirmy, int page, string search)
        {
            var Query = (from k in db.Kontrahents
                          where k.IdFirmy == idFirmy
                          select k).AsQueryable();

            if (search != null)
            {
                Query = Query.Where(
                    o => o.KodKontrahenta.ToLower().Contains(search.ToLower())
                    || o.NazwaKontrahenta.ToLower().Contains(search.ToLower())
                );
            }

            return new PagedSearchedQueryResult<Kontrahent>(Query, page);
        }

        public int GetDefaultId(int idFirmy)
        {
            var result = (from k in db.Kontrahents
                          where k.CzyDomyslny == true
                          && k.IdFirmy == idFirmy
                          select k).FirstOrDefault();

            if (result == null)
            {
                result = (from k in db.Kontrahents
                          where k.IdFirmy == idFirmy
                          select k).First();
            }

            if (result == null) return 0;

            return result.Id;
        }

        public void SetDefault(int idFirmy, int id)
        {
            var OldDefault = (from k in db.Kontrahents
                              where k.CzyDomyslny == true
                              && k.IdFirmy == idFirmy
                              select k).FirstOrDefault();

            if (OldDefault != null)
            {
                OldDefault.CzyDomyslny = false;
            }

            // musze tu zrobic save, bo update idzie kolejno po liscie, wiec jesli ustawimy domyslny jakis rekord na na true
            // jednoczesnie odznaczajac stary domyslny na false, to bez save'a to przejdzie tylko wtedy, jesli najpierw przesatwi sie ten na false 
            // a dopiero potem ten na true (a to zalezy od kolejnosci na pobranej liscie)
            Save();

            var NewDefault = (from k in db.Kontrahents
                              where k.Id == id
                              && k.IdFirmy == idFirmy
                              select k).First();

            NewDefault.CzyDomyslny = true;
        }

        public PagedSearchedQueryResult<Kontrahent> Kontrahenci(int idFirmy, int page, string search, string asc, string desc, int pageSize = KancelariaSettings.PageSize)
        {
            //var result = (from k in db.Kontrahents
            //        select k).AsQueryable();

            //return new PagedSearchedQueryResult<Kontrahent>(result, page, KancelariaSettings.PageSize);

            if (search == null) search = "";

            var Query = QueryStringParser<Kontrahent>.Parse(
                    (from k in db.Kontrahents where k.IdFirmy == idFirmy select k).SortBy(asc, desc, "KodKontrahenta"), new KontrahenciDictionary(), ref search
                ).Where(
                    q => q.KodKontrahenta.ToLower().Contains(search.ToLower())
                        || q.Miejscowosc.ToLower().Contains(search.ToLower())
                        || q.NazwaKontrahenta.ToLower().Contains(search.ToLower())
                        || q.NIP.ToLower().Contains(search.ToLower())
                        || q.Panstwo.ToLower().Contains(search.ToLower())
                        || q.Ulica.ToLower().Contains(search.ToLower())
                        );

            return new PagedSearchedQueryResult<Kontrahent>(Query, page, pageSize, search);
        }

        public Kontrahent Kontrahent(int id)
        {
            return (from k in db.Kontrahents
                    where k.Id == id
                    select k).FirstOrDefault();
        }

        public void Dodaj(Kontrahent kontrahent)
        {
            db.Kontrahents.InsertOnSubmit(kontrahent);
        }

        public void Usun(Kontrahent kontrahent)
        {
            db.Kontrahents.DeleteOnSubmit(kontrahent);
        }

        public int Count(int idFirmy)
        {
            return db.Kontrahents.Where(p => p.IdFirmy == idFirmy).Count();
        }
    }
}