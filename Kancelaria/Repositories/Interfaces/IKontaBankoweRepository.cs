using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IKontaBankoweRepository : IKancelariaRepository
    {
        int Count(int idFirmy);
        void Dodaj(KontoBankowe kontoBankowe);
        int GetDefaultId(int idFirmy);
        IQueryable<KontoBankowe> KontaBankowe(int idFirmy);
        PagedSearchedQueryResult<KontoBankowe> KontaBankowe(int idFirmy, int page);
        PagedSearchedQueryResult<KontoBankowe> KontaBankowe(int idFirmy, int page, string search, string asc, string desc);
        KontoBankowe KontoBankowe(int id);
        void SetDefault(int idFirmy, int id);
        void Usun(KontoBankowe kontoBankowe);
    }
}