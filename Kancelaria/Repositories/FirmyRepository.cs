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
    public class FirmyRepository : KancelariaRepository, IFirmyRepository
    {
        public PagedSearchedQueryResult<Firma> Firmy(int page)
        {
            var result = (from f in db.Firmas
                    select f).AsQueryable();

            return new PagedSearchedQueryResult<Firma>(result, page);
        }

        public PagedSearchedQueryResult<Firma> Firmy(int page, string search, string asc, string desc)
        {
            //var result = (from i in db.Firmas
            //        select i).AsQueryable();

            //return new PagedSearchedQueryResult<Firma>(result, page, KancelariaSettings.PageSize);

            if (search == null) search = "";

            var Query = QueryStringParser<Firma>.Parse(
                    (from f in db.Firmas select f).SortBy(asc, desc, "NazwaSkrocona"), new FirmyDictionary(), ref search
                ).Where(
                    q => q.NazwaSkrocona.ToLower().Contains(search.ToLower())
                    || q.NazwaPelna.ToLower().Contains(search.ToLower())
                );

            return new PagedSearchedQueryResult<Firma>(Query, page, search);
        }

        public Firma Firma(int id)
        {
            return (from i in db.Firmas
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public void Dodaj(Firma firma)
        {
            db.Firmas.InsertOnSubmit(firma);
        }

        public void Usun(Firma firma)
        {
            db.Firmas.DeleteOnSubmit(firma);
        }

        public int Count()
        {
            return db.Firmas.Count();
        }

        public void WybierzIdFirmy(int idFirmy, string userName)
        {
            try
            {
                db.Uzytkowniks.First(p => p.UserName == userName).WybraneIdFirmy = idFirmy;
            }
            catch (Exception e)
            {
                Logger.Error("Niepowodzenie zapisu wybranego Id firmy", e);
            }
        }

        public int? WybraneIdFirmy(string userName)
        {
            int? idFirmy = null;

            try
            {
                idFirmy = db.Uzytkowniks.First(p => p.UserName == userName).WybraneIdFirmy;
            }
            catch (Exception e)
            {
                Logger.Error("Niepowodzenie odczytu wybranego Id firmy", e);
            }

            return idFirmy;
        }
    }
}