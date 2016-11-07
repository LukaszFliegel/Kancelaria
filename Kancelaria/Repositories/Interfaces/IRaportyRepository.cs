using System;
using System.Linq;
using Kancelaria.Models;

namespace Kancelaria.Repositories.Interfaces
{
    public interface IRaportyRepository : IKancelariaRepository
    {
        IQueryable<KosztNaInwestycjach> AnalizaKosztowNaInwestycjach(int idFirmy, int? idKontrahenta, int? idTypuInwestycji, int page, string search, string asc, string desc, int pageSize = 10, DateTime? dateFrom = default(DateTime?), DateTime? dateTo = default(DateTime?));
        IQueryable<AnalizaRozrachunku> AnalizaRazrachunkow(int idFirmy, int idRoku);
    }
}