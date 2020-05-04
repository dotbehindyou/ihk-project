CREATE TABLE "admin"."SM_Modules_Config" (
	"Config_ID" UNIQUEIDENTIFIER NOT NULL DEFAULT "newid"(),
	"Module_ID" UNIQUEIDENTIFIER NOT NULL,
	"FileName" VARCHAR(50) NOT NULL,
	"Format" VARCHAR(10) NULL,
	"Data" "text" NULL,
	"Created" TIMESTAMP NOT NULL DEFAULT "now"(),
	"Modified" TIMESTAMP NULL,
	"Deleted" TIMESTAMP NULL,
	"IsActive" BIT NULL COMPUTE( case when "Deleted" is null then 1 else 0 end ),
	PRIMARY KEY ( "Config_ID" ASC )
) IN "system";
