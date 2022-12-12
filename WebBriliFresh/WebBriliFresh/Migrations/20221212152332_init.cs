using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBriliFresh.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discount_All",
                columns: table => new
                {
                    DisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<double>(type: "float", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Discount__E2AA7E64251E5A96", x => x.DisID);
                });

            migrationBuilder.CreateTable(
                name: "Discount_Order",
                columns: table => new
                {
                    DisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisRate = table.Column<double>(type: "float", nullable: false),
                    MaxDis = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CusType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Discount__E2AA7E6467B44496", x => x.DisID);
                });

            migrationBuilder.CreateTable(
                name: "Reward",
                columns: table => new
                {
                    RewardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CusType = table.Column<int>(type: "int", nullable: true),
                    Point = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reward__82501599E3AAB6E9", x => x.RewardID);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    StoreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecificAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Store__3B82F0E1CFE80ADD", x => x.StoreID);
                });

            migrationBuilder.CreateTable(
                name: "Transport",
                columns: table => new
                {
                    TransID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShippingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Transporter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transpor__9E5DDB1C2C5F3C11", x => x.TransID);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Type__516F0395E73FD56F", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserRole = table.Column<int>(type: "int", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__1788CCAC6C9E8B5F", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discount_Store",
                columns: table => new
                {
                    DisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Discount__E2AA7E642ADFC807", x => x.DisID);
                    table.ForeignKey(
                        name: "fk_Discount_Store_1",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discount_Type",
                columns: table => new
                {
                    DisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeID = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Discount__E2AA7E646325E4F3", x => x.DisID);
                    table.ForeignKey(
                        name: "fk_Discount_Type_1",
                        column: x => x.TypeID,
                        principalTable: "Type",
                        principalColumn: "TypeID");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TypeID = table.Column<int>(type: "int", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__620295F03EE0F206", x => x.ProID);
                    table.ForeignKey(
                        name: "fk_Product_1",
                        column: x => x.TypeID,
                        principalTable: "Type",
                        principalColumn: "TypeID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    RewardID = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__2F187130CC48DD9C", x => x.CusID);
                    table.ForeignKey(
                        name: "fk_Customer_1",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_Customer_2",
                        column: x => x.RewardID,
                        principalTable: "Reward",
                        principalColumn: "RewardID");
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmpID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    StoreID = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecificAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__AF2DBA79D9282422", x => x.EmpID);
                    table.ForeignKey(
                        name: "fk_Employee_1",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_Employee_2",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID");
                });

            migrationBuilder.CreateTable(
                name: "Discount_Product",
                columns: table => new
                {
                    DisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Discount__E2AA7E64283CAB89", x => x.DisID);
                    table.ForeignKey(
                        name: "fk_Discount_Product_1",
                        column: x => x.ProID,
                        principalTable: "Product",
                        principalColumn: "ProID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Image",
                columns: table => new
                {
                    ImgID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProID = table.Column<int>(type: "int", nullable: true),
                    ImgData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product___352F54130CD1158A", x => x.ImgID);
                    table.ForeignKey(
                        name: "fk_Product_Image_1",
                        column: x => x.ProID,
                        principalTable: "Product",
                        principalColumn: "ProID");
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    StoreID = table.Column<int>(type: "int", nullable: false),
                    ProID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Stock__2DA2D9BE717269A2", x => new { x.StoreID, x.ProID });
                    table.ForeignKey(
                        name: "fk_Stock_1",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID");
                    table.ForeignKey(
                        name: "fk_Stock_2",
                        column: x => x.ProID,
                        principalTable: "Product",
                        principalColumn: "ProID");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CusID = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Default = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Address__A0E1ADEE5C6A8975", x => x.AddID);
                    table.ForeignKey(
                        name: "fk_Address_1",
                        column: x => x.CusID,
                        principalTable: "Customer",
                        principalColumn: "CusID");
                });

            migrationBuilder.CreateTable(
                name: "QnA",
                columns: table => new
                {
                    QnAID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReplyID = table.Column<int>(type: "int", nullable: true),
                    ProID = table.Column<int>(type: "int", nullable: true),
                    CusID = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Like = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QnA__C4DF8B2922DE5FDF", x => x.QnAID);
                    table.ForeignKey(
                        name: "fk_QnA_1",
                        column: x => x.ReplyID,
                        principalTable: "QnA",
                        principalColumn: "QnAID");
                    table.ForeignKey(
                        name: "fk_QnA_2",
                        column: x => x.ProID,
                        principalTable: "Product",
                        principalColumn: "ProID");
                    table.ForeignKey(
                        name: "fk_QnA_3",
                        column: x => x.CusID,
                        principalTable: "Customer",
                        principalColumn: "CusID");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddID = table.Column<int>(type: "int", nullable: true),
                    TransID = table.Column<int>(type: "int", nullable: true),
                    DisID = table.Column<int>(type: "int", nullable: true),
                    StoreID = table.Column<int>(type: "int", nullable: true),
                    CusID = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OrderTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayBy = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__C3905BAF1691363C", x => x.OrderID);
                    table.ForeignKey(
                        name: "fk_Order_1",
                        column: x => x.AddID,
                        principalTable: "Address",
                        principalColumn: "AddID");
                    table.ForeignKey(
                        name: "fk_Order_2",
                        column: x => x.TransID,
                        principalTable: "Transport",
                        principalColumn: "TransID");
                    table.ForeignKey(
                        name: "fk_Order_3",
                        column: x => x.DisID,
                        principalTable: "Discount_Order",
                        principalColumn: "DisID");
                    table.ForeignKey(
                        name: "fk_Order_4",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID");
                    table.ForeignKey(
                        name: "fk_Order_5",
                        column: x => x.CusID,
                        principalTable: "Customer",
                        principalColumn: "CusID");
                });

            migrationBuilder.CreateTable(
                name: "QnA_Image",
                columns: table => new
                {
                    QnAImgID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QnAID = table.Column<int>(type: "int", nullable: true),
                    ImgData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QnA_Imag__1924FF469328EBC5", x => x.QnAImgID);
                    table.ForeignKey(
                        name: "fk_QnA_Image_1",
                        column: x => x.QnAID,
                        principalTable: "QnA",
                        principalColumn: "QnAID");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FbID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReplyID = table.Column<int>(type: "int", nullable: true),
                    ProID = table.Column<int>(type: "int", nullable: true),
                    CusID = table.Column<int>(type: "int", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Rate = table.Column<int>(type: "int", nullable: true),
                    Like = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__36769D6C67C84AAA", x => x.FbID);
                    table.ForeignKey(
                        name: "fk_Feedback_1",
                        column: x => x.ReplyID,
                        principalTable: "Feedback",
                        principalColumn: "FbID");
                    table.ForeignKey(
                        name: "fk_Feedback_2",
                        column: x => x.ProID,
                        principalTable: "Product",
                        principalColumn: "ProID");
                    table.ForeignKey(
                        name: "fk_Feedback_3",
                        column: x => x.CusID,
                        principalTable: "Customer",
                        principalColumn: "CusID");
                    table.ForeignKey(
                        name: "fk_Feedback_4",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID");
                });

            migrationBuilder.CreateTable(
                name: "Order_Details",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order_De__D5B072F06219988F", x => new { x.OrderID, x.ProID });
                    table.ForeignKey(
                        name: "fk_Order_Details_1",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "fk_Order_Details_2",
                        column: x => x.ProID,
                        principalTable: "Product",
                        principalColumn: "ProID");
                });

            migrationBuilder.CreateTable(
                name: "Feedback_Image",
                columns: table => new
                {
                    FbImgID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FbID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__05FB6A30A82988FC", x => x.FbImgID);
                    table.ForeignKey(
                        name: "fk_Feedback_Image_1",
                        column: x => x.FbID,
                        principalTable: "Feedback",
                        principalColumn: "FbID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CusID",
                table: "Address",
                column: "CusID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_RewardID",
                table: "Customer",
                column: "RewardID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserID",
                table: "Customer",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_Product_ProID",
                table: "Discount_Product",
                column: "ProID");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_Store_StoreID",
                table: "Discount_Store",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_Type_TypeID",
                table: "Discount_Type",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_StoreID",
                table: "Employee",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserID",
                table: "Employee",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_CusID",
                table: "Feedback",
                column: "CusID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_OrderID",
                table: "Feedback",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ProID",
                table: "Feedback",
                column: "ProID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ReplyID",
                table: "Feedback",
                column: "ReplyID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_Image_FbID",
                table: "Feedback_Image",
                column: "FbID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AddID",
                table: "Order",
                column: "AddID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CusID",
                table: "Order",
                column: "CusID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DisID",
                table: "Order",
                column: "DisID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StoreID",
                table: "Order",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TransID",
                table: "Order",
                column: "TransID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Details_ProID",
                table: "Order_Details",
                column: "ProID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_TypeID",
                table: "Product",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Image_ProID",
                table: "Product_Image",
                column: "ProID");

            migrationBuilder.CreateIndex(
                name: "IX_QnA_CusID",
                table: "QnA",
                column: "CusID");

            migrationBuilder.CreateIndex(
                name: "IX_QnA_ProID",
                table: "QnA",
                column: "ProID");

            migrationBuilder.CreateIndex(
                name: "IX_QnA_ReplyID",
                table: "QnA",
                column: "ReplyID");

            migrationBuilder.CreateIndex(
                name: "IX_QnA_Image_QnAID",
                table: "QnA_Image",
                column: "QnAID");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ProID",
                table: "Stock",
                column: "ProID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Discount_All");

            migrationBuilder.DropTable(
                name: "Discount_Product");

            migrationBuilder.DropTable(
                name: "Discount_Store");

            migrationBuilder.DropTable(
                name: "Discount_Type");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Feedback_Image");

            migrationBuilder.DropTable(
                name: "Order_Details");

            migrationBuilder.DropTable(
                name: "Product_Image");

            migrationBuilder.DropTable(
                name: "QnA_Image");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "QnA");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Transport");

            migrationBuilder.DropTable(
                name: "Discount_Order");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Reward");
        }
    }
}
