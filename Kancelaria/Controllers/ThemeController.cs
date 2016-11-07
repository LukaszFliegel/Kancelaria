using Kancelaria.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kancelaria.Controllers
{
    public class ThemeController : KancelariaController
    {
        [HttpPost]
        public ActionResult ChangeSkin(string skinNames, string returnUrl)
        {
            if (String.IsNullOrEmpty(skinNames))
                return Redirect(returnUrl);

            System.Web.HttpContext.Current.Cache.Insert(User.Identity.Name + "CurrentTheme", skinNames);

            SetSkinNames(User.Identity.Name);

            return Redirect(returnUrl);
        }

    }
}
