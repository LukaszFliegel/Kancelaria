using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;

namespace Kancelaria.Models.ViewModels
{
    public class ReadOnlyAbleModel<T>
    {
        public T Model;
        public bool ReadOnly;
        public string DialogElementId;
        public string GridElementId;
        public bool DisplaySummary;

        public ReadOnlyAbleModel(T model, bool readOnly, string dialogElementId, string gridElementId)
        {
            Model = model;
            ReadOnly = readOnly;
            DialogElementId = dialogElementId;
            GridElementId = gridElementId;
            DisplaySummary = false;
        }

        public ReadOnlyAbleModel(T model, bool readOnly, string dialogElementId, string gridElementId, bool displaySummary)
            :this(model, readOnly, dialogElementId, gridElementId)
        {
            DisplaySummary = displaySummary;
        }
    }

    public class AjaxEditModel<T>
    {
        public T Model;
        public bool ReadOnly;
        public string Controller;
        public string Action;
        public string DialogElementId;
        public string GridElementId;
        public InsertionMode InsertionMode;

        public AjaxEditModel(T model, bool readOnly, string controller, string action, InsertionMode insertionMode, string dialogElementId, string gridElementId)
        {
            Model = model;
            ReadOnly = readOnly;
            Controller = controller;
            Action = action;
            InsertionMode = insertionMode;
            DialogElementId = dialogElementId;
            GridElementId = gridElementId;            
        }
    }

    public class AjaxCompanyEditModel<T> : AjaxEditModel<T>
    {
        public int IdFirmy;

        public AjaxCompanyEditModel(T model, bool readOnly, string controller, string action, InsertionMode insertionMode, string dialogElementId, string gridElementId, int idFirmy)
            : base(model, readOnly, controller, action, insertionMode, dialogElementId, gridElementId)
        {
            IdFirmy = idFirmy;
        }
    }

    public class AjaxContractorEditModel<T> : AjaxEditModel<T>
    {
        public int IdKontrahenta;

        public AjaxContractorEditModel(T model, bool readOnly, string controller, string action, InsertionMode insertionMode, string dialogElementId, string gridElementId, int idKontrahenta)
            : base(model, readOnly, controller, action, insertionMode, dialogElementId, gridElementId)
        {
            IdKontrahenta = idKontrahenta; ;
        }
    }

    public class AjaxCompanyContracotrEditModel<T> : AjaxCompanyEditModel<T>
    {
        public int IdKontrahenta;

        public AjaxCompanyContracotrEditModel(T model, bool readOnly, string controller, string action, InsertionMode insertionMode, string dialogElementId, string gridElementId, int idFirmy, int idKontrahenta)
            : base(model, readOnly, controller, action, insertionMode, dialogElementId, gridElementId, idFirmy)
        {
            IdKontrahenta = idKontrahenta;
        }
    }
}