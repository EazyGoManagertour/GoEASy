using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GoEASy.Models;

public partial class GoEasyContext : DbContext
{
    public GoEasyContext()
    {
    }

    public GoEasyContext(DbContextOptions<GoEasyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessLog> AccessLogs { get; set; }

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<BlogDetail> BlogDetails { get; set; }

    public virtual DbSet<BlogImage> BlogImages { get; set; }

    public virtual DbSet<BlogTag> BlogTags { get; set; }

    public virtual DbSet<BlogTagMapping> BlogTagMappings { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Companion> Companions { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<DestinationImage> DestinationImages { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<SemanticQuery> SemanticQueries { get; set; }

    public virtual DbSet<SystemStatistic> SystemStatistics { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourCategory> TourCategories { get; set; }

    public virtual DbSet<TourDetail> TourDetails { get; set; }

    public virtual DbSet<TourDto> TourDtos { get; set; }

    public virtual DbSet<TourFAQ> TourFAQs { get; set; }

    public virtual DbSet<TourImage> TourImages { get; set; }

    public virtual DbSet<TourItinerary> TourItineraries { get; set; }

    public virtual DbSet<TourPolicy> TourPolicies { get; set; }

    public virtual DbSet<TourViewLog> TourViewLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VIPPointHistory> VIPPointHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=GoEasy;User Id=sa;Password=123456;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessLog>(entity =>
        {
            entity.HasKey(e => e.LogID).HasName("PK__AccessLo__5E5499A848E14535");

            entity.ToTable("AccessLog");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IPAddress).HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.AccessLogs)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK__AccessLog__UserI__756D6ECB");
        });

        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PK__Actions__FFE3F4D96FF455FB");

            entity.HasIndex(e => e.ActionSlug, "UQ__Actions__3148953D345B849D").IsUnique();

            entity.Property(e => e.ActionName).HasMaxLength(100);
            entity.Property(e => e.ActionSlug).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminID).HasName("PK__Admins__719FE4E8103D4241");

            entity.HasIndex(e => e.RuleId, "IX_Admins_RuleId");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E4EC6305B7").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D1053452D3FF2D")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Rule).WithMany(p => p.Admins)
                .HasForeignKey(d => d.RuleId)
                .HasConstraintName("FK_Admins_Rules");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogID).HasName("PK__Blogs__54379E5054BD6FC5");

            entity.HasIndex(e => e.AuthorAdminID, "IX_Blogs_AuthorAdminID");

            entity.HasIndex(e => e.AuthorUserID, "IX_Blogs_AuthorUserID");

            entity.HasIndex(e => e.CategoryID, "IX_Blogs_CategoryID");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsApproved).HasDefaultValue((byte)0);
            entity.Property(e => e.ShortDescription).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.AuthorAdmin).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorAdminID)
                .HasConstraintName("FK__Blogs__AuthorAdm__18EBB532");

            entity.HasOne(d => d.AuthorUser).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorUserID)
                .HasConstraintName("FK__Blogs__AuthorUse__17F790F9");

            entity.HasOne(d => d.Category).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blogs__CategoryI__19DFD96B");
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.CommentID).HasName("PK__BlogComm__C3B4DFAA9DEABC40");

            entity.HasIndex(e => e.BlogID, "IX_BlogComments_BlogID");

            entity.HasIndex(e => e.UserID, "IX_BlogComments_UserID");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogComme__BlogI__236943A5");

            entity.HasOne(d => d.User).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK__BlogComme__UserI__245D67DE");
        });

        modelBuilder.Entity<BlogDetail>(entity =>
        {
            entity.HasKey(e => e.BlogDetailID).HasName("PK__BlogDeta__2383E81E48705695");

            entity.HasIndex(e => e.BlogID, "UQ__BlogDeta__54379E51A558E9D1").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.QuoteAuthor).HasMaxLength(100);
            entity.Property(e => e.Section1Title).HasMaxLength(255);
            entity.Property(e => e.Section2Title).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Blog).WithOne(p => p.BlogDetail)
                .HasForeignKey<BlogDetail>(d => d.BlogID)
                .HasConstraintName("FK__BlogDetai__BlogI__1F98B2C1");
        });

        modelBuilder.Entity<BlogImage>(entity =>
        {
            entity.HasKey(e => e.ImageID).HasName("PK__BlogImag__7516F4EC54DE70B5");

            entity.HasIndex(e => e.BlogID, "IX_BlogImages_BlogID");

            entity.Property(e => e.ImageURL).HasMaxLength(255);
            entity.Property(e => e.IsMain).HasDefaultValue(false);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogImages)
                .HasForeignKey(d => d.BlogID)
                .HasConstraintName("FK__BlogImage__BlogI__29221CFB");
        });

        modelBuilder.Entity<BlogTag>(entity =>
        {
            entity.HasKey(e => e.TagID).HasName("PK__BlogTags__657CFA4CBAFE00EC");

            entity.HasIndex(e => e.Name, "UQ__BlogTags__737584F6AEFE61A6")
                .IsUnique()
                .HasFilter("([Name] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<BlogTagMapping>(entity =>
        {
            entity.HasKey(e => new { e.BlogID, e.TagID }).HasName("PK__BlogTagM__826051F465023A30");

            entity.ToTable("BlogTagMapping");

            entity.HasIndex(e => e.TagID, "IX_BlogTagMapping_TagID");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogTagMappings)
                .HasForeignKey(d => d.BlogID)
                .HasConstraintName("FK__BlogTagMa__BlogI__30C33EC3");

            entity.HasOne(d => d.Tag).WithMany(p => p.BlogTagMappings)
                .HasForeignKey(d => d.TagID)
                .HasConstraintName("FK__BlogTagMa__TagID__31B762FC");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingID).HasName("PK__Bookings__73951ACDFA0E4E79");

            entity.HasIndex(e => e.DiscountID, "IX_Bookings_DiscountID");

            entity.HasIndex(e => e.TourID, "IX_Bookings_TourID");

            entity.HasIndex(e => e.UserID, "IX_Bookings_UserID");

            entity.Property(e => e.BookingDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0.0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TotalPrice)
                .HasDefaultValue(0.0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UsedVIPPoints).HasDefaultValue(0);

            entity.HasOne(d => d.Discount).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.DiscountID)
                .HasConstraintName("FK__Bookings__Discou__778AC167");

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourID)
                .HasConstraintName("FK__Bookings__TourID__76969D2E");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK__Bookings__UserID__75A278F5");
        });

        modelBuilder.Entity<Companion>(entity =>
        {
            entity.HasKey(e => e.CompanionID).HasName("PK__Companio__8B53BE8BB4D8595E");

            entity.HasIndex(e => e.UserID, "IX_Companions_UserID");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.NationalID).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.User).WithMany(p => p.Companions)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK__Companion__UserI__367C1819");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactID).HasName("PK__Contacts__5C6625BBB0D17CC4");

            entity.Property(e => e.ContactID).HasColumnName("ContactID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("New");
            entity.Property(e => e.UserID).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Contacts__UserID__5C37ACAD");
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.DestinationID).HasName("PK__Destinat__DB5FE4AC0E1447D3");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DestinationName).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<DestinationImage>(entity =>
        {
            entity.HasKey(e => e.ImageID).HasName("PK__Destinat__7516F4ECDC4B0B39");

            entity.HasIndex(e => e.DestinationID, "IX_DestinationImages_DestinationID");

            entity.Property(e => e.Caption).HasMaxLength(255);
            entity.Property(e => e.ImageURL).HasMaxLength(255);
            entity.Property(e => e.IsCover).HasDefaultValue(false);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Destination).WithMany(p => p.DestinationImages)
                .HasForeignKey(d => d.DestinationID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Destinati__Desti__52593CB8");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountID).HasName("PK__Discount__E43F6DF60725D9D5");

            entity.HasIndex(e => e.Code, "UQ__Discount__A25C5AA75290125B").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.MaxAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MinTotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => new { e.UserID, e.TourID }).HasName("PK__Favorite__018C020D2A7422D0");

            entity.HasIndex(e => e.TourID, "IX_Favorites_TourID");

            entity.Property(e => e.AddedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Tour).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.TourID)
                .HasConstraintName("FK__Favorites__TourI__09A971A2");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK__Favorites__UserI__08B54D69");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12B5A7FFA5");

            entity.HasIndex(e => e.UserId, "IX_Notifications_UserId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__47A6A41B");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentID).HasName("PK__Payments__9B556A583D71F123");

            entity.HasIndex(e => e.BookingID, "IX_Payments_BookingID");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingID)
                .HasConstraintName("FK__Payments__Bookin__7C4F7684");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewID).HasName("PK__Reviews__74BC79AEE3A98EAA");

            entity.HasIndex(e => e.TourID, "IX_Reviews_TourID");

            entity.HasIndex(e => e.UserID, "IX_Reviews_UserID");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.SentimentScore).HasMaxLength(20);

            entity.HasOne(d => d.Tour).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.TourID)
                .HasConstraintName("FK__Reviews__TourID__02084FDA");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK__Reviews__UserID__01142BA1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK__Roles__8AFACE3AD5502F2C");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61601238DB49").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(e => e.RuleId).HasName("PK__Rules__110458E2CA33AFC8");

            entity.HasIndex(e => e.Slug, "UQ__Rules__BC7B5FB673C7CE77").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsOpen).HasDefaultValue(true);
            entity.Property(e => e.RuleName).HasMaxLength(100);
            entity.Property(e => e.Slug).HasMaxLength(100);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleID).HasName("PK__Schedule__9C8A5B696B2581C2");

            entity.HasIndex(e => e.TourID, "IX_Schedules_TourID");

            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Tour).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TourID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Schedules__TourI__656C112C");
        });

        modelBuilder.Entity<SemanticQuery>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Semantic__3214EC27CD216E46");

            entity.Property(e => e.Query).HasMaxLength(500);
            entity.Property(e => e.TopK).HasDefaultValue(5);
        });

        modelBuilder.Entity<SystemStatistic>(entity =>
        {
            entity.HasKey(e => e.StatisticID).HasName("PK__SystemSt__367DEB37FEE8B7B6");

            entity.Property(e => e.BestSellingCount).HasDefaultValue(0);
            entity.Property(e => e.BestSellingTourName).HasMaxLength(255);
            entity.Property(e => e.CancelledCount).HasDefaultValue(0);
            entity.Property(e => e.HighestRatedTourName).HasMaxLength(255);
            entity.Property(e => e.MostCancelledTourName).HasMaxLength(255);
            entity.Property(e => e.MostViewedCount).HasDefaultValue(0);
            entity.Property(e => e.MostViewedTourName).HasMaxLength(255);
            entity.Property(e => e.SnapshotAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TopCountry).HasMaxLength(100);
            entity.Property(e => e.TopCountryVisitCount).HasDefaultValue(0);
            entity.Property(e => e.TotalBookings).HasDefaultValue(0);
            entity.Property(e => e.TotalRevenue)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalUsers).HasDefaultValue(0);
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.TourID).HasName("PK__Tours__604CEA10C292330F");

            entity.HasIndex(e => e.CategoryID, "IX_Tours_CategoryID");

            entity.HasIndex(e => e.CreatedBy, "IX_Tours_CreatedBy");

            entity.HasIndex(e => e.DestinationID, "IX_Tours_DestinationID");

            entity.Property(e => e.AdultPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ChildPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TourName).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Tours)
                .HasForeignKey(d => d.CategoryID)
                .HasConstraintName("FK__Tours__CategoryI__5DCAEF64");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Tours)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tours__CreatedBy__5BE2A6F2");

            entity.HasOne(d => d.Destination).WithMany(p => p.Tours)
                .HasForeignKey(d => d.DestinationID)
                .HasConstraintName("FK__Tours__Destinati__5CD6CB2B");
        });

        modelBuilder.Entity<TourCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryID).HasName("PK__TourCate__19093A2B59400476");

            entity.HasIndex(e => e.CategoryName, "UQ__TourCate__8517B2E0BF7EDA02").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<TourDetail>(entity =>
        {
            entity.HasKey(e => e.TourDetailID).HasName("PK__TourDeta__5055BCFC4F3EBFE2");

            entity.HasIndex(e => e.TourID, "UQ__TourDeta__604CEA11FC75AA34").IsUnique();

            entity.Property(e => e.Activities).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Tour).WithOne(p => p.TourDetail)
                .HasForeignKey<TourDetail>(d => d.TourID)
                .HasConstraintName("FK__TourDetai__TourI__3B40CD36");
        });

        modelBuilder.Entity<TourDto>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__TourDtos__3214EC27758DAF54");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<TourFAQ>(entity =>
        {
            entity.HasKey(e => e.FAQID).HasName("PK__TourFAQs__4B89D1E2AA962951");

            entity.HasIndex(e => e.TourID, "IX_TourFAQs_TourID");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Question).HasMaxLength(255);

            entity.HasOne(d => d.Tour).WithMany(p => p.TourFAQs)
                .HasForeignKey(d => d.TourID)
                .HasConstraintName("FK__TourFAQs__TourID__42E1EEFE");
        });

        modelBuilder.Entity<TourImage>(entity =>
        {
            entity.HasKey(e => e.ImageID).HasName("PK__TourImag__7516F4EC3FE9F5B0");

            entity.HasIndex(e => e.TourID, "IX_TourImages_TourID");

            entity.Property(e => e.Caption).HasMaxLength(255);
            entity.Property(e => e.ImageURL).HasMaxLength(255);
            entity.Property(e => e.IsCover).HasDefaultValue(false);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourImages)
                .HasForeignKey(d => d.TourID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TourImage__TourI__628FA481");
        });

        modelBuilder.Entity<TourItinerary>(entity =>
        {
            entity.HasKey(e => e.ItineraryID).HasName("PK__TourItin__361216A6B2E37400");

            entity.HasIndex(e => e.TourID, "IX_TourItineraries_TourID");

            entity.Property(e => e.Accommodation).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Meals).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Tour).WithMany(p => p.TourItineraries)
                .HasForeignKey(d => d.TourID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TourItine__TourI__3F115E1A");
        });

        modelBuilder.Entity<TourPolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyID).HasName("PK__TourPoli__2E133944C72CF8B2");

            entity.HasIndex(e => e.PolicyType, "IX_TourPolicies_PolicyType");

            entity.HasIndex(e => e.TourID, "IX_TourPolicies_TourID");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PolicyDescription).HasMaxLength(500);
            entity.Property(e => e.PolicyName).HasMaxLength(100);
            entity.Property(e => e.PolicyType).HasMaxLength(50);

            entity.HasOne(d => d.Tour).WithMany(p => p.TourPolicies)
                .HasForeignKey(d => d.TourID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TourPolicies_Tours");
        });

        modelBuilder.Entity<TourViewLog>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__TourView__3214EC27460BF978");

            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.ViewTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("PK__Users__1788CCACF72813F8");

            entity.HasIndex(e => e.RoleID, "IX_Users_RoleID");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E462158B79").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053449FFE89D")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsVIP).HasDefaultValue(false);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Sex).HasMaxLength(10);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.VIPPoints).HasDefaultValue(0);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleID)
                .HasConstraintName("FK__Users__RoleID__412EB0B6");
        });

        modelBuilder.Entity<VIPPointHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryID).HasName("PK__VIPPoint__4D7B4ADD00D970C3");

            entity.ToTable("VIPPointHistory");

            entity.HasIndex(e => e.UserID, "IX_VIPPointHistory_UserID");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Reason).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.VIPPointHistories)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__VIPPointH__UserI__0D7A0286");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
