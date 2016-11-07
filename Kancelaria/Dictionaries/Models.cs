using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Dictionaries
{
    public class AppliedFiltersModel
    {
        public List<string> List;
        public string Name;

        public AppliedFiltersModel(List<string> list, string name)
        {
            List = list;
            Name = name;
        }
    }

    public enum DateTimeSearchControlType
    {
        None,
        DateRange,
        OneDate
    }

    public class FilterBoxModel
    {
        public string ControlName;
        public string FilterActionName;
        public StringDictionary Dictionary;
        public List<KeyValuePair<string, object>> RouteValues;
        public DateTimeSearchControlType ShowDatesFilter;
        public DateTime? DateFrom;
        public DateTime? DateTo;

        public FilterBoxModel(string controlName, string filterActionName, StringDictionary dictionary)
        {
            ControlName = controlName;
            FilterActionName = filterActionName;
            Dictionary = dictionary;
            RouteValues = new List<KeyValuePair<string, object>>();
            DateFrom = null;
            DateTo = null;
            ShowDatesFilter = DateTimeSearchControlType.None;
        }

        public FilterBoxModel(string controlName, string filterActionName, StringDictionary dictionary, DateTimeSearchControlType showDatesFilter)
            : this(controlName, filterActionName, dictionary)
        {
            ShowDatesFilter = showDatesFilter;
        }

        public FilterBoxModel(string controlName, string filterActionName, StringDictionary dictionary, KeyValuePair<string, object> routeValues)
            :this(controlName, filterActionName, dictionary)
        {
            // TODO: pass whole list throught constructor
            RouteValues.Add(routeValues);
        }

        public FilterBoxModel(string controlName, string filterActionName, StringDictionary dictionary, DateTime dateFrom, DateTime dateTo)
            : this(controlName, filterActionName, dictionary, DateTimeSearchControlType.None)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
    }

    public class FilterBoxToggleButtonModel
    {
        public string ControlName;

        public FilterBoxToggleButtonModel(string controlName)
        {
            ControlName = controlName;
        }
    }

    public class SortCaptionModel
    {
        public string Caption;
        public string FieldName;
        public string Url;

        public SortCaptionModel(string caption, string fieldName, string url)
        {
            Caption = caption;
            FieldName = fieldName;
            Url = url;
        }
    }
}