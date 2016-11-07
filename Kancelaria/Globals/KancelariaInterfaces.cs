using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Globals
{
    public interface ICompanyRequirable
    {
        int GetCompanyId();
    }

    public interface IPaged
    {
        int GetCurrentPage();
        int GetPageSize();
        int GetPagesCount();
        int GetTotalRows();
    }

    public interface ISearched
    {
        string GetSearchString();
    }

    public interface IIsPaged
    {
        bool RenderDefaultPaged();
        IPaged GetPagedList();
    }
}