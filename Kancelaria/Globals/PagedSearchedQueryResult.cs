using Kancelaria.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Globals
{
    public class PagedSearchedQueryResult<T> : IPaged, ISearched
    {
        public IQueryable<T> Result;
        protected int CurrentPage;
        protected int PageSize;
        protected int PagesCount;
        protected int TotalRows;
        protected string SearchString;

        public PagedSearchedQueryResult(IQueryable<T> result, int currentPage, int pageSize, string searchString = "")
        {
            if (result != null)
            {
                TotalRows = result.Count();

                if (pageSize > 0)
                {
                    PagesCount = (TotalRows - 1) / pageSize;
                    Result = result.Skip(currentPage * pageSize).Take(pageSize);
                }
                else
                {
                    PagesCount = 1;
                    Result = result;
                }
            }
            CurrentPage = currentPage;
            PageSize = pageSize;
            SearchString = searchString;
        }

        public PagedSearchedQueryResult(IQueryable<T> result, int currentPage, string searchString = "")
            : this(result, currentPage, KancelariaSettings.PageSize, searchString)
        {

        }

        //public PagedSearchedQueryResult(IQueryable<T> result, int currentPage, int pageSize, int totalPages, int totalRows, string searchString = "")
        //{
        //    TotalPages = totalPages;
        //    TotalRows = totalRows;
        //    if (result != null)
        //    {
        //        Result = result.Skip(currentPage * pageSize).Take(pageSize);
        //    }
        //    CurrentPage = currentPage;
        //    PageSize = pageSize;
        //}

        //public PagedSearchedQueryResult(PagedSearchedQueryResult<T> pagedQuery)
        //{
        //    Result = pagedQuery.Result;
        //    CurrentPage = pagedQuery.CurrentPage;
        //    PageSize = pagedQuery.PageSize;
        //    TotalPages = pagedQuery.TotalPages;
        //    TotalRows = pagedQuery.TotalRows;
        //    SearchString = pagedQuery.SearchString;
        //}

        ///// <summary>
        ///// Statyczna metoda tworząca PagedResult&lt;X&gt; na podstawie listy (nieobciętej).
        ///// Uwaga! Lista wędrująca jako parametr jest obcinana do rozmiaru strony, 
        ///// HasMoreRows jest ustawiane na true, jeżeli lista miała wiącej wierszy niż rozmiar strony
        ///// </summary>
        ///// <typeparam name="X"></typeparam>
        ///// <param name="list"></param>
        ///// <param name="maxRecords"></param>
        ///// <returns></returns>
        //public static PagedSearchedQueryResult<X> TruncateList<X>(List<X> list, int maxRecords)
        //{
        //    PagedSearchedQueryResult<X> pr = new PagedSearchedQueryResult<X>();
        //    pr.List = list;
        //    pr.HasMoreRows = DBUtils.ShowNextPagerPage<X>(pr.List, maxRecords);
        //    return pr;
        //}

        ///// <summary>
        ///// Statyczna metoda tworzaca PagedResult&lt;X&gt; na podstawie przychodzacego query.
        ///// Wynik zostaje obciety do podanej strony i podanego rozmiaru tej strony
        ///// </summary>
        ///// <typeparam name="X"></typeparam>
        ///// <param name="list"></param>
        ///// <param name="currentPage"></param>
        ///// <param name="pageSize"></param>
        ///// <returns></returns>
        //public static PagedSearchedQueryResult<X> SkipTakeQuery<X>(IQueryable<X> list, int currentPage, int pageSize)
        //{
        //    if (pageSize > 0)
        //    {
        //        list = list.Skip(currentPage * pageSize).Take(pageSize + 1);
        //        return PagedSearchedQueryResult<X>.TruncateList(list.ToList(), pageSize);
        //    }
        //    else
        //    {
        //        return new PagedSearchedQueryResult<X>(list.ToList(), false);
        //    }
        //}

        #region ISearched Interface

        public string GetSearchString()
        {
            return SearchString;
        }

        #endregion

        #region IPaged Interface

        public int GetCurrentPage()
        {
            return CurrentPage;
        }

        public int GetPageSize()
        {
            return PageSize;
        }

        public int GetPagesCount()
        {
            return PagesCount;
        }

        public int GetTotalRows()
        {
            return TotalRows;
        }

        #endregion
    }

    //public class PagedListResult<T> : IPaged
    //{
    //    public List<T> Result;
    //    protected int CurrentPage;
    //    protected int PageSize;
    //    protected int TotalPages;
    //    protected int TotalRows;

    //    public PagedListResult(List<T> result, int currentPage, int pageSize)
    //    {
    //        if (result != null)
    //        {
    //            TotalRows = result.Count();
    //            TotalPages = (TotalRows - 1) / pageSize;
    //            Result = result.Skip(currentPage * pageSize).Take(pageSize).ToList();
    //        }
    //        CurrentPage = currentPage;
    //        PageSize = pageSize;
    //    }

    //    public PagedListResult(List<T> result, int currentPage)
    //        : this(result, currentPage, KancelariaSettings.PageSize)
    //    {

    //    }

    //    public PagedListResult(List<T> result, int currentPage, int pageSize, int totalPages, int totalRows)
    //    {
    //        TotalPages = totalPages;
    //        TotalRows = totalRows;
    //        if (result != null)
    //        {
    //            Result = result.Skip(currentPage * pageSize).Take(pageSize).ToList();
    //        }
    //        CurrentPage = currentPage;
    //        PageSize = pageSize;
    //    }

    //    public PagedListResult(List<T> result, int currentPage, int pageSize, Func<T, bool> predicate)
    //        : this(result, currentPage, pageSize)
    //    {
    //        Result = Result.Where(predicate).ToList();
    //    }

    //    public PagedListResult(PagedListResult<T> pagedQuery)
    //    {
    //        Result = pagedQuery.Result;
    //        CurrentPage = pagedQuery.CurrentPage;
    //        PageSize = pagedQuery.PageSize;
    //        TotalPages = pagedQuery.TotalPages;
    //        TotalRows = pagedQuery.TotalRows;
    //    }

    //    #region IPaged Interface

    //    public int GetCurrentPage()
    //    {
    //        return CurrentPage;
    //    }

    //    public int GetPageSize()
    //    {
    //        return PageSize;
    //    }

    //    public int GetTotalPages()
    //    {
    //        return TotalPages;
    //    }

    //    public int GetTotalRows()
    //    {
    //        return TotalRows;
    //    }

    //    #endregion
    //}
}