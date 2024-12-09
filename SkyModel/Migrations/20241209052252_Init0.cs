using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace SkyModel.Migrations
{
	/// <inheritdoc />
	public partial class Init0 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Categories",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categories", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "NewsArticles",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					TimeUp = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_NewsArticles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					Role = table.Column<int>(type: "int", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "MinIOs",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false),
					FileName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
					ETag = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Size = table.Column<long>(type: "bigint", nullable: false),
					TimeUp = table.Column<DateTime>(type: "datetime2", nullable: false),
					ProductDetailId = table.Column<long>(type: "bigint", nullable: true),
					NewsArticleId = table.Column<long>(type: "bigint", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MinIOs", x => x.Id);
					table.ForeignKey(
						name: "FK_MinIOs_NewsArticles_NewsArticleId",
						column: x => x.NewsArticleId,
						principalTable: "NewsArticles",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "ProductDetails",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
					PricePerSquareMeter = table.Column<double>(type: "float", nullable: false),
					Features = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Area = table.Column<double>(type: "float", nullable: false),
					Length = table.Column<double>(type: "float", nullable: false),
					Width = table.Column<double>(type: "float", nullable: false),
					Structure = table.Column<string>(type: "nvarchar(max)", nullable: false),
					ProductId = table.Column<long>(type: "bigint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ProductDetails", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Products",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Price = table.Column<double>(type: "float", nullable: false),
					Hot = table.Column<bool>(type: "bit", nullable: false),
					TimeUp = table.Column<DateTime>(type: "datetime2", nullable: false),
					CategoryId = table.Column<long>(type: "bigint", nullable: false),
					ProductDetailId = table.Column<long>(type: "bigint", nullable: false),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Products", x => x.Id);
					table.ForeignKey(
						name: "FK_Products_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Products_ProductDetails_ProductDetailId",
						column: x => x.ProductDetailId,
						principalTable: "ProductDetails",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Products_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_MinIOs_NewsArticleId",
				table: "MinIOs",
				column: "NewsArticleId");

			migrationBuilder.CreateIndex(
				name: "IX_MinIOs_ProductDetailId",
				table: "MinIOs",
				column: "ProductDetailId");

			migrationBuilder.CreateIndex(
				name: "IX_ProductDetails_ProductId",
				table: "ProductDetails",
				column: "ProductId");

			migrationBuilder.CreateIndex(
				name: "IX_Products_CategoryId",
				table: "Products",
				column: "CategoryId");

			migrationBuilder.CreateIndex(
				name: "IX_Products_Name",
				table: "Products",
				column: "Name");

			migrationBuilder.CreateIndex(
				name: "IX_Products_ProductDetailId",
				table: "Products",
				column: "ProductDetailId");

			migrationBuilder.CreateIndex(
				name: "IX_Products_UserId",
				table: "Products",
				column: "UserId");

			migrationBuilder.AddForeignKey(
				name: "FK_MinIOs_ProductDetails_ProductDetailId",
				table: "MinIOs",
				column: "ProductDetailId",
				principalTable: "ProductDetails",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_ProductDetails_Products_ProductId",
				table: "ProductDetails",
				column: "ProductId",
				principalTable: "Products",
				principalColumn: "Id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Products_ProductDetails_ProductDetailId",
				table: "Products");

			migrationBuilder.DropTable(
				name: "MinIOs");

			migrationBuilder.DropTable(
				name: "NewsArticles");

			migrationBuilder.DropTable(
				name: "ProductDetails");

			migrationBuilder.DropTable(
				name: "Products");

			migrationBuilder.DropTable(
				name: "Categories");

			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
