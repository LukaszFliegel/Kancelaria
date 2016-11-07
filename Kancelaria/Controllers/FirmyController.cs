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
    public class FirmyController : KancelariaController
    {
        protected FirmyRepository FirmyRepository = new FirmyRepository();

        public ActionResult Kartoteka(int? page, string search, string asc, string desc)
        {
            if (KancelariaSettings.IsOneFirm())
            {
                return RedirectToAction("Edytuj", new { id = KancelariaSettings.IdFirmy(User.Identity.Name) });
            }

            var Model = FirmyRepository.Firmy(page ?? 0, search, asc, desc);

            return View(Grid(Model));
        }

        private GridSettings<Firma> Grid(PagedSearchedQueryResult<Firma> quertResult)
        {
            GridSettings<Firma> GridSettings = new GridSettings<Firma>(quertResult);

            return GridSettings;
        }

        public ActionResult Dodaj()
        {
            var Model = new Firma();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new Firma();
            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    FirmyRepository.Dodaj(Model);
                    FirmyRepository.Save();

                    TempData["Message"] = String.Format("Dodano firmę");

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania firmy";
                Logger.ErrorFormat("Wystąpił błąd podczas dodawania firmy\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Edytuj(int id)
        {
            var Model = FirmyRepository.Firma(id);

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
            var Model = FirmyRepository.Firma(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    FirmyRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano firmę");

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji firmy";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji firmy\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = FirmyRepository.Firma(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.FakturaZakupus.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć firmy, na którą wprowadzone są faktury zakupu");
                return RedirectToAction("Kartoteka");
            }

            if (Model.FakturaSprzedazies.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć firmy, na którą wprowadzone są faktury sprzedaży");
                return RedirectToAction("Kartoteka");
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usun(int id, FormCollection collection)
        {
            var Model = FirmyRepository.Firma(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.FakturaZakupus.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć firmy, na którą wprowadzone są faktury zakupu");
                return RedirectToAction("Kartoteka");
            }

            if (Model.FakturaSprzedazies.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć firmy, na którą wprowadzone są faktury sprzedaży");
                return RedirectToAction("Kartoteka");
            }

            try
            {
                FirmyRepository.Usun(Model);
                FirmyRepository.Save();

                TempData["Message"] = String.Format("Usunięto firmę \"{0}\"", Model.NazwaSkrocona);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania firmy";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania firmy\n{0}", ex);

                return View(Model);
            }
        }

        // sluzy do wyboru firmy w kontekscie ktorej pracuje user (zapisuje ja cache'u)
        public ActionResult Wybor(string returnUrl, int? page)
        {
            var Model = FirmyRepository.Firmy(page ?? 0);

            return View(new WyborFirmyModel(new GridSettings<Firma>(Model), returnUrl));
        }

        [HttpPost]
        public ActionResult Wybor(int id, string returnUrl)
        {
            //System.Web.HttpContext.Current.Cache.Insert("CompanyId", id);

            FirmyRepository.WybierzIdFirmy(id, User.Identity.Name);
            FirmyRepository.Save();

            //var Model = FirmyRepository.Firma(id);          

            //ViewBag.Message = string.Format("Wybrano firmę {0}", Model.NazwaSkrocona);
            //TempData["Message"] = string.Format("Wybrano firmę {0}", Model.NazwaSkrocona);

            //System.Web.HttpContext.Current.Cache.Insert("CompanyName", Model.NazwaSkrocona);
            ////System.Web.HttpContext.Current.Cache.Remove("YearId");

            if (String.IsNullOrEmpty(returnUrl))
            {
                return RedirectToRoute("Menu");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }
    }
}
