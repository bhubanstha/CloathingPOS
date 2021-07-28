
create proc GetAllBrands
as
begin
	select b.Id, b.[Name] from Brand b with (nolock)
end
go

create proc GetBrandById
@Id bigint
as
begin
	select b.Id, b.[Name] from Brand b with (nolock) where b.Id = @Id
end
go

create proc SaveBrand
@Name varchar(50)
as
begin
	insert into Brand([Name]) values (@Name)
	select scope_identity()
end

go

create proc UpdateBrand
@Id bigint,
@Name varchar(50)
as
begin
	update Brand set
		[Name] = @Name
	where Id = @Id
end
go

create proc RemoveBrand
@Id bigint
as
begin
	delete from Brand where Id = @Id
end