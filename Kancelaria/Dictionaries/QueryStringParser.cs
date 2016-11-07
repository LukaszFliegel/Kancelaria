using Kancelaria.Globals;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Dictionaries
{
    public static class QueryStringParser<T>
    {
        private static ILog _log = LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public static IQueryable<T> Parse(IQueryable<T> query, QueryStringDictionary<T> dictionary, ref string searchQuery)
        {
            if (searchQuery != null && searchQuery.Length > 0)
                dictionary.ParseDictionary(ref query, ref searchQuery);

            return query;
        }
    }
}