using Kancelaria.Globals;
using Kancelaria.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kancelaria.Controllers
{
    public class HomeController : KancelariaController
    {
        [Authorize(Roles = "Kancelaria")]
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View(new MenuModel(KancelariaSettings.IsOneFirm()));
        }

        public ActionResult Ustawienia()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        //public ActionResult About()
        //{
        //    //ViewBag.Message = "Your app description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    //ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}
