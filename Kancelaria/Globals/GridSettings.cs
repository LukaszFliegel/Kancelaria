using Omu.Awesome.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Globals
{
    public class GridSettings<T> //: Pageable<T>
    {
        //public GridSettings(PagedSearchedQueryResult<T> model, int? page)
        //{
        //    int Count = model.Result.Count();
        //    Page = model.Result.Skip((page ?? 0) * KancelariaSettings.PageSize).Take(KancelariaSettings.PageSize);
        //    PageCount = (int)Decimal.Ceiling((decimal)Count / (decimal)KancelariaSettings.PageSize);

        //    PageIndex = page ?? 0;
        //}

        public int PageCount;
        public int CurrentPageIndex;

        //public bool IsAddEnabled;
        //public bool IsEditEnabled;
        //public bool IsDeleteEnabled;
        //public bool IsPrintEnabled;

        public PagedSearchedQueryResult<T> Query;

        //public List<GridColumn> ColumnList;

        public GridSettings(PagedSearchedQueryResult<T> quertResult/*, bool isAddEnabled = true, bool isEditEnabled = true, bool isDeleteEnabled = true, bool isPrintEnabled = false*/)
        {
            int Count = quertResult.GetTotalRows();
            Query = quertResult;
            PageCount = quertResult.GetPagesCount();

            CurrentPageIndex = quertResult.GetCurrentPage();

            //IsAddEnabled = isAddEnabled;
            //IsEditEnabled = isEditEnabled;
            //IsDeleteEnabled = isDeleteEnabled;
            //IsPrintEnabled = isPrintEnabled;

            //ColumnList = new List<GridColumn>();
        }

        //public void AddColumn(string caption, string dBFieldName, bool isSortable = true)
        //{
        //    ColumnList.Add(new GridColumn(caption, dBFieldName, isSortable));
        //}
    }

    //public class GridColumn
    //{
    //    public string Caption;
    //    public string DBFieldName;

    //    public bool IsSortable;

    //    public GridColumn(string caption, string dBFieldName, bool isSoratble = true)
    //    {
    //        Caption = caption;
    //        DBFieldName = dBFieldName;
    //        IsSortable = isSoratble;
    //    }
    //}
}