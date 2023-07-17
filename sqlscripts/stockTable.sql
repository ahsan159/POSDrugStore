CREATE TABLE stockTable (
[Sr] INT NOT NULL IDENTITY(1,1),
[ProductID] INT NOT NULL,
[QuantityAdded] INT NOT NULL,
[Purchase] REAL NULL,
[Retail] REAL NULL,
[Supplier] varchar(50) NULL,
[SupplierContact] varchar(10) NULL,
[Added] Date NOT NULL,
[User] VARCHAR(50) NULL
)