using System.Data.Linq;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IKancelariaRepository
    {
        Table<Uzytkownik> Uzytkownicy { get; }

        void DodajUzytkownika(Uzytkownik uzytkownik);
        void Save();
        Uzytkownik Uzytkownik(string userName);
        Uzytkownik Uzytkownik(int userId);
    }
}