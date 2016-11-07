using Kancelaria.Globals;
using Kancelaria.Models;
using Kancelaria.Models.ViewModels;
using Kancelaria.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kancelaria.Controllers
{
    [Authorize(Roles = "Kancelaria")]
    public class RaportyController : KancelariaController
    {
        FakturySprzedazyRepository FakturySprzedazyRepository = new FakturySprzedazyRepository();

        FakturyZakupuRepository FakturyZakupuRepository = new FakturyZakupuRepository();

        InwestycjeRepository InwestycjeRepository = new InwestycjeRepository();

        KontrahenciRepository KontrahenciRepository = new KontrahenciRepository();

        RaportyRepository RaportyRepository = new RaportyRepository();

        public ActionResult Index()
        {
            return View();
        }

        [CompanyRequired]
        [YearRequired]
        public ActionResult RejestrSprzedazy(string search, string asc, string desc, int? IdKontrahenta, int? IdInwestycji, DateTime? dateFrom, DateTime? dateTo)
        {
            string Subtitle = String.Format("Rok obrotowy {0}",
                (new LataObrotoweRepository()).RokObrotowy(KancelariaSettings.IdRoku(User.Identity.Name)).NazwaRoku
            );

            if (dateFrom.HasValue)
            {
                Subtitle = String.Format("{0} od {1}",
                    Subtitle,
                    dateFrom.Value.ToShortDateString()
                );
            }

            if (dateTo.HasValue)
            {
                Subtitle = String.Format("{0} do {1}",
                    Subtitle,
                    dateTo.Value.ToShortDateString()
                );
            }

            var Model = new ReportModel<PagedSearchedQueryResult<FakturaSprzedazy>>(
                FakturySprzedazyRepository.FakturySprzedazy(
                    KancelariaSettings.IdFirmy(User.Identity.Name), 
                    KancelariaSettings.IdRoku(User.Identity.Name), 
                    IdKontrahenta, 
                    IdInwestycji, 
                    0, 
                    search, 
                    asc, 
                    desc, 
                    0,
                    dateFrom,
                    dateTo
                ),
                "Rejestr sprzedaży",
                Subtitle
            );

            if(IdInwestycji.HasValue)
                Model.AddSubTitle(String.Format("Dostarczony materiał na inwestycję \"{0}\"", InwestycjeRepository.Inwestycja(IdInwestycji.Value).NumerInwestycji.ToString()));

            if (IdKontrahenta.HasValue)
                Model.AddSubTitle(String.Format("Faktury na kontrahenta \"{0}\"", KontrahenciRepository.Kontrahent(IdKontrahenta.Value).KodKontrahenta.ToString()));

            return View(Model);            
        }

        [CompanyRequired]
        [YearRequired]
        public ActionResult RejestrZakupu(string search, string asc, string desc, int? IdKontrahenta, DateTime? dateFrom, DateTime? dateTo)
        {
            //ViewBag.NazwaRoku = (new LataObrotoweRepository()).RokObrotowy(KancelariaSettings.IdRoku(User.Identity.Name)).NazwaRoku;

            string Subtitle = String.Format("Rok obrotowy {0}",
                (new LataObrotoweRepository()).RokObrotowy(KancelariaSettings.IdRoku(User.Identity.Name)).NazwaRoku
            );

            if (dateFrom.HasValue)
            {
                Subtitle = String.Format("{0} od {1}",
                    Subtitle,
                    dateFrom.Value.ToShortDateString()
                );
            }

            if (dateTo.HasValue)
            {
                Subtitle = String.Format("{0} do {1}",
                    Subtitle,
                    dateTo.Value.ToShortDateString()
                );
            }

            var Model = new ReportModel<PagedSearchedQueryResult<FakturaZakupu>>(
                FakturyZakupuRepository.FakturyZakupu(
                    KancelariaSettings.IdFirmy(User.Identity.Name), 
                    KancelariaSettings.IdRoku(User.Identity.Name), 
                    IdKontrahenta, 
                    0, 
                    search, 
                    asc, 
                    desc, 
                    0,
                    dateFrom,
                    dateTo
                ),
                "Rejestr zakupu",
                Subtitle
            );

            if (IdKontrahenta.HasValue)
                Model.AddSubTitle(String.Format("Faktury na kontrahenta \"{0}\"", KontrahenciRepository.Kontrahent(IdKontrahenta.Value).KodKontrahenta.ToString()));

            return View(Model);
        }

        [CompanyRequired]
        [YearRequired]
        public ActionResult NieuregulowaneFakturySprzedazy(string search, string asc, string desc, int? IdKontrahenta, int? IdInwestycji, DateTime? stanNaDzien = null)
        {
            var Model = new ReportModel<PagedSearchedQueryResult<NieuregulowanaFakturaSprzedazy>>(
                FakturySprzedazyRepository.NieuregulowaneFakturySprzedazy(KancelariaSettings.IdFirmy(User.Identity.Name), KancelariaSettings.IdRoku(User.Identity.Name), IdKontrahenta, IdInwestycji, 0, search, asc, desc, 0, false, stanNaDzien),
                "Nieuregulowane faktury sprzedaży",
                String.Format("Rok obrotowy {0}", (new LataObrotoweRepository()).RokObrotowy(KancelariaSettings.IdRoku(User.Identity.Name)).NazwaRoku)
            );

            if (IdInwestycji.HasValue)
                Model.AddSubTitle(String.Format("Dostarczony materiał na inwestycję \"{0}\"", InwestycjeRepository.Inwestycja(IdInwestycji.Value).NumerInwestycji.ToString()));

            if (IdKontrahenta.HasValue)
                Model.AddSubTitle(String.Format("Faktury na kontrahenta \"{0}\"", KontrahenciRepository.Kontrahent(IdKontrahenta.Value).KodKontrahenta.ToString()));

            return View(Model);
        }

        [CompanyRequired]
        [YearRequired]
        public ActionResult NieuregulowaneFakturyZakupu(string search, string asc, string desc, int? IdKontrahenta, DateTime? stanNaDzien = null)
        {
            var Model = new ReportModel<PagedSearchedQueryResult<NieuregulowanaFakturaZakupu>>(
                FakturyZakupuRepository.NieuregulowaneFakturyZakupu(KancelariaSettings.IdFirmy(User.Identity.Name), KancelariaSettings.IdRoku(User.Identity.Name), IdKontrahenta, 0, search, asc, desc, 0, stanNaDzien),
                "Nieuregulowane faktury zakupu",
                String.Format("Rok obrotowy {0}", (new LataObrotoweRepository()).RokObrotowy(KancelariaSettings.IdRoku(User.Identity.Name)).NazwaRoku)
            );

            if (IdKontrahenta.HasValue)
                Model.AddSubTitle(String.Format("Faktury na kontrahenta \"{0}\"", KontrahenciRepository.Kontrahent(IdKontrahenta.Value).KodKontrahenta.ToString()));

            return View(Model);
        }

        [CompanyRequired]
        [YearRequired]
        public ActionResult RejestrKontrahentow(string search, string asc, string desc)
        {
            var Model = new ReportModel<PagedSearchedQueryResult<Kontrahent>>(
                KontrahenciRepository.Kontrahenci(KancelariaSettings.IdFirmy(User.Identity.Name), 0, search, asc, desc, 0),
                "Rejestr kontrahentów",
                String.Format("Rok obrotowy {0}", (new LataObrotoweRepository()).RokObrotowy(KancelariaSettings.IdRoku(User.Identity.Name)).NazwaRoku)
            );

            return View(Model);
        }

        [CompanyRequired]
        [YearRequired]
        public ActionResult AnalizaRozrachunkow(string search, string asc, string desc)
        {
            //var Model = new ReportModel<PagedSearchedQueryResult<Kontrahent>>(
            //    KontrahenciRepository.Kontrahenci(KancelariaSettings.IdFirmy(User.Identity.Name), 0, search, asc, desc, 0),
            //    "Analiza rozrachunków",
            //    String.Format("Rok obrotowy {0}", (new LataObrotoweRepository()).RokObrotowy(KancelariaSettings.IdRoku(User.Identity.Name)).NazwaRoku)
            //);

            var Model = new ReportModel<IQueryable<AnalizaRozrachunku>>(
                RaportyRepository.AnalizaRazrachunkow(KancelariaSettings.IdFirmy(User.Identity.Name), KancelariaSettings.IdRoku(User.Identity.Name)),
                "Analiza rozrachunków",
                String.Format("Rok obrotowy {0}", (new LataObrotoweRepository()).RokObrotowy(KancelariaSettings.IdRoku(User.Identity.Name)).NazwaRoku)
            );

            return View(Model);
        }

        [CompanyRequired]
        //[YearRequired]
        public ActionResult AnalizaKosztow(string search, string asc, string desc, int? IdKontrahenta, int? IdTypuInwestycji, DateTime? dateFrom, DateTime? dateTo)
        {
            //string Subtitle = String.Format("Rok obrotowy {0}",
            //    (new LataObrotoweRepository()).RokObrotowy(KancelariaSettings.IdRoku(User.Identity.Name)).NazwaRoku
            //);

            string Subtitle = string.Empty;

            if (dateFrom.HasValue)
            {
                Subtitle = String.Format("{0} od {1}",
                    string.IsNullOrEmpty(Subtitle) ? "Raport" : Subtitle,
                    dateFrom.Value.ToShortDateString()
                );
            }

            if (dateTo.HasValue)
            {
                Subtitle = String.Format("{0} do {1}",
                    string.IsNullOrEmpty(Subtitle) ? "Raport" : Subtitle,
                    dateTo.Value.ToShortDateString()
                );
            }

            var Model = new ReportModel<IQueryable<KosztNaInwestycjach>>(
                RaportyRepository.AnalizaKosztowNaInwestycjach(
                    KancelariaSettings.IdFirmy(User.Identity.Name),
                    //KancelariaSettings.IdRoku(User.Identity.Name), 
                    IdKontrahenta, 
                    IdTypuInwestycji,
                    0, 
                    search, 
                    asc, 
                    desc, 
                    0, 
                    dateFrom, 
                    dateTo
                ),
                "Analiza kosztów na inwestycjach",
                Subtitle
            );

            return View(Model);
        }
    }
}
