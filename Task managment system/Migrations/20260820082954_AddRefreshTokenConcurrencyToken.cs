using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_managment_system.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenConcurrencyToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "RefreshToken",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "RefreshToken");
        }
    }
}
