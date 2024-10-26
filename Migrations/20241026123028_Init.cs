using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseStudyFinal.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    ApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimateAmount = table.Column<int>(type: "int", nullable: false),
                    TypeOfEmployment = table.Column<string>(type: "varchar(10)", nullable: false),
                    RetirementAge = table.Column<int>(type: "int", nullable: false),
                    OrganizationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NetSalary = table.Column<int>(type: "int", nullable: false),
                    MaxLoanAmountGrantable = table.Column<double>(type: "float", nullable: false),
                    InterestRate = table.Column<double>(type: "float", nullable: false),
                    LoanAmount = table.Column<int>(type: "int", nullable: false),
                    Tenure = table.Column<int>(type: "int", nullable: false),
                    AppliedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "Registers",
                columns: table => new
                {
                    EmailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(10)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.EmailId);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanDetailsApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Loans_LoanDetailsApplicationId",
                        column: x => x.LoanDetailsApplicationId,
                        principalTable: "Loans",
                        principalColumn: "ApplicationId");
                });

            migrationBuilder.CreateTable(
                name: "PersonalDetails",
                columns: table => new
                {
                    AadharNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PanNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoanDetailsApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RegisterEmailId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetails", x => x.AadharNo);
                    table.ForeignKey(
                        name: "FK_PersonalDetails_Loans_LoanDetailsApplicationId",
                        column: x => x.LoanDetailsApplicationId,
                        principalTable: "Loans",
                        principalColumn: "ApplicationId");
                    table.ForeignKey(
                        name: "FK_PersonalDetails_Registers_RegisterEmailId",
                        column: x => x.RegisterEmailId,
                        principalTable: "Registers",
                        principalColumn: "EmailId");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTracking",
                columns: table => new
                {
                    TrackerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AadharNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanDetailsApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PersonalDetailsAadharNo = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTracking", x => x.TrackerID);
                    table.ForeignKey(
                        name: "FK_ApplicationTracking_Loans_LoanDetailsApplicationId",
                        column: x => x.LoanDetailsApplicationId,
                        principalTable: "Loans",
                        principalColumn: "ApplicationId");
                    table.ForeignKey(
                        name: "FK_ApplicationTracking_PersonalDetails_PersonalDetailsAadharNo",
                        column: x => x.PersonalDetailsAadharNo,
                        principalTable: "PersonalDetails",
                        principalColumn: "AadharNo");
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrackerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicationTrackingTrackerID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountNumber);
                    table.ForeignKey(
                        name: "FK_Accounts_ApplicationTracking_ApplicationTrackingTrackerID",
                        column: x => x.ApplicationTrackingTrackerID,
                        principalTable: "ApplicationTracking",
                        principalColumn: "TrackerID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ApplicationTrackingTrackerID",
                table: "Accounts",
                column: "ApplicationTrackingTrackerID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTracking_LoanDetailsApplicationId",
                table: "ApplicationTracking",
                column: "LoanDetailsApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTracking_PersonalDetailsAadharNo",
                table: "ApplicationTracking",
                column: "PersonalDetailsAadharNo");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_LoanDetailsApplicationId",
                table: "Documents",
                column: "LoanDetailsApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetails_LoanDetailsApplicationId",
                table: "PersonalDetails",
                column: "LoanDetailsApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetails_RegisterEmailId",
                table: "PersonalDetails",
                column: "RegisterEmailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "ApplicationTracking");

            migrationBuilder.DropTable(
                name: "PersonalDetails");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Registers");
        }
    }
}
