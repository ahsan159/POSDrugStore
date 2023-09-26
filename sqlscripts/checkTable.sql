				use DSPOS
				go
				CREATE TABLE mainLedger (
                                [id]           INT          NOT NULL identity(101,7),
                                [name]         VARCHAR (25) NOT NULL,
                                [manufacturer] VARCHAR (40) NOT NULL,
                                [supplier]     VARCHAR (40) NULL,
                                [costIn]       REAL         NULL,
                                [Cost]         REAL          NOT NULL,
                                [expiry]       DATE         NULL,
                                [formula]      VARCHAR (50) NULL,
                                [quantity]     INT          NOT NULL
                            )
				select 'created mainLedger' as message
				go
				CREATE TABLE loginTable (
                                [Sr] INT NOT NULL IDENTITY(1,1),
                                [Name] varchar(50) NOT NULL,
                                [Password] varchar(50) NOT NULL,
                                [Level] varchar(10) NOT NULL Default('User'),
                                [Added] DateTime NOT NULL Default(GetDate())
                                )
				select 'created loginTable' as message
				go
CREATE TABLE invoiceLedger
                                    (
                                        [Sr] INT IDENTITY(1,1) NOT NULL, 	
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
	                                    [CheckoutTime] TIME NULL,
                                        [DBName] VARCHAR(50) NULL
                                    )
				select 'created invoicetable' as message
				go
CREATE TABLE stockTable (
                                [Sr] INT NOT NULL IDENTITY(1,1),
                                [ProductID] INT NOT NULL,
                                [QuantityAdded] INT NOT NULL,
                                [Purchase] REAL NULL,
                                [Retail] REAL NULL,
                                [Supplier] varchar(50) NULL,
                                [SupplierContact] varchar(10) NULL,
                                [Added] Date NOT NULL,
                                [Users] VARCHAR(50) NULL
                                )
				select 'create admintable' as message
				go

				insert into loginTable(Name,Password,Level) values('admin','admin','admin')
				go
				insert into loginTable(Name,Password,Level) values('ahsan','ahsan','admin')
				go
				insert into loginTable(Name,Password,Level) values('rukhsar','8002','admin')
				go
				insert into loginTable(Name,Password) values('hammad','68046804')