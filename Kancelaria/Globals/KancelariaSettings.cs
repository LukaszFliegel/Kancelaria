using Kancelaria.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Kancelaria.Globals
{
    public class KancelariaSettings
    {
        public const int PageSize = 10;

        public static string CompanyName()
        {
            if (!String.IsNullOrEmpty(WebConfigurationManager.AppSettings["OwnerCompanyName"]))
            {
                return (WebConfigurationManager.AppSettings["OwnerCompanyName"]).ToString();
            }
            else
            {
                return "";
            }
        }

        public static int DefaultDayOfPaymentDaysAdded()
        {
            int DefaultDayOfPaymentDaysAdded;

            if (Int32.TryParse(WebConfigurationManager.AppSettings["DomyslnaLiczbaDniTerminuPlatnosci"], out DefaultDayOfPaymentDaysAdded))
            {
                // parsowanie DomyslnaLiczbaDniTerminuPlatnosci sie powiodlo - zwracamy liczbe dni dodawanych domyslnie do terminu platnosci z AppSettings
                return DefaultDayOfPaymentDaysAdded;
            }
            
            // nie ma w WebConfig liczby dni, zwracamy 21
            return 21;
        }

        public static string AdminUserName()
        {
            return WebConfigurationManager.AppSettings["AdminUserName"];
        }

        public static int DefaultVatValue()
        {
            int DefaultVatValue;

            if (Int32.TryParse(WebConfigurationManager.AppSettings["DomyslnaStawkaVat"], out DefaultVatValue))
            {
                // parsowanie DomyslnaStawkaVat sie powiodlo - zwracamy domsylna stawke z AppSettings
                return DefaultVatValue;
            }
            
            // nie ma w WebConfig stawki, zwracamy 23
            return 23;
        }

        public static bool IsOneFirm()
        {
            int ParsedCompanyId;

            if (Int32.TryParse(WebConfigurationManager.AppSettings["idFirmy"], out ParsedCompanyId))
            {
                // parsowanie idFirmy sie powiodlo - czyli ustawione i mamy jednofirmowosc
                return true;
            }

            return false;
        }

        public static int IdFirmy(string userName)
        {
            FirmyRepository FirmyRepository = new FirmyRepository();

            int? CompanyId = FirmyRepository.WybraneIdFirmy(userName);
            //if ((CompanyId = (int?)System.Web.HttpContext.Current.Cache.Get("CompanyId")).HasValue)
            //{
            //    return CompanyId.Value;
            //}

            if (CompanyId.HasValue)
            {
                return CompanyId.Value;
            }
            else
            {
                int ParsedCompanyId;

                if (Int32.TryParse(WebConfigurationManager.AppSettings["idFirmy"], out ParsedCompanyId))
                {
                    return ParsedCompanyId;
                }
                else
                {
                    throw new Exception("Niepowodzenie odczytu id firmy");
                }
            }
        }

        public static int IdRoku(string userName)
        {
            LataObrotoweRepository LataObrotoweRepository = new LataObrotoweRepository();   
            //return (int)System.Web.HttpContext.Current.Cache.Get("YearId");

            int? YearId = LataObrotoweRepository.WybraneIdRoku(userName);
            //if ((YearId = (int?)System.Web.HttpContext.Current.Cache.Get("YearId")).HasValue)
            //{
            //    return YearId.Value;
            //}

            if (YearId.HasValue)
            {
                return YearId.Value;
            }
            else
            {
                int ParsedYearId;

                if (Int32.TryParse(WebConfigurationManager.AppSettings["idRoku"], out ParsedYearId))
                {
                    return ParsedYearId;
                }
                else
                {
                    throw new Exception("Niepowodzenie odczytu id roku");
                }
            }
        }
    }
}