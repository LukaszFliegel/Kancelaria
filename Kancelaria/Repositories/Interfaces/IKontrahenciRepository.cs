using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IKontrahenciRepository : IKancelariaRepository
    {
        int Count(int idFirmy);
        void Dodaj(Kontrahent kontrahent);
        int GetDefaultId(int idFirmy);
        IQueryable<Kontrahent> Kontrahenci(int idFirmy);
        PagedSearchedQueryResult<Kontrahent> Kontrahenci(int idFirmy, int page, string search);
        PagedSearchedQueryResult<Kontrahent> Kontrahenci(int idFirmy, int page, string search, string asc, string desc, int pageSize = 10);
        Kontrahent Kontrahent(int id);
        void SetDefault(int idFirmy, int id);
        void Usun(Kontrahent kontrahent);
    }
}