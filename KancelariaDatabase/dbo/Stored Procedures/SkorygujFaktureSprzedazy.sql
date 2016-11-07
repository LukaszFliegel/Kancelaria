CREATE PROCEDURE [dbo].[SkorygujFaktureSprzedazy] @IdFaktury int
AS
BEGIN
	declare @InsertedIdTable table (ID int);
	declare @InsertedId int;

	insert into FakturySprzedazy 
	(
		IdFirmy, IdRoku, NumerFaktury, DataFaktury, TerminPlatnosci, Opis, IdKontrahenta, CzyZaliczka, 
		IdInwestycji, IdKontaBankowegoFirmy, IdSposobuPlatnosci, DataSprzedazy, CzyUmowa, IdFakturyKorygujacej
	)
	output INSERTED.Id into @InsertedIdTable(Id)
	(
		select
			IdFirmy, IdRoku, NumerFaktury + '/Korekta', DataFaktury, TerminPlatnosci, Opis, IdKontrahenta, CzyZaliczka, 
			IdInwestycji, IdKontaBankowegoFirmy, IdSposobuPlatnosci, DataSprzedazy, CzyUmowa, NULL
		from
			FakturySprzedazy
		where 1 = 1
			and id = @IdFaktury
	)

	select
		@InsertedId = Id
	from
		@InsertedIdTable

	update 
		FakturySprzedazy
	set
		IdFakturyKorygujacej = @InsertedId
	where 1= 1
		and id = @IdFaktury

	insert into PozycjeFakturSprzedazy
	(
		NumerPozycji, IdFaktury, CenaJednostkowa, StawkaVat, Opis, IdJednostkiMiary, Ilosc
	)
	(
		select
			NumerPozycji, @InsertedId, CenaJednostkowa, StawkaVat, Opis, IdJednostkiMiary, Ilosc
		from
			PozycjeFakturSprzedazy
		where 1 = 1
			and IdFaktury = @IdFaktury
	)

	SELECT @InsertedId
END