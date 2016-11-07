CREATE view [dbo].[PozycjeFakturZakupu_vw] as
select
	V.*,
	(V.Brutto - V.Netto) Vat
from
(
	select
		PFZ.*,
		CASE WHEN PFZ.CzyBrutto = 1 THEN PFZ.CenaJednostkowa * PFZ.Ilosc ELSE (PFZ.CenaJednostkowa * PFZ.Ilosc) + (PFZ.CenaJednostkowa * PFZ.Ilosc) * PFZ.StawkaVat / 100 END Brutto,
		CASE WHEN PFZ.CzyBrutto = 1 THEN (PFZ.CenaJednostkowa * PFZ.Ilosc) / (1 + PFZ.StawkaVat / 100) ELSE PFZ.CenaJednostkowa * PFZ.Ilosc END Netto
	from
		PozycjeFakturZakupu PFZ
) V