CREATE view [dbo].[PozycjeFakturSprzedazy_vw] as
select
	V.*,
	(V.Brutto - V.Netto) Vat
from
(
	select
		PFS.*,
		CASE WHEN PFS.CzyBrutto = 1 THEN PFS.CenaJednostkowa * PFS.Ilosc ELSE (PFS.CenaJednostkowa * PFS.Ilosc) + (PFS.CenaJednostkowa * PFS.Ilosc) * PFS.StawkaVat / 100 END Brutto,
		CASE WHEN PFS.CzyBrutto = 1 THEN (PFS.CenaJednostkowa * PFS.Ilosc) / (1 + PFS.StawkaVat / 100) ELSE PFS.CenaJednostkowa * PFS.Ilosc END Netto
	from
		PozycjeFakturSprzedazy PFS
) V