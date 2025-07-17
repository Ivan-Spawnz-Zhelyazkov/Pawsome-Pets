using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawsome_Pets.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdoptionRequests_Animal_AnimalId",
                table: "AdoptionRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Animal_AspNetUsers_AdopterId",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_Animal_AspNetUsers_GiverId",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_Animal_Categories_CategoryId",
                table: "Animal");

            migrationBuilder.DropForeignKey(
                name: "FK_CaretakingRequests_Animal_AnimalId",
                table: "CaretakingRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animal",
                table: "Animal");

            migrationBuilder.RenameTable(
                name: "Animal",
                newName: "Animals");

            migrationBuilder.RenameIndex(
                name: "IX_Animal_GiverId",
                table: "Animals",
                newName: "IX_Animals_GiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Animal_CategoryId",
                table: "Animals",
                newName: "IX_Animals_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Animal_AdopterId",
                table: "Animals",
                newName: "IX_Animals_AdopterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animals",
                table: "Animals",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Fishes");

            migrationBuilder.AddForeignKey(
                name: "FK_AdoptionRequests_Animals_AnimalId",
                table: "AdoptionRequests",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_AspNetUsers_AdopterId",
                table: "Animals",
                column: "AdopterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_AspNetUsers_GiverId",
                table: "Animals",
                column: "GiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Categories_CategoryId",
                table: "Animals",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaretakingRequests_Animals_AnimalId",
                table: "CaretakingRequests",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdoptionRequests_Animals_AnimalId",
                table: "AdoptionRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_AspNetUsers_AdopterId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_AspNetUsers_GiverId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Categories_CategoryId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_CaretakingRequests_Animals_AnimalId",
                table: "CaretakingRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animals",
                table: "Animals");

            migrationBuilder.RenameTable(
                name: "Animals",
                newName: "Animal");

            migrationBuilder.RenameIndex(
                name: "IX_Animals_GiverId",
                table: "Animal",
                newName: "IX_Animal_GiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Animals_CategoryId",
                table: "Animal",
                newName: "IX_Animal_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Animals_AdopterId",
                table: "Animal",
                newName: "IX_Animal_AdopterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animal",
                table: "Animal",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Fish");

            migrationBuilder.AddForeignKey(
                name: "FK_AdoptionRequests_Animal_AnimalId",
                table: "AdoptionRequests",
                column: "AnimalId",
                principalTable: "Animal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_AspNetUsers_AdopterId",
                table: "Animal",
                column: "AdopterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_AspNetUsers_GiverId",
                table: "Animal",
                column: "GiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_Categories_CategoryId",
                table: "Animal",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaretakingRequests_Animal_AnimalId",
                table: "CaretakingRequests",
                column: "AnimalId",
                principalTable: "Animal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
