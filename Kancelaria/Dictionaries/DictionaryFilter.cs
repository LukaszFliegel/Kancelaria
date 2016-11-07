using Kancelaria.Globals;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Kancelaria.Dictionaries
{
    // abstrakcyjna klasa sluzaca do definiowania filtrow (~wzorzec polecenie)
    public abstract class DictionaryFilter
    {
        // slowo kluczowe po ktorym rozpoznawany jest filtr
        public string SearchKey;
        // opis do czego sluzy filtr (do zaprezentowania uzytkownikowi na UI)
        public string Description;
        // flaga ustawiany gdy filtr zostal zaaplikowany (odnaleziony w searchQuery i wywolany)
        public bool Executed;
        // opis jaki ma zostac zwrocony uzytkownikowi po zaaplikowaniu filtra
        public string ExecutedCaption;

        //protected static ILog _log = LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static ILog _log = LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        // moteda sprawdzajaca czy w podanym searchQuery ma zasosowanie filtr
        public abstract bool CheckStringForBeingApplied(string searchQuery);

        // constructor
        public DictionaryFilter()
        {
            Executed = false;
        }


    }

    public abstract class QueryDictionaryFilter<T> : DictionaryFilter
    {
        // metoda abstrakcyjna - wywolanie filtra (w klasach potomnych ma nalozyc filtr na query)
        public abstract void ExecuteFilter(ref IQueryable<T> query, ref string searchQuery);

        #region operatory
        // operator przypisania - sprawdzamy jedynei fieldname - aby mozna w List<SearchQueryDictionaryType> .Contains() sprawdzac
        public static bool operator ==(QueryDictionaryFilter<T> x, QueryDictionaryFilter<T> y)
        {
            return x.SearchKey == y.SearchKey;
        }

        public static bool operator !=(QueryDictionaryFilter<T> x, QueryDictionaryFilter<T> y)
        {
            return x.SearchKey != y.SearchKey;
        }

        public override bool Equals(object o)
        {
            try
            {
                return this == (QueryDictionaryFilter<T>)o;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return SearchKey.GetHashCode();
        }
        #endregion operatory
    }

    public class DictionaryFieldFilter<T> : QueryDictionaryFilter<T>
    {
        public string FieldName;
        // wartosc filtra wprowadzona przez usera
        public string SearchValue;

        protected string Delimiter = "=";

        public override bool CheckStringForBeingApplied(string searchQuery)
        {
            string fieldName = "";
            string value = "";

            return CheckStringForBeingApplied(searchQuery, out fieldName, out value);
        }

        // moteda sprawdzajaca czy w podanym searchQuery ma zasosowanie filtr, jesli tak to podaje ktorego pola i wartosci filtr dotyczy
        public bool CheckStringForBeingApplied(string searchQuery, out string fieldName, out string value)
        {
            fieldName = "";
            value = "";
            if (!(searchQuery.Length > 0)) return false;

            string[] words = searchQuery.Trim().Split(new char[] { ' ' });

            foreach (string s in words)
            {
                if (s.Trim().ToUpper().StartsWith(SearchKey.ToUpper() + Delimiter))
                {
                    foreach (var prop in (typeof(T)).GetProperties())
                    {
                        if (prop.Name == FieldName)
                        {
                            value = s.Trim().Remove(0, (SearchKey + Delimiter).Length);
                            fieldName = FieldName;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override void ExecuteFilter(ref IQueryable<T> query, ref string searchQuery)
        {
            string Field = "";
            //string Value = "";
            if (CheckStringForBeingApplied(searchQuery, out Field, out SearchValue))
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                Expression property = Expression.Property(parameter, Field);
                Expression target = Expression.Constant(SearchValue/*.ToUpper()*/, typeof(string));
                MethodInfo mi = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                Expression equalsMethod = Expression.Call(property, mi, target);
                Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(equalsMethod, parameter);
                query = query.Where(lambda);
                Executed = true;

                int iStart = searchQuery.ToUpper().IndexOf(SearchKey.ToUpper() + Delimiter + SearchValue.ToUpper());
                int iLen = (SearchKey + Delimiter + SearchValue).Length;
                searchQuery = searchQuery.Remove(iStart, iLen).Trim();
            }
        }

        #region operatory
        // operator przypisania - sprawdzamy jedynei fieldname - aby mozna w List<SearchQueryDictionaryType> .Contains() sprawdzac
        public static bool operator ==(DictionaryFieldFilter<T> x, DictionaryFieldFilter<T> y)
        {
            return x.FieldName == y.FieldName;
        }

        public static bool operator !=(DictionaryFieldFilter<T> x, DictionaryFieldFilter<T> y)
        {
            return x.FieldName != y.FieldName;
        }

        public override bool Equals(object o)
        {
            try
            {
                return this == (DictionaryFieldFilter<T>)o;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return FieldName.GetHashCode();
        }
        #endregion operatory
    }
}