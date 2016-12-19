using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Repositories
{
    public class FakturyZakupuRepository : KancelariaRepository
    {
        public IQueryable<FakturaZakupu> FakturyZakupu(int idFirmy, int idRoku, int? idKontrahenta)
        {
            return (from fz in db.FakturaZakupus
                    where fz.IdFirmy == idFirmy
                        && fz.IdRoku == idRoku
                        && (idKontrahenta.HasValue ? fz.IdKontrahenta == idKontrahenta : true)
                    select fz).AsQueryable();
        }

        public PagedSearchedQueryResult<FakturaZakupu> FakturyZakupu(int idFirmy, int idRoku, int page, string search, string asc, string desc, int pageSize = KancelariaSettings.PageSize, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var Query = QueryStringParser<FakturaZakupu>.Parse(
                    (from fz in db.FakturaZakupus 
                     where fz.IdFirmy == idFirmy 
                        && fz.IdRoku == idRoku
                        //&& (!dateFrom.HasValue || fz.DataFaktury >= dateFrom.Value) && (!dateTo.HasValue || fz.DataFaktury <= dateTo.Value)
                     select fz).SortBy(asc, desc, "DataFaktury", true), new FakturyZakupuDictionary(), ref search
                );

            if (dateFrom.HasValue)
                Query = Query.Where(p => p.DataFaktury >= dateFrom.Value);

            if (dateTo.HasValue)
                Query = Query.Where(p => p.DataFaktury <= dateTo.Value);

            if (search != null)
            {
                Query = Query.Where(
                    q => q.NumerFaktury.Contains(search.ToLower())
                        || q.Opis.ToLower().Contains(search.ToLower())
                        || q.WlasnyNumerFaktury.ToLower().Contains(search.ToLower())
                        || q.Kontrahent.KodKontrahenta.ToLower().Contains(search.ToLower())
                        || q.Kontrahent.NazwaKontrahenta.ToLower().Contains(search.ToLower())
                    //|| q.PozycjaFakturyZakupus.Sum(s => s.KwotaNetto).ToString().Contains(search) // TODO: dorobic na FZ - FS metody zwracajace sumy netto, brutto, vat
                );
            }

            return new PagedSearchedQueryResult<FakturaZakupu>(Query, page, pageSize, search);
        }

        public PagedSearchedQueryResult<FakturaZakupu> FakturyZakupu(int idFirmy, int idRoku, int? idKontrahenta, int page, string search, string asc, string desc, int pageSize = KancelariaSettings.PageSize, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var Query = QueryStringParser<FakturaZakupu>.Parse(
                    (from fz in db.FakturaZakupus
                     where fz.IdFirmy == idFirmy
                        && fz.IdRoku == idRoku
                        && (idKontrahenta.HasValue ? fz.IdKontrahenta == idKontrahenta : true)
                     select fz).SortBy(asc, desc, "DataFaktury", true), new FakturyZakupuDictionary(), ref search
                );

            if (dateFrom.HasValue)
                Query = Query.Where(p => p.DataFaktury >= dateFrom.Value);

            if (dateTo.HasValue)
                Query = Query.Where(p => p.DataFaktury <= dateTo.Value);

            if (search != null)
            {
                Query = Query.Where(
                    q => q.NumerFaktury.Contains(search.ToLower())
                        || q.Opis.Contains(search.ToLower())
                        || q.WlasnyNumerFaktury.Contains(search.ToLower())
                    //|| q.PozycjaFakturyZakupus.Sum(s => s.KwotaNetto).ToString().Contains(search) // TODO: dorobic na FZ - FS metody zwracajace sumy netto, brutto, vat
                );
            }

            return new PagedSearchedQueryResult<FakturaZakupu>(Query, page, pageSize, search);
        }

        public PagedSearchedQueryResult<NieuregulowanaFakturaZakupu> NieuregulowaneFakturyZakupu(int idFirmy, int idRoku, int? idKontrahenta, int page, string search, string asc, string desc, 
            int pageSize = KancelariaSettings.PageSize, DateTime? stanNaDzien = null)
        {
            var Query = QueryStringParser<NieuregulowanaFakturaZakupu>.Parse(
                    (from fz in db.NieuregulowanaFakturaZakupus
                     where fz.IdFirmy == idFirmy
                        && fz.IdRoku == idRoku
                        && (idKontrahenta.HasValue ? fz.IdKontrahenta == idKontrahenta : true)
                        && fz.KwotaNieuregulowana != 0
                     select fz).SortBy(asc, desc, "DataFaktury"), new NieuregulowanaFakturyZakupuDictionary(), ref search
                );

            if (search != null)
            {
                Query = Query.Where(
                    q => q.NumerFaktury.Contains(search.ToLower())
                        || q.Opis.Contains(search.ToLower())
                        || q.NumerFaktury.Contains(search.ToLower())
                );
            }

            if (stanNaDzien.HasValue)
                Query = Query.Where(p => p.TerminPlatnosci <= stanNaDzien.Value);

            if (idKontrahenta.HasValue)
                Query = Query.Where(q => q.IdKontrahenta == idKontrahenta);

            return new PagedSearchedQueryResult<NieuregulowanaFakturaZakupu>(Query, page, pageSize, search);
        }

        public FakturaZakupu FakturaZakupu(int id)
        {
            return (from fz in db.FakturaZakupus
                    where fz.Id == id
                    select fz).FirstOrDefault();
        }

        public void DodajPozycjeFakturyZakupu(PozycjaFakturyZakupu pozycjaFakturyZakupu)
        {
            db.PozycjaFakturyZakupus.InsertOnSubmit(pozycjaFakturyZakupu);
        }

        public PozycjaFakturyZakupu PozycjaFakturyZakupu(int id)
        {
            return (from pfz in db.PozycjaFakturyZakupus
                    where pfz.Id == id
                    select pfz).FirstOrDefault();
        }

        public void UsunPozycjeFaktury(PozycjaFakturyZakupu PozycjaFaktury)
        {
            db.PozycjaFakturyZakupus.DeleteOnSubmit(PozycjaFaktury);
        }

        public void Dodaj(FakturaZakupu fakturaZakupu)
        {
            db.FakturaZakupus.InsertOnSubmit(fakturaZakupu);
        }

        public void Usun(FakturaZakupu FakturaZakupu)
        {
            db.FakturaZakupus.DeleteOnSubmit(FakturaZakupu);
        }

        //public IQueryable<PozycjaFakturyZakupu> PozycjeFakturyZakupu(int id)
        //{
        //    return (from pfz in db.PozycjaFakturyZakupus
        //            where pfz.IdFaktury == id
        //            select pfz).AsQueryable();
        //}

        public PagedSearchedQueryResult<ZaplataFakturyZakupu> Zaplaty(int idFakturyZakupu, int page, string search, string asc, string desc, int pageSize = KancelariaSettings.PageSize)
        {
            var Query = QueryStringParser<ZaplataFakturyZakupu>.Parse(
                    (from zfz in db.ZaplataFakturyZakupus
                     where zfz.IdFakturyZakupu == idFakturyZakupu
                     select zfz).SortBy(asc, desc, "DataZaplaty", true), new ZaplatyFakturZakupuDictionary(), ref search
                );

            if (search != null)
            {
                Query = Query.Where(
                    q => search.Length == 0 || q.Opis.Contains(search.ToLower())
                );
            }

            return new PagedSearchedQueryResult<ZaplataFakturyZakupu>(Query, page, pageSize, search);
        }

        public ZaplataFakturyZakupu Zaplata(int id)
        {
            return (from zfz in db.ZaplataFakturyZakupus
                    where zfz.Id == id
                    select zfz).FirstOrDefault();
        }

        public void DodajZaplate(ZaplataFakturyZakupu fakturaZakupu)
        {
            db.ZaplataFakturyZakupus.InsertOnSubmit(fakturaZakupu);
        }

        public void UsunZaplate(ZaplataFakturyZakupu FakturaZakupu)
        {
            db.ZaplataFakturyZakupus.DeleteOnSubmit(FakturaZakupu);
        }

        public PagedSearchedQueryResult<KosztNaInwestycjach> AnalizaKosztowNaInwestycjach(int idFirmy, int idRoku, int? idKontrahenta, int? idTypuInwestycji, int page, string search, string asc, string desc, int pageSize = KancelariaSettings.PageSize, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var Query = (from kni in db.KosztNaInwestycjaches
                         where kni.IdFirmy == idFirmy
                            && kni.IdRoku == idRoku
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
                    //|| q.PozycjaFakturyZakupus.Sum(s => s.KwotaNetto).ToString().Contains(search) // TODO: dorobic na FZ - FS metody zwracajace sumy netto, brutto, vat
                );
            }

            return new PagedSearchedQueryResult<KosztNaInwestycjach>(Query, page, pageSize, search);
        }
    }
}