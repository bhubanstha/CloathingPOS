
ALTER proc [dbo].[GetBillById]
@Id bigint
as
begin

	Select b.Id, b.BillDate, b.VAT,  b.BranchId,
	b.UserId, b.CalculateVAT, b.CustomerId
	from dbo.Bill b with (nolock) where b.Id = @Id
end

