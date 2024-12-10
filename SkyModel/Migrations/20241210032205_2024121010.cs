using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyModel.Migrations
{
	/// <inheritdoc />
	public partial class _2024121010 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Products_DetailId",
				table: "Products");

			migrationBuilder.AlterColumn<long>(
				name: "DetailId",
				table: "Products",
				type: "bigint",
				nullable: false,
				defaultValue: 0L,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true);

			migrationBuilder.AlterColumn<double>(
				name: "Width",
				table: "ProductDetails",
				type: "float",
				nullable: true,
				oldClrType: typeof(double),
				oldType: "float");

			migrationBuilder.AlterColumn<string>(
				name: "Structure",
				table: "ProductDetails",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<double>(
				name: "PricePerSquareMeter",
				table: "ProductDetails",
				type: "float",
				nullable: true,
				oldClrType: typeof(double),
				oldType: "float");

			migrationBuilder.AlterColumn<double>(
				name: "Length",
				table: "ProductDetails",
				type: "float",
				nullable: true,
				oldClrType: typeof(double),
				oldType: "float");

			migrationBuilder.AlterColumn<string>(
				name: "Features",
				table: "ProductDetails",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "Description",
				table: "ProductDetails",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<double>(
				name: "Area",
				table: "ProductDetails",
				type: "float",
				nullable: true,
				oldClrType: typeof(double),
				oldType: "float");

			migrationBuilder.AlterColumn<string>(
				name: "Address",
				table: "ProductDetails",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.CreateIndex(
				name: "IX_Products_DetailId",
				table: "Products",
				column: "DetailId",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Products_DetailId",
				table: "Products");

			migrationBuilder.AlterColumn<long>(
				name: "DetailId",
				table: "Products",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint");

			migrationBuilder.AlterColumn<double>(
				name: "Width",
				table: "ProductDetails",
				type: "float",
				nullable: false,
				defaultValue: 0.0,
				oldClrType: typeof(double),
				oldType: "float",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Structure",
				table: "ProductDetails",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<double>(
				name: "PricePerSquareMeter",
				table: "ProductDetails",
				type: "float",
				nullable: false,
				defaultValue: 0.0,
				oldClrType: typeof(double),
				oldType: "float",
				oldNullable: true);

			migrationBuilder.AlterColumn<double>(
				name: "Length",
				table: "ProductDetails",
				type: "float",
				nullable: false,
				defaultValue: 0.0,
				oldClrType: typeof(double),
				oldType: "float",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Features",
				table: "ProductDetails",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Description",
				table: "ProductDetails",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<double>(
				name: "Area",
				table: "ProductDetails",
				type: "float",
				nullable: false,
				defaultValue: 0.0,
				oldClrType: typeof(double),
				oldType: "float",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Address",
				table: "ProductDetails",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Products_DetailId",
				table: "Products",
				column: "DetailId",
				unique: true,
				filter: "[DetailId] IS NOT NULL");
		}
	}
}
