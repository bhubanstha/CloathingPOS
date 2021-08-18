create table dbo.Customer(
	ID bigint primary key identity(1,1),
	[Name] varchar(50) not null,
	[Address] varchar(250) not null,
	GoogleMap varchar(300),
	Mobile1 varchar(10) not null,
	Mobile2 varchar(10)
)
go


create proc dbo.GetAllCustomer
as
begin

select top 500 ID, [Name], [Address], GoogleMap, Mobile1, Mobile2 from dbo.Customer with (nolock)

end

go

create proc dbo.GetCustomerById
@Id bigint
as
begin
	select ID, [Name], [Address], GoogleMap, Mobile1, Mobile2 from dbo.Customer with (nolock) where ID = @Id
end

go

create proc dbo.SaveCustomer
@Name varchar(50),
@Address varchar(250),
@GoogleMap varchar(300) = null,
@Mobile1 varchar(10),
@Mobile2 varchar(10) = null
as
begin

	insert into dbo.Customer([Name], [Address], GoogleMap, Mobile1, Mobile2) values
	(@Name, @Address, @GoogleMap, @Mobile1, @Mobile2)

	select SCOPE_IDENTITY();
end

go

create proc dbo.UpdateCustomer
@ID bigint,
@Name varchar(50),
@Address varchar(250),
@GoogleMap varchar(300)=null,
@Mobile1 varchar(10),
@Mobile2 varchar(10) = null
as
begin
	update dbo.Customer set
		[Name] = @Name,
		[Address] = @Address,
		GoogleMap = @GoogleMap,
		Mobile1 = @Mobile1,
		Mobile2 = @Mobile2
	where ID = @ID

end

go

create proc dbo.SearchCustomer
@SearchText varchar(50)
as
begin
	select top 20 * from dbo.Customer c with (nolock) 
	where c.[Name] like @SearchText Or Mobile1 like @SearchText
end

exec SearchCustomer @SearchText=N'%'
go