create proc GetInventoryHistory
@ItemId bigint
as
begin
	select 
	ih.Id,
	ih.Quantity,
	ih.PurchaseRate,
	ih.RetailRate,
	ih.PurchaseDate,
	ih.InventoryId
	from InventoryHistory ih with (nolock)
	where InventoryId = @ItemId
end