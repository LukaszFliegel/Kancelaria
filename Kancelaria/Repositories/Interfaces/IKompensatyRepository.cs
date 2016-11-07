using System.Linq;
using Kancelaria.Globals;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IKompensatyRepository : IKancelariaRepository
    {
        int Count(int idFirmy);
        void Dodaj(Kompensata kompensata);
        void DodajPozycjeKompensaty(KompensataPowiazanie kompensataPowiazanie);
        Kompensata Kompensata(int id);
        KompensataPowiazanie KompensataPowiazanie(int id);
        IQueryable<Kompensata> Kompensaty(int idFirmy, int idRoku);
        PagedSearchedQueryResult<Kompensata> Kompensaty(int idFirmy, int idRoku, int page);
        PagedSearchedQueryResult<Kompensata> Kompensaty(int idFirmy, int idRoku, int page, string search, string asc, string desc);
        void Usun(Kompensata kompensata);
        void UsunPowiazanieKompensaty(KompensataPowiazanie kompensataPowiazanie);
    }
}