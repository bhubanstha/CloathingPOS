
ALTER TABLE dbo.Bill
DROP COLUMN BillTo;

go

ALTER TABLE dbo.Bill
DROP COLUMN BillingAddress;
go

ALTER TABLE dbo.Bill
DROP COLUMN BillingPAN;
go


ALTER TABLE dbo.Bill
    ADD CustomerId bigint,
    FOREIGN KEY(CustomerId) REFERENCES dbo.Customer(ID);
