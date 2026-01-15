using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChecktonPld.DOM.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValidacionCurp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrimerApellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SegundoApellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    Genero = table.Column<int>(type: "int", nullable: false),
                    NombreEstado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurpGenerada = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    TipoCheckton = table.Column<int>(type: "int", nullable: false),
                    NombreServicioCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValidationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyToken = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestCaseID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidacionCurp", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValidacionCurp");
        }
    }
}
