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
    public class InwestycjeController : KancelariaLookupController
    {
        protected InwestycjeRepository InwestycjeRepository = new InwestycjeRepository();
        protected TypyInwestycjiRepository TypyInwestycjiRepository = new TypyInwestycjiRepository(); 

        public ActionResult Search(string search, int? page)
        {
            //obtain the result somehow (an IEnumerable<Fruit>)
            var result = InwestycjeRepository.Inwestycje(
                    KancelariaSettings.IdFirmy(User.Identity.Name)
                ).Where(
                    o => o.NumerInwestycji.ToLower().Contains(search.ToLower())
                        || o.Opis.ToLower().Contains(search.ToLower())
                    );

            var rows = this.RenderView(@"Awesome\LookupList", result.Skip((page.Value - 1) * KancelariaSettings.PageSize).Take(KancelariaSettings.PageSize));
            return Json(new { rows, more = result.Count() > page * KancelariaSettings.PageSize });
        }

        public ActionResult Get(int id)
        {
            string Kod = InwestycjeRepository.Inwestycja(id).NumerInwestycji;
            return Content(Kod);
        }

        public ActionResult Kartoteka(int? page, string search)
        {
            var Model = InwestycjeRepository.Inwestycje(KancelariaSettings.IdFirmy(User.Identity.Name), page ?? 0, search);

            return View(Grid(Model));
        }

        private GridSettings<Inwestycja> Grid(PagedSearchedQueryResult<Inwestycja> quertResult)
        {
            GridSettings<Inwestycja> GridSettings = new GridSettings<Inwestycja>(quertResult);

            return GridSettings;
        }

        public ActionResult UstawDomyslny(int id, string returnUrl)
        {
            InwestycjeRepository.SetDefault(KancelariaSettings.IdFirmy(User.Identity.Name), id);
            InwestycjeRepository.Save();

            TempData["Message"] = String.Format("Ustawiono domyślną inwestycję");

            return Redirect(returnUrl);
        }

        public ActionResult Dodaj()
        {
            var domyslnyTypInwestycji = TypyInwestycjiRepository.GetDefaultId();

            if(domyslnyTypInwestycji == null)
            {
                TempData["Message"] = "Brak typu inwestycji, dla którego można by stworzyć inwestycję";
                return RedirectToAction("Kartoteka");
            }

            var Model = new Inwestycja()
            {
                IdTypuInwestycji = domyslnyTypInwestycji.Value
            };            

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dodaj(FormCollection collection)
        {
            var Model = new Inwestycja();
            try
            {
                Model.IdFirmy = KancelariaSettings.IdFirmy(User.Identity.Name);

                UpdateModel(Model);

                if (Model.IsValid)
                {
                    InwestycjeRepository.Dodaj(Model);
                    InwestycjeRepository.Save();

                    TempData["Message"] = String.Format("Dodano inwestycję \"{0}\"", Model.NumerInwestycji);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas dodawania inwestycji";
                Logger.ErrorFormat("Wystąpił błąd podczas dodawania inwestycji\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Edytuj(int id)
        {
            var Model = InwestycjeRepository.Inwestycja(id);

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
            var Model = InwestycjeRepository.Inwestycja(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            try
            {
                UpdateModel(Model);

                if (Model.IsValid)
                {
                    InwestycjeRepository.Save();

                    TempData["Message"] = String.Format("Zmodyfikowano inwestycję \"{0}\"", Model.NumerInwestycji);

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
                ViewBag.ErrorMessage = "Wystąpił błąd podczas modyfikacji inwestycji";
                Logger.ErrorFormat("Wystąpił błąd podczas modyfikacji inwestycji\n{0}", ex);

                return View(Model);
            }
        }

        public ActionResult Usun(int id)
        {
            var Model = InwestycjeRepository.Inwestycja(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.PozycjaFakturyZakupus.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć inwestycji, która występuje na fakturach zakupu");
                return RedirectToAction("Kartoteka");
            }

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usun(int id, FormCollection collection)
        {
            var Model = InwestycjeRepository.Inwestycja(id);

            if (Model == null)
            {
                return View("NotFound");
            }

            if (Model.PozycjaFakturyZakupus.Count() > 0)
            {
                TempData["Message"] = String.Format("Nie można usunąć inwestycji, która występuje na fakturach zakupu");
                return RedirectToAction("Kartoteka");
            }

            try
            {
                InwestycjeRepository.Usun(Model);
                InwestycjeRepository.Save();

                TempData["Message"] = String.Format("Usunięto inwestycję \"{0}\"", Model.NumerInwestycji);

                return RedirectToAction("Kartoteka");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Wystąpił błąd podczas usuwania inwestycji";
                Logger.ErrorFormat("Wystąpił błąd podczas usuwania inwestycji\n{0}", ex);

                return View(Model);
            }
        }
    }
}
