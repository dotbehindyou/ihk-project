CREATE TABLE "admin"."SM_Customers_Change_Items" (
	"Change_ID" UNIQUEIDENTIFIER NOT NULL,
	"Module_ID" UNIQUEIDENTIFIER NOT NULL,
	"Version" VARCHAR(16) NOT NULL,
	"IsSuccess" BIT NULL,
	"IsFailed" BIT NULL,
	"IsWarning" BIT NULL,
	"Changed" TIMESTAMP NULL,
	"Operation" VARCHAR(20) NULL,
	PRIMARY KEY ( "Change_ID" ASC, "Module_ID" ASC, "Version" ASC )
) IN "system";
COMMENT ON COLUMN "admin"."SM_Customers_Change_Items"."Operation" IS 'INSTALL, DEL, CONFIG, UPDATE';
