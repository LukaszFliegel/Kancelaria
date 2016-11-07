using System;
using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IFakturySprzedazyRepository : IKancelariaRepository
    {
        void Dodaj(FakturaSprzedazy fakturaSprzedazy);
        void DodajPozycjeFakturySprzedazy(PozycjaFakturySprzedazy pozycjaFakturySprzedazy);
        void DodajZaplate(ZaplataFakturySprzedazy fakturaSprzedazy);
        FakturaSprzedazy FakturaSprzedazy(int id);
        IQueryable<FakturaSprzedazy> FakturySprzedazy(int idFirmy, int idRoku, int? idKontrahenta);
        PagedSearchedQueryResult<FakturaSprzedazy> FakturySprzedazy(int idFirmy, int idRoku, int page, string search, string asc, string desc, int pageSize = 10, DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool czyRazemZKorektami = false);
        PagedSearchedQueryResult<FakturaSprzedazy> FakturySprzedazy(int idFirmy, int idRoku, int? idKontrahenta, int? idInwestycji, int page, string search, string asc, string desc, int pageSize = 10, DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?), bool czyRazemZKorektami = false);
        PagedSearchedQueryResult<NieuregulowanaFakturaSprzedazy> NieuregulowaneFakturySprzedazy(int idFirmy, int idRoku, int? idKontrahenta, int? idInwestycji, int page, string search, string asc, string desc, int pageSize = 10, bool czyRazemZKorektami = false, DateTime? stanNaDzien = default(DateTime?));
        PozycjaFakturySprzedazy PozycjaFakturySprzedazy(int id);
        int SkorygujFaktureSprzdazy(int id);
        void Usun(FakturaSprzedazy FakturaSprzedazy);
        void UsunPozycjeFaktury(PozycjaFakturySprzedazy PozycjaFaktury);
        void UsunZaplate(ZaplataFakturySprzedazy FakturaSprzedazy);
        ZaplataFakturySprzedazy Zaplata(int id);
        PagedSearchedQueryResult<ZaplataFakturySprzedazy> Zaplaty(int idFakturySprzedazy, int page, string search, string asc, string desc, int pageSize = 10);
    }
}