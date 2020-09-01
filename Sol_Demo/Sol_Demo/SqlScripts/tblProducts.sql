CREATE TABLE tblProducts
(
	ProductId Numeric(18,0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name Varchar(100) NOT NULL,
	UnitPrice decimal NOT NULL,
	Version rowversion
)