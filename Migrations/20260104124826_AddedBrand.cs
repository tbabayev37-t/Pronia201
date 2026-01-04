using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCProniaTask.Migrations
{
    public partial class AddedBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Brands table yarat
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                              .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            // 2️⃣ Products cədvəlinə BrandId column əlavə et (nullable)
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Products",
                nullable: true);

            // 3️⃣ Index əlavə et
            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            // 4️⃣ FK əlavə et (mövcud Products NULL qalsın deyə Restrict)
            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // geri qaytarma əməliyyatları
            migrationBuilder.DropForeignKey(name: "FK_Products_Brands_BrandId", table: "Products");
            migrationBuilder.DropIndex(name: "IX_Products_BrandId", table: "Products");
            migrationBuilder.DropColumn(name: "BrandId", table: "Products");
            migrationBuilder.DropTable(name: "Brands");
        }
    }
}
