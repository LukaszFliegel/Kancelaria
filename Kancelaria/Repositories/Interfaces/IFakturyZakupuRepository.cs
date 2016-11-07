using System;
using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IFakturyZakupuRepository : IKancelariaRepository
    {
        PagedSearchedQueryResult<KosztNaInwestycjach> AnalizaKosztowNaInwestycjach(int idFirmy, int idRoku, int? idKontrahenta, int? idTypuInwestycji, int page, string search, string asc, string desc, int pageSize = 10, DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?));
        void Dodaj(FakturaZakupu fakturaZakupu);
        void DodajPozycjeFakturyZakupu(PozycjaFakturyZakupu pozycjaFakturyZakupu);
        void DodajZaplate(ZaplataFakturyZakupu fakturaZakupu);
        FakturaZakupu FakturaZakupu(int id);
        IQueryable<FakturaZakupu> FakturyZakupu(int idFirmy, int idRoku, int? idKontrahenta);
        PagedSearchedQueryResult<FakturaZakupu> FakturyZakupu(int idFirmy, int idRoku, int page, string search, string asc, string desc, int pageSize = 10, DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?));
        PagedSearchedQueryResult<FakturaZakupu> FakturyZakupu(int idFirmy, int idRoku, int? idKontrahenta, int page, string search, string asc, string desc, int pageSize = 10, DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?));
        PagedSearchedQueryResult<NieuregulowanaFakturaZakupu> NieuregulowaneFakturyZakupu(int idFirmy, int idRoku, int? idKontrahenta, int page, string search, string asc, string desc, int pageSize = 10, DateTime? stanNaDzien = default(DateTime?));
        PozycjaFakturyZakupu PozycjaFakturyZakupu(int id);
        void Usun(FakturaZakupu FakturaZakupu);
        void UsunPozycjeFaktury(PozycjaFakturyZakupu PozycjaFaktury);
        void UsunZaplate(ZaplataFakturyZakupu FakturaZakupu);
        ZaplataFakturyZakupu Zaplata(int id);
        PagedSearchedQueryResult<ZaplataFakturyZakupu> Zaplaty(int idFakturyZakupu, int page, string search, string asc, string desc, int pageSize = 10);
    }
}