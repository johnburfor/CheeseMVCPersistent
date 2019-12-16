using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CheeseMVC.Migrations
{
    public partial class AddMenu2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheeseMenus_Categories_CategoryID",
                table: "CheeseMenus");

            migrationBuilder.DropIndex(
                name: "IX_CheeseMenus_CategoryID",
                table: "CheeseMenus");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "CheeseMenus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "CheeseMenus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheeseMenus_CategoryID",
                table: "CheeseMenus",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_CheeseMenus_Categories_CategoryID",
                table: "CheeseMenus",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
