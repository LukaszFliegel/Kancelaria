CREATE view [dbo].[KosztyNaInwestycjach] as
select
	FZ.IdFirmy,
	FZ.IdRoku,
	PFZ.IdInwestycji,
	I.NumerInwestycji,
	I.IdTypuInwestycji,
	TI.KodTypuInwestycji,
	FZ.NumerFaktury,
	FZ.DataFaktury,
	FZ.WlasnyNumerFaktury,
	K.id "IdKontrahenta",
	K.KodKontrahenta,
	K.NazwaKontrahenta,
	PFZ.Opis,
	sum(PFZ.Netto) KwotaNetto,
	sum(PFZ.Vat) KwotaVat,
	sum(PFZ.Brutto) KwotaBrutto
from
	PozycjeFakturZakupu_vw PFZ
	JOIN Inwestycje I ON PFZ.IdInwestycji = I.Id
	JOIN FakturyZakupu FZ ON FZ.Id = PFZ.IdFaktury
	JOIN Kontrahenci K ON FZ.IdKontrahenta = K.Id
	JOIN TypyInwestycji TI ON I.IdTypuInwestycji = TI.Id
where 1 = 1
group by
	FZ.IdFirmy,
	FZ.IdRoku,
	PFZ.IdInwestycji,
	I.NumerInwestycji,
	I.IdTypuInwestycji,
	TI.KodTypuInwestycji,
	FZ.NumerFaktury,
	FZ.DataFaktury,
	FZ.WlasnyNumerFaktury,
	K.id,
	K.KodKontrahenta,
	K.NazwaKontrahenta,
	PFZ.Opis