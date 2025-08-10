using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookStore.Migrations
{
    /// <inheritdoc />
    public partial class AddBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Books_BookID1",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_BookID1",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "BookID1",
                table: "Cart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookID1",
                table: "Cart",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_BookID1",
                table: "Cart",
                column: "BookID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Books_BookID1",
                table: "Cart",
                column: "BookID1",
                principalTable: "Books",
                principalColumn: "BookID");
        }
    }
}
