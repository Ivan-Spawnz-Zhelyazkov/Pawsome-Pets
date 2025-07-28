using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawsome_Pets.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdoptionRequestFieldsAdjusted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "AdoptionRequests");

            migrationBuilder.RenameColumn(
                name: "RequestedOn",
                table: "AdoptionRequests",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AdoptionRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AdoptionRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "AdoptionRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "AdoptionRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AdoptionRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "AdoptionRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AdoptionRequests");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "AdoptionRequests",
                newName: "RequestedOn");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "AdoptionRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
