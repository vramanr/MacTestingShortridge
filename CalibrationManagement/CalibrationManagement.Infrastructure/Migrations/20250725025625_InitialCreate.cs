using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cal_setup",
                columns: table => new
                {
                    cal_setup_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cal_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    cal_std = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cal_std2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cal_setup", x => x.cal_setup_id);
                });

            migrationBuilder.CreateTable(
                name: "cal_techs",
                columns: table => new
                {
                    cal_tech_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tech_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tech_initials = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cal_techs", x => x.cal_tech_id);
                });

            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    co_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    co_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    address1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    state = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    zip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    fax = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    website = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    tax_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    terms = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    discount_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    credit_limit = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    balance = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.company_id);
                    table.UniqueConstraint("AK_company_co_id", x => x.co_id);
                });

            migrationBuilder.CreateTable(
                name: "tolerances",
                columns: table => new
                {
                    tolerance_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cal_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    range_min = table.Column<decimal>(type: "numeric(15,6)", precision: 15, scale: 6, nullable: true),
                    range_max = table.Column<decimal>(type: "numeric(15,6)", precision: 15, scale: 6, nullable: true),
                    tolerance_value = table.Column<decimal>(type: "numeric(15,6)", precision: 15, scale: 6, nullable: true),
                    tolerance_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    units = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tolerances", x => x.tolerance_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "cal_info",
                columns: table => new
                {
                    cal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cal_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    co_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    order_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    serial_no = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    model_no = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cal_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cal_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    cal_tech = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    cal_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    temperature = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    humidity = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    pressure = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cal_info", x => x.cal_id);
                    table.ForeignKey(
                        name: "FK_cal_info_company_co_id",
                        column: x => x.co_id,
                        principalTable: "company",
                        principalColumn: "co_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "contact",
                columns: table => new
                {
                    contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    co_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    primary_contact = table.Column<bool>(type: "boolean", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact", x => x.contact_id);
                    table.ForeignKey(
                        name: "FK_contact_company_co_id",
                        column: x => x.co_id,
                        principalTable: "company",
                        principalColumn: "co_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice",
                columns: table => new
                {
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    co_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    order_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    inv_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    subtotal = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    tax_amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    total_amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    paid_amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    balance = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    qa_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice", x => x.invoice_id);
                    table.UniqueConstraint("AK_invoice_invoice_no", x => x.invoice_no);
                    table.ForeignKey(
                        name: "FK_invoice_company_co_id",
                        column: x => x.co_id,
                        principalTable: "company",
                        principalColumn: "co_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "model_no",
                columns: table => new
                {
                    model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    serial_no = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    model_no = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    co_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    co_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    order_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    cal_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cal_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_model_no", x => x.model_id);
                    table.ForeignKey(
                        name: "FK_model_no_company_co_id",
                        column: x => x.co_id,
                        principalTable: "company",
                        principalColumn: "co_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ordrstat",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    co_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    order_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordrstat", x => x.order_id);
                    table.UniqueConstraint("AK_ordrstat_order_no", x => x.order_no);
                    table.ForeignKey(
                        name: "FK_ordrstat_company_co_id",
                        column: x => x.co_id,
                        principalTable: "company",
                        principalColumn: "co_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "cal_data",
                columns: table => new
                {
                    cal_data_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    mode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    set_point = table.Column<decimal>(type: "numeric(15,6)", precision: 15, scale: 6, nullable: true),
                    actual_reading = table.Column<decimal>(type: "numeric(15,6)", precision: 15, scale: 6, nullable: true),
                    deviation = table.Column<decimal>(type: "numeric(15,6)", precision: 15, scale: 6, nullable: true),
                    percent_deviation = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: true),
                    tolerance = table.Column<decimal>(type: "numeric(15,6)", precision: 15, scale: 6, nullable: true),
                    pass_fail = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    units = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    sequence_no = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cal_data", x => x.cal_data_id);
                    table.ForeignKey(
                        name: "FK_cal_data_cal_info_cal_id",
                        column: x => x.cal_id,
                        principalTable: "cal_info",
                        principalColumn: "cal_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cal_standards",
                columns: table => new
                {
                    cal_std_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    std_serial_no = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    std_model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    std_cal_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    std_due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    std_uncertainty = table.Column<decimal>(type: "numeric(15,6)", precision: 15, scale: 6, nullable: true),
                    std_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cal_standards", x => x.cal_std_id);
                    table.ForeignKey(
                        name: "FK_cal_standards_cal_info_cal_id",
                        column: x => x.cal_id,
                        principalTable: "cal_info",
                        principalColumn: "cal_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pay_dtl",
                columns: table => new
                {
                    payment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payment_amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    check_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pay_dtl", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_pay_dtl_invoice_invoice_no",
                        column: x => x.invoice_no,
                        principalTable: "invoice",
                        principalColumn: "invoice_no",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ordetail",
                columns: table => new
                {
                    order_detail_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    line_no = table.Column<int>(type: "integer", nullable: true),
                    part_no = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    extended_price = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordetail", x => x.order_detail_id);
                    table.ForeignKey(
                        name: "FK_ordetail_ordrstat_order_no",
                        column: x => x.order_no,
                        principalTable: "ordrstat",
                        principalColumn: "order_no",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cal_data_cal_id",
                table: "cal_data",
                column: "cal_id");

            migrationBuilder.CreateIndex(
                name: "IX_cal_info_cal_date",
                table: "cal_info",
                column: "cal_date");

            migrationBuilder.CreateIndex(
                name: "IX_cal_info_cal_no_unique",
                table: "cal_info",
                column: "cal_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cal_info_cal_type",
                table: "cal_info",
                column: "cal_type");

            migrationBuilder.CreateIndex(
                name: "IX_cal_info_co_id",
                table: "cal_info",
                column: "co_id");

            migrationBuilder.CreateIndex(
                name: "IX_cal_info_due_date",
                table: "cal_info",
                column: "due_date");

            migrationBuilder.CreateIndex(
                name: "IX_cal_info_serial_no",
                table: "cal_info",
                column: "serial_no");

            migrationBuilder.CreateIndex(
                name: "IX_cal_standards_cal_id",
                table: "cal_standards",
                column: "cal_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_co_id_unique",
                table: "company",
                column: "co_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_company_co_name",
                table: "company",
                column: "co_name");

            migrationBuilder.CreateIndex(
                name: "IX_contact_co_id",
                table: "contact",
                column: "co_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_co_id",
                table: "invoice",
                column: "co_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_invoice_no_unique",
                table: "invoice",
                column: "invoice_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_model_no_co_id",
                table: "model_no",
                column: "co_id");

            migrationBuilder.CreateIndex(
                name: "IX_model_no_serial_no_unique",
                table: "model_no",
                column: "serial_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ordetail_order_no",
                table: "ordetail",
                column: "order_no");

            migrationBuilder.CreateIndex(
                name: "IX_ordrstat_co_id",
                table: "ordrstat",
                column: "co_id");

            migrationBuilder.CreateIndex(
                name: "IX_ordrstat_order_no_unique",
                table: "ordrstat",
                column: "order_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pay_dtl_invoice_no",
                table: "pay_dtl",
                column: "invoice_no");

            migrationBuilder.CreateIndex(
                name: "IX_users_username_unique",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cal_data");

            migrationBuilder.DropTable(
                name: "cal_setup");

            migrationBuilder.DropTable(
                name: "cal_standards");

            migrationBuilder.DropTable(
                name: "cal_techs");

            migrationBuilder.DropTable(
                name: "contact");

            migrationBuilder.DropTable(
                name: "model_no");

            migrationBuilder.DropTable(
                name: "ordetail");

            migrationBuilder.DropTable(
                name: "pay_dtl");

            migrationBuilder.DropTable(
                name: "tolerances");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "cal_info");

            migrationBuilder.DropTable(
                name: "ordrstat");

            migrationBuilder.DropTable(
                name: "invoice");

            migrationBuilder.DropTable(
                name: "company");
        }
    }
}
