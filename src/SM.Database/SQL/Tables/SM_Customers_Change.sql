CREATE TABLE "admin"."SM_Customers_Change" (
	"Kdnr" INTEGER NOT NULL DEFAULT "newid"(),
	"Customer_ID" UNIQUEIDENTIFIER NOT NULL,
	"Changed" TIMESTAMP NULL,
	"IsSuccess" BIT NULL,
	"IsFailed" BIT NULL,
	"IsWarning" BIT NULL,
	"LogMessage" VARCHAR(32767) NULL,
	"Created" TIMESTAMP NOT NULL DEFAULT "now"(),
	"Deleted" TIMESTAMP NULL,
	"IsActive" BIT NOT NULL COMPUTE( case when "Deleted" is null and "Changed" is null then 1 else 0 end ),
	PRIMARY KEY ( "Kdnr" ASC )
) IN "system";
