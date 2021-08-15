/*
select * from [User]
delete from [User]
DBCC CHECKIDENT ('dbo.User', RESEED, 0);  
*/

insert into Shop(Name, PANNumber, LogoPath, CalculateVATOnSales, PrintInvoice, PdfPassword) values
('Give Your Shop Name', '123456', 'companyLogo1.png', 1,0,'123')
go

insert into Branch (BranchName, BranchAddress, ShopId) values
('Main Branch', 'Your Main Branch Address', 1)

go


insert into [User](UserName, DisplayName, Password, IsAdmin, IsActive, PromptForPasswordReset, ProfileImage, CreatedDate, DeactivationDate, BranchId, CanAccessAllBranch) values
('sysadmin', 'System Admin', 'Eo4s8867dmG8CM+hkPVUcQ==', 1, 1, 0, null, GETDATE(), null, 1, 1),
('admin', 'Default Administrator', 'QeJssTHc3geIUDJ2PdPzzA==', 1, 1, 0, null, GETDATE(), null, 1, 1)

go

insert into Brand (Name) values
('Adidas'),
('Armani'),
('Ralpha Lauren'),
('Chanel'),
('Prada'),
('Hermes'),
('Gucci'),
('Louis Vuitton'),
('Nike')

go

insert into Category (Name) values
('Men'),
('Woman'),
('Children')

go
