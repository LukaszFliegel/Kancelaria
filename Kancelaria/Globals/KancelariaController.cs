using log4net;
using Omu.Awesome.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kancelaria.Globals
{
    [Authorize]
    public class KancelariaController : Controller
    {
        protected static readonly ILog Logger = LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Logger.IsDebugEnabled)
                Logger.Debug(
                    String.Format("{0} -> {1} ({2}) START", 
                        filterContext.Controller.ToString(), 
                        filterContext.ActionDescriptor.ActionName.ToString(),
                        filterContext.ActionParameters.Select(p => String.Format("{0} = {1}", p.Key, p.Value)).ToString(", ")
                    )
                );

            base.OnActionExecuting(filterContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            SetSkinNames(User != null ? User.Identity.Name : "");

            base.OnResultExecuting(filterContext);
        }

        private string CurrentTheme = "start";

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //ViewBag.CurrentTheme = CurrentTheme; // skorka - na sztywno - do zaimplementowania mozliwosc zmiany skorki
            if (User != null)
                ViewBag.UserName = User.Identity.Name;

            ViewBag.returnUrl = filterContext.HttpContext.Request.Url;
        }

        protected void SetSkinNames(string userName)
        {
            List<SelectListItem> skinNames = new List<SelectListItem>();

            List<string> skinList;

            if ((skinList = (List<string>)System.Web.HttpContext.Current.Cache.Get("KancelariaSkins")) == null)
            {
                skinList = new List<string>();

                foreach (DirectoryInfo di in (new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Themes")).GetDirectories())
                {
                    if(di.Name != "base") // TODO: co to jest skorka base? potrzebna?
                        skinList.Add(di.Name);
                }

                System.Web.HttpContext.Current.Cache.Insert("KancelariaSkins", skinList);
            }

            ViewData["CurrentTheme"] = CurrentTheme;
            ViewBag.CurrentTheme = CurrentTheme;

            foreach (string s in skinList)
            {
                if (s.Equals("debug"))
                    continue;

                if (s.Equals((string)System.Web.HttpContext.Current.Cache.Get(userName + "CurrentTheme")))
                {
                    skinNames.Add(new SelectListItem { Text = s, Value = s, Selected = true });
                    ViewData["CurrentTheme"] = s;
                    ViewBag.CurrentTheme = s;
                }
                else
                    skinNames.Add(new SelectListItem { Text = s, Value = s });
            }

            ViewData["skinList"] = skinList;
            ViewBag.skinList = skinList;
            ViewData["skinNames"] = skinNames;
            ViewBag.skinNames = skinNames;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (Logger.IsErrorEnabled)
            {
                try
                {
                    Logger.ErrorFormat("Wystapil blad podczas wykonywania {0}.{1}\n{2}",
                        filterContext.RouteData.Values["controller"].ToString(),
                        filterContext.RouteData.Values["action"].ToString(),
                        filterContext.Exception);
                }
                catch (Exception)
                {
                }
            }

            base.OnException(filterContext);
        }
    }

    public class KancelariaLookupController : LookupController
    {
        protected static readonly ILog Logger = LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Logger.IsDebugEnabled)
                Logger.Debug(
                    String.Format("{0} -> {1} ({2}) START",
                        filterContext.Controller.ToString(),
                        filterContext.ActionDescriptor.ActionName.ToString(),
                        filterContext.ActionParameters.Select(p => String.Format("{0} = {1}", p.Key, p.Value)).ToString(", ")
                    )
                );

            base.OnActionExecuting(filterContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            SetSkinNames(User != null ? User.Identity.Name : "");

            base.OnResultExecuting(filterContext);
        }

        private string CurrentTheme = "start";

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //ViewBag.CurrentTheme = CurrentTheme; // skorka - na sztywno - do zaimplementowania mozliwosc zmiany skorki
            if (User != null)
                ViewBag.UserName = User.Identity.Name;

            ViewBag.returnUrl = filterContext.HttpContext.Request.Url;
        }

        protected void SetSkinNames(string userName)
        {
            List<SelectListItem> skinNames = new List<SelectListItem>();

            List<string> skinList;

            if ((skinList = (List<string>)System.Web.HttpContext.Current.Cache.Get("KancelariaSkins")) == null)
            {
                skinList = new List<string>();

                foreach (DirectoryInfo di in (new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Themes")).GetDirectories())
                {
                    skinList.Add(di.Name);
                }

                System.Web.HttpContext.Current.Cache.Insert("KancelariaSkins", skinList);
            }

            ViewData["CurrentTheme"] = CurrentTheme;
            ViewBag.CurrentTheme = CurrentTheme;

            foreach (string s in skinList)
            {
                if (s.Equals("debug"))
                    continue;

                if (s.Equals((string)System.Web.HttpContext.Current.Cache.Get(userName + "CurrentTheme")))
                {
                    skinNames.Add(new SelectListItem { Text = s, Value = s, Selected = true });
                    ViewData["CurrentTheme"] = s;
                    ViewBag.CurrentTheme = s;
                }
                else
                    skinNames.Add(new SelectListItem { Text = s, Value = s });
            }

            ViewData["skinList"] = skinList;
            ViewBag.skinList = skinList;
            ViewData["skinNames"] = skinNames;
            ViewBag.skinNames = skinNames;
        }
    }
}