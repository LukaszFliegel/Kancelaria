using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Repositories
{
    public class LataObrotoweRepository : KancelariaRepository
    {
        public PagedSearchedQueryResult<RokObrotowy> LataObrotowe(int idFirmy, int page)
        {
            var result = (from ro in db.RokObrotowies
                          where ro.IdFirmy == idFirmy
                          select ro).AsQueryable();

            return new PagedSearchedQueryResult<RokObrotowy>(result, page);
        }

        public PagedSearchedQueryResult<RokObrotowy> LataObrotowe(int idFirmy, int page, string search, string asc, string desc)
        {
            //var result = (from lo in db.RokObrotowies
            //              where lo.IdFirmy == idFirmy
            //              select lo).AsQueryable();

            //return new PagedSearchedQueryResult<RokObrotowy>(result, page, KancelariaSettings.PageSize);

            if (search == null) search = "";

            var Query = QueryStringParser<RokObrotowy>.Parse(
                    (from ro in db.RokObrotowies where ro.IdFirmy == idFirmy select ro).SortBy(asc, desc, "NazwaRoku"), new LataObrotoweDictionary(), ref search
                ).Where(
                    q => q.NazwaRoku.ToLower().Contains(search.ToLower())
                );

            return new PagedSearchedQueryResult<RokObrotowy>(Query, page, search);
        }

        public RokObrotowy RokObrotowy(int id)
        {
            return (from i in db.RokObrotowies
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public void Dodaj(RokObrotowy rokObrotowy)
        {
            db.RokObrotowies.InsertOnSubmit(rokObrotowy);
        }

        public void Usun(RokObrotowy rokObrotowy)
        {
            db.RokObrotowies.DeleteOnSubmit(rokObrotowy);
        }

        public int Count(int idFirmy)
        {
            return db.RokObrotowies.Where(p => p.IdFirmy == idFirmy).Count();
        }

        public void WybierzIdRoku(int idRoku, string userName)
        {
            try
            {
                db.Uzytkowniks.First(p => p.UserName == userName).WybraneIdRoku = idRoku;
            }
            catch (Exception e)
            {
                Logger.Error("Niepowodzenie zapisu wybranego Id roku", e);
            }
        }

        public int? WybraneIdRoku(string userName)
        {
            int? idRoku = null;

            try
            {
                idRoku = db.Uzytkowniks.First(p => p.UserName == userName).WybraneIdRoku;
            }
            catch (Exception e)
            {
                Logger.Error("Niepowodzenie odczytu wybranego Id roku", e);
            }

            return idRoku;
        }

        // for testing purphoses
        public int? WybraneIdRokuOrExcepion(string userName)
        {
            int? idRoku = null;

            try
            {
                idRoku = db.Uzytkowniks.First(p => p.UserName == userName).WybraneIdRoku;
            }
            catch (Exception e)
            {
                Logger.Error("Niepowodzenie odczytu wybranego Id roku", e);
                throw;
            }

            return idRoku;
        }
    }
}