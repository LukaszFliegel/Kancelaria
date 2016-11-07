using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface ILataObrotoweRepository : IKancelariaRepository
    {
        int Count(int idFirmy);
        void Dodaj(RokObrotowy rokObrotowy);
        PagedSearchedQueryResult<RokObrotowy> LataObrotowe(int idFirmy, int page);
        PagedSearchedQueryResult<RokObrotowy> LataObrotowe(int idFirmy, int page, string search, string asc, string desc);
        RokObrotowy RokObrotowy(int id);
        void Usun(RokObrotowy rokObrotowy);
        void WybierzIdRoku(int idRoku, string userName);
        int? WybraneIdRoku(string userName);
        int? WybraneIdRokuOrExcepion(string userName);
    }
}