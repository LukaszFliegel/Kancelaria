using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IFirmyRepository : IKancelariaRepository
    {
        int Count();
        void Dodaj(Firma firma);
        Firma Firma(int id);
        PagedSearchedQueryResult<Firma> Firmy(int page);
        PagedSearchedQueryResult<Firma> Firmy(int page, string search, string asc, string desc);
        void Usun(Firma firma);
        void WybierzIdFirmy(int idFirmy, string userName);
        int? WybraneIdFirmy(string userName);
    }
}