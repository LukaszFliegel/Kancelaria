using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kancelaria.Repositories.Interfaces;

namespace Kancelaria.Repositories
{
    public class RaportyRepository : KancelariaRepository, IRaportyRepository
    {
        public IQueryable<AnalizaRozrachunku> AnalizaRazrachunkow(int idFirmy, int idRoku)
        {
            return (from ar in db.AnalizaRozrachunkus
                    where ar.IdFirmy == idFirmy
                        && ar.IdRoku == idRoku
                    orderby ar.KodKontrahenta, ar.DataFaktury
                    select ar).AsQueryable();
        }

        public IQueryable<KosztNaInwestycjach> AnalizaKosztowNaInwestycjach(int idFirmy, int? idKontrahenta, int? idTypuInwestycji, int page, string search, string asc, string desc, int pageSize = KancelariaSettings.PageSize, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var Query = (from kni in db.KosztNaInwestycjaches
                         where kni.IdFirmy == idFirmy
                            && (idKontrahenta.HasValue ? kni.IdKontrahenta == idKontrahenta : true)
                            && (idTypuInwestycji.HasValue ? kni.IdTypuInwestycji == idTypuInwestycji : true)
                         orderby kni.NumerInwestycji
                         select kni).AsQueryable();

            if (dateFrom.HasValue)
                Query = Query.Where(p => p.DataFaktury >= dateFrom.Value);

            if (dateTo.HasValue)
                Query = Query.Where(p => p.DataFaktury <= dateTo.Value);

            if (search != null)
            {
                Query = Query.Where(
                    q => q.NumerInwestycji.Contains(search.ToLower())
                        || q.NumerFaktury.Contains(search.ToLower())
                        || q.Opis.Contains(search.ToLower())
                );
            }

            return Query;
        }
    }
}