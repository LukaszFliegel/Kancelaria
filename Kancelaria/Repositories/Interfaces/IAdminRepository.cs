using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IAdminRepository : IKancelariaRepository
    {
        void DodajRole(int userId, string roleName);
        void ObierzRole(int userId, string roleName);
        IQueryable<UzytkownikMembership> UzytkownicyMembership();
        PagedSearchedQueryResult<UzytkownikMembership> UzytkownicyMembership(int page);
    }
}