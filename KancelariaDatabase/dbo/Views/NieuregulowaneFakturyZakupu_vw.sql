CREATE view [dbo].[NieuregulowaneFakturyZakupu_vw] as
select
	FZ.Id,
	FZ.IdFirmy,
	FZ.IdRoku,
	FZ.NumerFaktury,
	FZ.DataFaktury,
	FZ.TerminPlatnosci,
	FZ.Opis,
	FZ.IdKontrahenta,
	IsNull((select sum(ZFZ.Kwota) from ZaplatyFakturZakupu ZFZ where ZFZ.IdFakturyZakupu = FZ.Id), 0) KwotaZaplacona,
	sum(FZ.KwotaNetto) KwotaNetto,
	sum(FZ.KwotaVat) KwotaVat,
	sum(FZ.KwotaBrutto) KwotaBrutto,
	sum(IsNull(FZ.KwotaSkompensowana, 0)) KwotaSkompensowana,
	sum(FZ.KwotaBrutto) - IsNull((select sum(ZFZ.Kwota) from ZaplatyFakturZakupu ZFZ where ZFZ.IdFakturyZakupu = FZ.Id), 0) - sum(IsNull(FZ.KwotaSkompensowana, 0)) KwotaNieuregulowana,
	K.KodKontrahenta
from
	(
		select 
			FZ.Id,
			FZ.IdFirmy,
			FZ.IdRoku,
			FZ.NumerFaktury,
			FZ.DataFaktury,
			FZ.TerminPlatnosci,
			FZ.Opis,
			FZ.IdKontrahenta,
			PFZ.Netto KwotaNetto,
			PFZ.Vat KwotaVat,
			PFZ.Brutto KwotaBrutto,
			kp.Kwota KwotaSkompensowana
		from 
			FakturyZakupu FZ LEFT OUTER JOIN PozycjeFakturZakupu_vw PFZ ON FZ.Id = PFZ.IdFaktury 
			LEFT OUTER JOIN KompensatyPowiazania KP ON FZ.Id = KP.IdFakturyZakupu
		where 1 = 1
	) FZ,
	Kontrahenci K
where 1 = 1
	and FZ.IdKontrahenta = K.Id
group by
	FZ.Id,
	FZ.IdFirmy,
	FZ.IdRoku,
	FZ.NumerFaktury,
	FZ.DataFaktury,
	FZ.TerminPlatnosci,
	FZ.Opis,
	FZ.IdKontrahenta,
	K.KodKontrahenta