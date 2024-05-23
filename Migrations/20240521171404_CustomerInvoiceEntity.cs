using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingProgram.Migrations
{
    /// <inheritdoc />
    public partial class CustomerInvoiceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Customers",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Customers",
                newName: "Street");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NIPNumber",
                table: "Customers",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "NIPNumber",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Customers",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Customers",
                newName: "ZipCode");
        }
    }
}
