using Kancelaria.Globals;
using Kancelaria.Models;
using Kancelaria.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Kancelaria.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : KancelariaController
    {
        protected AdminRepository AdminRepository = new AdminRepository();

        public ActionResult Kartoteka(int? page)
        {
            var Model = AdminRepository.UzytkownicyMembership(page ?? 0);

            return View(Grid(Model));
        }

        private GridSettings<UzytkownikMembership> Grid(PagedSearchedQueryResult<UzytkownikMembership> quertResult)
        {
            GridSettings<UzytkownikMembership> GridSettings = new GridSettings<UzytkownikMembership>(quertResult);

            return GridSettings;
        }

        public ActionResult PrzestawRoleAdmin(int userId)
        {
            var uzytkownik = AdminRepository.Uzytkownik(userId);

            string Role = "Admin";

            if(uzytkownik == null)
            {
                TempData["Message"] = String.Format("Niepoprawna nazwa użytkownika");
                return RedirectToAction("Kartoteka");
            }

            if(Roles.IsUserInRole(uzytkownik.UserName, Role))
            {
                AdminRepository.ObierzRole(uzytkownik.UserId, Role);
                TempData["Message"] = String.Format("Odebrano rolę \"{0}\" użytkownikowi \"{1}\"", Role, uzytkownik.UserName);
            }
            else
            {
                AdminRepository.DodajRole(uzytkownik.UserId, Role);
                TempData["Message"] = String.Format("Nadano rolę \"{0}\" użytkownikowi \"{1}\"", Role, uzytkownik.UserName);
            }

            return RedirectToAction("Kartoteka");
        }

        public ActionResult PrzestawRoleKancelaria(int userId)
        {
            var uzytkownik = AdminRepository.Uzytkownik(userId);

            string Role = "Kancelaria";

            if (uzytkownik == null)
            {
                TempData["Message"] = String.Format("Niepoprawna nazwa użytkownika");
                return RedirectToAction("Kartoteka");
            }

            if(Roles.IsUserInRole(uzytkownik.UserName, Role))
            {
                AdminRepository.ObierzRole(uzytkownik.UserId, Role);
                TempData["Message"] = String.Format("Odebrano rolę \"{0}\" użytkownikowi \"{1}\"", Role, uzytkownik.UserName);
            }
            else
            {
                AdminRepository.DodajRole(uzytkownik.UserId, Role);
                TempData["Message"] = String.Format("Nadano rolę \"{0}\" użytkownikowi \"{1}\"", Role, uzytkownik.UserName);
            }

            return RedirectToAction("Kartoteka");
        }
    }
}
