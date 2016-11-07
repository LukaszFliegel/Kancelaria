using Kancelaria.Globals;
using Kancelaria.Models;
using Kancelaria.Repositories;
using Kancelaria.Dictionaries;
using Omu.Awesome.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kancelaria.Models.ViewModels;
using Omu.Awesome.Mvc;
using System.Data.SqlClient;

namespace Kancelaria.Controllers
{
    [Authorize(Roles = "Kancelaria")]
    [CompanyRequired]
    [YearRequired]
    public class FakturyZakupuController : KancelariaLookupController
    {
        protected FakturyZakupuRepository FakturyZakupuRepository = new FakturyZakupuRepository();

        public ActionResult Search(string search, int? page, int idKontrahenta)
        {
            var result = FakturyZakupuRepository.FakturyZakupu(KancelariaSettings.IdFirmy(User.Identity.Name), KancelariaSettings.IdRoku(User.Identity.Name), idKontrahenta)
                .Where(
                    o => o.NumerFaktury.ToLower().Contains(search.ToLower())
                    || o.WlasnyNumerFaktury.ToLower().Contains(search.ToLower())
                    );

            var rows = this.RenderView(@"Awesome\LookupList", result.Skip((page.Value - 1) * KancelariaSettings.PageSize).Take(KancelariaSettings.PageSize));
            return Json(new { rows, more = result.Count() > page * KancelariaSettings.PageSize });
        }

        public ActionResult Get(int id)
        {
            string Kod = FakturyZakupuRepository.FakturaZakupu(id).NumerFaktury;
            return Content(Kod);
        }

        public ActionResult GetJSON(int id)
        {
            var FakturaZakupu = FakturyZakupuRepository.FakturaZakupu(id);
            //return Json(myData);
            var myData = new[] { new { Kod = FakturaZakupu.NumerFaktury, Kwota = FakturaZakupu.KwotaBrutto - FakturaZakupu.KwotaZaplacona } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Kartoteka(int? page, string search, string asc, string desc, DateTime? dateFrom, DateTime? dateTo)
        {
            var Model = FakturyZakupuRepository.FakturyZakupu(
                KancelariaSettings.IdFirmy(User.Identity.Name), 
                KancelariaSettings.IdRoku(User.Identity.Name), 
                page ?? 0, 
                search, 
                asc, 
                desc,
                KancelariaSettings.PageSize,
                dateFrom,
                dateTo
            );

            return View(Grid(Model));
        }

        private GridSettings<FakturaZakupu> Grid(PagedSearchedQueryResult<FakturaZakupu> queryResult)
        {
            GridSettings<FakturaZakupu> GridSettings = new GridSettings<FakturaZakupu>(queryResult);

            return GridSettings;
        }

        public ActionResult Slownik(string controlName, string search)
        {
            FakturyZakupuDictionary dictionary = new FakturyZakupuDictionary();
            return PartialView("_AppliedFilters.cshtml", new AppliedFiltersModel(dictionary.GetAppliedFilterNames(search), controlName));
        }

        public ActionResult Drukuj(int id)
        {
            var Model = FakturyZakupuRepository.FakturaZakupu(id);

            return View(Model);
        }

        public ActionResult Dodaj()
        {
            if ((new KontrahenciRepository()).Count(KancelariaSettings.IdFirmy(User.Identity.Name)) == 0)
            {
                TempData["Message"] = "Brak kontrahenta, na którego można by dodać fakturę";
                return RedirectToAction("Kartoteka");
            }

            if ((new SposobyPlatnosciRepository()).Count() == 0)
            {
                TempData["Message"] = "Brak sposobu płatności, który można by wskazać na fakturze";
                return RedirectToAction("Kartoteka");
            }

            return View(new FakturaZakupu() 
            { 
                DataFaktury = DateTime.Now, 
                TerminPlatnosci = DateTime.Now.AddDays(KancelariaSettings.DefaultDayOfPaymentDaysAdded()),
                IdKontrahenta = (new KontrahenciRepository()).GetDefaultId(KancelariaSettings.IdFirmy(User.Identity.Name)),
                IdSposobuPlatnosci = (new SposobyPlatnosciRepository()).GetDefaultId()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new FakturaZakupu();

            try
            {
                Model.IdFirmy = KancelariaSettings.IdFirmy(User.Identity.Name);
                Logger.DebugFormat("Model.IdFirmy = {0}", Model.IdFirmy);
                Model.IdRoku = KancelariaSettings.IdRoku(User.Identity.Name);
                Logger.DebugFormat("Model.IdRoku = {0}", Model.IdRoku);
                UpdateModel(Model);
                Logger.DebugFormat("After update model");

                if (collection["CheckCzyZaliczka"] != null && collection["CheckCzyZaliczka"].ToString() == "on")
                    Model.CzyZaliczka = true;
                else
                    Model.CzyZaliczka = false;

                // dodane dla debugu - usunac

                Logger.ErrorFormat("Model.CzyZaliczka = {0}", Model.CzyZaliczka);
                Logger.ErrorFormat("Model.DataFaktury = {0}", Model.DataFaktury);
                Logger.ErrorFormat("Model.DataZakupu = {0}", Model.DataZakupu);
                Logger.ErrorFormat("Model.IdFirmy = {0}", Model.IdFirmy);
                Logger.ErrorFormat("Model.IdKontrahenta = {0}", Model.IdKontrahenta);
                Logger.ErrorFormat("Model.IdRoku = {0}", Model.IdRoku);
                Logger.ErrorFormat("Model.IdSposobuPlatnosci = {0}", Model.IdSposobuPlatnosci);
                Logger.ErrorFormat("Model.KwotaBrutto = {0}", Model.KwotaBrutto);
                Logger.ErrorFormat("Model.KwotaNetto = {0}", Model.KwotaNetto);
                Logger.ErrorFormat("Model.KwotaNiezaplacona = {0}", Model.KwotaNiezaplacona);
                Logger.ErrorFormat("Model.KwotaVat = {0}", Model.KwotaVat);
                Logger.ErrorFormat("Model.KwotaZaplacona = {0}", Model.KwotaZaplacona);
                Logger.ErrorFormat("Model.NumerFaktury = {0}", Model.NumerFaktury);
                Logger.ErrorFormat("Model.WlasnyNumerFaktury = {0}", Model.WlasnyNumerFaktury);
                Logger.ErrorFormat("Model.Opis = {0}", Model.Opis);
                Logger.ErrorFormat("Model.TerminPlatnosci = {0}", Model.TerminPlatnosci);

                // koniec debugu

                Logger.DebugFormat("After Model.CzyZaliczka");

                if (Model.IsValid)
                {
                    FakturyZakupuRepository.Dodaj(Model);
                    Logger.DebugFormat("After FakturyZakupuRepository.Dodaj(Model);");
                    FakturyZakupuRepository.Save();
                    Logger.DebugFormat("After FakturyZakupuRepository.Save();");

                    TempData["Message"] = "Dodano fakturę zakupu";

                    return RedirectToAction("Edytuj", new { @id = Model.Id });
                }
                else
                {
                    foreach (var rule in Model.GetRuleViolations())
                    {
                        ModelState.AddModelError(rule.PropertyName, rule.ErrorMessage);
                    }

                    return View(Model);
                }
            }
            catch(Exception ex)
            {
                Logger.ErrorFormat("Dodawanie faktur zakupu\n{0}", ex);

                ViewBag.ErrorMessage = String.Format("Nie powiodło się dodawnie faktury\n{0}", ex.Message);

                return View(Model);
            }
        }

        public ActionResult Edytuj(int id)
        {
            if ((new KontrahenciRepository()).Count(KancelariaSettings.IdFirmy(User.Identity.Name)) == 0)
            {
                TempData["Message"] = "Brak kontrahenta, na którego można by dodać fakturę";
                return RedirectToAction("Kartoteka");
            }

            if ((new SposobyPlatnosciRepository()).Count() == 0)
            {
                TempData["Message"] = "Brak sposobu płatności, który można by wskazać na fakturze";
                return RedirectToAction("Kartoteka");
            }

            var Model = FakturyZakupuRepository.FakturaZakupu(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edytuj(int id, FormCollection collection)
        {
            var Model = FakturyZakupuRepository.FakturaZakupu(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                UpdateModel(Model);

                if (collection["CheckCzyZaliczka"] != null && collection["CheckCzyZaliczka"].ToString() == "on")
                    Model.CzyZaliczka = true;
                else
                    Model.CzyZaliczka = false;

                if (Model.IsValid)
                {
                    FakturyZakupuRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano fakturę \"{0}\"", Model.NumerFaktury);

                    return RedirectToAction("Kartoteka");
                }
                else
                {
                    foreach (var rule in Model.GetRuleViolations())
                    {
                        ModelState.AddModelError(rule.PropertyName, rule.ErrorMessage);
                    }

                    return View(Model);
                }
            }
            catch(SqlException ex)
            {
                if (ex.Number == 547) // Error Message 547: "Update statement conflicted with TABLE.."
                {
                    ViewBag.ErrorMessage = "Faktura skompensowana, nie można zmienić kontrahenta";
                    Logger.ErrorFormat("Faktura skompensowana, nie można zmienić kontrahenta\n{0}", ex);
                }
                else
                {
                    ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji faktury zakupu";
                    Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji faktury zakupu\n{0}", ex);
                }

                return View(Model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji faktury zakupu";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji faktury zakupu\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = FakturyZakupuRepository.FakturaZakupu(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usun(int id, FormCollection collection)
        {
            var Model = FakturyZakupuRepository.FakturaZakupu(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                FakturyZakupuRepository.Usun(Model);
                FakturyZakupuRepository.Save();

                TempData["Message"] = String.Format("Usunięto fakturę zakupu \"{0}\"", Model.NumerFaktury);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania faktury zakupu";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania faktury zakupu\n{0}", ex);

                return View(Model);
            }
        }

        // pozycje faktury

        public PartialViewResult DodajPozycjeFakturyZakupu(int id, string dialogElementId, string gridElementId)
        {
            var FakturaZakupu = FakturyZakupuRepository.FakturaZakupu(id);

            return PartialView("_PozycjaFakturyZakupu",
                new AjaxEditModel<PozycjaFakturyZakupu>(
                    new PozycjaFakturyZakupu()
                    {
                        IdFaktury = id,
                        StawkaVat = FakturaZakupu.Kontrahent.CzyVatowiec ? 0 : KancelariaSettings.DefaultVatValue(),
                        IdInwestycji = (new InwestycjeRepository()).GetDefaultId(KancelariaSettings.IdFirmy(User.Identity.Name)),
                        IdJednostkiMiary = (new JednostkiMiaryRepository()).GetDefaultId(),
                        Ilosc = 1
                    },
                    false, "FakturyZakupu", "DodajPozycjeFakturyZakupu", System.Web.Mvc.Ajax.InsertionMode.Replace, dialogElementId, gridElementId
                )
            );
        }

        [HttpPost]
        public ActionResult DodajPozycjeFakturyZakupu(int id, FormCollection collection)
        {
            PozycjaFakturyZakupu PozycjaFakturyZakupu = new PozycjaFakturyZakupu()
            {
                IdFaktury = id
            };

            if (collection["BruttoNettoRadio"] != null && collection["BruttoNettoRadio"].ToString() == "Brutto")
            {
                //Decimal Brutto = Decimal.Parse(collection["CenaJednostkowa"]);
                //// B = N + N*S/100 => N = B/(1 + S/100)
                //Int32 StawkaVat = Int32.Parse(collection["StawkaVat"]);
                //Decimal Netto = Brutto / (1 + new Decimal(StawkaVat) / 100);
                //PozycjaFakturyZakupu.CenaJednostkowa = Netto;
                PozycjaFakturyZakupu.CzyBrutto = true;
            }
            //else
            //{
            //    PozycjaFakturyZakupu.CenaJednostkowa = Decimal.Parse(collection["CenaJednostkowa"]);
            //}

            PozycjaFakturyZakupu.CenaJednostkowa = Decimal.Parse(collection["CenaJednostkowa"]);

            PozycjaFakturyZakupu.StawkaVat = Int32.Parse(collection["StawkaVat"]);
            PozycjaFakturyZakupu.IdInwestycji = Int32.Parse(collection["IdInwestycji"]);
            PozycjaFakturyZakupu.IdJednostkiMiary = Int32.Parse(collection["IdJednostkiMiary"]);
            PozycjaFakturyZakupu.Ilosc = Int32.Parse(collection["Ilosc"]);
            PozycjaFakturyZakupu.Opis = collection["Opis"];
            string DialogElementId = collection["DialogElementId"];
            string GridElementId = collection["GridElementId"];

            // nadanie kolejnego numeru pozycji
            FakturaZakupu Faktura = FakturyZakupuRepository.FakturaZakupu(id);

            if (Faktura == null)
            {
                return Content("Nie powiodło się dodawane pozycji faktury"); // TODO: zwracac content?
            }

            if (Faktura.PozycjaFakturyZakupus != null && Faktura.PozycjaFakturyZakupus.Count > 0)
            {
                PozycjaFakturyZakupu.NumerPozycji = (Faktura.PozycjaFakturyZakupus.Max(p => p.NumerPozycji)) + 1;
            }
            else
            {
                PozycjaFakturyZakupu.NumerPozycji = 1;
            }

            if (PozycjaFakturyZakupu.IsValid)
            {
                // lista pozycji przed dodaniem tej nowej do bazy
                var Model = FakturyZakupuRepository.FakturaZakupu(id);

                // dodanie nowej pozycji do bazy
                FakturyZakupuRepository.DodajPozycjeFakturyZakupu(PozycjaFakturyZakupu);
                FakturyZakupuRepository.Save();

                // dodanie nowej do juz wyciagnietej listy - takie sztuczki, bo zaciagniecie listy pozycji zaraz po DodajPozycjeFakturyZakupu
                // nie zawsze zwracalo nowa pozycje - baza nie zdazyla przetworzyc INSERTa i zwracala liste bez nowej pozycji
                Model.PozycjaFakturyZakupus.Add(PozycjaFakturyZakupu);

                return PartialView("_GridPozycjeFakturyZakupu", new ReadOnlyAbleModel<FakturaZakupu>(Model, false, DialogElementId, GridElementId));
            }

            return Content("Nie powiodło się dodawane pozycji faktury");
        }

        public PartialViewResult EdytujPozycjeFakturyZakupu(int id, string dialogElementId, string gridElementId)
        {
            var Model = FakturyZakupuRepository.PozycjaFakturyZakupu(id);

            return PartialView("_PozycjaFakturyZakupu",
                new AjaxEditModel<PozycjaFakturyZakupu>(
                    Model,
                    false, "FakturyZakupu", "EdytujPozycjeFakturyZakupu", System.Web.Mvc.Ajax.InsertionMode.Replace, dialogElementId, gridElementId
                )
            );
        }

        [HttpPost]
        public ActionResult EdytujPozycjeFakturyZakupu(int id, FormCollection collection)
        {
            var PozycjaFakturyZakupu = FakturyZakupuRepository.PozycjaFakturyZakupu(id);

            if (PozycjaFakturyZakupu == null)
            {
                return Content("Nie znaleziono pozycji");
            }

            try
            {
                UpdateModel(PozycjaFakturyZakupu);

                if (collection["BruttoNettoRadio"] != null && collection["BruttoNettoRadio"].ToString() == "Brutto")
                {
                    //Decimal Brutto = Decimal.Parse(collection["CenaJednostkowa"]);
                    //// B = N + N*S/100 => N = B/(1 + S/100)
                    //Int32 StawkaVat = Int32.Parse(collection["StawkaVat"]);
                    //Decimal Netto = Brutto / (1 + new Decimal(StawkaVat) / 100);
                    //PozycjaFakturyZakupu.CenaJednostkowa = Netto;
                    PozycjaFakturyZakupu.CzyBrutto = true;
                }
                //else
                //{
                //    PozycjaFakturyZakupu.CenaJednostkowa = Decimal.Parse(collection["CenaJednostkowa"]);
                //}

                string DialogElementId = collection["DialogElementId"];
                string GridElementId = collection["GridElementId"];

                if (PozycjaFakturyZakupu.IsValid)
                {
                    FakturyZakupuRepository.Save();

                    var Model = FakturyZakupuRepository.FakturaZakupu(PozycjaFakturyZakupu.IdFaktury);

                    return PartialView("_GridPozycjeFakturyZakupu", new ReadOnlyAbleModel<FakturaZakupu>(Model, false, DialogElementId, GridElementId));
                }

                return Content("Nie powiodła się edycja pozycji faktury");
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji pozycji faktury zakupu\n{0}", ex);

                return Content("Nie powiodła się edycja pozycji faktury");
            }
        }

        public PartialViewResult UsunPozycjeFakturyZakupu(int id, string dialogElementId, string gridElementId)
        {
            var Model = FakturyZakupuRepository.PozycjaFakturyZakupu(id);

            return PartialView("_UsunPozycjeFakturyZakupu",
                new AjaxEditModel<PozycjaFakturyZakupu>(
                    Model,
                    false, "FakturyZakupu", "UsunPozycjeFakturyZakupu", System.Web.Mvc.Ajax.InsertionMode.Replace, dialogElementId, gridElementId
                )
            );
        }

        [HttpPost]
        public ActionResult UsunPozycjeFakturyZakupu(int id, FormCollection collection)
        {
            var PozycjaFaktury = FakturyZakupuRepository.PozycjaFakturyZakupu(id);

            if (PozycjaFaktury == null)
            {
                return Content("Nie znaleziono pozycji");
            }

            try
            {
                string DialogElementId = collection["DialogElementId"];
                string GridElementId = collection["GridElementId"];

                var Model = FakturyZakupuRepository.FakturaZakupu(PozycjaFaktury.IdFaktury);

                FakturyZakupuRepository.UsunPozycjeFaktury(PozycjaFaktury);
                FakturyZakupuRepository.Save();

                return PartialView("_GridPozycjeFakturyZakupu", new ReadOnlyAbleModel<FakturaZakupu>(Model, false, DialogElementId, GridElementId));
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania pozycji faktury zakupu\n{0}", ex);

                return Content("Nie powiodło się usuwanie pozycji faktury");
            }
        }

        // Zaplaty

        private GridSettings<ZaplataFakturyZakupu> GridZaplaty(PagedSearchedQueryResult<ZaplataFakturyZakupu> queryResult)
        {
            GridSettings<ZaplataFakturyZakupu> GridSettings = new GridSettings<ZaplataFakturyZakupu>(queryResult);

            return GridSettings;
        }

        public ActionResult Zaplaty(int idFakturyZakupu, int? page, string search, string asc, string desc)
        {
            var Model = FakturyZakupuRepository.Zaplaty(idFakturyZakupu, page ?? 0, search, asc, desc);

            var FakturaZakupu = FakturyZakupuRepository.FakturaZakupu(idFakturyZakupu);

            return View(new ZaplatyFakturyZakupuModel(FakturaZakupu, GridZaplaty(Model)));
        }

        public ActionResult DodajZaplate(int idFakturyZakupu)
        {
            return View(new ZaplataFakturyZakupu()
            {
                DataZaplaty = DateTime.Now,
                IdFakturyZakupu = idFakturyZakupu
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajZaplate(int idFakturyZakupu, FormCollection collection)
        {
            var Model = new ZaplataFakturyZakupu()
            {
                IdFakturyZakupu = idFakturyZakupu
            };

            var FakturaZakupu = FakturyZakupuRepository.FakturaZakupu(idFakturyZakupu);

            try
            {
                //UpdateModel(Model);

                Model.Kwota = Decimal.Parse(collection["Kwota"].Replace(".", ","), System.Globalization.CultureInfo.GetCultureInfo("de-DE"));
                Model.DataZaplaty = DateTime.Parse(collection["DataZaplaty"]);
                Model.Opis = collection["Opis"];

                if (Model.IsValid)
                {
                    FakturyZakupuRepository.DodajZaplate(Model);
                    FakturyZakupuRepository.Save();

                    TempData["Message"] = String.Format("Dodano zapłatę faktury zakupu \"{0}\"", FakturaZakupu.NumerFaktury);

                    return RedirectToAction("Zaplaty", new { @idFakturyZakupu = idFakturyZakupu });
                }
                else
                {
                    foreach (var rule in Model.GetRuleViolations())
                    {
                        ModelState.AddModelError(rule.PropertyName, rule.ErrorMessage);
                    }

                    return View(Model);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Dodawanie faktur zakupu\n{0}", ex);

                ViewBag.ErrorMessage = String.Format("Nie powiodło się dodawnie faktury\n{0}", ex.Message);

                return View(Model);
            }
        }

        public ActionResult EdytujZaplate(int id)
        {
            var Model = FakturyZakupuRepository.Zaplata(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EdytujZaplate(int id, FormCollection collection)
        {
            var Model = FakturyZakupuRepository.Zaplata(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                Model.Kwota = Decimal.Parse(collection["Kwota"].Replace(".", ","), System.Globalization.CultureInfo.GetCultureInfo("de-DE"));
                Model.DataZaplaty = DateTime.Parse(collection["DataZaplaty"]);
                Model.Opis = collection["Opis"];

                if (Model.IsValid)
                {
                    FakturyZakupuRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano zapłatę z dnia {0} dla faktury \"{1}\"", Model.DataZaplaty.ToShortDateString(), Model.FakturaZakupu.NumerFaktury);

                    return RedirectToAction("Zaplaty", new { @idFakturyZakupu = Model.FakturaZakupu.Id });
                }
                else
                {
                    foreach (var rule in Model.GetRuleViolations())
                    {
                        ModelState.AddModelError(rule.PropertyName, rule.ErrorMessage);
                    }

                    return View(Model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji faktury zakupu";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji faktury zakupu\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult UsunZaplate(int id)
        {
            var Model = FakturyZakupuRepository.Zaplata(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsunZaplate(int id, FormCollection collection)
        {
            var Model = FakturyZakupuRepository.Zaplata(id);            

            if (Model == null)
            {
                return View("NotFound");
            }

            int IdFakturyZakupu = Model.IdFakturyZakupu;
            string NumerFaktury = Model.FakturaZakupu.NumerFaktury;

            try
            {
                FakturyZakupuRepository.UsunZaplate(Model);
                FakturyZakupuRepository.Save();

                TempData["Message"] = String.Format("Usunięto zapłatę z dnia {0} dla faktury zakupu \"{1}\"", Model.DataZaplaty.ToShortDateString(), NumerFaktury);

                return RedirectToAction("Zaplaty", new { @idFakturyZakupu = IdFakturyZakupu });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania zapłaty faktury zakupu";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania zapłaty faktury zakupu\n{0}", ex);

                return View(Model);
            }
        }
    }
}
