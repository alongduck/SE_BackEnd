using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyModel.Migrations
{
	/// <inheritdoc />
	public partial class _2024121011 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ProductDetails_Products_ProductId",
				table: "ProductDetails");

			migrationBuilder.DropIndex(
				name: "IX_Products_DetailId",
				table: "Products");

			migrationBuilder.DropIndex(
				name: "IX_ProductDetails_ProductId",
				table: "ProductDetails");

			migrationBuilder.DropColumn(
				name: "ProductId",
				table: "ProductDetails");

			migrationBuilder.CreateIndex(
				name: "IX_Products_DetailId",
				table: "Products",
				column: "DetailId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Products_DetailId",
				table: "Products");

			migrationBuilder.AddColumn<long>(
				name: "ProductId",
				table: "ProductDetails",
				type: "bigint",
				nullable: false,
				defaultValue: 0L);

			migrationBuilder.CreateIndex(
				name: "IX_Products_DetailId",
				table: "Products",
				column: "DetailId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ProductDetails_ProductId",
				table: "ProductDetails",
				column: "ProductId");

			migrationBuilder.AddForeignKey(
				name: "FK_ProductDetails_Products_ProductId",
				table: "ProductDetails",
				column: "ProductId",
				principalTable: "Products",
				principalColumn: "Id");
		}
	}
}