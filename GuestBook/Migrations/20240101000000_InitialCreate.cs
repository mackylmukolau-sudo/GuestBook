using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuestBook.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id           = table.Column<int>(nullable: false)
                                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login        = table.Column<string>(maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: t => t.PrimaryKey("PK_Users", x => x.Id));

            migrationBuilder.CreateIndex(
                name:    "IX_Users_Login",
                table:   "Users",
                column:  "Login",
                unique:  true);

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id       = table.Column<int>(nullable: false)
                                    .Annotation("SqlServer:Identity", "1, 1"),
                    UserId   = table.Column<int>(nullable: false),
                    Text     = table.Column<string>(maxLength: 2000, nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: t =>
                {
                    t.PrimaryKey("PK_Messages", x => x.Id);
                    t.ForeignKey(
                        name:             "FK_Messages_Users_UserId",
                        column:           x => x.UserId,
                        principalTable:   "Users",
                        principalColumn:  "Id",
                        onDelete:         ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name:   "IX_Messages_UserId",
                table:  "Messages",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Messages");
            migrationBuilder.DropTable(name: "Users");
        }
    }
}
