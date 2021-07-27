select top 1 * from Sales
go

create proc GetAllSales
@SalesQuantity	int,
@PurchaseRate	decimal(18,2),
@Discount	decimal(18,2),
@ProductId	bigint,
@BillNo	bigint,
@SalesRate	decimal(18,2)
as
begin
	insert into Sales(SalesQuantity, PurchaseRate, Discount, ProductId, BillNo, SalesRate) values
	(@SalesQuantity, @PurchaseRate, @Discount, @ProductId, @BillNo, @SalesRate)

	select SCOPE_IDENTITY()
end

go

create proc GetSalesOnDate
@SalesDate date,
@BranchId bigint
as
begin
	select * from Bill b with (nolock)
	inner join Sales s with (nolock) on b.Id = s.Id
	where b.BranchId = @BranchId 
	and CONVERT(date, b.BillDate) = @SalesDate
end

go

create proc CheckoutSales
@SalesQuantity	int,
@PurchaseRate	decimal(18,2),
@Discount	decimal(18,2),
@ProductId	bigint,
@BillNo	bigint,
@SalesRate	decimal(18,2)
as
begin
	declare @CurrentID bigint
	insert into Sales(SalesQuantity, PurchaseRate, Discount, ProductId, BillNo, SalesRate) values
	(@SalesQuantity, @PurchaseRate, @Discount, @ProductId, @BillNo, @SalesRate)
	set @CurrentID = SCOPE_IDENTITY()

	update Inventory set
		Quantity -= @SalesQuantity
	where Id = @ProductId

	select @CurrentID
end

go

create proc UpdateSales
@Id bigint,
@SalesQuantity	int,
@PurchaseRate	decimal(18,2),
@Discount	decimal(18,2),
@ProductId	bigint,
@BillNo	bigint,
@SalesRate	decimal(18,2)
as
begin
	update Sales set
		SalesQuantity = @SalesQuantity,
		PurchaseRate = @PurchaseRate,
		Discount = @Discount,
		ProductId = @ProductId,
		BillNo = @BillNo,
		SalesRate = @SalesRate
	where Id = @Id
end

go

create proc RemoveSales
@Id bigint
as
begin
	declare @BillNo bigint
	select @BillNo = BillNo from Sales where Id = @Id
	delete from Sales where Id = @Id
	if (select COUNT(Id) from Sales where BillNo = @BillNo) < 1
	begin
		delete from Bill where Id = @BillNo
	end
end