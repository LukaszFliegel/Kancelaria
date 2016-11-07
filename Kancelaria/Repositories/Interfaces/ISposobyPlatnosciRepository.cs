using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface ISposobyPlatnosciRepository : IKancelariaRepository
    {
        int Count();
        void Dodaj(SposobPlatnosci inwestycja);
        int GetDefaultId();
        void SetDefault(int id);
        SposobPlatnosci SposobPlatnosci(int id);
        IQueryable<SposobPlatnosci> SposobyPlatnosci();
        PagedSearchedQueryResult<SposobPlatnosci> SposobyPlatnosci(int page);
        PagedSearchedQueryResult<SposobPlatnosci> SposobyPlatnosci(int page, string search, string asc, string desc);
        void Usun(SposobPlatnosci inwestycja);
    }
}