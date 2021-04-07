using Microsoft.EntityFrameworkCore.Migrations;

namespace DnDungeons.Migrations
{
    public partial class MediaColumnRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Filetype",
                table: "Media",
                newName: "FileType");

            migrationBuilder.RenameColumn(
                name: "Filepath",
                table: "Media",
                newName: "FileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "Media",
                newName: "Filetype");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Media",
                newName: "Filepath");
        }
    }
}
