create proc dbo.UpdateBillingCustomer
@BillId bigint,
@BillDate datetime,
@CustomerId bigint,
@Name varchar(50),
@Address varchar(250),
@GoogleMap varchar(300) = null,
@Mobile1 varchar(10),
@Mobile2 varchar(10)= null
as
begin
	update Bill set BillDate = @BillDate where Id = @BillId
	update Customer set 
		[Name] = @Name,
		[Address] = @Address,
		GoogleMap = @GoogleMap,
		Mobile1 = @Mobile1,
		Mobile2 = @Mobile2
	where Id = @CustomerId
end