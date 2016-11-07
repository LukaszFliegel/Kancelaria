using Kancelaria.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Models.ViewModels
{
    public class WyborRokuObrotowegoModel
    {
        public GridSettings<RokObrotowy> GridSettings;
        public string ReturnUrl;

        public WyborRokuObrotowegoModel(GridSettings<RokObrotowy> gridSettings, string returnUrl)
        {
            GridSettings = gridSettings;
            ReturnUrl = returnUrl;
        }
    }
}