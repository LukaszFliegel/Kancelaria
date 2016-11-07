using Kancelaria.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Models.ViewModels
{
    public class ZaplatyFakturyZakupuModel
    {
        public GridSettings<ZaplataFakturyZakupu> GridSettings { get; private set; }
        public FakturaZakupu FakturaZakupu { get; private set; }

        public ZaplatyFakturyZakupuModel(FakturaZakupu fakturaZakupu, GridSettings<ZaplataFakturyZakupu> gridSettings)
        {
            GridSettings = gridSettings;
            FakturaZakupu = fakturaZakupu;
        }
    }
}