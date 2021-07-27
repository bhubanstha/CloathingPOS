select top 1 * from Category
go

create proc GetAllCategory
as
begin
	select c.Id, c.Name from dbo.Category c with (nolock)
end

go

create proc GetCategoryById
@Id bigint
as
begin
	select c.Id, c.Name from dbo.Category c with (nolock)
	where c.Id = @Id
end

go

create proc SaveCategory
@Name varchar(50)
as
begin
	insert into Category values (@Name)
end

go

create proc UpdateCategory
@Id bigint,
@Name varchar(50)
as
begin
	update Category set
	[Name] = @Name
	where Id = @Id
end

go

create proc RemoveCategory
@Id bigint
as
begin
	Delete from Category where Id = @Id
end
