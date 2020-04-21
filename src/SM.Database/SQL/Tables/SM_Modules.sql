CREATE TABLE "admin"."SM_Modules" (
	"Module_ID" UNIQUEIDENTIFIER NOT NULL DEFAULT "newid"(),
	"Name" VARCHAR(255) NOT NULL,
	"Created" TIMESTAMP NOT NULL DEFAULT "now"(),
	"Deleted" TIMESTAMP NULL,
	"IsActive" BIT NOT NULL COMPUTE( case when "Deleted" is null then 0 else 1 end ),
	PRIMARY KEY ( "Module_ID" ASC )
) IN "system";
