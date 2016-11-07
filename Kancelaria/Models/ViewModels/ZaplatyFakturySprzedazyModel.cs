using Kancelaria.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Models.ViewModels
{
    public class ZaplatyFakturySprzedazyModel
    {
        public GridSettings<ZaplataFakturySprzedazy> GridSettings { get; private set; }
        public FakturaSprzedazy FakturaSprzedazy { get; private set; }

        public ZaplatyFakturySprzedazyModel(FakturaSprzedazy fakturaSprzedazy, GridSettings<ZaplataFakturySprzedazy> gridSettings)
        {
            GridSettings = gridSettings;
            FakturaSprzedazy = fakturaSprzedazy;
        }
    }
}