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
    [CompanyRequired]
    public class KontaBankoweController : KancelariaLookupController
    {
        protected KontaBankoweRepository KontaBankoweRepository = new KontaBankoweRepository();

        public ActionResult Search(string search, int? page)
        {
            //obtain the result somehow (an IEnumerable<Fruit>)
            var result = KontaBankoweRepository.KontaBankowe(KancelariaSettings.IdFirmy(User.Identity.Name)).Where(o => o.NumerKonta.StartsWith(search) || o.Nazwa.ToLower().Contains(search.ToLower()));

            var rows = this.RenderView(@"Awesome\LookupList", result.Skip((page.Value - 1) * KancelariaSettings.PageSize).Take(KancelariaSettings.PageSize));
            return Json(new { rows, more = result.Count() > page * KancelariaSettings.PageSize });
        }

        public ActionResult Get(int id)
        {
            KontoBankowe KontoBankowe = KontaBankoweRepository.KontoBankowe(id);
            string Kod = KontoBankowe.NumerKonta;
            return Content(Kod);
        }

        public ActionResult Kartoteka(int? page, string search, string asc, string desc)
        {
            var Model = KontaBankoweRepository.KontaBankowe(KancelariaSettings.IdFirmy(User.Identity.Name), page ?? 0, search, asc, desc);

            return View(Grid(Model));
        }

        private GridSettings<KontoBankowe> Grid(PagedSearchedQueryResult<KontoBankowe> queryResult)
        {
            GridSettings<KontoBankowe> GridSettings = new GridSettings<KontoBankowe>(queryResult);

            return GridSettings;
        }

        public ActionResult UstawDomyslny(int id, string returnUrl)
        {
            KontaBankoweRepository.SetDefault(KancelariaSettings.IdFirmy(User.Identity.Name), id);
            KontaBankoweRepository.Save();

            TempData["Message"] = String.Format("Ustawiono domyślne konto bankowe");

            return Redirect(returnUrl);
        }

        public ActionResult Dodaj()
        {
            var Model = new KontoBankowe();

            Model.IdFirmy = KancelariaSettings.IdFirmy(User.Identity.Name);

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new KontoBankowe();
            try
            {
                UpdateModel(Model);

                // usuniecie spacji z numeru konta
                Model.NumerKonta = Model.NumerKonta.Replace(" ", "");

                if (Model.IsValid)
                {
                    KontaBankoweRepository.Dodaj(Model);
                    KontaBankoweRepository.Save();

                    TempData["Message"] = String.Format("Dodano konto bankowe");

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania konta bankowego";
                Logger.ErrorFormat("Wystąpił błąd podczas dodawania konta bankowego\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Edytuj(int id)
        {
            var Model = KontaBankoweRepository.KontoBankowe(id);

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
            var Model = KontaBankoweRepository.KontoBankowe(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    KontaBankoweRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano konto bankowe");

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji konta bankowego";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji konta bankowego\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = KontaBankoweRepository.KontoBankowe(id);

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
            var Model = KontaBankoweRepository.KontoBankowe(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                KontaBankoweRepository.Usun(Model);
                KontaBankoweRepository.Save();

                TempData["Message"] = String.Format("Usunięto konto");

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania konta bankowego";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania konta bankowego\n{0}", ex);

                return View(Model);
            }
        }
    }
}
