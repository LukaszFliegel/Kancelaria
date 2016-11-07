using Kancelaria.Globals;
using Kancelaria.Models;
using Kancelaria.Repositories;
using Omu.Awesome.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kancelaria.Controllers
{
    [Authorize(Roles = "Kancelaria")]
    [CompanyRequired]
    public class KontrahenciController : KancelariaLookupController
    {
        protected KontrahenciRepository KontrahenciRepository = new KontrahenciRepository();

        public ActionResult Search(string search, int? page)
        {
            var result = KontrahenciRepository.Kontrahenci(KancelariaSettings.IdFirmy(User.Identity.Name))
                .Where(
                    o => o.KodKontrahenta.ToLower().Contains(search.ToLower())
                    || o.NazwaKontrahenta.ToLower().Contains(search.ToLower())
                );

            var rows = this.RenderView(@"Awesome\LookupList", result.Skip((page.Value - 1) * KancelariaSettings.PageSize).Take(KancelariaSettings.PageSize));
            return Json(new { rows, more = result.Count() > page * KancelariaSettings.PageSize });
        }

        public ActionResult Get(int id)
        {
            string Kod = KontrahenciRepository.Kontrahent(id).KodKontrahenta;
            return Content(Kod);
        }

        public ActionResult Kartoteka(int? page, string search)
        {
            var Model = KontrahenciRepository.Kontrahenci(KancelariaSettings.IdFirmy(User.Identity.Name), page ?? 0, search);

            return View(Grid(Model));
        }

        private GridSettings<Kontrahent> Grid(PagedSearchedQueryResult<Kontrahent> quertResult)
        {
            GridSettings<Kontrahent> GridSettings = new GridSettings<Kontrahent>(quertResult);

            return GridSettings;
        }

        public ActionResult UstawDomyslny(int id, string returnUrl)
        {
            KontrahenciRepository.SetDefault(KancelariaSettings.IdFirmy(User.Identity.Name), id);
            KontrahenciRepository.Save();

            TempData["Message"] = String.Format("Ustawiono domyślnego kontrahenta");

            return Redirect(returnUrl);
        }

        public ActionResult Dodaj()
        {
            return View(new Kontrahent() { Panstwo = "Polska" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new Kontrahent();

            try
            {
                Model.IdFirmy = KancelariaSettings.IdFirmy(User.Identity.Name);

                if (collection["CheckCzyVatowiec"] != null && collection["CheckCzyVatowiec"].ToString() == "on")
                    Model.CzyVatowiec = true;
                else
                    Model.CzyVatowiec = false;

                UpdateModel(Model);

                if (Model.IsValid && ModelState.IsValid)
                {
                    KontrahenciRepository.Dodaj(Model);
                    KontrahenciRepository.Save();

                    TempData["Message"] = "Dodano kontrahenta";

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
                Logger.ErrorFormat("Dodawanie faktur zakupu\n{0}", ex);

                ViewBag.ErrorMessage = String.Format("Nie powiodło się dodawnie kontrahenta\n{0}", ex.Message);

                return View(Model);
            }
        }

        public ActionResult Edytuj(int id)
        {
            var Model = KontrahenciRepository.Kontrahent(id);

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
            var Model = KontrahenciRepository.Kontrahent(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                if (collection["CheckCzyVatowiec"] != null && collection["CheckCzyVatowiec"].ToString() == "on")
                    Model.CzyVatowiec = true;
                else
                    Model.CzyVatowiec = false;

                UpdateModel(Model);

                if (Model.IsValid)
                {
                    KontrahenciRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano kontrahenta \"{0}\"", Model.KodKontrahenta);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji kontrahenta";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji kontrahenta\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = KontrahenciRepository.Kontrahent(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.FakturaZakupus.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć kontrahenta, który występuje na fakturach zakupu");
                return RedirectToAction("Kartoteka");
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usun(int id, FormCollection collection)
        {
            var Model = KontrahenciRepository.Kontrahent(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.FakturaZakupus.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć kontrahenta, który występuje na fakturach zakupu");
                return RedirectToAction("Kartoteka");
            }

            try
            {
                KontrahenciRepository.Usun(Model);
                KontrahenciRepository.Save();

                TempData["Message"] = String.Format("Usunięto kontrahenta \"{0}\"", Model.KodKontrahenta);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania kontrahenta";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania kontrahenta\n{0}", ex);

                return View(Model);
            }
        }
    }
}
