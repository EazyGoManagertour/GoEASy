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

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<BlogDetail> BlogDetails { get; set; }

    public virtual DbSet<BlogImage> BlogImages { get; set; }

    public virtual DbSet<BlogTag> BlogTags { get; set; }

    public virtual DbSet<BlogTagMapping> BlogTagMappings { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Companion> Companions { get; set; }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<DestinationImage> DestinationImages { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourCategory> TourCategories { get; set; }

    public virtual DbSet<TourDetail> TourDetails { get; set; }

    public virtual DbSet<TourFaq> TourFaqs { get; set; }

    public virtual DbSet<TourImage> TourImages { get; set; }

    public virtual DbSet<TourItinerary> TourItineraries { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VippointHistory> VippointHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=GoEasy;User Id=sa;Password=123456;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AccessLo__5E5499A85C951DF8");

            entity.ToTable("AccessLog");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.AccessLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__AccessLog__UserI__10566F31");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E80D073614");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E4370D36E6").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534F3ACDEC4").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
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
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blogs__54379E50DF1D11DB");

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.AuthorAdminId).HasColumnName("AuthorAdminID");
            entity.Property(e => e.AuthorUserId).HasColumnName("AuthorUserID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsApproved).HasDefaultValue((byte)0);
            entity.Property(e => e.ShortDescription).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.AuthorAdmin).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorAdminId)
                .HasConstraintName("FK__Blogs__AuthorAdm__0B5CAFEA");

            entity.HasOne(d => d.AuthorUser).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorUserId)
                .HasConstraintName("FK__Blogs__AuthorUse__0A688BB1");

            entity.HasOne(d => d.Category).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blogs__CategoryI__0C50D423");
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__BlogComm__C3B4DFAAAA8338B9");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogComme__BlogI__15DA3E5D");

            entity.HasOne(d => d.User).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BlogComme__UserI__16CE6296");
        });

        modelBuilder.Entity<BlogDetail>(entity =>
        {
            entity.HasKey(e => e.BlogDetailId).HasName("PK__BlogDeta__2383E81EBB0A289E");

            entity.HasIndex(e => e.BlogId, "UQ__BlogDeta__54379E519D2CECE0").IsUnique();

            entity.Property(e => e.BlogDetailId).HasColumnName("BlogDetailID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.QuoteAuthor).HasMaxLength(100);
            entity.Property(e => e.Section1Title).HasMaxLength(255);
            entity.Property(e => e.Section2Title).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Blog).WithOne(p => p.BlogDetail)
                .HasForeignKey<BlogDetail>(d => d.BlogId)
                .HasConstraintName("FK__BlogDetai__BlogI__1209AD79");
        });

        modelBuilder.Entity<BlogImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__BlogImag__7516F4EC50ABB3E6");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.IsMain).HasDefaultValue(false);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogImages)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__BlogImage__BlogI__1B9317B3");
        });

        modelBuilder.Entity<BlogTag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__BlogTags__657CFA4CD3671040");

            entity.HasIndex(e => e.Name, "UQ__BlogTags__737584F6C7DCC467").IsUnique();

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<BlogTagMapping>(entity =>
        {
            entity.HasKey(e => new { e.BlogId, e.TagId }).HasName("PK__BlogTagM__826051F48E5A3A93");

            entity.ToTable("BlogTagMapping");

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogTagMappings)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__BlogTagMa__BlogI__2334397B");

            entity.HasOne(d => d.Tag).WithMany(p => p.BlogTagMappings)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__BlogTagMa__TagID__24285DB4");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951ACD7995049D");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TotalPrice)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UsedVippoints)
                .HasDefaultValue(0)
                .HasColumnName("UsedVIPPoints");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Discount).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK__Bookings__Discou__76969D2E");

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Bookings__TourID__75A278F5");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bookings__UserID__74AE54BC");
        });

        modelBuilder.Entity<Companion>(entity =>
        {
            entity.HasKey(e => e.CompanionId).HasName("PK__Companio__8B53BE8BF2A6B57E");

            entity.Property(e => e.CompanionId).HasColumnName("CompanionID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.NationalId)
                .HasMaxLength(20)
                .HasColumnName("NationalID");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Companions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Companion__UserI__1BC821DD");
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.DestinationId).HasName("PK__Destinat__DB5FE4AC0F8388C0");

            entity.Property(e => e.DestinationId).HasColumnName("DestinationID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DestinationName).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<DestinationImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Destinat__7516F4ECAC84A0A5");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.Caption).HasMaxLength(255);
            entity.Property(e => e.DestinationId).HasColumnName("DestinationID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.IsCover).HasDefaultValue(false);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Destination).WithMany(p => p.DestinationImages)
                .HasForeignKey(d => d.DestinationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Destinati__Desti__52593CB8");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__Discount__E43F6DF60F0748A1");

            entity.HasIndex(e => e.Code, "UQ__Discount__A25C5AA77F6F5670").IsUnique();

            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.MaxAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MinTotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.TourId }).HasName("PK__Favorite__018C020DB0FD2F72");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.AddedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Tour).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Favorites__TourI__08B54D69");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Favorites__UserI__07C12930");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12C3DD730E");

            entity.HasIndex(e => e.UserId, "IX_Notifications_UserId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A588D559114");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Payments__Bookin__7B5B524B");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AE1781A59E");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.SentimentScore).HasMaxLength(20);
            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Tour).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Reviews__TourID__01142BA1");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__UserID__00200768");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AC60AA4DD");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160E55A842F").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B693C60215D");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.TourId).HasColumnName("TourID");

            entity.HasOne(d => d.Tour).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Schedules__TourI__6477ECF3");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.TourId).HasName("PK__Tours__604CEA109ED306E5");

            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.AdultPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ChildPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DestinationId).HasColumnName("DestinationID");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TourName).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Tours)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Tours__CategoryI__5CD6CB2B");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Tours)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tours__CreatedBy__5AEE82B9");

            entity.HasOne(d => d.Destination).WithMany(p => p.Tours)
                .HasForeignKey(d => d.DestinationId)
                .HasConstraintName("FK__Tours__Destinati__5BE2A6F2");
        });

        modelBuilder.Entity<TourCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__TourCate__19093A2B9A58ECFD");

            entity.HasIndex(e => e.CategoryName, "UQ__TourCate__8517B2E005F3C24C").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<TourDetail>(entity =>
        {
            entity.HasKey(e => e.TourDetailId).HasName("PK__TourDeta__5055BCFC2BC8AA78");

            entity.HasIndex(e => e.TourId, "UQ__TourDeta__604CEA11C9A3D5D6").IsUnique();

            entity.Property(e => e.TourDetailId).HasColumnName("TourDetailID");
            entity.Property(e => e.Activities).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TourId).HasColumnName("TourID");

            entity.HasOne(d => d.Tour).WithOne(p => p.TourDetail)
                .HasForeignKey<TourDetail>(d => d.TourId)
                .HasConstraintName("FK__TourDetai__TourI__5BAD9CC8");
        });

        modelBuilder.Entity<TourFaq>(entity =>
        {
            entity.HasKey(e => e.Faqid).HasName("PK__TourFAQs__4B89D1E29DD13A3D");

            entity.ToTable("TourFAQs");

            entity.Property(e => e.Faqid).HasColumnName("FAQID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Question).HasMaxLength(255);
            entity.Property(e => e.TourId).HasColumnName("TourID");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourFaqs)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__TourFAQs__TourID__634EBE90");
        });

        modelBuilder.Entity<TourImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__TourImag__7516F4EC6D54EB2C");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.Caption).HasMaxLength(255);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.IsCover).HasDefaultValue(false);
            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourImages)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TourImage__TourI__619B8048");
        });

        modelBuilder.Entity<TourItinerary>(entity =>
        {
            entity.HasKey(e => e.ItineraryId).HasName("PK__TourItin__361216A643F3D5DB");

            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.Accommodation).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Meals).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.TourId).HasColumnName("TourID");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourItineraries)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TourItine__TourI__5F7E2DAC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACA986BA0B");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4240F0354").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105348693C07F").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsVip)
                .HasDefaultValue(false)
                .HasColumnName("IsVIP");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.Vippoints)
                .HasDefaultValue(0)
                .HasColumnName("VIPPoints");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__412EB0B6");
        });

        modelBuilder.Entity<VippointHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__VIPPoint__4D7B4ADD9AEB3685");

            entity.ToTable("VIPPointHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.VippointHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__VIPPointH__UserI__0C85DE4D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
