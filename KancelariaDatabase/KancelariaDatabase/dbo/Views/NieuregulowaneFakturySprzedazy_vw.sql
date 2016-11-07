CREATE view [dbo].[NieuregulowaneFakturySprzedazy_vw] as
select
	FS.Id,
	FS.IdFirmy,
	FS.IdRoku,
	FS.NumerFaktury,
	FS.DataFaktury,
	FS.TerminPlatnosci,
	FS.Opis,
	FS.IdKontrahenta,
	IsNull((select sum(ZFS.Kwota) from ZaplatyFakturSprzedazy ZFS where ZFS.IdFakturySprzedazy = FS.Id), 0) KwotaZaplacona,
	FS.IdInwestycji,
	sum(FS.KwotaNetto) KwotaNetto,
	sum(FS.KwotaVat) KwotaVat,
	sum(FS.KwotaBrutto) KwotaBrutto,
	sum(IsNull(FS.KwotaSkompensowana, 0)) KwotaSkompensowana,
	sum(FS.KwotaBrutto) - IsNull((select sum(ZFS.Kwota) from ZaplatyFakturSprzedazy ZFS where ZFS.IdFakturySprzedazy = FS.Id), 0) - sum(IsNull(FS.KwotaSkompensowana, 0)) KwotaNieuregulowana,
	K.KodKontrahenta,
	I.NumerInwestycji,
	I.NumerUmowy,
	FS.IdFakturyKorygujacej
from
	(
		select 
			FS.Id,
			FS.IdFirmy,
			FS.IdRoku,
			FS.NumerFaktury,
			FS.DataFaktury,
			FS.TerminPlatnosci,
			FS.Opis,
			FS.IdKontrahenta,
			FS.IdInwestycji,
			pfs.CenaJednostkowa * PFS.Ilosc KwotaNetto,
			pfs.CenaJednostkowa * PFS.Ilosc * pfs.StawkaVat/100.0 KwotaVat,
			pfs.CenaJednostkowa * PFS.Ilosc + pfs.CenaJednostkowa * PFS.Ilosc * pfs.StawkaVat/100.0 KwotaBrutto,
			kp.Kwota KwotaSkompensowana,
			FS.IdFakturyKorygujacej
		from 
			--FakturySprzedazy FS, PozycjeFakturSprzedazy PFS, KompensatyPowiazania KP
			FakturySprzedazy FS LEFT OUTER JOIN PozycjeFakturSprzedazy_vw PFS ON FS.Id = PFS.IdFaktury 
			LEFT OUTER JOIN KompensatyPowiazania KP ON FS.Id = KP.IdFakturySprzedazy
		where 1 = 1
			--and FS.Id = PFS.IdFaktury
			--and FS.Id = KP.IdFakturySprzedazy
	) FS,
	Kontrahenci K,
	Inwestycje I
where 1 = 1
	and FS.IdKontrahenta = K.Id
	and FS.IdInwestycji = I.Id
group by
	FS.Id,
	FS.IdFirmy,
	FS.IdRoku,
	FS.NumerFaktury,
	FS.DataFaktury,
	FS.TerminPlatnosci,
	FS.Opis,
	FS.IdKontrahenta,
	--FS.KwotaZaplacona,
	FS.IdInwestycji,
	--FS.KwotaSkompensowana,
	K.KodKontrahenta,
	I.NumerInwestycji,
	I.NumerUmowy,
	FS.IdFakturyKorygujacej