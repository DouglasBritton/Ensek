using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeededAccountsTableAndMeterReadingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurnameName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterReadings_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "FirstName", "SurnameName" },
                values: new object[,]
                {
                    { 1234, "Freya", "Test" },
                    { 1239, "Noddy", "Test" },
                    { 1240, "Archie", "Test" },
                    { 1241, "Lara", "Test" },
                    { 1242, "Tim", "Test" },
                    { 1243, "Graham", "Test" },
                    { 1244, "Tony", "Test" },
                    { 1245, "Neville", "Test" },
                    { 1246, "Jo", "Test" },
                    { 1247, "Jim", "Test" },
                    { 1248, "Pam", "Test" },
                    { 2233, "Barry", "Test" },
                    { 2344, "Tommy", "Test" },
                    { 2345, "Jerry", "Test" },
                    { 2346, "Ollie", "Test" },
                    { 2347, "Tara", "Test" },
                    { 2348, "Tammy", "Test" },
                    { 2349, "Simon", "Test" },
                    { 2350, "Colin", "Test" },
                    { 2351, "Gladys", "Test" },
                    { 2352, "Greg", "Test" },
                    { 2353, "Tony", "Test" },
                    { 2355, "Arthur", "Test" },
                    { 2356, "Craig", "Test" },
                    { 4534, "JOSH", "TEST" },
                    { 6776, "Laura", "Test" },
                    { 8766, "Sally", "Test" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_AccountId",
                table: "MeterReadings",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterReadings");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
