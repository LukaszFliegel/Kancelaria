using Kancelaria.Globals;
using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Kancelaria.Repositories.Interfaces;

namespace Kancelaria.Repositories
{
    public class AdminRepository : KancelariaRepository, IAdminRepository
    {
        public IQueryable<UzytkownikMembership> UzytkownicyMembership()
        {
            return (from u in db.UzytkownikMemberships
                    select u).AsQueryable();
        }

        public PagedSearchedQueryResult<UzytkownikMembership> UzytkownicyMembership(int page)
        {
            var result = (from u in db.UzytkownikMemberships
                          select u).AsQueryable();

            return new PagedSearchedQueryResult<UzytkownikMembership>(result, page);
        }

        public void DodajRole(int userId, string roleName)
        {
            var Uzytkownik = (from u in db.Uzytkowniks
                              where u.UserId == userId
                              select u).FirstOrDefault();

            if(Uzytkownik != null)
                Roles.AddUserToRole(Uzytkownik.UserName, roleName);
        }

        public void ObierzRole(int userId, string roleName)
        {
            var Uzytkownik = (from u in db.Uzytkowniks
                              where u.UserId == userId
                              select u).FirstOrDefault();

            if(Uzytkownik != null)
                Roles.RemoveUserFromRole(Uzytkownik.UserName, roleName);
        }
    }
}