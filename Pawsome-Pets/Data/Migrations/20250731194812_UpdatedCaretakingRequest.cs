using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawsome_Pets.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCaretakingRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CaretakingRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "CaretakingRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "CaretakingRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "CaretakingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "CaretakingRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "CaretakingRequests");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "CaretakingRequests");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "CaretakingRequests");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "CaretakingRequests");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "CaretakingRequests");
        }
    }
}
