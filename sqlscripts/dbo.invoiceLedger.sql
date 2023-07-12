CREATE TABLE invoiceLedger
(
    --[Sr] INT IDENTITY(1,1) NOT NULL, 	
    [Invoice] VARCHAR(50) NOT NULL, 
    [UserName] VARCHAR(50) NULL, 
    [Customer] VARCHAR(50) NULL, 
    [Contact] VARCHAR(50) NULL, 
	[Total] REAL NULL,
    [Discount] REAL NULL, 
	[Payment] REAL NULL, 
    [Sale_Tax] REAL NULL, 
	[Balance] REAL NULL,
	[PaymentType] VARCHAR(50) NULL, 
	[DrugCount] INT NULL,
	[CheckoutDate] DATE NULL,
	[CheckoutTime] TIME NULL    
)
