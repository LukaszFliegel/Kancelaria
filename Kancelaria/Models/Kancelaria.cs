using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kancelaria.Globals;
using System.Data.Linq;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Kancelaria.Models
{
    public abstract class ModelEntityClass
    {
        public bool IsValid
        {
            get
            {
                return (GetRuleViolations().Count() == 0);
            }
        }

        public virtual IEnumerable<RuleViolation> GetRuleViolations() { yield break; }
    }

    partial class SposobPlatnosci : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(KodSposobuPlatnosci))
                yield return new RuleViolation("Kod sposobu p³atnoœci nie mo¿e byæ pusty", "KodSposobuPlatnosci");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    partial class JednostkaMiary : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(KodJednostkiMiary))
                yield return new RuleViolation("Kod jednostki miary nie mo¿e byæ pusty", "KodJednostkiMiary");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    partial class TypInwestycji : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(KodTypuInwestycji))
                yield return new RuleViolation("Kod typu inwestycji nie mo¿e byæ pusty", "KodTypuInwestycji");

            if (String.IsNullOrEmpty(NazwaTypuInwestycji))
                yield return new RuleViolation("Nazwa typu inwestycji nie mo¿e byæ pusta", "NazwaTypuInwestycji");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    partial class Inwestycja : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations() 
        {
            if (String.IsNullOrEmpty(NumerInwestycji))
                yield return new RuleViolation("Numer inwestycji nie mo¿e byæ pusty", "NumerInwestycji");

            //if (String.IsNullOrEmpty(NumerUmowy))
            //    yield return new RuleViolation("Numer umowy nie mo¿e byæ pusty", "NumerUmowy"); 

            yield break; 
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    partial class Kompensata : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(NumerKompensaty))
                yield return new RuleViolation("Numer kompensaty nie mo¿e byæ pusty", "NumerInwestycji");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    partial class KompensataPowiazanie : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if(Kompensata == null && IdKompensaty <= 0)
                yield return new RuleViolation("Pozycja kompensaty musi byæ przypisana do kompensaty", "IdKompensaty");

            if ((!IdFakturySprzedazy.HasValue) && (!IdFakturyZakupu.HasValue))
                yield return new RuleViolation("Pozycja kompensaty musi dotyczyæ faktury sprzeda¿y b¹dŸ faktury zakupu");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    partial class FakturaZakupu : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(NumerFaktury))
                yield return new RuleViolation("Numer faktury nie mo¿e byæ pusty", "NumerFaktury");

            if((this.IdSposobuPlatnosci == 0))
                yield return new RuleViolation("Sposób p³atnoœci nie mo¿e byæ pusty", "IdSposobuPlatnosci");

            if(this.NumerFaktury.Length > 50)
                yield return new RuleViolation("D³ugoœæ numeru faktury nie mo¿e przekraczaæ 50 znaków", "NumerFaktury");

            if (this.WlasnyNumerFaktury.Length > 50)
                yield return new RuleViolation("D³ugoœæ w³asnego numeru faktury nie mo¿e przekraczaæ 50 znaków", "WlasnyNumerFaktury");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }

        public decimal KwotaZaplacona
        {
            get
            {
                return this.ZaplataFakturyZakupus.Sum(p => p.Kwota);
            }
        }

        public decimal KwotaNiezaplacona
        {
            get
            {
                //decimal KwotaNiezaplacona = this.KwotaBrutto - (this.KwotaZaplacona ?? 0);
                decimal KwotaNiezaplacona = this.KwotaBrutto - this.ZaplataFakturyZakupus.Sum(p => p.Kwota);

                foreach (var a in this.KompensataPowiazanies)
                {
                    KwotaNiezaplacona -= a.Kwota;
                }

                return KwotaNiezaplacona;
            }
        }

        public decimal KwotaZaplaconaBadzSkompensowana
        {
            get
            {
                decimal KwotaZaplacona = this.KwotaZaplacona;

                foreach (var a in this.KompensataPowiazanies)
                {
                    KwotaZaplacona += a.Kwota;
                }

                return KwotaZaplacona;
            }
        }

        public decimal KwotaBrutto
        {
            get { 
                    return this.PozycjaFakturyZakupus.Sum(
                        p => decimal.Round(p.KwotaBrutto, 2)
                    ); 
            }
        }

        public decimal KwotaNetto
        {
            get
            {
                return this.PozycjaFakturyZakupus.Sum(
                    p => decimal.Round(p.KwotaNetto, 2)
                );
            }
        }

        public decimal KwotaVat
        {
            get { return KwotaBrutto - KwotaNetto; }
        }
    }

    partial class PozycjaFakturyZakupu : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (FakturaZakupu == null && IdFaktury <= 0)
                yield return new RuleViolation("Pozycja faktury musi byæ przypisana do faktury", "IdFaktury");

            if (Inwestycja == null && IdInwestycji <= 0)
                yield return new RuleViolation("Inwetycja nie mo¿e byæ pusta", "IdInwestycji");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }

        public decimal KwotaBrutto
        {
            get { 
                return CzyBrutto ? 
                (CenaJednostkowa * Ilosc) : 
                (CenaJednostkowa * Ilosc) + (CenaJednostkowa * Ilosc) * StawkaVat / 100; 
            }
        }

        public decimal KwotaNetto
        {
            // B = N + N*S/100 => N = B/(1 + S/100)
            get { 
                return CzyBrutto ? 
                KwotaBrutto / (1 + new Decimal(StawkaVat) / 100) : 
                (CenaJednostkowa * Ilosc); 
            }
        }

        public decimal KwotaVat
        {
            //get { return (CenaJednostkowa * Ilosc) * StawkaVat / 100; }
            get { return KwotaBrutto - KwotaNetto; }
        }
    }

    partial class ZaplataFakturyZakupu : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (FakturaZakupu == null && IdFakturyZakupu <= 0)
                yield return new RuleViolation("Zap³ata faktury musi byæ przypisana do faktury", "IdFakturyZakupu");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    partial class FakturaSprzedazy : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(NumerFaktury))
                yield return new RuleViolation("Numer faktury nie mo¿e byæ pusty", "NumerFaktury");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }

        //public string KodKontrahenta
        //{
        //    get { return this.Kontrahent.KodKontrahenta; }
        //}

        //public string NumerInwestycji
        //{
        //    get { return this.Inwestycja.NumerInwestycji; }
        //}

        public decimal KwotaZaplacona
        {
            get
            {
                return this.ZaplataFakturySprzedazies.Sum(p => p.Kwota);
            }
        }

        public decimal KwotaNiezaplacona
        {
            get
            {
                //decimal KwotaNiezaplacona = this.KwotaBrutto - (this.KwotaZaplacona ?? 0);
                decimal KwotaNiezaplacona = this.KwotaBrutto - this.ZaplataFakturySprzedazies.Sum(p => p.Kwota);

                foreach (var a in this.KompensataPowiazanies)
                {
                    KwotaNiezaplacona -= a.Kwota;
                }

                return KwotaNiezaplacona;
            }
        }

        public decimal KwotaZaplaconaBadzSkompensowana
        {
            get
            {
                decimal KwotaZaplacona = this.KwotaZaplacona;

                foreach (var a in this.KompensataPowiazanies)
                {
                    KwotaZaplacona += a.Kwota;
                }

                return KwotaZaplacona;
            }
        }

        public decimal KwotaBrutto
        {
            get
            {
                return this.PozycjaFakturySprzedazies.Sum(
                    p => decimal.Round(p.KwotaBrutto, 2)
                );
            }
        }

        public decimal KwotaNetto
        {
            get
            {
                return this.PozycjaFakturySprzedazies.Sum(
                    p => decimal.Round(p.KwotaNetto, 2)
                );
            }
        }

        public decimal KwotaVat
        {
            get { return KwotaBrutto - KwotaNetto; }
        }

        //public decimal KwotaBrutto
        //{
        //    get { return this.PozycjaFakturySprzedazies.Sum(p => (p.CenaJednostkowa * p.Ilosc) + (p.CenaJednostkowa * p.Ilosc) * p.StawkaVat / 100); }
        //}

        //public decimal KwotaNetto
        //{
        //    get { return this.PozycjaFakturySprzedazies.Sum(p => (p.CenaJednostkowa * p.Ilosc)); }
        //}

        //public decimal KwotaVat
        //{
        //    get { return this.PozycjaFakturySprzedazies.Sum(p => (p.CenaJednostkowa * p.Ilosc) * p.StawkaVat / 100); }
        //}
    }

    partial class PozycjaFakturySprzedazy : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (FakturaSprzedazy == null && IdFaktury <= 0)
                yield return new RuleViolation("Pozycja faktury musi byæ przypisana do faktury", "IdFaktury");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }

        public decimal KwotaBrutto
        {
            get
            {
                return CzyBrutto ?
                (CenaJednostkowa * Ilosc) :
                (CenaJednostkowa * Ilosc) + (CenaJednostkowa * Ilosc) * StawkaVat / 100;
            }
        }

        public decimal KwotaNetto
        {
            // B = N + N*S/100 => N = B/(1 + S/100)
            get
            {
                return CzyBrutto ?
                KwotaBrutto / (1 + new Decimal(StawkaVat) / 100) :
                (CenaJednostkowa * Ilosc);
            }
        }

        public decimal KwotaVat
        {
            //get { return (CenaJednostkowa * Ilosc) * StawkaVat / 100; }
            get { return KwotaBrutto - KwotaNetto; }
        }

        //public decimal KwotaBrutto
        //{
        //    get { return (CenaJednostkowa * Ilosc) + (CenaJednostkowa * Ilosc) * StawkaVat / 100; }
        //}

        //public decimal KwotaNetto
        //{
        //    get { return (CenaJednostkowa * Ilosc); }
        //}

        //public decimal KwotaVat
        //{
        //    get { return (CenaJednostkowa * Ilosc) * StawkaVat / 100; }
        //}
    }

    partial class ZaplataFakturySprzedazy : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (FakturaSprzedazy == null && IdFakturySprzedazy <= 0)
                yield return new RuleViolation("Zap³ata faktury musi byæ przypisana do faktury", "IdFakturySprzedazy");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    partial class KontoBankowe : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            //if (this.LiczbaKontrolna == 0)
            //    yield return new RuleViolation("Liczba kontrolna nie mo¿e byæ pusta", "LiczbaKontrolna");

            //if (this.WyroznikKonta == 0)
            //    yield return new RuleViolation("Wyroznik konta nie mo¿e byæ pusty", "WyroznikKonta");

            //if (this.NumerKonta == 0)
            //    yield return new RuleViolation("Numer konta nie mo¿e byæ pusty", "NumerKonta");

            if (String.IsNullOrEmpty(NumerKonta))
                    yield return new RuleViolation("Numer konta nie mo¿e byæ pusty", "NumerKonta");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    [MetadataType(typeof(KontrahentMetadata))]
    partial class Kontrahent : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(KodKontrahenta))
                yield return new RuleViolation("Kod kontrahenta nie mo¿e byæ pusty", "KodKontrahenta");

            if (String.IsNullOrEmpty(NazwaKontrahenta))
                yield return new RuleViolation("Nazwa kontrahenta nie mo¿e byæ pusta", "NazwaKontrahenta");

            if (!String.IsNullOrEmpty(KodPocztowy) && !KodValidator.IsValidNumber(KodPocztowy, "PL"))
                yield return new RuleViolation("Niepoprawny kod pocztowy", "KodPocztowy");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }

        public Kontrahent KontrahentNadrzedny
        {
            get { return this.Kontrahenci1; }
        }
    }

    partial class Firma : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(NazwaSkrocona))
                yield return new RuleViolation("Nazwa skrócona nie mo¿e byæ pusta", "NazwaSkrocona");

            if (String.IsNullOrEmpty(NazwaPelna))
                yield return new RuleViolation("Nazwa pe³na nie mo¿e byæ pusta", "NazwaPelna");

            if (!String.IsNullOrEmpty(KodPocztowy) && !KodValidator.IsValidNumber(KodPocztowy, "PL"))
                yield return new RuleViolation("Niepoprawny kod pocztowy", "KodPocztowy");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    partial class RokObrotowy : ModelEntityClass
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(NazwaRoku))
                yield return new RuleViolation("Nazwa roku nie mo¿e byæ pusta", "NazwaRoku");            

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Zapisz niemo¿liwy - nie wszystkie warunki walidacyjne spe³nione");
        }
    }

    /* Validator help classes */

    public class KodValidator
    {
        static IDictionary<string, Regex> kodRegex =
            new Dictionary<string, Regex>() {
                { "PL", new Regex("[0-9][0-9]-[0-9][0-9][0-9]")},
                };

        public static bool IsValidNumber(string kodPocztowy, string country)
        {
            if (country != null && kodRegex.ContainsKey(country))
                return kodRegex[country].IsMatch(kodPocztowy);
            else
                return false;
        }

        public static IEnumerable<string> Countries
        {
            get
            {
                return kodRegex.Keys;
            }
        }
    }
}
