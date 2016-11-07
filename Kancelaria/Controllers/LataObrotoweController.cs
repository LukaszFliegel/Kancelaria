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
    public class LataObrotoweController : KancelariaController
    {
        protected LataObrotoweRepository LataObrotoweRepository = new LataObrotoweRepository();

        public ActionResult Kartoteka(int? page)
        {
            var Model = LataObrotoweRepository.LataObrotowe(KancelariaSettings.IdFirmy(User.Identity.Name), page ?? 0);

            return View(Grid(Model));
        }

        private GridSettings<RokObrotowy> Grid(PagedSearchedQueryResult<RokObrotowy> quertResult)
        {
            GridSettings<RokObrotowy> GridSettings = new GridSettings<RokObrotowy>(quertResult);

            return GridSettings;
        }

        public ActionResult Dodaj()
        {
            var Model = new RokObrotowy();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new RokObrotowy();
            try
            {
                Model.IdFirmy = KancelariaSettings.IdFirmy(User.Identity.Name);

                UpdateModel(Model);

                if (Model.IsValid)
                {
                    LataObrotoweRepository.Dodaj(Model);
                    LataObrotoweRepository.Save();

                    TempData["Message"] = String.Format("Dodano rok oborotowy");

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania roku obrotowego";
                Logger.ErrorFormat("Wystąpił błąd podczas dodawania roku obrotowego\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Edytuj(int id)
        {
            var Model = LataObrotoweRepository.RokObrotowy(id);

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
            var Model = LataObrotoweRepository.RokObrotowy(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    LataObrotoweRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano rok obrotowy");

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji roku obrotowego";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji roku obrotowego\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = LataObrotoweRepository.RokObrotowy(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.FakturaZakupus.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć roku obrotowego, na który wprowadzone są faktury zakupu");
                return RedirectToAction("Kartoteka");
            }

            if (Model.FakturaSprzedazies.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć roku obrotowego, na który wprowadzone są faktury sprzedaży");
                return RedirectToAction("Kartoteka");
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usun(int id, FormCollection collection)
        {
            var Model = LataObrotoweRepository.RokObrotowy(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.FakturaZakupus.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć roku obrotowego, na który wprowadzone są faktury zakupu");
                return RedirectToAction("Kartoteka");
            }

            if (Model.FakturaSprzedazies.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć roku obrotowego, na który wprowadzone są faktury sprzedaży");
                return RedirectToAction("Kartoteka");
            }

            try
            {
                LataObrotoweRepository.Usun(Model);
                LataObrotoweRepository.Save();

                TempData["Message"] = String.Format("Usunięto rok obrotowy \"{0}\"", Model.NazwaRoku);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania roku obrotowego";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania roku obrotowego\n{0}", ex);

                return View(Model);
            }
        }

        // sluzy do wyboru roku w kontekscie ktorego pracuje user (zapisuje go w cache'u)
        public ActionResult Wybor(string returnUrl, int? page)
        {
            var Model = LataObrotoweRepository.LataObrotowe(KancelariaSettings.IdFirmy(User.Identity.Name), page ?? 0);

            return View(new WyborRokuObrotowegoModel(new GridSettings<RokObrotowy>(Model), returnUrl));
        }

        [HttpPost]
        public ActionResult Wybor(int id, string returnUrl)
        {
            //System.Web.HttpContext.Current.Cache.Remove("YearId");
            //System.Web.HttpContext.Current.Cache.Insert("YearId", id);

            LataObrotoweRepository.WybierzIdRoku(id, User.Identity.Name);
            LataObrotoweRepository.Save();

            //var Model = LataObrotoweRepository.RokObrotowy(id);

            //System.Web.HttpContext.Current.Cache.Insert("YearName", Model.NazwaRoku);

            //KancelariaHttpSessionState sessionState = new KancelariaHttpSessionState();
            //sessionState.Add("YearId", id);
            //sessionState.Add("YearName", Model.NazwaRoku);

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
