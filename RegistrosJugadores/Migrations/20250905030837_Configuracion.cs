using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrosJugadores.Migrations
{
    /// <inheritdoc />
    public partial class Configuracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Jugadores",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_Nombre",
                table: "Jugadores",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jugadores_Nombre",
                table: "Jugadores");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Jugadores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
