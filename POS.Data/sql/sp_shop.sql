select * from Shop
go

create proc GetShop
as
begin
	select s.Id, s.Name, s.PANNumber, s.LogoPath, s.CalculateVATOnSales, s.PrintInvoice, s.PdfPassword from Shop s with (nolock)
end

go

create proc SaveShop
@Name	varchar(200),
@PANNumber	varchar(20),
@LogoPath	varchar(200),
@CalculateVATOnSales	bit,
@PrintInvoice	bit,
@PdfPassword	varchar(10)
as
begin
	insert into Shop(Name, PANNumber, LogoPath, CalculateVATOnSales,PrintInvoice,PdfPassword) values
	(@Name, @PANNumber, @LogoPath, @CalculateVATOnSales,@PrintInvoice,@PdfPassword)
end

go

create proc UpdateShop
@Id bigint,
@Name	varchar(200),
@PANNumber	varchar(20),
@LogoPath	varchar(200),
@CalculateVATOnSales	bit,
@PrintInvoice	bit,
@PdfPassword	varchar(10)
as
begin
	update Shop set
	[Name] = @Name,
	PANNumber = @PANNumber,
	LogoPath = @LogoPath,
	CalculateVATOnSales = @CalculateVATOnSales,
	PrintInvoice = @PrintInvoice,
	PdfPassword = @PdfPassword
	where Id = @Id
end