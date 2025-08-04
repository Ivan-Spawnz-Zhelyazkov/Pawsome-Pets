using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawsome_Pets.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdoptionRequestStatusAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CaretakingRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CaretakingRequests");
        }
    }
}
