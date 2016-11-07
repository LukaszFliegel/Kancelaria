using Kancelaria.Globals;
using Kancelaria.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Kancelaria.Repositories.Interfaces;

namespace Kancelaria.Repositories
{
    public class KancelariaRepository : IKancelariaRepository
    {
        protected KancelariaDataContext db = new KancelariaDataContext();

        protected static readonly ILog Logger = LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public System.Data.Linq.Table<Uzytkownik> Uzytkownicy
        {
            get { return db.Uzytkowniks; }
        }

        public void DodajUzytkownika(Uzytkownik uzytkownik)
        {
            db.Uzytkowniks.InsertOnSubmit(uzytkownik);
        }

        public Uzytkownik Uzytkownik(int userId)
        {
            return db.Uzytkowniks.Where(p => p.UserId == userId).SingleOrDefault();
        }

        public Uzytkownik Uzytkownik(string userName)
        {
            return db.Uzytkowniks.Where(p => p.UserName == userName).SingleOrDefault();
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}