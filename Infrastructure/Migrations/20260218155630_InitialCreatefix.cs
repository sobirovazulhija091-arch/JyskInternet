using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_UserP_UserId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_UserP_UserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserP_UserId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserP",
                table: "UserP");

            migrationBuilder.RenameTable(
                name: "UserP",
                newName: "UsersP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersP",
                table: "UsersP",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_UsersP_UserId",
                table: "CartItems",
                column: "UserId",
                principalTable: "UsersP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_UsersP_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "UsersP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UsersP_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "UsersP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_UsersP_UserId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_UsersP_UserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UsersP_UserId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersP",
                table: "UsersP");

            migrationBuilder.RenameTable(
                name: "UsersP",
                newName: "UserP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserP",
                table: "UserP",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_UserP_UserId",
                table: "CartItems",
                column: "UserId",
                principalTable: "UserP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_UserP_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "UserP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserP_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "UserP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
