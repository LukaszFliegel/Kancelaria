using Kancelaria.Dictionaries;
using Kancelaria.Globals;
using Kancelaria.Models;
using Kancelaria.Models.ViewModels;
using Kancelaria.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.Awesome.Mvc;

namespace Kancelaria.Controllers
{
    [Authorize(Roles = "Kancelaria")]
    [CompanyRequired]
    [YearRequired]
    public class FakturySprzedazyController : KancelariaLookupController
    {
        protected FakturySprzedazyRepository FakturySprzedazyRepository = new FakturySprzedazyRepository();

        public ActionResult Search(string search, int? page, int idKontrahenta)
        {
            //obtain the result somehow (an IEnumerable<Fruit>)
            //var result = JednostkiMiaryRepository.SposobyPlatnosci().Where(o => o.KodJednostkiMiary.ToLower().Contains(search.ToLower()));

            var result = FakturySprzedazyRepository.FakturySprzedazy(KancelariaSettings.IdFirmy(User.Identity.Name), KancelariaSettings.IdRoku(User.Identity.Name), idKontrahenta).Where(o => o.NumerFaktury.ToLower().Contains(search.ToLower()));

            var rows = this.RenderView(@"Awesome\LookupList", result.Skip((page.Value - 1) * KancelariaSettings.PageSize).Take(KancelariaSettings.PageSize));
            return Json(new { rows, more = result.Count() > page * KancelariaSettings.PageSize });
        }

        public ActionResult Get(int id)
        {
            string Kod = FakturySprzedazyRepository.FakturaSprzedazy(id).NumerFaktury;
            return Content(Kod);
        }

        public ActionResult GetJSON(int id)
        {
            var FakturaSprzedazy = FakturySprzedazyRepository.FakturaSprzedazy(id);
            //return Json(myData);
            var myData = new[] { new { Kod = FakturaSprzedazy.NumerFaktury, Kwota = FakturaSprzedazy.KwotaBrutto - FakturaSprzedazy.KwotaZaplacona } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Kartoteka(int? page, string search, string asc, string desc, DateTime? dateFrom, DateTime? dateTo)
        {
            var Model = FakturySprzedazyRepository.FakturySprzedazy(
                KancelariaSettings.IdFirmy(User.Identity.Name), 
                KancelariaSettings.IdRoku(User.Identity.Name), 
                page ?? 0, 
                search, 
                asc,
                desc,
                KancelariaSettings.PageSize,
                dateFrom,
                dateTo,
                true

            );

            return View(Grid(Model));
        }

        private GridSettings<FakturaSprzedazy> Grid(PagedSearchedQueryResult<FakturaSprzedazy> quertResult)
        {
            GridSettings<FakturaSprzedazy> GridSettings = new GridSettings<FakturaSprzedazy>(quertResult);

            return GridSettings;
        }

        public ActionResult Slownik(string controlName, string search)
        {
            FakturySprzedazyDictionary dictionary = new FakturySprzedazyDictionary();
            return PartialView("_AppliedFilters.cshtml", new AppliedFiltersModel(dictionary.GetAppliedFilterNames(search), controlName));
        }

        public ActionResult Drukuj(int id)
        {
            var Model = FakturySprzedazyRepository.FakturaSprzedazy(id);

            ViewBag.Imie = FakturySprzedazyRepository.Uzytkownik(User.Identity.Name).Imie;
            ViewBag.Nazwisko = FakturySprzedazyRepository.Uzytkownik(User.Identity.Name).Nazwisko;

            return View(Model);
        }

        public ActionResult Dodaj()
        {
            if ((new KontrahenciRepository()).Count(KancelariaSettings.IdFirmy(User.Identity.Name)) == 0)
            {
                TempData["Message"] = "Brak kontrahenta, na którego można by dodać fakturę";
                return RedirectToAction("Kartoteka");
            }

            if ((new InwestycjeRepository()).Count(KancelariaSettings.IdFirmy(User.Identity.Name)) == 0)
            {
                TempData["Message"] = "Brak inwestycji, na którą można by dodać fakturę";
                return RedirectToAction("Kartoteka");
            }

            if ((new KontaBankoweRepository()).Count(KancelariaSettings.IdFirmy(User.Identity.Name)) == 0)
            {
                TempData["Message"] = "Brak konta bankowego firmy, które można by podać na fakturze";
                return RedirectToAction("Kartoteka");
            }

            if ((new JednostkiMiaryRepository()).Count() == 0)
            {
                TempData["Message"] = "Brak jakiekolwiek jednostki miary, którą można by podać na fakturze";
                return RedirectToAction("Kartoteka");
            }

            if ((new SposobyPlatnosciRepository()).Count() == 0)
            {
                TempData["Message"] = "Brak sposobu płatności, który można by wskazać na fakturze";
                return RedirectToAction("Kartoteka");
            }

            return View(new FakturaSprzedazy()
            {
                DataFaktury = DateTime.Now,
                TerminPlatnosci = DateTime.Now.AddDays(KancelariaSettings.DefaultDayOfPaymentDaysAdded()),
                IdKontrahenta = (new KontrahenciRepository()).GetDefaultId(KancelariaSettings.IdFirmy(User.Identity.Name)),
                IdInwestycji = (new InwestycjeRepository()).GetDefaultId(KancelariaSettings.IdFirmy(User.Identity.Name)),
                IdKontaBankowegoFirmy = (new KontaBankoweRepository()).GetDefaultId(KancelariaSettings.IdFirmy(User.Identity.Name)),
                IdSposobuPlatnosci = (new SposobyPlatnosciRepository()).GetDefaultId(),
                CzyZaliczka = false
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new FakturaSprzedazy();

            try
            {
                Model.IdFirmy = KancelariaSettings.IdFirmy(User.Identity.Name);
                Model.IdRoku = KancelariaSettings.IdRoku(User.Identity.Name);
                UpdateModel(Model);

                if (collection["CheckCzyZaliczka"] != null && collection["CheckCzyZaliczka"].ToString() == "on")
                    Model.CzyZaliczka = true;
                else
                    Model.CzyZaliczka = false;

                if (collection["CheckCzyUmowa"] != null && collection["CheckCzyUmowa"].ToString() == "on")
                    Model.CzyUmowa = true;
                else
                    Model.CzyUmowa = false;

                if (Model.IsValid)
                {
                    FakturySprzedazyRepository.Dodaj(Model);
                    FakturySprzedazyRepository.Save();

                    TempData["Message"] = "Dodano fakturę sprzedaży";

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
            catch (Exception ex)
            {
                Logger.ErrorFormat("Dodawanie faktur sprzedaży\n{0}", ex);

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

            if ((new InwestycjeRepository()).Count(KancelariaSettings.IdFirmy(User.Identity.Name)) == 0)
            {
                TempData["Message"] = "Brak inwestycji, na którą można by dodać fakturę";
                return RedirectToAction("Kartoteka");
            }

            if ((new KontaBankoweRepository()).Count(KancelariaSettings.IdFirmy(User.Identity.Name)) == 0)
            {
                TempData["Message"] = "Brak konta bankowego firmy, które można by podać na fakturze";
                return RedirectToAction("Kartoteka");
            }

            if ((new JednostkiMiaryRepository()).Count() == 0)
            {
                TempData["Message"] = "Brak jakiekolwiek jednostki miary, którą można by podać na fakturze";
                return RedirectToAction("Kartoteka");
            }

            if ((new SposobyPlatnosciRepository()).Count() == 0)
            {
                TempData["Message"] = "Brak sposobu płatności, który można by wskazać na fakturze";
                return RedirectToAction("Kartoteka");
            }

            var model = FakturySprzedazyRepository.FakturaSprzedazy(id);

            if (model.IdFakturyKorygujacej.HasValue)
            {
                var fakturaKorygujaca = FakturySprzedazyRepository.FakturaSprzedazy(model.IdFakturyKorygujacej.Value);
                TempData["Message"] = String.Format("Ta faktura została skorygowana. Numer faktury korygującej: {0}", fakturaKorygujaca.NumerFaktury);
            }

            if (model == null)
            {
                return View("NotFound");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edytuj(int id, FormCollection collection)
        {
            var Model = FakturySprzedazyRepository.FakturaSprzedazy(id);

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

                if (collection["CheckCzyUmowa"] != null && collection["CheckCzyUmowa"].ToString() == "on")
                    Model.CzyUmowa = true;
                else
                    Model.CzyUmowa = false;

                if (Model.IsValid)
                {
                    FakturySprzedazyRepository.Save();

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
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji faktury sprzedaży";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji faktury sprzedaży\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = FakturySprzedazyRepository.FakturaSprzedazy(id);

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
            var Model = FakturySprzedazyRepository.FakturaSprzedazy(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                FakturySprzedazyRepository.Usun(Model);
                FakturySprzedazyRepository.Save();

                TempData["Message"] = String.Format("Usunięto fakturę sprzedaży \"{0}\"", Model.NumerFaktury);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania faktury sprzedaży";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania faktury sprzedaży\n{0}", ex);

                return View(Model);
            }
        }

        // pozycje faktury

        public PartialViewResult DodajPozycjeFakturySprzedazy(int id, string dialogElementId, string gridElementId)
        {
            var FakturaSprzedazy = FakturySprzedazyRepository.FakturaSprzedazy(id);

            return PartialView("_PozycjaFakturySprzedazy",
                new AjaxEditModel<PozycjaFakturySprzedazy>(
                    new PozycjaFakturySprzedazy()
                    {
                        IdFaktury = id,
                        StawkaVat = FakturaSprzedazy.Kontrahent.CzyVatowiec ? 0 : KancelariaSettings.DefaultVatValue(),
                        //IdInwestycji = (new InwestycjeRepository()).GetDefaultId()
                        IdJednostkiMiary = (new JednostkiMiaryRepository()).GetDefaultId(),
                        Ilosc = 1
                    },
                    false, "FakturySprzedazy", "DodajPozycjeFakturySprzedazy", System.Web.Mvc.Ajax.InsertionMode.Replace, dialogElementId, gridElementId
                )
            );
        }

        [HttpPost]
        public ActionResult DodajPozycjeFakturySprzedazy(int id, FormCollection collection)
        {
            PozycjaFakturySprzedazy PozycjaFakturySprzedazy = new PozycjaFakturySprzedazy()
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
                PozycjaFakturySprzedazy.CzyBrutto = true;
            }

            PozycjaFakturySprzedazy.CenaJednostkowa = Decimal.Parse(collection["CenaJednostkowa"]);
            PozycjaFakturySprzedazy.StawkaVat = Int32.Parse(collection["StawkaVat"]);
            PozycjaFakturySprzedazy.IdJednostkiMiary = Int32.Parse(collection["IdJednostkiMiary"]);
            PozycjaFakturySprzedazy.Ilosc = Int32.Parse(collection["Ilosc"]);
            PozycjaFakturySprzedazy.Opis = collection["Opis"];
            string DialogElementId = collection["DialogElementId"];
            string GridElementId = collection["GridElementId"];

            // nadanie kolejnego numeru pozycji
            FakturaSprzedazy Faktura = FakturySprzedazyRepository.FakturaSprzedazy(id);

            if (Faktura == null)
            {
                return Content("Nie powiodło się dodawane pozycji faktury"); // TODO: zwracac content?
            }

            if (Faktura.PozycjaFakturySprzedazies != null && Faktura.PozycjaFakturySprzedazies.Count > 0)
            {
                PozycjaFakturySprzedazy.NumerPozycji = (Faktura.PozycjaFakturySprzedazies.Max(p => p.NumerPozycji)) + 1;
            }
            else
            {
                PozycjaFakturySprzedazy.NumerPozycji = 1;
            }

            if (PozycjaFakturySprzedazy.IsValid)
            {
                // lista pozycji przed dodaniem tej nowej do bazy
                var Model = FakturySprzedazyRepository.FakturaSprzedazy(id);

                // dodanie nowej pozycji do bazy
                FakturySprzedazyRepository.DodajPozycjeFakturySprzedazy(PozycjaFakturySprzedazy);
                FakturySprzedazyRepository.Save();

                // dodanie nowej do juz wyciagnietej listy - takie sztuczki, bo zaciagniecie listy pozycji zaraz po DodajPozycjeFakturySprzedazy
                // nie zawsze zwracalo nowa pozycje - baza nie zdazyla przetworzyc INSERTa i zwracala liste bez nowej pozycji
                Model.PozycjaFakturySprzedazies.Add(PozycjaFakturySprzedazy);

                return PartialView("_GridPozycjeFakturySprzedazy", new ReadOnlyAbleModel<FakturaSprzedazy>(Model, false, DialogElementId, GridElementId));
            }

            return Content("Nie powiodło się dodawane pozycji faktury");
        }

        public PartialViewResult EdytujPozycjeFakturySprzedazy(int id, string dialogElementId, string gridElementId)
        {
            var Model = FakturySprzedazyRepository.PozycjaFakturySprzedazy(id);

            return PartialView("_PozycjaFakturySprzedazy",
                new AjaxEditModel<PozycjaFakturySprzedazy>(
                    Model,
                    false, "FakturySprzedazy", "EdytujPozycjeFakturySprzedazy", System.Web.Mvc.Ajax.InsertionMode.Replace, dialogElementId, gridElementId
                )
            );
        }

        [HttpPost]
        public ActionResult EdytujPozycjeFakturySprzedazy(int id, FormCollection collection)
        {
            var PozycjaFakturySprzedazy = FakturySprzedazyRepository.PozycjaFakturySprzedazy(id);

            if (PozycjaFakturySprzedazy == null)
            {
                return Content("Nie znaleziono pozycji");
            }

            try
            {
                UpdateModel(PozycjaFakturySprzedazy);

                if (collection["BruttoNettoRadio"] != null && collection["BruttoNettoRadio"].ToString() == "Brutto")
                {
                    //Decimal Brutto = Decimal.Parse(collection["CenaJednostkowa"]);
                    //// B = N + N*S/100 => N = B/(1 + S/100)
                    //Int32 StawkaVat = Int32.Parse(collection["StawkaVat"]);
                    //Decimal Netto = Brutto / (1 + new Decimal(StawkaVat) / 100);
                    //PozycjaFakturyZakupu.CenaJednostkowa = Netto;
                    PozycjaFakturySprzedazy.CzyBrutto = true;
                }

                string DialogElementId = collection["DialogElementId"];
                string GridElementId = collection["GridElementId"];

                if (PozycjaFakturySprzedazy.IsValid)
                {
                    FakturySprzedazyRepository.Save();

                    var Model = FakturySprzedazyRepository.FakturaSprzedazy(PozycjaFakturySprzedazy.IdFaktury);

                    return PartialView("_GridPozycjeFakturySprzedazy", new ReadOnlyAbleModel<FakturaSprzedazy>(Model, false, DialogElementId, GridElementId));
                }

                return Content("Nie powiodła się edycja pozycji faktury");
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji pozycji faktury sprzedaży\n{0}", ex);

                return Content("Nie powiodła się edycja pozycji faktury");
            }
        }

        public PartialViewResult UsunPozycjeFakturySprzedazy(int id, string dialogElementId, string gridElementId)
        {
            var Model = FakturySprzedazyRepository.PozycjaFakturySprzedazy(id);

            return PartialView("_UsunPozycjeFakturySprzedazy",
                new AjaxEditModel<PozycjaFakturySprzedazy>(
                    Model,
                    false, "FakturySprzedazy", "UsunPozycjeFakturySprzedazy", System.Web.Mvc.Ajax.InsertionMode.Replace, dialogElementId, gridElementId
                )
            );
        }

        [HttpPost]
        public ActionResult UsunPozycjeFakturySprzedazy(int id, FormCollection collection)
        {
            var PozycjaFakturySprzedazy = FakturySprzedazyRepository.PozycjaFakturySprzedazy(id);

            if (PozycjaFakturySprzedazy == null)
            {
                return Content("Nie znaleziono pozycji");
            }

            try
            {
                string DialogElementId = collection["DialogElementId"];
                string GridElementId = collection["GridElementId"];

                var Model = FakturySprzedazyRepository.FakturaSprzedazy(PozycjaFakturySprzedazy.IdFaktury);

                FakturySprzedazyRepository.UsunPozycjeFaktury(PozycjaFakturySprzedazy);
                FakturySprzedazyRepository.Save();

                return PartialView("_GridPozycjeFakturySprzedazy", new ReadOnlyAbleModel<FakturaSprzedazy>(Model, false, DialogElementId, GridElementId));
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania pozycji faktury sprzedaży\n{0}", ex);

                return Content("Nie powiodło się usuwanie pozycji faktury");
            }
        }

        // Zaplaty

        private GridSettings<ZaplataFakturySprzedazy> GridZaplaty(PagedSearchedQueryResult<ZaplataFakturySprzedazy> queryResult)
        {
            GridSettings<ZaplataFakturySprzedazy> GridSettings = new GridSettings<ZaplataFakturySprzedazy>(queryResult);

            return GridSettings;
        }

        public ActionResult Zaplaty(int idFakturySprzedazy, int? page, string search, string asc, string desc)
        {
            var Model = FakturySprzedazyRepository.Zaplaty(idFakturySprzedazy, page ?? 0, search, asc, desc);

            var FakturaSprzedazy = FakturySprzedazyRepository.FakturaSprzedazy(idFakturySprzedazy);

            return View(new ZaplatyFakturySprzedazyModel(FakturaSprzedazy, GridZaplaty(Model)));
        }

        public ActionResult DodajZaplate(int idFakturySprzedazy)
        {
            return View(new ZaplataFakturySprzedazy()
            {
                DataZaplaty = DateTime.Now,
                IdFakturySprzedazy = idFakturySprzedazy
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajZaplate(int idFakturySprzedazy, FormCollection collection)
        {
            var Model = new ZaplataFakturySprzedazy()
            {
                IdFakturySprzedazy = idFakturySprzedazy
            };

            var FakturaSprzedazy = FakturySprzedazyRepository.FakturaSprzedazy(idFakturySprzedazy);

            try
            {
                //UpdateModel(Model);

                Model.Kwota = Decimal.Parse(collection["Kwota"].Replace(".", ","), System.Globalization.CultureInfo.GetCultureInfo("de-DE"));
                Model.DataZaplaty = DateTime.Parse(collection["DataZaplaty"]);
                Model.Opis = collection["Opis"];

                if (Model.IsValid)
                {
                    FakturySprzedazyRepository.DodajZaplate(Model);
                    FakturySprzedazyRepository.Save();

                    TempData["Message"] = String.Format("Dodano zapłatę faktury sprzdażdy \"{0}\"", FakturaSprzedazy.NumerFaktury);

                    return RedirectToAction("Zaplaty", new { @idFakturySprzedazy = idFakturySprzedazy });
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
                Logger.ErrorFormat("Dodawanie faktur sprzedaży\n{0}", ex);

                ViewBag.ErrorMessage = String.Format("Nie powiodło się dodawnie faktury\n{0}", ex.Message);

                return View(Model);
            }
        }

        public ActionResult EdytujZaplate(int id)
        {
            var Model = FakturySprzedazyRepository.Zaplata(id);

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
            var Model = FakturySprzedazyRepository.Zaplata(id);

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
                    FakturySprzedazyRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano zapłatę z dnia {0} dla faktury \"{1}\"", Model.DataZaplaty.ToShortDateString(), Model.FakturaSprzedazy.NumerFaktury);

                    return RedirectToAction("Zaplaty", new { @idFakturySprzedazy = Model.FakturaSprzedazy.Id });
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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji faktury sprzedaży";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji faktury sprzedaży\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult UsunZaplate(int id)
        {
            var Model = FakturySprzedazyRepository.Zaplata(id);

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
            var Model = FakturySprzedazyRepository.Zaplata(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            int IdFakturySprzedazy = Model.IdFakturySprzedazy;
            string NumerFaktury = Model.FakturaSprzedazy.NumerFaktury;

            try
            {
                FakturySprzedazyRepository.UsunZaplate(Model);
                FakturySprzedazyRepository.Save();

                TempData["Message"] = String.Format("Usunięto zapłatę z dnia {0} dla faktury sprzedaży \"{1}\"", Model.DataZaplaty.ToShortDateString(), NumerFaktury);

                return RedirectToAction("Zaplaty", new { @idFakturySprzedazy = IdFakturySprzedazy });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania zapłaty faktury sprzedaży";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania zapłaty faktury sprzedaży\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Skoryguj(int id)
        {
            var fakturyKorygujaca = FakturySprzedazyRepository.FakturaSprzedazy(id);

            return View(fakturyKorygujaca);
        }

        [HttpPost]
        public ActionResult Skoryguj(int id, FormCollection collection)
        {
            var fakturaKorygowana = FakturySprzedazyRepository.FakturaSprzedazy(id);

            var idFakturyKorygujacej = FakturySprzedazyRepository.SkorygujFaktureSprzdazy(id);

            var fakturyKorygujaca = FakturySprzedazyRepository.FakturaSprzedazy(idFakturyKorygujacej);

            TempData["Message"] = String.Format("Skorygowano fakturę sprzedaży \"{0}\", oto faktura korygująca", fakturaKorygowana.NumerFaktury);

            //return View("Edytuj", fakturyKorygujaca);
            return RedirectToAction("Edytuj", new { @id = idFakturyKorygujacej });
        }

        public ActionResult DrukujKorekte(int id)
        {
            var Model = FakturySprzedazyRepository.FakturaSprzedazy(id);

            ViewBag.Imie = FakturySprzedazyRepository.Uzytkownik(User.Identity.Name).Imie;
            ViewBag.Nazwisko = FakturySprzedazyRepository.Uzytkownik(User.Identity.Name).Nazwisko;

            return View(Model);
        }
    }
}
