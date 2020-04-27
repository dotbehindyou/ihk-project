CREATE TABLE "admin"."SM_Customers_Modules" (
	"Kdnr" INTEGER NOT NULL,
	"Module_ID" UNIQUEIDENTIFIER NOT NULL,
	"Version" VARCHAR(16) NOT NULL,
	"Created" TIMESTAMP NOT NULL DEFAULT "now"(),
	"Modified" TIMESTAMP NULL,
	"Deleted" TIMESTAMP NOT NULL,
	"IsActive" BIT NOT NULL COMPUTE( case when "Deleted" is null then 1 else 0 end ),
	"Status" VARCHAR(25) NULL,
	"Config" "text" NOT NULL,
	PRIMARY KEY ( "Module_ID" ASC, "Kdnr" ASC )
) IN "system";
COMMENT ON COLUMN "admin"."SM_Customers_Modules"."Status" IS 'Installing, Idle, Running, Stop, Failed, etc.';
