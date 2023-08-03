CREATE TABLE loginTable (
[Sr] INT NOT NULL IDENTITY(1,1),
[Name] varchar(50) NOT NULL,
[Password] varchar(50) NOT NULL,
[Level] varchar(10) NOT NULL Default('User'),
[Added] DateTime NOT NULL Default(GetDate())
)