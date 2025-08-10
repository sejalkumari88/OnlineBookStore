using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookStore.Migrations
{
    /// <inheritdoc />
    public partial class AddCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Books_BookID",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Users_UserID",
                table: "Cart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.RenameTable(
                name: "Cart",
                newName: "Carts");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_UserID",
                table: "Carts",
                newName: "IX_Carts_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_BookID",
                table: "Carts",
                newName: "IX_Carts_BookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "CartID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Books_BookID",
                table: "Carts",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserID",
                table: "Carts",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Books_BookID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserID",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Cart");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_UserID",
                table: "Cart",
                newName: "IX_Cart_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_BookID",
                table: "Cart",
                newName: "IX_Cart_BookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                column: "CartID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Books_BookID",
                table: "Cart",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Users_UserID",
                table: "Cart",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
