using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Repositories
{
    public class KompensatyRepository : KancelariaRepository
    {
        public IQueryable<Kompensata> Kompensaty(int idFirmy, int idRoku)
        {
            return (from i in db.Kompensatas
                    where i.IdFirmy == idFirmy
                        && i.IdRoku == idRoku
                    select i).AsQueryable();
        }

        public PagedSearchedQueryResult<Kompensata> Kompensaty(int idFirmy, int idRoku, int page)
        {
            var result = (from i in db.Kompensatas
                          where i.IdFirmy == idFirmy
                            && i.IdRoku == idRoku
                          select i).AsQueryable();

            return new PagedSearchedQueryResult<Kompensata>(result, page);
        }

        public PagedSearchedQueryResult<Kompensata> Kompensaty(int idFirmy, int idRoku, int page, string search, string asc, string desc)
        {
            if (search == null) search = "";

            var Query = QueryStringParser<Kompensata>.Parse(
                    (from i in db.Kompensatas where i.IdFirmy == idFirmy && i.IdRoku == idRoku select i).SortBy(asc, desc, "NumerKompensaty"), new KompensatyDictionary(), ref search
                ).Where(
                    q => q.NumerKompensaty.ToLower().Contains(search.ToLower())
                        );

            return new PagedSearchedQueryResult<Kompensata>(Query, page, KancelariaSettings.PageSize, search);
        }

        public Kompensata Kompensata(int id)
        {
            return (from i in db.Kompensatas
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public void Dodaj(Kompensata kompensata)
        {
            db.Kompensatas.InsertOnSubmit(kompensata);
        }

        public void Usun(Kompensata kompensata)
        {
            db.Kompensatas.DeleteOnSubmit(kompensata);
        }

        public int Count(int idFirmy)
        {
            return db.Kompensatas.Where(p => p.IdFirmy == idFirmy).Count();
        }

        // pozycje kompensaty

        public void DodajPozycjeKompensaty(KompensataPowiazanie kompensataPowiazanie)
        {
            db.KompensataPowiazanies.InsertOnSubmit(kompensataPowiazanie);
        }

        public KompensataPowiazanie KompensataPowiazanie(int id)
        {
            return (from pfz in db.KompensataPowiazanies
                    where pfz.Id == id
                    select pfz).FirstOrDefault();
        }

        public void UsunPowiazanieKompensaty(KompensataPowiazanie kompensataPowiazanie)
        {
            db.KompensataPowiazanies.DeleteOnSubmit(kompensataPowiazanie);
        }
    }
}