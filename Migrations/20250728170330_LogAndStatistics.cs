using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoEASy.Migrations
{
    /// <inheritdoc />
    public partial class LogAndStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__AccessLog__UserI__114A936A",
                table: "AccessLog");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogComme__BlogI__236943A5",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogComme__UserI__245D67DE",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogDetai__BlogI__1F98B2C1",
                table: "BlogDetails");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogImage__BlogI__29221CFB",
                table: "BlogImages");

            migrationBuilder.DropForeignKey(
                name: "FK__Blogs__AuthorAdm__18EBB532",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK__Blogs__AuthorUse__17F790F9",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK__Blogs__CategoryI__19DFD96B",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogTagMa__BlogI__30C33EC3",
                table: "BlogTagMapping");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogTagMa__TagID__31B762FC",
                table: "BlogTagMapping");

            migrationBuilder.DropForeignKey(
                name: "FK__Bookings__Discou__778AC167",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK__Bookings__TourID__76969D2E",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK__Bookings__UserID__75A278F5",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK__Companion__UserI__367C1819",
                table: "Companions");

            migrationBuilder.DropForeignKey(
                name: "FK__Favorites__TourI__09A971A2",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK__Favorites__UserI__08B54D69",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK__Notificat__UserI__47A6A41B",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK__Payments__Bookin__7C4F7684",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK__Reviews__TourID__02084FDA",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK__Reviews__UserID__01142BA1",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK__Schedules__TourI__656C112C",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK__TourDetai__TourI__3B40CD36",
                table: "TourDetails");

            migrationBuilder.DropForeignKey(
                name: "FK__TourFAQs__TourID__42E1EEFE",
                table: "TourFAQs");

            migrationBuilder.DropForeignKey(
                name: "FK__TourImage__TourI__628FA481",
                table: "TourImages");

            migrationBuilder.DropForeignKey(
                name: "FK__TourItine__TourI__3F115E1A",
                table: "TourItineraries");

            migrationBuilder.DropForeignKey(
                name: "FK__Tours__CategoryI__5DCAEF64",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK__Tours__CreatedBy__5BE2A6F2",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK__Tours__Destinati__5CD6CB2B",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK__VIPPointH__UserI__0D7A0286",
                table: "VIPPointHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK__VIPPoint__4D7B4ADD00D970C3",
                table: "VIPPointHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Users__1788CCACF72813F8",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Tours__604CEA10C292330F",
                table: "Tours");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourItin__361216A6B2E37400",
                table: "TourItineraries");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourImag__7516F4EC3FE9F5B0",
                table: "TourImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourFAQs__4B89D1E2AA962951",
                table: "TourFAQs");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourDeta__5055BCFC4F3EBFE2",
                table: "TourDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourCate__19093A2B59400476",
                table: "TourCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Schedule__9C8A5B696B2581C2",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Rules__110458E2CA33AFC8",
                table: "Rules");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Roles__8AFACE3AD5502F2C",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Reviews__74BC79AEE3A98EAA",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Payments__9B556A583D71F123",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Notifica__20CF2E12B5A7FFA5",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Favorite__018C020D2A7422D0",
                table: "Favorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Discount__E43F6DF60725D9D5",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Destinat__DB5FE4AC0E1447D3",
                table: "Destinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Destinat__7516F4ECDC4B0B39",
                table: "DestinationImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Companio__8B53BE8BB4D8595E",
                table: "Companions");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Bookings__73951ACDFA0E4E79",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogTags__657CFA4CBAFE00EC",
                table: "BlogTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogTagM__826051F465023A30",
                table: "BlogTagMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Blogs__54379E5054BD6FC5",
                table: "Blogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogImag__7516F4EC54DE70B5",
                table: "BlogImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogDeta__2383E81E48705695",
                table: "BlogDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogComm__C3B4DFAA9DEABC40",
                table: "BlogComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Admins__719FE4E8103D4241",
                table: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Actions__FFE3F4D96FF455FB",
                table: "Actions");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AccessLo__5E5499A829083A81",
                table: "AccessLog");

            migrationBuilder.RenameIndex(
                name: "UQ__Users__A9D1053449FFE89D",
                table: "Users",
                newName: "UQ__Users__A9D105348693C07F");

            migrationBuilder.RenameIndex(
                name: "UQ__Users__536C85E462158B79",
                table: "Users",
                newName: "UQ__Users__536C85E4240F0354");

            migrationBuilder.RenameIndex(
                name: "UQ__TourDeta__604CEA11FC75AA34",
                table: "TourDetails",
                newName: "UQ__TourDeta__604CEA11C9A3D5D6");

            migrationBuilder.RenameIndex(
                name: "UQ__TourCate__8517B2E0BF7EDA02",
                table: "TourCategories",
                newName: "UQ__TourCate__8517B2E005F3C24C");

            migrationBuilder.RenameIndex(
                name: "UQ__Rules__BC7B5FB673C7CE77",
                table: "Rules",
                newName: "UQ__Rules__BC7B5FB60152F65E");

            migrationBuilder.RenameIndex(
                name: "UQ__Roles__8A2B61601238DB49",
                table: "Roles",
                newName: "UQ__Roles__8A2B6160E55A842F");

            migrationBuilder.RenameIndex(
                name: "UQ__Discount__A25C5AA75290125B",
                table: "Discounts",
                newName: "UQ__Discount__A25C5AA77F6F5670");

            migrationBuilder.RenameIndex(
                name: "UQ__BlogTags__737584F6AEFE61A6",
                table: "BlogTags",
                newName: "UQ__BlogTags__737584F6C7DCC467");

            migrationBuilder.RenameIndex(
                name: "UQ__BlogDeta__54379E51A558E9D1",
                table: "BlogDetails",
                newName: "UQ__BlogDeta__54379E519D2CECE0");

            migrationBuilder.RenameIndex(
                name: "UQ__Admins__A9D1053452D3FF2D",
                table: "Admins",
                newName: "UQ__Admins__A9D10534F3ACDEC4");

            migrationBuilder.RenameIndex(
                name: "UQ__Admins__536C85E4EC6305B7",
                table: "Admins",
                newName: "UQ__Admins__536C85E4370D36E6");

            migrationBuilder.RenameIndex(
                name: "UQ__Actions__3148953D345B849D",
                table: "Actions",
                newName: "UQ__Actions__3148953D6EAEC540");

            migrationBuilder.AddPrimaryKey(
                name: "PK__VIPPoint__4D7B4ADD9AEB3685",
                table: "VIPPointHistory",
                column: "HistoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Users__1788CCACA986BA0B",
                table: "Users",
                column: "UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Tours__604CEA109ED306E5",
                table: "Tours",
                column: "TourID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourItin__361216A643F3D5DB",
                table: "TourItineraries",
                column: "ItineraryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourImag__7516F4EC6D54EB2C",
                table: "TourImages",
                column: "ImageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourFAQs__4B89D1E29DD13A3D",
                table: "TourFAQs",
                column: "FAQID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourDeta__5055BCFC2BC8AA78",
                table: "TourDetails",
                column: "TourDetailID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourCate__19093A2B9A58ECFD",
                table: "TourCategories",
                column: "CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Schedule__9C8A5B693C60215D",
                table: "Schedules",
                column: "ScheduleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Rules__110458E23D463170",
                table: "Rules",
                column: "RuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Roles__8AFACE3AC60AA4DD",
                table: "Roles",
                column: "RoleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Reviews__74BC79AE1781A59E",
                table: "Reviews",
                column: "ReviewID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Payments__9B556A588D559114",
                table: "Payments",
                column: "PaymentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Notifica__20CF2E12C3DD730E",
                table: "Notifications",
                column: "NotificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Favorite__018C020DB0FD2F72",
                table: "Favorites",
                columns: new[] { "UserID", "TourID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__Discount__E43F6DF60F0748A1",
                table: "Discounts",
                column: "DiscountID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Destinat__DB5FE4AC0F8388C0",
                table: "Destinations",
                column: "DestinationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Destinat__7516F4ECAC84A0A5",
                table: "DestinationImages",
                column: "ImageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Companio__8B53BE8BF2A6B57E",
                table: "Companions",
                column: "CompanionID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Bookings__73951ACD7995049D",
                table: "Bookings",
                column: "BookingID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogTags__657CFA4CD3671040",
                table: "BlogTags",
                column: "TagID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogTagM__826051F48E5A3A93",
                table: "BlogTagMapping",
                columns: new[] { "BlogID", "TagID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__Blogs__54379E50DF1D11DB",
                table: "Blogs",
                column: "BlogID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogImag__7516F4EC50ABB3E6",
                table: "BlogImages",
                column: "ImageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogDeta__2383E81EBB0A289E",
                table: "BlogDetails",
                column: "BlogDetailID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogComm__C3B4DFAAAA8338B9",
                table: "BlogComments",
                column: "CommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Admins__719FE4E80D073614",
                table: "Admins",
                column: "AdminID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Actions__FFE3F4D9CB247AFF",
                table: "Actions",
                column: "ActionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AccessLo__5E5499A85C951DF8",
                table: "AccessLog",
                column: "LogID");

            migrationBuilder.CreateTable(
                name: "SemanticQueries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TopK = table.Column<int>(type: "int", nullable: false, defaultValue: 5)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Semantic__3214EC276AF0C059", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TourDtos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TourDtos__3214EC27A24F8788", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TourPolicies",
                columns: table => new
                {
                    PolicyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourID = table.Column<int>(type: "int", nullable: false),
                    PolicyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PolicyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PolicyDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PolicyValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TourPoli__PolicyID", x => x.PolicyID);
                    table.ForeignKey(
                        name: "FK__TourPolicies__TourID",
                        column: x => x.TourID,
                        principalTable: "Tours",
                        principalColumn: "TourID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourViewLogs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ViewTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TourView__3214EC27BB638F86", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TourPolicies_TourID",
                table: "TourPolicies",
                column: "TourID");

            migrationBuilder.AddForeignKey(
                name: "FK__AccessLog__UserI__10566F31",
                table: "AccessLog",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__BlogComme__BlogI__15DA3E5D",
                table: "BlogComments",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "BlogID");

            migrationBuilder.AddForeignKey(
                name: "FK__BlogComme__UserI__16CE6296",
                table: "BlogComments",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__BlogDetai__BlogI__1209AD79",
                table: "BlogDetails",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__BlogImage__BlogI__1B9317B3",
                table: "BlogImages",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Blogs__AuthorAdm__0B5CAFEA",
                table: "Blogs",
                column: "AuthorAdminID",
                principalTable: "Admins",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK__Blogs__AuthorUse__0A688BB1",
                table: "Blogs",
                column: "AuthorUserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Blogs__CategoryI__0C50D423",
                table: "Blogs",
                column: "CategoryID",
                principalTable: "TourCategories",
                principalColumn: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK__BlogTagMa__BlogI__2334397B",
                table: "BlogTagMapping",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__BlogTagMa__TagID__24285DB4",
                table: "BlogTagMapping",
                column: "TagID",
                principalTable: "BlogTags",
                principalColumn: "TagID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Bookings__Discou__76969D2E",
                table: "Bookings",
                column: "DiscountID",
                principalTable: "Discounts",
                principalColumn: "DiscountID");

            migrationBuilder.AddForeignKey(
                name: "FK__Bookings__TourID__75A278F5",
                table: "Bookings",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID");

            migrationBuilder.AddForeignKey(
                name: "FK__Bookings__UserID__74AE54BC",
                table: "Bookings",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Companion__UserI__1BC821DD",
                table: "Companions",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Favorites__TourI__08B54D69",
                table: "Favorites",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Favorites__UserI__07C12930",
                table: "Favorites",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Payments__Bookin__7B5B524B",
                table: "Payments",
                column: "BookingID",
                principalTable: "Bookings",
                principalColumn: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK__Reviews__TourID__01142BA1",
                table: "Reviews",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID");

            migrationBuilder.AddForeignKey(
                name: "FK__Reviews__UserID__00200768",
                table: "Reviews",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Schedules__TourI__6477ECF3",
                table: "Schedules",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__TourDetai__TourI__5BAD9CC8",
                table: "TourDetails",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__TourFAQs__TourID__634EBE90",
                table: "TourFAQs",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__TourImage__TourI__619B8048",
                table: "TourImages",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__TourItine__TourI__5F7E2DAC",
                table: "TourItineraries",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Tours__CategoryI__5CD6CB2B",
                table: "Tours",
                column: "CategoryID",
                principalTable: "TourCategories",
                principalColumn: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK__Tours__CreatedBy__5AEE82B9",
                table: "Tours",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Tours__Destinati__5BE2A6F2",
                table: "Tours",
                column: "DestinationID",
                principalTable: "Destinations",
                principalColumn: "DestinationID");

            migrationBuilder.AddForeignKey(
                name: "FK__VIPPointH__UserI__0C85DE4D",
                table: "VIPPointHistory",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__AccessLog__UserI__10566F31",
                table: "AccessLog");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogComme__BlogI__15DA3E5D",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogComme__UserI__16CE6296",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogDetai__BlogI__1209AD79",
                table: "BlogDetails");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogImage__BlogI__1B9317B3",
                table: "BlogImages");

            migrationBuilder.DropForeignKey(
                name: "FK__Blogs__AuthorAdm__0B5CAFEA",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK__Blogs__AuthorUse__0A688BB1",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK__Blogs__CategoryI__0C50D423",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogTagMa__BlogI__2334397B",
                table: "BlogTagMapping");

            migrationBuilder.DropForeignKey(
                name: "FK__BlogTagMa__TagID__24285DB4",
                table: "BlogTagMapping");

            migrationBuilder.DropForeignKey(
                name: "FK__Bookings__Discou__76969D2E",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK__Bookings__TourID__75A278F5",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK__Bookings__UserID__74AE54BC",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK__Companion__UserI__1BC821DD",
                table: "Companions");

            migrationBuilder.DropForeignKey(
                name: "FK__Favorites__TourI__08B54D69",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK__Favorites__UserI__07C12930",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK__Payments__Bookin__7B5B524B",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK__Reviews__TourID__01142BA1",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK__Reviews__UserID__00200768",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK__Schedules__TourI__6477ECF3",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK__TourDetai__TourI__5BAD9CC8",
                table: "TourDetails");

            migrationBuilder.DropForeignKey(
                name: "FK__TourFAQs__TourID__634EBE90",
                table: "TourFAQs");

            migrationBuilder.DropForeignKey(
                name: "FK__TourImage__TourI__619B8048",
                table: "TourImages");

            migrationBuilder.DropForeignKey(
                name: "FK__TourItine__TourI__5F7E2DAC",
                table: "TourItineraries");

            migrationBuilder.DropForeignKey(
                name: "FK__Tours__CategoryI__5CD6CB2B",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK__Tours__CreatedBy__5AEE82B9",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK__Tours__Destinati__5BE2A6F2",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK__VIPPointH__UserI__0C85DE4D",
                table: "VIPPointHistory");

            migrationBuilder.DropTable(
                name: "SemanticQueries");

            migrationBuilder.DropTable(
                name: "TourDtos");

            migrationBuilder.DropTable(
                name: "TourPolicies");

            migrationBuilder.DropTable(
                name: "TourViewLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK__VIPPoint__4D7B4ADD9AEB3685",
                table: "VIPPointHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Users__1788CCACA986BA0B",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Tours__604CEA109ED306E5",
                table: "Tours");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourItin__361216A643F3D5DB",
                table: "TourItineraries");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourImag__7516F4EC6D54EB2C",
                table: "TourImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourFAQs__4B89D1E29DD13A3D",
                table: "TourFAQs");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourDeta__5055BCFC2BC8AA78",
                table: "TourDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TourCate__19093A2B9A58ECFD",
                table: "TourCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Schedule__9C8A5B693C60215D",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Rules__110458E23D463170",
                table: "Rules");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Roles__8AFACE3AC60AA4DD",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Reviews__74BC79AE1781A59E",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Payments__9B556A588D559114",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Notifica__20CF2E12C3DD730E",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Favorite__018C020DB0FD2F72",
                table: "Favorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Discount__E43F6DF60F0748A1",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Destinat__DB5FE4AC0F8388C0",
                table: "Destinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Destinat__7516F4ECAC84A0A5",
                table: "DestinationImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Companio__8B53BE8BF2A6B57E",
                table: "Companions");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Bookings__73951ACD7995049D",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogTags__657CFA4CD3671040",
                table: "BlogTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogTagM__826051F48E5A3A93",
                table: "BlogTagMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Blogs__54379E50DF1D11DB",
                table: "Blogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogImag__7516F4EC50ABB3E6",
                table: "BlogImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogDeta__2383E81EBB0A289E",
                table: "BlogDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BlogComm__C3B4DFAAAA8338B9",
                table: "BlogComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Admins__719FE4E80D073614",
                table: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Actions__FFE3F4D9CB247AFF",
                table: "Actions");

            migrationBuilder.DropPrimaryKey(
                name: "PK__AccessLo__5E5499A85C951DF8",
                table: "AccessLog");

            migrationBuilder.RenameIndex(
                name: "UQ__Users__A9D105348693C07F",
                table: "Users",
                newName: "UQ__Users__A9D1053449FFE89D");

            migrationBuilder.RenameIndex(
                name: "UQ__Users__536C85E4240F0354",
                table: "Users",
                newName: "UQ__Users__536C85E462158B79");

            migrationBuilder.RenameIndex(
                name: "UQ__TourDeta__604CEA11C9A3D5D6",
                table: "TourDetails",
                newName: "UQ__TourDeta__604CEA11FC75AA34");

            migrationBuilder.RenameIndex(
                name: "UQ__TourCate__8517B2E005F3C24C",
                table: "TourCategories",
                newName: "UQ__TourCate__8517B2E0BF7EDA02");

            migrationBuilder.RenameIndex(
                name: "UQ__Rules__BC7B5FB60152F65E",
                table: "Rules",
                newName: "UQ__Rules__BC7B5FB673C7CE77");

            migrationBuilder.RenameIndex(
                name: "UQ__Roles__8A2B6160E55A842F",
                table: "Roles",
                newName: "UQ__Roles__8A2B61601238DB49");

            migrationBuilder.RenameIndex(
                name: "UQ__Discount__A25C5AA77F6F5670",
                table: "Discounts",
                newName: "UQ__Discount__A25C5AA75290125B");

            migrationBuilder.RenameIndex(
                name: "UQ__BlogTags__737584F6C7DCC467",
                table: "BlogTags",
                newName: "UQ__BlogTags__737584F6AEFE61A6");

            migrationBuilder.RenameIndex(
                name: "UQ__BlogDeta__54379E519D2CECE0",
                table: "BlogDetails",
                newName: "UQ__BlogDeta__54379E51A558E9D1");

            migrationBuilder.RenameIndex(
                name: "UQ__Admins__A9D10534F3ACDEC4",
                table: "Admins",
                newName: "UQ__Admins__A9D1053452D3FF2D");

            migrationBuilder.RenameIndex(
                name: "UQ__Admins__536C85E4370D36E6",
                table: "Admins",
                newName: "UQ__Admins__536C85E4EC6305B7");

            migrationBuilder.RenameIndex(
                name: "UQ__Actions__3148953D6EAEC540",
                table: "Actions",
                newName: "UQ__Actions__3148953D345B849D");

            migrationBuilder.AddPrimaryKey(
                name: "PK__VIPPoint__4D7B4ADD00D970C3",
                table: "VIPPointHistory",
                column: "HistoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Users__1788CCACF72813F8",
                table: "Users",
                column: "UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Tours__604CEA10C292330F",
                table: "Tours",
                column: "TourID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourItin__361216A6B2E37400",
                table: "TourItineraries",
                column: "ItineraryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourImag__7516F4EC3FE9F5B0",
                table: "TourImages",
                column: "ImageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourFAQs__4B89D1E2AA962951",
                table: "TourFAQs",
                column: "FAQID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourDeta__5055BCFC4F3EBFE2",
                table: "TourDetails",
                column: "TourDetailID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__TourCate__19093A2B59400476",
                table: "TourCategories",
                column: "CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Schedule__9C8A5B696B2581C2",
                table: "Schedules",
                column: "ScheduleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Rules__110458E2CA33AFC8",
                table: "Rules",
                column: "RuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Roles__8AFACE3AD5502F2C",
                table: "Roles",
                column: "RoleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Reviews__74BC79AEE3A98EAA",
                table: "Reviews",
                column: "ReviewID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Payments__9B556A583D71F123",
                table: "Payments",
                column: "PaymentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Notifica__20CF2E12B5A7FFA5",
                table: "Notifications",
                column: "NotificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Favorite__018C020D2A7422D0",
                table: "Favorites",
                columns: new[] { "UserID", "TourID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__Discount__E43F6DF60725D9D5",
                table: "Discounts",
                column: "DiscountID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Destinat__DB5FE4AC0E1447D3",
                table: "Destinations",
                column: "DestinationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Destinat__7516F4ECDC4B0B39",
                table: "DestinationImages",
                column: "ImageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Companio__8B53BE8BB4D8595E",
                table: "Companions",
                column: "CompanionID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Bookings__73951ACDFA0E4E79",
                table: "Bookings",
                column: "BookingID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogTags__657CFA4CBAFE00EC",
                table: "BlogTags",
                column: "TagID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogTagM__826051F465023A30",
                table: "BlogTagMapping",
                columns: new[] { "BlogID", "TagID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__Blogs__54379E5054BD6FC5",
                table: "Blogs",
                column: "BlogID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogImag__7516F4EC54DE70B5",
                table: "BlogImages",
                column: "ImageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogDeta__2383E81E48705695",
                table: "BlogDetails",
                column: "BlogDetailID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BlogComm__C3B4DFAA9DEABC40",
                table: "BlogComments",
                column: "CommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Admins__719FE4E8103D4241",
                table: "Admins",
                column: "AdminID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Actions__FFE3F4D96FF455FB",
                table: "Actions",
                column: "ActionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__AccessLo__5E5499A829083A81",
                table: "AccessLog",
                column: "LogID");

            migrationBuilder.AddForeignKey(
                name: "FK__AccessLog__UserI__114A936A",
                table: "AccessLog",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__BlogComme__BlogI__236943A5",
                table: "BlogComments",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "BlogID");

            migrationBuilder.AddForeignKey(
                name: "FK__BlogComme__UserI__245D67DE",
                table: "BlogComments",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__BlogDetai__BlogI__1F98B2C1",
                table: "BlogDetails",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__BlogImage__BlogI__29221CFB",
                table: "BlogImages",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Blogs__AuthorAdm__18EBB532",
                table: "Blogs",
                column: "AuthorAdminID",
                principalTable: "Admins",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK__Blogs__AuthorUse__17F790F9",
                table: "Blogs",
                column: "AuthorUserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Blogs__CategoryI__19DFD96B",
                table: "Blogs",
                column: "CategoryID",
                principalTable: "TourCategories",
                principalColumn: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK__BlogTagMa__BlogI__30C33EC3",
                table: "BlogTagMapping",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__BlogTagMa__TagID__31B762FC",
                table: "BlogTagMapping",
                column: "TagID",
                principalTable: "BlogTags",
                principalColumn: "TagID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Bookings__Discou__778AC167",
                table: "Bookings",
                column: "DiscountID",
                principalTable: "Discounts",
                principalColumn: "DiscountID");

            migrationBuilder.AddForeignKey(
                name: "FK__Bookings__TourID__76969D2E",
                table: "Bookings",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID");

            migrationBuilder.AddForeignKey(
                name: "FK__Bookings__UserID__75A278F5",
                table: "Bookings",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Companion__UserI__367C1819",
                table: "Companions",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Favorites__TourI__09A971A2",
                table: "Favorites",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Favorites__UserI__08B54D69",
                table: "Favorites",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Notificat__UserI__47A6A41B",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Payments__Bookin__7C4F7684",
                table: "Payments",
                column: "BookingID",
                principalTable: "Bookings",
                principalColumn: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK__Reviews__TourID__02084FDA",
                table: "Reviews",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID");

            migrationBuilder.AddForeignKey(
                name: "FK__Reviews__UserID__01142BA1",
                table: "Reviews",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Schedules__TourI__656C112C",
                table: "Schedules",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__TourDetai__TourI__3B40CD36",
                table: "TourDetails",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__TourFAQs__TourID__42E1EEFE",
                table: "TourFAQs",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__TourImage__TourI__628FA481",
                table: "TourImages",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__TourItine__TourI__3F115E1A",
                table: "TourItineraries",
                column: "TourID",
                principalTable: "Tours",
                principalColumn: "TourID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Tours__CategoryI__5DCAEF64",
                table: "Tours",
                column: "CategoryID",
                principalTable: "TourCategories",
                principalColumn: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK__Tours__CreatedBy__5BE2A6F2",
                table: "Tours",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Tours__Destinati__5CD6CB2B",
                table: "Tours",
                column: "DestinationID",
                principalTable: "Destinations",
                principalColumn: "DestinationID");

            migrationBuilder.AddForeignKey(
                name: "FK__VIPPointH__UserI__0D7A0286",
                table: "VIPPointHistory",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
