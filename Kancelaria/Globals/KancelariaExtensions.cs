using Kancelaria.Models;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kancelaria.Globals
{
    public static class KancelariaExtensions
    {
        // for generic interface IEnumerable<T>
        public static string ToString<T>(this IEnumerable<T> source, string separator)
        {
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            if (string.IsNullOrEmpty(separator))
                throw new ArgumentException("Parameter separator can not be null or empty.");

            string[] array = source.Where(n => n != null).Select(n => n.ToString()).ToArray();

            return string.Join(separator, array);
        }

        // for interface IEnumerable
        public static string ToString(this IEnumerable source, string separator)
        {
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            if (string.IsNullOrEmpty(separator))
                throw new ArgumentException("Parameter separator can not be null or empty.");

            string[] array = source.Cast<object>().Where(n => n != null).Select(n => n.ToString()).ToArray();

            return string.Join(separator, array);
        }

        public static MvcHtmlString AdresKontrahenta(this HtmlHelper html, Kontrahent kontrahent)
        {
            string Adres = String.Format(
                    "{0}<br />{1} {2} ul. {3} {4}{5}{6}",
                    kontrahent.NazwaKontrahenta,
                    kontrahent.KodPocztowy,
                    kontrahent.Miejscowosc,
                    kontrahent.Ulica,
                    kontrahent.NumerDomu,
                    (!String.IsNullOrEmpty(kontrahent.NumerDomu) && !String.IsNullOrEmpty(kontrahent.NumerLokalu) ? "/" : " "),
                    kontrahent.NumerLokalu
                );

            if (kontrahent.KontrahentNadrzedny != null)
            {
                Adres = String.Format("{0}<br />{1}",
                    String.Format(
                        "{0}<br />{1} {2} ul. {3} {4}{5}{6}",
                        kontrahent.KontrahentNadrzedny.NazwaKontrahenta,
                        kontrahent.KontrahentNadrzedny.KodPocztowy,
                        kontrahent.KontrahentNadrzedny.Miejscowosc,
                        kontrahent.KontrahentNadrzedny.Ulica,
                        kontrahent.KontrahentNadrzedny.NumerDomu,
                        (!String.IsNullOrEmpty(kontrahent.KontrahentNadrzedny.NumerDomu) && !String.IsNullOrEmpty(kontrahent.KontrahentNadrzedny.NumerLokalu) ? "/" : " "),
                        kontrahent.KontrahentNadrzedny.NumerLokalu
                    ),
                    Adres
                );
            }

            return MvcHtmlString.Create(Adres);
        }

        public static MvcHtmlString KancelariaGridSortingColumn(this HtmlHelper html, //Expression<Func<TModel, TValue>> expression, 
            string fieldName, string caption,
            string urlAbsolutePath, string urlQuery, string controlName = "")
        {
            string ascSortingParamName = "asc" + controlName;
            string descSortingParamName = "desc" + controlName;

            NameValueCollection QueryCollection = HttpUtility.ParseQueryString(urlQuery, Encoding.UTF8);

            bool AscFound = false;
            bool DescFound = false;

            //////////////////////////////////////////////////////

            // usuwamy informacje o stronie pagera
            QueryCollection.Remove("page" + controlName);

            if (QueryCollection[ascSortingParamName] == fieldName)
            {
                QueryCollection.Remove(ascSortingParamName);
                QueryCollection.Add(descSortingParamName, fieldName);
                AscFound = true;
            }
            else
                if (QueryCollection[descSortingParamName] == fieldName)
                {
                    QueryCollection.Remove(descSortingParamName);
                    QueryCollection.Add(ascSortingParamName, fieldName);
                    DescFound = true;
                }
                else
                {
                    QueryCollection.Remove(descSortingParamName);
                    QueryCollection[ascSortingParamName] = fieldName;
                }
            ///////////////////////////////////////////////////////
            string Url = urlAbsolutePath + "?" + QueryCollection.ToString();

            string LinkClass = "";

            //<th class="ui-widget">Kod sposobu płatności</th>

            string Symbol = "";

            if (AscFound)
            {
                LinkClass = "header-asc";
                Symbol = "▼";
            }

            if (DescFound)
            {
                LinkClass = "header-desc";
                Symbol = "▲";
            }

            return MvcHtmlString.Create(String.Format("<th class=\"ui-widget {1}\"><a href=\"{0}\">{2}{3}</a></th>", Url, LinkClass, caption, Symbol));
        }
    }

    public static class QueryExtensions
    {
        private static readonly ILog _log = LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, string asc, string desc)
        {
            return SortBy<T>(query, asc, desc, "", false);
        }

        //public static IQueryable<T> SortBy<T>(this IQueryable<T> query, string asc, string desc, string defaultFieldName)
        //{
        //    return SortBy<T>(query, asc, desc, defaultFieldName, false);
        //}

        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, string asc, string desc, string defaultFieldName, bool defaultDescending = false)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            string propertyName;
            string methodName;
            if (!String.IsNullOrEmpty(asc) && asc.Length > 0)
            {
                propertyName = asc.Trim();
                methodName = "OrderBy";
            }

            else if (!String.IsNullOrEmpty(desc) && desc.Length > 0)
            {
                propertyName = desc.Trim();
                methodName = "OrderByDescending";
            }
            else if (!String.IsNullOrEmpty(defaultFieldName) && defaultFieldName.Length > 0)
            {
                propertyName = defaultFieldName.Trim();
                if (defaultDescending)
                    methodName = "OrderByDescending";
                else
                    methodName = "OrderBy";
            }
            else
                return query;

            if (String.IsNullOrEmpty(propertyName))
            {
                return query;
            }

            ParameterExpression parameter = Expression.Parameter(query.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);

            // jesli typem podanego pola jest data to odwracamy sortowanie
            if (property.Type.BaseType == (DateTime.Now).GetType().BaseType)
            {
                if (methodName == "OrderBy")
                    methodName = "OrderByDescending";
                else
                    methodName = "OrderBy";
            }

            LambdaExpression lambda = Expression.Lambda(property, parameter);

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { query.ElementType, property.Type },
                                                query.Expression, Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}