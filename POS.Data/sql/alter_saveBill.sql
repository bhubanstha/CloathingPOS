
ALTER proc [dbo].[SaveBill]
@BillDate datetime,
@VAT decimal(18,2),
@CustomerId bigint,
@BranchID bigint,
@UserID bigint,
@CalculateVAT bit = 1
as
begin
	insert into Bill(BillDate, VAT, BranchId, UserId, CalculateVAT, CustomerId) values 
	(@BillDate, @VAT, @BranchID, @UserID, @CalculateVAT,@CustomerId)

	select SCOPE_IDENTITY()

end

