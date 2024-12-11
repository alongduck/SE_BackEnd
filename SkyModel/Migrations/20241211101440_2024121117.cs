using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyModel.Migrations
{
	/// <inheritdoc />
	public partial class _2024121117 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_MinIOs_NewsArticles_NewsArticleId",
				table: "MinIOs");

			migrationBuilder.DropIndex(
				name: "IX_MinIOs_NewsArticleId",
				table: "MinIOs");

			migrationBuilder.DropColumn(
				name: "Thumbnail",
				table: "Products");

			migrationBuilder.DropColumn(
				name: "NewsArticleId",
				table: "MinIOs");

			migrationBuilder.AddColumn<long>(
				name: "ThumbnailImageId",
				table: "Products",
				type: "bigint",
				nullable: true);

			migrationBuilder.AddColumn<long>(
				name: "ThumbnailImageId",
				table: "NewsArticles",
				type: "bigint",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Products_ThumbnailImageId",
				table: "Products",
				column: "ThumbnailImageId");

			migrationBuilder.CreateIndex(
				name: "IX_NewsArticles_ThumbnailImageId",
				table: "NewsArticles",
				column: "ThumbnailImageId");

			migrationBuilder.AddForeignKey(
				name: "FK_NewsArticles_MinIOs_ThumbnailImageId",
				table: "NewsArticles",
				column: "ThumbnailImageId",
				principalTable: "MinIOs",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Products_MinIOs_ThumbnailImageId",
				table: "Products",
				column: "ThumbnailImageId",
				principalTable: "MinIOs",
				principalColumn: "Id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_NewsArticles_MinIOs_ThumbnailImageId",
				table: "NewsArticles");

			migrationBuilder.DropForeignKey(
				name: "FK_Products_MinIOs_ThumbnailImageId",
				table: "Products");

			migrationBuilder.DropIndex(
				name: "IX_Products_ThumbnailImageId",
				table: "Products");

			migrationBuilder.DropIndex(
				name: "IX_NewsArticles_ThumbnailImageId",
				table: "NewsArticles");

			migrationBuilder.DropColumn(
				name: "ThumbnailImageId",
				table: "Products");

			migrationBuilder.DropColumn(
				name: "ThumbnailImageId",
				table: "NewsArticles");

			migrationBuilder.AddColumn<string>(
				name: "Thumbnail",
				table: "Products",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.AddColumn<long>(
				name: "NewsArticleId",
				table: "MinIOs",
				type: "bigint",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_MinIOs_NewsArticleId",
				table: "MinIOs",
				column: "NewsArticleId");

			migrationBuilder.AddForeignKey(
				name: "FK_MinIOs_NewsArticles_NewsArticleId",
				table: "MinIOs",
				column: "NewsArticleId",
				principalTable: "NewsArticles",
				principalColumn: "Id");
		}
	}
}
