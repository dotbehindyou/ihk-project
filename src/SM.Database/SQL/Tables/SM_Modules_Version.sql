CREATE TABLE "admin"."SM_Modules_Version" (
	"Version" VARCHAR(16) NOT NULL DEFAULT "newid"(),
	"Module_ID" UNIQUEIDENTIFIER NOT NULL,
	"Validation_Token" VARCHAR(128) NOT NULL,
	"Config_ID" UNIQUEIDENTIFIER NOT NULL,
	"Created" TIMESTAMP NOT NULL DEFAULT "now"(),
	"Deleted" TIMESTAMP NULL,
	"IsActive" BIT NOT NULL COMPUTE( case when "Deleted" is null then 1 else 0 end ),
	PRIMARY KEY ( "Version" ASC, "Module_ID" ASC )
) IN "system";
