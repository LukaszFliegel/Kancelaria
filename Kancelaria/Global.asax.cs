using Kancelaria.Globals;
using Omu.Awesome.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace Kancelaria
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Uzytkownicy", "UserId", "UserName", autoCreateTables: true);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // added
            RegisterRoutes(RouteTable.Routes);

            ModelMetadataProviders.Current = new AwesomeModelMetadataProvider();

            if (!System.Web.Security.Roles.IsUserInRole(KancelariaSettings.AdminUserName(), "Admin") && WebSecurity.UserExists(KancelariaSettings.AdminUserName()))
                System.Web.Security.Roles.AddUserToRole(KancelariaSettings.AdminUserName(), "Admin");

            if (!System.Web.Security.Roles.IsUserInRole(KancelariaSettings.AdminUserName(), "Kancelaria") && WebSecurity.UserExists(KancelariaSettings.AdminUserName()))
                System.Web.Security.Roles.AddUserToRole(KancelariaSettings.AdminUserName(), "Kancelaria");
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Menu", // Route name
                "Home/Index/", // URL with parameters
                new { controller = "Home", action = "Index" }
            );

            //routes.MapRoute(
            //    "Firma", // Route name
            //    "Firmy/Wybor/", // URL with parameters
            //    new { controller = "Firmy", action = "Wybor" }
            //);

            //routes.MapRoute(
            //    "RokObrotowy", // Route name
            //    "LataObrotowe/Wybor/{idFirmy}", // URL with parameters
            //    new { controller = "LataObrotowe", action = "Wybor", idFirmy = "", returnUrl = "" }
            //);
        }
    }
}