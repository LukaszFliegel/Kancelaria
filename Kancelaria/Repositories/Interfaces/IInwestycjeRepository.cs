using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IInwestycjeRepository : IKancelariaRepository
    {
        int Count(int idFirmy);
        void Dodaj(Inwestycja inwestycja);
        int GetDefaultId(int idFirmy);
        Inwestycja Inwestycja(int id);
        IQueryable<Inwestycja> Inwestycje(int idFirmy);
        PagedSearchedQueryResult<Inwestycja> Inwestycje(int idFirmy, int page, string search);
        PagedSearchedQueryResult<Inwestycja> Inwestycje(int idFirmy, int page, string search, string asc, string desc, int pageSize = 10);
        void SetDefault(int idFirmy, int id);
        void Usun(Inwestycja inwestycja);
    }
}