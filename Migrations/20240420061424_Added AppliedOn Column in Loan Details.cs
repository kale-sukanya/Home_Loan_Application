using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseStudyFinal.Migrations
{
    public partial class AddedAppliedOnColumninLoanDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AppliedOn",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedOn",
                table: "Loans");
        }
    }
}
