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
    public class TypyInwestycjiController : KancelariaLookupController
    {
        protected TypyInwestycjiRepository TypyInwestycjiRepository = new TypyInwestycjiRepository();

        public ActionResult Search(string search, int? page)
        {
            //obtain the result somehow (an IEnumerable<Fruit>)
            var result = TypyInwestycjiRepository.TypyInwestycji().Where(o => o.KodTypuInwestycji.ToLower().Contains(search.ToLower()));

            var rows = this.RenderView(@"Awesome\LookupList", result.Skip((page.Value - 1) * KancelariaSettings.PageSize).Take(KancelariaSettings.PageSize));
            return Json(new { rows, more = result.Count() > page * KancelariaSettings.PageSize });
        }

        public ActionResult Get(int id)
        {
            string Kod = TypyInwestycjiRepository.TypInwestycji(id).KodTypuInwestycji;
            return Content(Kod);
        }

        public ActionResult Kartoteka(int? page)
        {
            var Model = TypyInwestycjiRepository.TypyInwestycji(page ?? 0);

            return View(Grid(Model));
        }

        private GridSettings<TypInwestycji> Grid(PagedSearchedQueryResult<TypInwestycji> quertResult)
        {
            GridSettings<TypInwestycji> GridSettings = new GridSettings<TypInwestycji>(quertResult);

            return GridSettings;
        }

        public ActionResult UstawDomyslny(int id, string returnUrl)
        {
            TypyInwestycjiRepository.SetDefault(id);
            TypyInwestycjiRepository.Save();

            TempData["Message"] = String.Format("Ustawiono domyślny typ inwestycji");

            return Redirect(returnUrl);
        }

        public ActionResult Dodaj()
        {
            var Model = new TypInwestycji();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new TypInwestycji();
            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    TypyInwestycjiRepository.Dodaj(Model);
                    TypyInwestycjiRepository.Save();

                    TempData["Message"] = String.Format("Dodano typ inwestycji \"{0}\"", Model.KodTypuInwestycji);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania typu inwestycji";
                Logger.ErrorFormat("Wystąpił błąd podczas dodawania typu inwestycji\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Edytuj(int id)
        {
            var Model = TypyInwestycjiRepository.TypInwestycji(id);

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
            var Model = TypyInwestycjiRepository.TypInwestycji(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    TypyInwestycjiRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano typ inwestycji \"{0}\"", Model.KodTypuInwestycji);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji typu inwestycji";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji typu inwestycji\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = TypyInwestycjiRepository.TypInwestycji(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.Inwestycjas.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć typu inwestycji, który jest przypisany do wprowadzonych inwestycji");
                return RedirectToAction("Kartoteka");
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usun(int id, FormCollection collection)
        {
            var Model = TypyInwestycjiRepository.TypInwestycji(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.Inwestycjas.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć typu inwestycji, który jest przypisany do wprowadzonych inwestycji");
                return RedirectToAction("Kartoteka");
            }

            try
            {
                TypyInwestycjiRepository.Usun(Model);
                TypyInwestycjiRepository.Save();

                TempData["Message"] = String.Format("Usunięto typ inwestycji \"{0}\"", Model.KodTypuInwestycji);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania typu inwestycji";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania typu inwestycji\n{0}", ex);

                return View(Model);
            }
        }
    }
}
