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

namespace Kancelaria.Controllers
{
    [Authorize(Roles = "Kancelaria")]
    [CompanyRequired]
    [YearRequired]
    public class KompensatyController : KancelariaController
    {
        protected KompensatyRepository KompensatyRepository = new KompensatyRepository();

        public ActionResult Kartoteka(int? page)
        {
            var Model = KompensatyRepository.Kompensaty(KancelariaSettings.IdFirmy(User.Identity.Name), KancelariaSettings.IdRoku(User.Identity.Name), page ?? 0);

            return View(Grid(Model));
        }

        private GridSettings<Kompensata> Grid(PagedSearchedQueryResult<Kompensata> quertResult)
        {
            GridSettings<Kompensata> GridSettings = new GridSettings<Kompensata>(quertResult);

            return GridSettings;
        }

        public ActionResult Slownik(string controlName, string search)
        {
            KompensatyDictionary dictionary = new KompensatyDictionary();
            return PartialView("_AppliedFilters.cshtml", new AppliedFiltersModel(dictionary.GetAppliedFilterNames(search), controlName));
        }

        public ActionResult Drukuj(int id)
        {
            var Model = KompensatyRepository.Kompensata(id);

            return View(Model);
        }

        public ActionResult Dodaj()
        {
            if ((new KontrahenciRepository()).Count(KancelariaSettings.IdFirmy(User.Identity.Name)) == 0)
            {
                TempData["Message"] = "Brak kontrahenta, któremu można by wystawić kompensatę";
                return RedirectToAction("Kartoteka");
            }

            return View(new Kompensata()
            {
                DataKompensaty = DateTime.Now,
                IdKontrahenta = (new KontrahenciRepository()).GetDefaultId(KancelariaSettings.IdFirmy(User.Identity.Name)),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new Kompensata();
            try
            {
                Model.IdFirmy = KancelariaSettings.IdFirmy(User.Identity.Name);
                Model.IdRoku = KancelariaSettings.IdRoku(User.Identity.Name);

                UpdateModel(Model);

                if (Model.IsValid)
                {
                    KompensatyRepository.Dodaj(Model);
                    KompensatyRepository.Save();

                    TempData["Message"] = String.Format("Dodano kompensatę \"{0}\"", Model.NumerKompensaty);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania kompensaty";
                Logger.ErrorFormat("Wystąpił błąd podczas dodawania kompensaty\n{0}", ex);

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

            var Model = KompensatyRepository.Kompensata(id);

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
            var Model = KompensatyRepository.Kompensata(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    KompensatyRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano kompensatę \"{0}\"", Model.NumerKompensaty);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji kompensaty";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji kompensaty\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = KompensatyRepository.Kompensata(id);

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
            var Model = KompensatyRepository.Kompensata(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                KompensatyRepository.Usun(Model);
                KompensatyRepository.Save();

                TempData["Message"] = String.Format("Usunięto kompensatę \"{0}\"", Model.NumerKompensaty);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania kompensaty";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania kompensaty\n{0}", ex);

                return View(Model);
            }
        }

        // Pozycje kompensaty

        public PartialViewResult DodajPozycjeKompensaty(int id, string dialogElementId, string gridElementId)
        {
            var Model = KompensatyRepository.Kompensata(id);

            return PartialView("_PozycjaKompensaty",
                new AjaxContractorEditModel<KompensataPowiazanie>(
                    new KompensataPowiazanie()
                    {
                        IdKompensaty = Model.Id,
                        IdKontrahenta = Model.IdKontrahenta
                    },
                    false, "Kompensaty", "DodajPozycjeKompensaty", System.Web.Mvc.Ajax.InsertionMode.Replace, dialogElementId, gridElementId, Model.IdKontrahenta
                )
            );
        }

        [HttpPost]
        public ActionResult DodajPozycjeKompensaty(int id, FormCollection collection)
        {
            var Model = KompensatyRepository.Kompensata(id);

            KompensataPowiazanie KompensataPowiazanie = new KompensataPowiazanie()
            {
                IdKompensaty = Model.Id,
                IdKontrahenta = Model.IdKontrahenta
            };

            string DialogElementId = collection["DialogElementId"];
            string GridElementId = collection["GridElementId"];

            //Decimal debug = Decimal.Parse("214,15");
            //debug = Decimal.Parse("214.15", System.Globalization.CultureInfo.GetCultureInfo("de-DE"));

            //debug = Decimal.Parse(collection["Kwota"]);

            KompensataPowiazanie.Kwota = Decimal.Parse(collection["Kwota"].Replace(".", ","), System.Globalization.CultureInfo.GetCultureInfo("de-DE"));
            KompensataPowiazanie.IdFakturySprzedazy = Int32.Parse(collection["IdFakturySprzedazy"]);
            KompensataPowiazanie.IdFakturyZakupu = Int32.Parse(collection["IdFakturyZakupu"]);

            //UpdateModel(KompensataPowiazanie);

            if (KompensataPowiazanie.IsValid)
            {
                // dodanie nowej pozycji do bazy
                KompensatyRepository.DodajPozycjeKompensaty(KompensataPowiazanie);
                KompensatyRepository.Save();

                // dodanie nowej do juz wyciagnietej listy - takie sztuczki, bo zaciagniecie listy pozycji zaraz po DodajPozycjeFakturyZakupu
                // nie zawsze zwracalo nowa pozycje - baza nie zdazyla przetworzyc INSERTa i zwracala liste bez nowej pozycji
                Model.KompensataPowiazanies.Add(KompensataPowiazanie);

                return PartialView("_GridPozycjeKompensaty", new ReadOnlyAbleModel<Kompensata>(Model, false, DialogElementId, GridElementId));
            }

            return Content("Nie powiodło się dodawane pozycji kompensaty");
        }

        public PartialViewResult EdytujPozycjeKompensaty(int id, string dialogElementId, string gridElementId)
        {
            var Model = KompensatyRepository.KompensataPowiazanie(id);

            return PartialView("_PozycjaKompensaty",
                new AjaxContractorEditModel<KompensataPowiazanie>(
                    Model,
                    false, "Kompensaty", "EdytujPozycjeKompensaty", System.Web.Mvc.Ajax.InsertionMode.Replace, dialogElementId, gridElementId, Model.IdKontrahenta
                )
            );
        }

        [HttpPost]
        public ActionResult EdytujPozycjeKompensaty(int id, FormCollection collection)
        {
            var KompensataPowiazanie = KompensatyRepository.KompensataPowiazanie(id);

            if (KompensataPowiazanie == null)
            {
                return Content("Nie znaleziono pozycji");
            }

            try
            {
                UpdateModel(KompensataPowiazanie);

                string DialogElementId = collection["DialogElementId"];
                string GridElementId = collection["GridElementId"];

                if (KompensataPowiazanie.IsValid)
                {
                    KompensatyRepository.Save();

                    var Model = KompensatyRepository.Kompensata(KompensataPowiazanie.IdKompensaty);

                    return PartialView("_GridPozycjeKompensaty", new ReadOnlyAbleModel<Kompensata>(Model, false, DialogElementId, GridElementId));
                }

                return Content("Nie powiodła się edycja pozycji kompensaty");
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji pozycji kompensaty\n{0}", ex);

                return Content("Nie powiodła się edycja pozycji kompensaty");
            }
        }

        public PartialViewResult UsunPozycjeKompensaty(int id, string dialogElementId, string gridElementId)
        {
            var Model = KompensatyRepository.KompensataPowiazanie(id);

            return PartialView("_UsunPozycjeKompensaty",
                new AjaxEditModel<KompensataPowiazanie>(
                    Model,
                    false, "Kompensaty", "UsunPozycjeKompensaty", System.Web.Mvc.Ajax.InsertionMode.Replace, dialogElementId, gridElementId
                )
            );
        }

        [HttpPost]
        public ActionResult UsunPozycjeKompensaty(int id, FormCollection collection)
        {
            var KompensataPowiazanie = KompensatyRepository.KompensataPowiazanie(id);

            if (KompensataPowiazanie == null)
            {
                return Content("Nie znaleziono pozycji");
            }

            try
            {
                string DialogElementId = collection["DialogElementId"];
                string GridElementId = collection["GridElementId"];

                var Model = KompensatyRepository.Kompensata(KompensataPowiazanie.IdKompensaty);

                KompensatyRepository.UsunPowiazanieKompensaty(KompensataPowiazanie);
                KompensatyRepository.Save();

                return PartialView("_GridPozycjeKompensaty", new ReadOnlyAbleModel<Kompensata>(Model, false, DialogElementId, GridElementId));
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania pozycji kompensaty\n{0}", ex);

                return Content("Nie powiodło się usuwanie pozycji kompensaty");
            }
        }

    }
}
