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
    public class JednostkiMiaryController : KancelariaLookupController
    {
        protected JednostkiMiaryRepository JednostkiMiaryRepository = new JednostkiMiaryRepository();

        public ActionResult Search(string search, int? page)
        {
            //obtain the result somehow (an IEnumerable<Fruit>)
            var result = JednostkiMiaryRepository.SposobyPlatnosci().Where(o => o.KodJednostkiMiary.ToLower().Contains(search.ToLower()));

            var rows = this.RenderView(@"Awesome\LookupList", result.Skip((page.Value - 1) * KancelariaSettings.PageSize).Take(KancelariaSettings.PageSize));
            return Json(new { rows, more = result.Count() > page * KancelariaSettings.PageSize });
        }

        public ActionResult Get(int id)
        {
            string Kod = JednostkiMiaryRepository.JednostkaMiary(id).KodJednostkiMiary;
            return Content(Kod);
        }

        public ActionResult Kartoteka(int? page, string search, string asc, string desc)
        {
            var Model = JednostkiMiaryRepository.SposobyPlatnosci(page ?? 0, search, asc, desc);

            return View(Grid(Model));
        }

        private GridSettings<JednostkaMiary> Grid(PagedSearchedQueryResult<JednostkaMiary> queryResult)
        {
            GridSettings<JednostkaMiary> GridSettings = new GridSettings<JednostkaMiary>(queryResult);

            return GridSettings;
        }

        public ActionResult UstawDomyslny(int id, string returnUrl)
        {
            JednostkiMiaryRepository.SetDefault(id);
            JednostkiMiaryRepository.Save();

            TempData["Message"] = String.Format("Ustawiono domyślną jednostkę miary");

            return Redirect(returnUrl);
        }

        public ActionResult Dodaj()
        {
            var Model = new JednostkaMiary();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new JednostkaMiary();
            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    JednostkiMiaryRepository.Dodaj(Model);
                    JednostkiMiaryRepository.Save();

                    TempData["Message"] = String.Format("Dodano jednostkę miary \"{0}\"", Model.KodJednostkiMiary);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania jednostki miary";
                Logger.ErrorFormat("Wystąpił błąd podczas dodawania jednostki miary\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Edytuj(int id)
        {
            var Model = JednostkiMiaryRepository.JednostkaMiary(id);

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
            var Model = JednostkiMiaryRepository.JednostkaMiary(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    JednostkiMiaryRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano jednostkę miary \"{0}\"", Model.KodJednostkiMiary);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji jednostki miary";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji jednostki miary\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = JednostkiMiaryRepository.JednostkaMiary(id);

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
            var Model = JednostkiMiaryRepository.JednostkaMiary(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                JednostkiMiaryRepository.Usun(Model);
                JednostkiMiaryRepository.Save();

                TempData["Message"] = String.Format("Usunięto jednostkę miary \"{0}\"", Model.KodJednostkiMiary);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania jednostki miary";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania jednostki miary\n{0}", ex);

                return View(Model);
            }
        }

    }
}
