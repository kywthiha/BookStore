using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class OrdersItemsCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Order_OrderID",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_OrderID",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Book");

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookID = table.Column<int>(nullable: false),
                    OrderID = table.Column<int>(nullable: false),
                    Qty = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderItem_Book_BookID",
                        column: x => x.BookID,
                        principalTable: "Book",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_BookID",
                table: "OrderItem",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderID",
                table: "OrderItem",
                column: "OrderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_OrderID",
                table: "Book",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Order_OrderID",
                table: "Book",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
