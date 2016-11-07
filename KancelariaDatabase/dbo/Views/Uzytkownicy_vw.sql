--drop view Uzytkownicy_vw

CREATE view [dbo].[Uzytkownicy_vw] as
select 
	u.UserId,
	u.UserName,
	u.Imie,
	u.Nazwisko,
	u.WybraneIdRoku,
	u.WybraneIdFirmy,
	m.CreateDate, 
	m.ConfirmationToken, 
	m.IsConfirmed, 
	m.LastPasswordFailureDate, 
	m.PasswordFailuresSinceLastSuccess, 
	m.PasswordChangedDate, 
	m.PasswordSalt, 
	m.PasswordVerificationToken, 
	m.PasswordVerificationTokenExpirationDate,
	(select 1 from webpages_Roles r, webpages_UsersInRoles uir where r.RoleId = uir.RoleId and uir.UserId = m.UserId and r.RoleName = 'Admin') CzyAdmin,
	(select 1 from webpages_Roles r, webpages_UsersInRoles uir where r.RoleId = uir.RoleId and uir.UserId = m.UserId and r.RoleName = 'Kancelaria') CzyKancelaria
from
	Uzytkownicy u,
	webpages_Membership m
where 1 = 1
	and u.UserId = m.UserId