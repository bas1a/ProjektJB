using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingProgram.Migrations
{
    /// <inheritdoc />
    public partial class NewColomnIssueDateAndSaleDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceDate",
                table: "Invoices",
                newName: "SaleDate");

            migrationBuilder.AddColumn<DateOnly>(
                name: "IssueDate",
                table: "Invoices",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "SaleDate",
                table: "Invoices",
                newName: "InvoiceDate");
        }
    }
}
