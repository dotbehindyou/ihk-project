CREATE TABLE "admin"."SM_Modules" (
	"Module_ID" UNIQUEIDENTIFIER NOT NULL DEFAULT "newid"(),
	"Name" VARCHAR(255) NOT NULL UNIQUE,
	"Created" TIMESTAMP NOT NULL DEFAULT "now"(),
	"Deleted" TIMESTAMP NULL,
	"IsActive" BIT NOT NULL COMPUTE( case when "Deleted" is null then 1 else 0 end ),
	PRIMARY KEY ( "Module_ID" ASC )
) IN "system";
