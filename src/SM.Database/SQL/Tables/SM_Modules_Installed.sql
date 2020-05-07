CREATE TABLE "admin"."SM_Modules_Installed" (
	"Module_ID" UNIQUEIDENTIFIER NOT NULL,
	"ServiceName" VARCHAR(255) NOT NULL UNIQUE,
	"Version" VARCHAR(25) NOT NULL,
	"ValidationToken" BINARY(64) NOT NULL,
	"ModuleName" VARCHAR(255) NOT NULL,
	"Path" VARCHAR(512) NOT NULL,
	PRIMARY KEY ( "Module_ID" ASC )
) IN "system";
COMMENT ON COLUMN "admin"."SM_Modules_Installed"."ServiceName" IS 'Dienstname beim Installieren';
COMMENT ON COLUMN "admin"."SM_Modules_Installed"."Version" IS 'installierte Version';
COMMENT ON COLUMN "admin"."SM_Modules_Installed"."ValidationToken" IS 'aktueller Hash der Version';
COMMENT ON COLUMN "admin"."SM_Modules_Installed"."ModuleName" IS 'aktueller Name des Moduls';
COMMENT ON COLUMN "admin"."SM_Modules_Installed"."Path" IS 'Pfad wo es installiert ist';
