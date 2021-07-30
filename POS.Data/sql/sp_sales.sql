create proc GetAllSales
as
begin
	select s.Id, s.SalesQuantity, s.PurchaseRate, s.Discount, s.ProductId, s.BillNo, s.SalesRate from Sales s with (nolock)
end
go

create proc SaveSales
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
	inner join Sales s with (nolock) on b.Id = s.BillNo
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
go

create proc GetSalesByBill
@BillNo bigint,
@BranchId bigint
as
begin
	select s.Id, s.SalesQuantity, s.PurchaseRate, s.Discount, s.ProductId, s.BillNo, s.SalesRate 
	from Sales s with (nolock)
	inner join Bill b with (nolock) on s.BillNo = b.Id
	where s.BillNo = @BillNo and b.BranchId = @BranchId
end

go

create proc GetSalesHistory
@ProductId bigint
as
begin
	select sum(s.SalesQuantity) AS SalesQuantity, convert(date,b.BillDate) AS BillDate
	from Sales s with (nolock)
	inner join Bill b with (nolock) on s.BillNo = b.Id
	--where s.ProductId = @ProductId
	group by convert(date, b.BillDate)
	order by convert(date, b.BillDate) asc

end