using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Models.ViewModels
{
    public class MenuModel
    {
        public bool IsOneFirm { get; set; }

        public MenuModel(bool isOneFirm)
        {
            IsOneFirm = isOneFirm;
        }
    }
}