﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormGeneratorAPI.Migrations
{
    /// <inheritdoc />
    public partial class options_fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "FormElements",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                table: "FormElements");
        }
    }
}
