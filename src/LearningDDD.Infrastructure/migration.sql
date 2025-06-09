CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Groups" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Groups" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Capacity" INTEGER NOT NULL
);

CREATE TABLE "ChargeStations" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_ChargeStations" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "GroupId" TEXT NOT NULL,
    CONSTRAINT "FK_ChargeStations_Groups_GroupId" FOREIGN KEY ("GroupId") REFERENCES "Groups" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Connectors" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Connectors" PRIMARY KEY,
    "ChargeStationContextId" INTEGER NOT NULL,
    "MaxCurrent" INTEGER NOT NULL,
    "ChargeStationId" TEXT NOT NULL,
    CONSTRAINT "FK_Connectors_ChargeStations_ChargeStationId" FOREIGN KEY ("ChargeStationId") REFERENCES "ChargeStations" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ChargeStations_GroupId" ON "ChargeStations" ("GroupId");

CREATE UNIQUE INDEX "IX_Connectors_ChargeStationContextId_ChargeStationId" ON "Connectors" ("ChargeStationContextId", "ChargeStationId");

CREATE INDEX "IX_Connectors_ChargeStationId" ON "Connectors" ("ChargeStationId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250609090541_Migrations', '9.0.5');

COMMIT;

