CREATE TABLE [dbo].[mainLedger] (
    [id]           INT          NOT NULL identity(101,7),
    [name]         VARCHAR (25) NOT NULL,
    [manufacturer] VARCHAR (40) NOT NULL,
    [supplier]     VARCHAR (40) NULL,
    [costIn]       REAL         NULL,
    [Cost]         REAL         NOT NULL,
    [expiry]       DATE         NULL,
    [formula]      VARCHAR (50) NULL,
    [quantity]     INT          NOT NULL
);

