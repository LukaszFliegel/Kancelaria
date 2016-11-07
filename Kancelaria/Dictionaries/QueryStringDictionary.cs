using Kancelaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kancelaria.Dictionaries
{
    public interface IFilterMarkable
    {
        List<string> GetAppliedFilterNames(string searchQuery);
    }

    // abstrakcyjna klasa po ktorej dziedzicza wszystkie slowniki zdefiniowane w systemie
    public abstract class StringDictionary : IFilterMarkable
    {
        /// <summary>
        /// Metoda tworzaca slownik - dodaje do niego kolejne filtry
        /// </summary>
        protected abstract void CreateSearchDictionary();

        // lista filtrow
        protected List<DictionaryFilter> SearchDictionaryList;

        // lista filtrow w slowniku
        public List<DictionaryFilter> SearchDictionary
        {
            get
            {
                if (SearchDictionaryList == null)
                {
                    CreateSearchDictionary();
                }
                return SearchDictionaryList;
            }
        }

        /// <summary>
        /// Wirtualna metoda zwracajaca opisowa liste zaaplikowanych filtrow
        /// </summary>
        /// <param name="searchQuery">query do przeszukania filtrow</param>
        /// <returns>Lista opisow zaaplikowanych filtrow</returns>
        public virtual List<string> GetAppliedFilterNames(string searchQuery)
        {
            List<string> Result = new List<string>();

            foreach (DictionaryFilter filter in SearchDictionary)
            {
                if (filter.CheckStringForBeingApplied(searchQuery))
                {
                    Result.Add(filter.ExecutedCaption);
                }
            }

            return Result;
        }
    }

    public abstract class QueryStringDictionary<T> : StringDictionary
    {
        //protected static ILog _log = LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ParseDictionary(ref IQueryable<T> query, ref string searchQuery)
        {
            if (searchQuery.Length > 0)
                foreach (QueryDictionaryFilter<T> filter in SearchDictionary)
                {
                    filter.ExecuteFilter(ref query, ref searchQuery);
                }
        }
    }

    public class FakturyZakupuDictionary : QueryStringDictionary<FakturaZakupu>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<FakturaZakupu>() { FieldName = "NumerFaktury", SearchKey = "NumerFaktury", ExecutedCaption = "Numer faktury", Description = "Pozwala odfiltrować faktury zakupu po numerze" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<FakturaZakupu>() { FieldName = "WlasnyNumerFaktury", SearchKey = "WlasnyNumerFaktury", ExecutedCaption = "Własny numer faktury", Description = "Pozwala odfiltrować faktury zakupu po własnym numerze" });
        }
    }

    public class FakturySprzedazyDictionary : QueryStringDictionary<FakturaSprzedazy>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<FakturaSprzedazy>() { FieldName = "NumerFaktury", SearchKey = "NumerFaktury", ExecutedCaption = "Numer faktury", Description = "Pozwala odfiltrować faktury sprzedaży po numerze" });
        }
    }

    public class ZaplatyFakturZakupuDictionary : QueryStringDictionary<ZaplataFakturyZakupu>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<ZaplataFakturyZakupu>() { FieldName = "Opis", SearchKey = "Opis", ExecutedCaption = "Opis", Description = "Pozwala odfiltrować zapłaty faktur zakupu po opisie" });
        }
    }

    public class ZaplatyFakturSprzedazyDictionary : QueryStringDictionary<ZaplataFakturySprzedazy>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<ZaplataFakturySprzedazy>() { FieldName = "Opis", SearchKey = "Opis", ExecutedCaption = "Opis", Description = "Pozwala odfiltrować zapłaty faktur zakupu po opisie" });
        }
    }

    public class NieuregulowanaFakturySprzedazyDictionary : QueryStringDictionary<NieuregulowanaFakturaSprzedazy>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<NieuregulowanaFakturaSprzedazy>() { FieldName = "NumerFaktury", SearchKey = "NumerFaktury", ExecutedCaption = "Numer faktury", Description = "Pozwala odfiltrować faktury sprzedaży po numerze" });
        }
    }

    public class NieuregulowanaFakturyZakupuDictionary : QueryStringDictionary<NieuregulowanaFakturaZakupu>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<NieuregulowanaFakturaZakupu>() { FieldName = "NumerFaktury", SearchKey = "NumerFaktury", ExecutedCaption = "Numer faktury", Description = "Pozwala odfiltrować faktury zakupu po numerze" });
        }
    } 

    public class SposobyPlatnosciDictionary : QueryStringDictionary<SposobPlatnosci>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<SposobPlatnosci>() { FieldName = "KodSposobuPlatnosci", SearchKey = "KodSposobuPlatnosci", ExecutedCaption = "Kod", Description = "Pozwala odfiltrować sposoby płatności po kodzie" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<SposobPlatnosci>() { FieldName = "OpisSposobuPlatnosci", SearchKey = "OpisSposobuPlatnosci", ExecutedCaption = "Opis", Description = "Pozwala odfiltrować sposoby płatności po opisie" });
        }
    }

    public class InwestycjeDictionary : QueryStringDictionary<Inwestycja>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<Inwestycja>() { FieldName = "NumerInwestycji", SearchKey = "NumerInwestycji", ExecutedCaption = "Numer inwestycji", Description = "Pozwala odfiltrować inwestycje po numerze" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<Inwestycja>() { FieldName = "NumerUmowy", SearchKey = "NumerUmowy", ExecutedCaption = "Numer umowy", Description = "Pozwala odfiltrować inwestycje po numerze umowy" });
        }
    }

    public class KompensatyDictionary : QueryStringDictionary<Kompensata>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<Kompensata>() { FieldName = "NumerKompensaty", SearchKey = "NumerKompensaty", ExecutedCaption = "Numer kompensaty", Description = "Pozwala odfiltrować kompensaty po numerze" });
        }
    }

    public class KontrahenciDictionary : QueryStringDictionary<Kontrahent>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<Kontrahent>() { FieldName = "KodKontrahenta", SearchKey = "Kod", ExecutedCaption = "Kod", Description = "Pozwala odfiltrować kontrahentów po kodzie" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<Kontrahent>() { FieldName = "Miejscowosc", SearchKey = "Miejscowosc", ExecutedCaption = "Miejscowość", Description = "Pozwala odfiltrować kontrahentów po miejscowości" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<Kontrahent>() { FieldName = "NazwaKontrahenta", SearchKey = "Nazwa", ExecutedCaption = "Nazwa", Description = "Pozwala odfiltrować kontrahentów po nazwie" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<Kontrahent>() { FieldName = "Nip", SearchKey = "Nip", ExecutedCaption = "Nip", Description = "Pozwala odfiltrować kontrahentów po numerze NIP" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<Kontrahent>() { FieldName = "NumerDomu", SearchKey = "NrDomu", ExecutedCaption = "Numer domu", Description = "Pozwala odfiltrować kontrahentów po numerze domu" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<Kontrahent>() { FieldName = "NumerMieszkania", SearchKey = "NrMieszkania", ExecutedCaption = "Numer mieszkania", Description = "Pozwala odfiltrować kontrahentów po numerze mieszkania" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<Kontrahent>() { FieldName = "Panstwo", SearchKey = "Panstwo", ExecutedCaption = "Państwo", Description = "Pozwala odfiltrować kontrahentów po państwie" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<Kontrahent>() { FieldName = "Ulica", SearchKey = "Ulica", ExecutedCaption = "Ulica", Description = "Pozwala odfiltrować kontrahentów po ulicy" });
        }
    }

    public class LataObrotoweDictionary : QueryStringDictionary<RokObrotowy>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<RokObrotowy>() { FieldName = "NazwaRoku", SearchKey = "NazwaRoku", ExecutedCaption = "Nazwa roku", Description = "Pozwala odfiltrować lata obrotowe po nazwie" });
        }
    }

    public class FirmyDictionary : QueryStringDictionary<Firma>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<Firma>() { FieldName = "NazwaSkrocona", SearchKey = "NazwaSkrocona", ExecutedCaption = "Nazwa skrócona", Description = "Pozwala odfiltrować firmy po nazwie skróconej" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<Firma>() { FieldName = "NazwaPelna", SearchKey = "NazwaPelna", ExecutedCaption = "Nazwa pełna", Description = "Pozwala odfiltrować firmy po nazwie pełnej" });
        }
    }

    public class KontaBankoweDictionary : QueryStringDictionary<KontoBankowe>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            // brak filtrow dla tego slownika
        }
    }

    public class JednostkiMiaryDictionary : QueryStringDictionary<JednostkaMiary>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            SearchDictionaryList.Add(new DictionaryFieldFilter<JednostkaMiary>() { FieldName = "KodJednostkiMiary", SearchKey = "KodJednostkiMiary", ExecutedCaption = "Kod", Description = "Pozwala odfiltrować jednostki miary po kodzie" });
            SearchDictionaryList.Add(new DictionaryFieldFilter<JednostkaMiary>() { FieldName = "OpisJednostkiMiary", SearchKey = "OpisJednostkiMiary", ExecutedCaption = "Opis", Description = "Pozwala odfiltrować jednostki miary po opisie" });
        }
    }

    public class AnalizaRozrachunkowDictionary : QueryStringDictionary<AnalizaRozrachunku>
    {
        override protected void CreateSearchDictionary()
        {
            SearchDictionaryList = new List<DictionaryFilter>();

            // wypelnienie danych slownika
            // brak filtrow dla tego slownika
        }
    }
}