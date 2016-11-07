using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface ITypyInwestycjiRepository: IKancelariaRepository
    {
        int Count();
        void Dodaj(TypInwestycji inwestycja);
        int? GetDefaultId();
        void SetDefault(int id);
        TypInwestycji TypInwestycji(int id);
        IQueryable<TypInwestycji> TypyInwestycji();
        PagedSearchedQueryResult<TypInwestycji> TypyInwestycji(int page);
        void Usun(TypInwestycji inwestycja);
    }
}