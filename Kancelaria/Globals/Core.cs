using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Kancelaria.Globals
{
    public class Core
    {
        /// <summary>
        /// Metody związane z Http
        /// </summary>
        public static class WebUtils
        {
            /// <summary>
            /// Zwraca AbsolutePath i QueryString z podanego requesta
            /// wycinając z QueryString wartość parametru paramName
            /// </summary>
            /// <param name="request"></param>
            /// <param name="paramName"></param>
            /// <returns></returns>
            public static string UrlRemoveParam(HttpRequestBase request, string paramName)
            {
                string newQueryString = UrlRemoveParam(request.Url.Query, paramName);
                if (String.IsNullOrEmpty(newQueryString))
                    return request.Url.AbsolutePath;
                return request.Url.AbsolutePath + "?" + newQueryString;
            }

            /// <summary>
            /// Zwraca zadane QueryString z wyciętą wartością parametru paramName
            /// </summary>
            /// <param name="query"></param>
            /// <param name="paramName"></param>
            /// <returns></returns>
            public static string UrlRemoveParam(string query, string paramName)
            {
                // Trick: HttpUtility.ParseQueryString zwraca HttpValueCollection, 
                // na którym działa ładnie ToString()
                NameValueCollection filtered = HttpUtility.ParseQueryString(query, Encoding.UTF8);
                filtered.Remove(paramName);
                return filtered.ToString();
            }

            /// <summary>
            /// Zwraca AbsolutePath i QueryString z podanego requesta
            /// zamieniając/wstawiając wartość parametru paramName na paramValue
            /// </summary>
            /// <param name="request"></param>
            /// <param name="paramName"></param>
            /// <param name="paramValue"></param>
            /// <returns></returns>
            public static string UrlReplaceParam(HttpRequestBase request, string paramName, object paramValue)
            {
                string newQueryString = UrlReplaceParam(request.Url.Query, paramName, paramValue);
                if (String.IsNullOrEmpty(newQueryString))
                    return request.Url.AbsolutePath;
                return request.Url.AbsolutePath + "?" + newQueryString;
            }

            /// <summary>
            /// Zwraca zadane QueryString zamieniając/wstawiając wartość parametru paramName na paramValue
            /// </summary>
            /// <param name="query"></param>
            /// <param name="paramName"></param>
            /// <param name="paramValue"></param>
            /// <returns></returns>
            public static string UrlReplaceParam(string query, string paramName, object paramValue)
            {
                // Trick: HttpUtility.ParseQueryString zwraca HttpValueCollection, 
                // na którym działa ładnie ToString()
                NameValueCollection filtered = HttpUtility.ParseQueryString(query, Encoding.UTF8);
                filtered[paramName] = paramValue.ToString();
                return filtered.ToString();
            }

        }

    }
}