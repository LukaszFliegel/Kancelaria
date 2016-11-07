using Kancelaria.Globals;
using Kancelaria.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Models.ViewModels
{
    public class WyborFirmyModel
    {
        public GridSettings<Firma> GridSettings;
        public string ReturnUrl;

        public WyborFirmyModel(GridSettings<Firma> gridSettings, string returnUrl)
        {
            GridSettings = gridSettings;
            ReturnUrl = returnUrl;
        }
    }
}