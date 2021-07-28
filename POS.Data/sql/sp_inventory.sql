create proc SaveInventory
@PurchaseRate	decimal(18,2),
@RetailRate	decimal(18,2),
@Quantity	int,
@FirstPurchaseDate	date,
@IsDeleted	bit,
@BranchId	bigint,
@UserId	bigint,
@Code	varchar(50),
@BarCode	varchar(40),
@Name	varchar(50),
@Size	varchar(5),
@Color	varchar(9),
@ColorName	varchar(20),
@CategoryId	bigint,
@BrandId	bigint
as
begin

	declare @CurrentId bigint;
	insert into Inventory(PurchaseRate, RetailRate, Quantity, FirstPurchaseDate, IsDeleted, BranchId, UserId, Code, BarCode, [Name], Size, Color, ColorName, CategoryId, BrandId) values
	(@PurchaseRate, @RetailRate, @Quantity, @FirstPurchaseDate, @IsDeleted, @BranchId, @UserId, @Code, @BarCode, @Name, @Size, @Color, @ColorName, @CategoryId, @BrandId)

	set @CurrentId = SCOPE_IDENTITY()

	insert into InventoryHistory(Quantity, PurchaseRate, RetailRate, PurchaseDate, InventoryId) values
	(@Quantity, @PurchaseRate, @RetailRate, @FirstPurchaseDate, @CurrentId)

	select @CurrentId
end

go

create proc GetAllActiveInventory
@BranchId bigint,
@Name varchar(50) = null
as
begin
	if(LEN(@Name) < 1)
	begin
		select 
	i.Id, 
	i.PurchaseRate, 
	i.RetailRate, 
	i.Quantity, 
	i.FirstPurchaseDate, 
	i.IsDeleted, 
	i.BranchId, 
	i.UserId, 
	i.Code, 
	i.BarCode, 
	i.[Name], 
	i.Size, 
	i.Color, 
	i.ColorName, 
	i.CategoryId, 
	i.BrandId
	from Inventory i with (nolock)
	where i.BranchId = @BranchId
	and i.IsDeleted = 0
	and i.Quantity >= 0
	end
	else
	begin
		set @Name = @Name + '%'
		select 
		i.Id, 
		i.PurchaseRate, 
		i.RetailRate, 
		i.Quantity, 
		i.FirstPurchaseDate, 
		i.IsDeleted, 
		i.BranchId, 
		i.UserId, 
		i.Code, 
		i.BarCode, 
		i.[Name], 
		i.Size, 
		i.Color, 
		i.ColorName, 
		i.CategoryId, 
		i.BrandId
		from Inventory i with (nolock)
		where i.BranchId = @BranchId
		and i.IsDeleted = 0
		and i.Quantity >= 0
		and (i.[Name] like @Name or Code like @Name)
	end
end
go

create proc GetInventoryById
@Id bigint
as
begin
	select 
		i.Id, 
		i.PurchaseRate, 
		i.RetailRate, 
		i.Quantity, 
		i.FirstPurchaseDate, 
		i.IsDeleted, 
		i.BranchId, 
		i.UserId, 
		i.Code, 
		i.BarCode, 
		i.[Name], 
		i.Size, 
		i.Color, 
		i.ColorName, 
		i.CategoryId, 
		i.BrandId
		from Inventory i with (nolock)
		where i.Id = @Id
end

go

create proc RemoveItem
@Id bigint
as
begin
	update Inventory set
	IsDeleted = 1
	where Id = @Id
end

go


create proc DeductInventoryQuantity
@Id bigint,
@Quantity int
as
begin
	update Inventory set
	Quantity -= @Quantity
	where Id = @Id
end
go

create proc UpdateInventory
@Id bigint,
@PurchaseRate	decimal(18,2),
@RetailRate	decimal(18,2),
@Quantity	int,
@FirstPurchaseDate	date,
@IsDeleted	bit,
@BranchId	bigint,
@UserId	bigint,
@Code	varchar(50),
@BarCode	varchar(40),
@Name	varchar(50),
@Size	varchar(5),
@Color	varchar(9),
@ColorName	varchar(20),
@CategoryId	bigint,
@BrandId	bigint,
@MaintainHistory bit
as
begin
	update Inventory set
	PurchaseRate = @PurchaseRate,
	RetailRate = @RetailRate,
	Quantity = @Quantity,
	FirstPurchaseDate = @FirstPurchaseDate,
	IsDeleted = @IsDeleted,
	BranchId = @BranchId,
	UserId = @UserId,
	Code = @Code,
	BarCode = @BarCode,
	Name = @Name,
	Size = @Size,
	Color = @Color,
	ColorName = @ColorName,
	CategoryId = @CategoryId,
	BrandId = @BrandId
	where Id = @Id

	if(@MaintainHistory = 1)
	begin
		insert into InventoryHistory (Quantity, PurchaseRate, RetailRate, PurchaseDate, InventoryId) values
		(@Quantity, @PurchaseRate, @RetailRate, @FirstPurchaseDate, @Id)
	end
end

go

create proc RestockInventory
@Id bigint,
@Quantity int
as
begin
	update Inventory set Quantity += @Quantity where Id = @Id
end

