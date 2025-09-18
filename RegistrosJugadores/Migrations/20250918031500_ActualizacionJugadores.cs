using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistrosJugadores.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionJugadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Partidas",
                table: "Jugadores",
                newName: "Victorias");

            migrationBuilder.AddColumn<int>(
                name: "Derrotas",
                table: "Jugadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Empates",
                table: "Jugadores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Derrotas",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "Empates",
                table: "Jugadores");

            migrationBuilder.RenameColumn(
                name: "Victorias",
                table: "Jugadores",
                newName: "Partidas");
        }
    }
}
