using Kancelaria.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kancelaria.Globals
{
    public class KancelariaFilterAttribute : ActionFilterAttribute
    {
        protected static readonly ILog Logger = LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());
    }

    public class CompanyRequiredAttribute : KancelariaFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                int? CompanyId = null;
                int ParsedCompanyId;

                // do not make this repository as a class property, it has to be instantiated in every request (or you wont see new results)
                FirmyRepository FirmyRepository = new FirmyRepository();

                // jesli w AppSettings ustawimy idFirmy to zapisujemy je w cache'u
                if (Int32.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["idFirmy"], out ParsedCompanyId))
                {
                    //System.Web.HttpContext.Current.Cache.Insert("CompanyId", ParsedCompanyId);
                    CompanyId = ParsedCompanyId;
                }

                // sprawdzam czy idFirmy jest w cache'u
                if (!CompanyId.HasValue && !(CompanyId = FirmyRepository.WybraneIdFirmy(filterContext.HttpContext.User.Identity.Name)).HasValue)    
                {
                    string returnUrl = filterContext.HttpContext.Request.Url.ToString();
                    string debug = filterContext.HttpContext.Request.Params["returnUrl"];
                    filterContext.Controller.TempData["Message"] = String.Format("Nie wybrano firmy.");
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Wybor" }, { "controller", "Firmy" }, { "returnUrl", returnUrl } });
                }
                else
                {
                    // jesli jest to do ViewBag
                    filterContext.Controller.ViewBag.CompanyId = CompanyId;
                    filterContext.Controller.TempData["CompanyId"] = CompanyId;

                    // jesli nazwa firmy (aby zaprezentowac userowy na View) jest w cache'u
                    // TTA: Czy robic strzal na baze gdy w cache'u nie ma nazwy firmy? Nie ma jej gdy, ustawie AppSettings
                    // Raczej nie, po co klopotac wtedy usera jakimis firmami, nie wyswietlajmy mu info o wybranej firmie jesli ma jednofirmowosc (czyli ustawione AppSettings CompanyId)
                    // jesli chce nazwe wyswietlac to wpisujemy ja do AppSettings CompanyName
                    //string CompanyName = (string)System.Web.HttpContext.Current.Cache.Get("CompanyName");
                    if (!KancelariaSettings.IsOneFirm())
                    {
                        string CompanyName = FirmyRepository.Firma(CompanyId.Value).NazwaSkrocona;

                        if (!String.IsNullOrEmpty(CompanyName))
                        {
                            filterContext.Controller.ViewBag.CompanyName = CompanyName;
                            filterContext.Controller.TempData["CompanyName"] = CompanyName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                filterContext.Result = new RedirectToRouteResult("Firma", null);
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class YearRequiredAttribute : KancelariaFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                int? CompanyId = null;
                int? YearId = null;
                int ParsedCompanyId;
                int ParsedYearId;

                // do not make this repositories as a class properties, they has to be instantiated in every request (or you wont see new results)
                FirmyRepository FirmyRepository = new FirmyRepository();
                LataObrotoweRepository LataObrotoweRepository = new LataObrotoweRepository(); 

                // jesli w AppSettings ustawimy idFirmy to zapisujemy je w cache'u
                if (Int32.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["idFirmy"], out ParsedCompanyId))
                {
                    CompanyId = ParsedCompanyId;
                    //System.Web.HttpContext.Current.Cache.Insert("CompanyId", ParsedCompanyId);
                }

                // jesli w AppSettings ustawimy idRoku to zapisujemy je w cache'u
                if (Int32.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["idRoku"], out ParsedYearId))
                {
                    YearId = ParsedYearId;
                    //System.Web.HttpContext.Current.Cache.Insert("YearId", ParsedYearId);
                }

                //if (!(CompanyId = (int?)System.Web.HttpContext.Current.Cache.Get("CompanyId")).HasValue)
                if (!CompanyId.HasValue && !(CompanyId = FirmyRepository.WybraneIdFirmy(filterContext.HttpContext.User.Identity.Name)).HasValue)                
                {
                    string returnUrl = filterContext.HttpContext.Request.Url.ToString();
                    string debug = filterContext.HttpContext.Request.Params["returnUrl"];
                    filterContext.Controller.TempData["Message"] = String.Format("Nie wybrano firmy.");
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Wybor" }, { "controller", "Firmy" }, { "returnUrl", returnUrl } });
                }
                else
                {
                    if (!YearId.HasValue && !(YearId = LataObrotoweRepository.WybraneIdRoku(filterContext.HttpContext.User.Identity.Name)).HasValue)
                    {
                        string returnUrl = filterContext.HttpContext.Request.Url.ToString();
                        string debug = filterContext.HttpContext.Request.Params["returnUrl"];
                        filterContext.Controller.TempData["Message"] = String.Format("Nie wybrano roku.");
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Wybor" }, { "controller", "LataObrotowe" }, { "idFirmy", CompanyId }, { "returnUrl", returnUrl } });
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.YearId = YearId;
                        filterContext.Controller.TempData["YearId"] = YearId;

                        //string YearName = (string)System.Web.HttpContext.Current.Cache.Get("YearName");
                        string YearName = LataObrotoweRepository.RokObrotowy(YearId.Value).NazwaRoku;

                        if (!String.IsNullOrEmpty(YearName))
                        {
                            filterContext.Controller.ViewBag.YearName = YearName;
                            filterContext.Controller.TempData["YearName"] = YearName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                filterContext.Result = new RedirectToRouteResult("Firma", null);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}