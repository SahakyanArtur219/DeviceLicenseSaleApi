using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeviceLicenseSaleApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRewardsBundlesAndDeviceSerials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RewardPoints",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Devices",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
WITH DeviceSerials AS (
    SELECT Id, ROW_NUMBER() OVER (ORDER BY Id) AS RowNum
    FROM Devices
)
UPDATE Devices
SET SerialNumber = CONCAT('DEV-', RIGHT('000000' + CAST(DeviceSerials.RowNum AS varchar(6)), 6))
FROM Devices
INNER JOIN DeviceSerials ON Devices.Id = DeviceSerials.Id
WHERE Devices.SerialNumber IS NULL OR LTRIM(RTRIM(Devices.SerialNumber)) = '';
");

            migrationBuilder.CreateTable(
                name: "WeeklyBundles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    StartsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyBundles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyBundles_DeviceTypes_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyBundleFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeeklyBundleId = table.Column<int>(type: "int", nullable: false),
                    FeatureKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyBundleFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyBundleFeatures_WeeklyBundles_WeeklyBundleId",
                        column: x => x.WeeklyBundleId,
                        principalTable: "WeeklyBundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_SerialNumber",
                table: "Devices",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyBundleFeatures_WeeklyBundleId",
                table: "WeeklyBundleFeatures",
                column: "WeeklyBundleId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyBundles_DeviceTypeId",
                table: "WeeklyBundles",
                column: "DeviceTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklyBundleFeatures");

            migrationBuilder.DropTable(
                name: "WeeklyBundles");

            migrationBuilder.DropIndex(
                name: "IX_Devices_SerialNumber",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "RewardPoints",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Devices");
        }
    }
}