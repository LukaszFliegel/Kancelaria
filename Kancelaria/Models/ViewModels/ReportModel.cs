using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Models.ViewModels
{
    public class ReportModel
    {
        public string Title { get; set; }
        public List<string> SubTitleList { get; set; }

        public ReportModel(string title, List<string> subTitleList)
        {
            Title = title;
            SubTitleList = subTitleList;
        }

        public ReportModel(string title, string subTitle)
        {
            Title = title;
            SubTitleList = new List<string>();
            SubTitleList.Add(subTitle);
        }

        public void AddSubTitle(string subTitle)
        {
            if (SubTitleList != null)
                SubTitleList.Add(subTitle);
        }
    }

    public class ReportModel<T> : ReportModel
    {
        public T Model { get; set; }
        
        public ReportModel(T model, string title, string subTitle)
            :base(title, subTitle)
        {
            Model = model;
        }
    }
}