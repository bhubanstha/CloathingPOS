create proc GetAllBranch
as
begin
	select b.Id, b.BranchName, b.BranchAddress, b.ShopId from dbo.Branch b with (nolock)
end

go

create proc GetBranchByAccessLevel
@Id bigint,
@CanAccessAllBranch bit
as
begin
	select b.Id, b.BranchName, b.BranchAddress, b.ShopId from dbo.Branch b with (nolock)
	where b.Id = @Id OR 1 = @CanAccessAllBranch
end

go

create proc GetBranchById
@Id bigint
as
begin
	select b.Id, b.BranchName, b.BranchAddress, b.ShopId from dbo.Branch b with (nolock)
	where b.Id = @Id 
end

go

create proc SaveBranch
@BranchName varchar(50),
@BranchAddress varchar(300),
@ShopId bigint
as
begin
	insert into Branch(BranchName, BranchAddress, ShopId) values
	(@BranchName, @BranchAddress, @ShopId)

	select SCOPE_IDENTITY()
end
go

create proc UpdateBranch
@Id bigint,
@BranchName varchar(50),
@BranchAddress varchar(300),
@ShopId bigint
as
begin
	update Branch set 
	BranchName = @BranchName,
	BranchAddress = @BranchAddress,
	ShopId = @ShopId
	where Id = @Id
end

go

create proc RemoveBranch
@Id bigint
as
begin
	delete from Branch where Id = @Id

end