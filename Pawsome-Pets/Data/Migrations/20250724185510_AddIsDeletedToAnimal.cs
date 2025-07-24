using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawsome_Pets.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Animals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Animals");
        }
    }
}
