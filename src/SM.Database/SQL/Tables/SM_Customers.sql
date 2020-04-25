CREATE TABLE "admin"."SM_Customers" (
	"Customer_ID" UNIQUEIDENTIFIER NOT NULL DEFAULT "newid"(),
	"Kdnr" INTEGER NOT NULL UNIQUE,
	"Auth_Token" BINARY(64) NOT NULL UNIQUE,
	"Created" TIMESTAMP NOT NULL DEFAULT "now"(),
	"Modified" TIMESTAMP NULL,
	"Deleted" TIMESTAMP NULL,
	"IsActive" BIT NOT NULL COMPUTE( case when "Deleted" is null then 1 else 0 end ),
	PRIMARY KEY ( "Customer_ID" ASC )
) IN "system";
COMMENT ON COLUMN "admin"."SM_Customers"."Kdnr" IS 'Kunden Nummer von PowerWeiss';
COMMENT ON COLUMN "admin"."SM_Customers"."Auth_Token" IS 'Random generierter SHA512 Wert, für Authenfizierung';
