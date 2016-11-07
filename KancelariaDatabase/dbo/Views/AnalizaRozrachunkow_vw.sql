CREATE VIEW [dbo].[AnalizaRozrachunkow_vw] as
select
	'FZ' + Convert(nvarchar, FZ.Id) "Key", FZ.IdFirmy, FZ.IdRoku, K.Id IdKontrahenta, K.KodKontrahenta, FZ.NumerFaktury, FZ.DataFaktury, FZ.TerminPlatnosci, 
	--round(sum(PFZ.CenaJednostkowa * PFZ.CenaJednostkowa), 2) NETTO,
	round(sum(PFZ.CenaJednostkowa * PFZ.Ilosc + (PFZ.CenaJednostkowa * PFZ.Ilosc * PFZ.StawkaVat / 100)), 2) Wn,
	null Ma
	--round(sum(PFZ.CenaJednostkowa * PFZ.CenaJednostkowa * PFZ.StawkaVat / 100), 2) VAT
from
	Kontrahenci K
	JOIN NieuregulowaneFakturyZakupu_vw FZ ON K.Id = FZ.IdKontrahenta
	JOIN PozycjeFakturZakupu PFZ ON FZ.Id = PFZ.IdFaktury
	--JOIN FakturySprzedazy FS ON K.Id = FS.IdKontrahenta
where 1 = 1
	and FZ.KwotaNieuregulowana <> 0
group by
	FZ.IdFirmy, FZ.IdRoku, FZ.Id, K.Id,K.KodKontrahenta, FZ.NumerFaktury, FZ.DataFaktury, FZ.TerminPlatnosci
union
select
	'FS' + Convert(nvarchar, FS.Id) "Key", FS.IdFirmy, FS.IdRoku, K.Id IdKontrahenta, K.KodKontrahenta, FS.NumerFaktury, FS.DataFaktury, FS.TerminPlatnosci, 
	--round(sum(PFS.CenaJednostkowa * PFS.CenaJednostkowa), 2) NETTO,
	null Wn,
	round(sum(PFS.CenaJednostkowa * PFS.Ilosc + (PFS.CenaJednostkowa * PFS.Ilosc * PFS.StawkaVat / 100)), 2) Ma
	--round(sum(PFS.CenaJednostkowa * PFS.CenaJednostkowa * PFS.StawkaVat / 100), 2) VAT
from
	Kontrahenci K
	JOIN NieuregulowaneFakturySprzedazy_vw FS ON K.Id = FS.IdKontrahenta
	JOIN PozycjeFakturSprzedazy PFS ON FS.Id = PFS.IdFaktury
	--JOIN FakturySprzedazy FS ON K.Id = FS.IdKontrahenta
where 1 = 1
	and FS.KwotaNieuregulowana <> 0
group by
	FS.IdFirmy, FS.IdRoku, FS.Id, K.Id,K.KodKontrahenta, FS.NumerFaktury, FS.DataFaktury, FS.TerminPlatnosci
--order by
--	K.Id