using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Repositories
{
    public class FakturySprzedazyRepository : KancelariaRepository
    {
        public IQueryable<FakturaSprzedazy> FakturySprzedazy(int idFirmy, int idRoku, int? idKontrahenta)
        {
            return (from fz in db.FakturaSprzedazies
                    where fz.IdFirmy == idFirmy
                        && fz.IdRoku == idRoku
                        && (idKontrahenta.HasValue ? fz.IdKontrahenta == idKontrahenta : true)
                    select fz).AsQueryable();
        }

        public PagedSearchedQueryResult<FakturaSprzedazy> FakturySprzedazy(int idFirmy, int idRoku, int page, string search, string asc, string desc, 
            int pageSize = KancelariaSettings.PageSize, DateTime? dateFrom = null, DateTime? dateTo = null, bool czyRazemZKorektami = false
        )
        {
            var Query = QueryStringParser<FakturaSprzedazy>.Parse(
                (
                    from fz in db.FakturaSprzedazies 
                    where fz.IdFirmy == idFirmy 
                        && fz.IdRoku == idRoku
                        && (czyRazemZKorektami ? true : !fz.IdFakturyKorygujacej.HasValue)
                    select fz
                ).SortBy(asc, desc, "DataFaktury", true), 
                new FakturySprzedazyDictionary(), 
                ref search
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
                        || q.NumerFaktury.Contains(search.ToLower())
                        || q.Kontrahent.KodKontrahenta.Contains(search.ToLower())
                        || q.Kontrahent.NazwaKontrahenta.Contains(search.ToLower())
                    //|| q.PozycjaFakturySprzedazies.Sum(s => s.KwotaNetto).ToString().Contains(search) // TODO: dorobic na FZ - FS metody zwracajace sumy netto, brutto, vat
                );
            }

            return new PagedSearchedQueryResult<FakturaSprzedazy>(Query, page, pageSize, search);
        }

        public PagedSearchedQueryResult<FakturaSprzedazy> FakturySprzedazy(int idFirmy, int idRoku, int? idKontrahenta, int? idInwestycji, int page, string search, string asc, 
            string desc, int pageSize = KancelariaSettings.PageSize, DateTime? dateFrom = null, DateTime? dateTo = null, bool czyRazemZKorektami = false)
        {
            var Query = QueryStringParser<FakturaSprzedazy>.Parse(
                    (from fz in db.FakturaSprzedazies 
                     where fz.IdFirmy == idFirmy 
                        && fz.IdRoku == idRoku 
                        && (idKontrahenta.HasValue ? fz.IdKontrahenta == idKontrahenta : true)
                        && (idInwestycji.HasValue ? fz.IdInwestycji == idInwestycji : true)
                        && (czyRazemZKorektami ? true : !fz.IdFakturyKorygujacej.HasValue)
                     select fz).SortBy(asc, desc, "DataFaktury", true), new FakturySprzedazyDictionary(), ref search
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
                        || q.NumerFaktury.Contains(search.ToLower())
                    //|| q.PozycjaFakturySprzedazies.Sum(s => s.KwotaNetto).ToString().Contains(search) // TODO: dorobic na FZ - FS metody zwracajace sumy netto, brutto, vat
                );
            }

            if (idKontrahenta.HasValue)
                Query = Query.Where(q => q.IdKontrahenta == idKontrahenta);

            return new PagedSearchedQueryResult<FakturaSprzedazy>(Query, page, pageSize, search);
        }

        public PagedSearchedQueryResult<NieuregulowanaFakturaSprzedazy> NieuregulowaneFakturySprzedazy(int idFirmy, int idRoku, int? idKontrahenta, int? idInwestycji,
            int page, string search, string asc, string desc, int pageSize = KancelariaSettings.PageSize, bool czyRazemZKorektami = false, DateTime? stanNaDzien = null)
        {
            var Query = QueryStringParser<NieuregulowanaFakturaSprzedazy>.Parse(
                    (from fz in db.NieuregulowanaFakturaSprzedazies
                     where fz.IdFirmy == idFirmy
                        && fz.IdRoku == idRoku
                        && (idKontrahenta.HasValue ? fz.IdKontrahenta == idKontrahenta : true)
                        && (idInwestycji.HasValue ? fz.IdInwestycji == idInwestycji : true)
                        && fz.KwotaNieuregulowana != 0
                        && (czyRazemZKorektami ? true : !fz.IdFakturyKorygujacej.HasValue)
                     select fz).SortBy(asc, desc, "DataFaktury"), new NieuregulowanaFakturySprzedazyDictionary(), ref search
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

            return new PagedSearchedQueryResult<NieuregulowanaFakturaSprzedazy>(Query, page, pageSize, search);
        }

        public FakturaSprzedazy FakturaSprzedazy(int id)
        {
            return (from fz in db.FakturaSprzedazies
                    where fz.Id == id
                    select fz).FirstOrDefault();
        }

        public void DodajPozycjeFakturySprzedazy(PozycjaFakturySprzedazy pozycjaFakturySprzedazy)
        {
            db.PozycjaFakturySprzedazies.InsertOnSubmit(pozycjaFakturySprzedazy);
        }

        public PozycjaFakturySprzedazy PozycjaFakturySprzedazy(int id)
        {
            return (from pfz in db.PozycjaFakturySprzedazies
                    where pfz.Id == id
                    select pfz).FirstOrDefault();
        }

        public void UsunPozycjeFaktury(PozycjaFakturySprzedazy PozycjaFaktury)
        {
            db.PozycjaFakturySprzedazies.DeleteOnSubmit(PozycjaFaktury);
        }

        public void Dodaj(FakturaSprzedazy fakturaSprzedazy)
        {
            db.FakturaSprzedazies.InsertOnSubmit(fakturaSprzedazy);
        }

        public void Usun(FakturaSprzedazy FakturaSprzedazy)
        {
            db.FakturaSprzedazies.DeleteOnSubmit(FakturaSprzedazy);
        }

        public PagedSearchedQueryResult<ZaplataFakturySprzedazy> Zaplaty(int idFakturySprzedazy, int page, string search, string asc, string desc, int pageSize = KancelariaSettings.PageSize)
        {
            var Query = QueryStringParser<ZaplataFakturySprzedazy>.Parse(
                    (from zfz in db.ZaplataFakturySprzedazies
                     where zfz.IdFakturySprzedazy == idFakturySprzedazy
                     select zfz).SortBy(asc, desc, "DataZaplaty", true), new ZaplatyFakturSprzedazyDictionary(), ref search
                );

            if (search != null)
            {
                Query = Query.Where(
                    q => search.Length == 0 || q.Opis.Contains(search.ToLower())
                );
            }

            return new PagedSearchedQueryResult<ZaplataFakturySprzedazy>(Query, page, pageSize, search);
        }

        public ZaplataFakturySprzedazy Zaplata(int id)
        {
            return (from zfz in db.ZaplataFakturySprzedazies
                    where zfz.Id == id
                    select zfz).FirstOrDefault();
        }

        public void DodajZaplate(ZaplataFakturySprzedazy fakturaSprzedazy)
        {
            db.ZaplataFakturySprzedazies.InsertOnSubmit(fakturaSprzedazy);
        }

        public void UsunZaplate(ZaplataFakturySprzedazy FakturaSprzedazy)
        {
            db.ZaplataFakturySprzedazies.DeleteOnSubmit(FakturaSprzedazy);
        }

        public int SkorygujFaktureSprzdazy(int id)
        {
            return db.SkorygujFaktureSprzedazy(id).Select(p => p.Column1.Value).First();
        }
    }
}