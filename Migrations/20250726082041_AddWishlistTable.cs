using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookStore.Migrations
{
    /// <inheritdoc />
    public partial class AddWishlistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_Books_BookID",
                table: "WishlistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_Books_BookID1",
                table: "WishlistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_Users_UserID",
                table: "WishlistItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WishlistItems",
                table: "WishlistItems");

            migrationBuilder.RenameTable(
                name: "WishlistItems",
                newName: "Wishlists");

            migrationBuilder.RenameIndex(
                name: "IX_WishlistItems_UserID",
                table: "Wishlists",
                newName: "IX_Wishlists_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_WishlistItems_BookID1",
                table: "Wishlists",
                newName: "IX_Wishlists_BookID1");

            migrationBuilder.RenameIndex(
                name: "IX_WishlistItems_BookID",
                table: "Wishlists",
                newName: "IX_Wishlists_BookID");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "Wishlists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists",
                column: "WishlistID");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Books_BookID",
                table: "Wishlists",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Books_BookID1",
                table: "Wishlists",
                column: "BookID1",
                principalTable: "Books",
                principalColumn: "BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Users_UserID",
                table: "Wishlists",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Books_BookID",
                table: "Wishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Books_BookID1",
                table: "Wishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Users_UserID",
                table: "Wishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "Wishlists");

            migrationBuilder.RenameTable(
                name: "Wishlists",
                newName: "WishlistItems");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlists_UserID",
                table: "WishlistItems",
                newName: "IX_WishlistItems_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlists_BookID1",
                table: "WishlistItems",
                newName: "IX_WishlistItems_BookID1");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlists_BookID",
                table: "WishlistItems",
                newName: "IX_WishlistItems_BookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishlistItems",
                table: "WishlistItems",
                column: "WishlistID");

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_Books_BookID",
                table: "WishlistItems",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_Books_BookID1",
                table: "WishlistItems",
                column: "BookID1",
                principalTable: "Books",
                principalColumn: "BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_Users_UserID",
                table: "WishlistItems",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
