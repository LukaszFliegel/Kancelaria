using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IJednostkiMiaryRepository : IKancelariaRepository
    {
        int Count();
        void Dodaj(JednostkaMiary inwestycja);
        int GetDefaultId();
        JednostkaMiary JednostkaMiary(int id);
        void SetDefault(int id);
        IQueryable<JednostkaMiary> SposobyPlatnosci();
        PagedSearchedQueryResult<JednostkaMiary> SposobyPlatnosci(int page);
        PagedSearchedQueryResult<JednostkaMiary> SposobyPlatnosci(int page, string search, string asc, string desc);
        void Usun(JednostkaMiary inwestycja);
    }
}