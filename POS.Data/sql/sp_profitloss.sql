alter proc GetSalesStat
@Year int, 
@Month int
AS
BEGIN

select 
b.BillDate,
c.[Name] as [Category], 
br.[Name] as [Brand], 
i.Code,
i.[Name] as [ItemName],
i.Color,
i.Size,
s.SalesQuantity,
(s.SalesQuantity * s.PurchaseRate) As [PurchaseTotal],
((s.SalesQuantity * s.SalesRate) - s.Discount) AS [SalesTotal]
from Sales s with (nolock)
inner join Bill b with (nolock) on s.BillNo = b.Id
inner join Inventory i with (nolock) on s.ProductId = i.Id
inner join Category c with (nolock) on c.Id = i.CategoryId
inner join Brand br with (nolock) on br.Id = i.BrandId
where YEAR(b.BillDate)= @Year 
and MONTH(b.BillDate)= @Month
order by convert(date, b.BillDate), i.Code, i.Name
END

