using Kancelaria.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kancelaria.Controllers
{
    [Authorize(Roles = "Kancelaria")]
    public class KalendarzController : KancelariaController
    {
        //
        // GET: /Kalendarz/

        public ActionResult Index()
        {
            return View();
        }

    }
}
