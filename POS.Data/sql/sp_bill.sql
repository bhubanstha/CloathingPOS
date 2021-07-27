create proc GetAllBill
as
begin

Select b.Id, b.BillDate, b.VAT, b.BillTo, b.BillingAddress, b.BillingPAN, b.BranchId,
b.UserId, b.CalculateVAT
from dbo.Bill b with (nolock)

end

go

create proc GenerateNewBillNo
as
begin

select ISNULL(MAX(Id),0) + 1 from dbo.Bill with (nolock)
end

go

create proc GetBillSalesCount
@BillId bigint
as
begin
	select COUNT(Id) from dbo.Sales s with (nolock) where s.BillNo = @BillId
end

go

create proc SaveBill
@BillDate datetime,
@VAT decimal(18,2),
@BillTo varchar(100),
@BillingAddress varchar(200),
@BillingPAN varchar(20),
@BranchID bigint,
@UserID bigint,
@CalculateVAT bit = 1
as
begin
	insert into Bill(BillDate, VAT, BillTo, BillingAddress, BillingPAN, BranchId, UserId, CalculateVAT) values 
	(@BillDate, @VAT, @BillTo, @BillingAddress, @BillingPAN, @BranchID, @UserID, @CalculateVAT)

	select SCOPE_IDENTITY()

end

go

create proc GetBillById
@Id bigint
as
begin

	Select b.Id, b.BillDate, b.VAT, b.BillTo, b.BillingAddress, b.BillingPAN, b.BranchId,
	b.UserId, b.CalculateVAT
	from dbo.Bill b with (nolock) where b.Id = @Id
end

go

create proc UpdateBill
@Id bigint,
@BillDate datetime,
@VAT decimal(18,2),
@BillTo varchar(100),
@BillingAddress varchar(200),
@BillingPAN varchar(20),
@BranchID bigint,
@UserID bigint,
@CalculateVAT bit = 1
as
begin
	update Bill set
		BillDate = @BillDate,
		VAT = @VAT,
		BillTo = @BillTo,
		BillingAddress = @BillingAddress,
		BillingPAN = @BillingPAN,
		BranchId = @BranchID,
		UserID = @UserID,
		CalculateVAT = @CalculateVAT
		where Id = @Id
end

go

create proc RemoveBill
@Id bigint
as
begin
	delete from dbo.Sales where BillNo = @Id
	delete from dbo.Bill where Id = @Id
end