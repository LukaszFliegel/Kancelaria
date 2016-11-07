using Kancelaria.Globals;
using Kancelaria.Models;
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
    public class SposobyPlatnosciController : KancelariaLookupController
    {
        protected SposobyPlatnosciRepository SposobyPlatnosciRepository = new SposobyPlatnosciRepository();

        public ActionResult Search(string search, int? page)
        {
            //obtain the result somehow (an IEnumerable<Fruit>)
            var result = SposobyPlatnosciRepository.SposobyPlatnosci().Where(o => o.KodSposobuPlatnosci.ToLower().Contains(search.ToLower()));

            var rows = this.RenderView(@"Awesome\LookupList", result.Skip((page.Value - 1) * KancelariaSettings.PageSize).Take(KancelariaSettings.PageSize));
            return Json(new { rows, more = result.Count() > page * KancelariaSettings.PageSize });
        }

        public ActionResult Get(int id)
        {
            string Kod = SposobyPlatnosciRepository.SposobPlatnosci(id).KodSposobuPlatnosci;
            return Content(Kod);
        }

        public ActionResult Kartoteka(int? page, string search, string asc, string desc)
        {
            var Model = SposobyPlatnosciRepository.SposobyPlatnosci(page ?? 0, search, asc, desc);

            return View(Grid(Model));
        }

        private GridSettings<SposobPlatnosci> Grid(PagedSearchedQueryResult<SposobPlatnosci> queryResult)
        {
            GridSettings<SposobPlatnosci> GridSettings = new GridSettings<SposobPlatnosci>(queryResult);

            return GridSettings;
        }

        public ActionResult UstawDomyslny(int id, string returnUrl)
        {
            SposobyPlatnosciRepository.SetDefault(id);
            SposobyPlatnosciRepository.Save();

            TempData["Message"] = String.Format("Ustawiono domyślny sposób płatności");

            return Redirect(returnUrl);
        }

        public ActionResult Dodaj()
        {
            var Model = new SposobPlatnosci();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new SposobPlatnosci();
            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    SposobyPlatnosciRepository.Dodaj(Model);
                    SposobyPlatnosciRepository.Save();

                    TempData["Message"] = String.Format("Dodano sposób płatności \"{0}\"", Model.KodSposobuPlatnosci);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania sposobu płatności";
                Logger.ErrorFormat("Wystąpił błąd podczas dodawania sposobu płatności\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Edytuj(int id)
        {
            var Model = SposobyPlatnosciRepository.SposobPlatnosci(id);

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
            var Model = SposobyPlatnosciRepository.SposobPlatnosci(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    SposobyPlatnosciRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano sposób płatności \"{0}\"", Model.KodSposobuPlatnosci);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji sposobu płatności";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji sposobu płatności\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = SposobyPlatnosciRepository.SposobPlatnosci(id);

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
            var Model = SposobyPlatnosciRepository.SposobPlatnosci(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                SposobyPlatnosciRepository.Usun(Model);
                SposobyPlatnosciRepository.Save();

                TempData["Message"] = String.Format("Usunięto sposób płatności \"{0}\"", Model.KodSposobuPlatnosci);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania sposobu płatności";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania sposobu płatności\n{0}", ex);

                return View(Model);
            }
        }
    }
}
