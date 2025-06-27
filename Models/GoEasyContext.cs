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

<<<<<<< HEAD
    public virtual DbSet<Admin> Admins { get; set; }

=======
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Companion> Companions { get; set; }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<DestinationImage> DestinationImages { get; set; }

<<<<<<< HEAD
    public virtual DbSet<Discount> Discounts { get; set; }

=======
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourCategory> TourCategories { get; set; }

    public virtual DbSet<TourImage> TourImages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VippointHistory> VippointHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=GoEasy;User Id=sa;Password=123456;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessLog>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.LogId).HasName("PK__AccessLo__5E5499A8F610439C");
=======
            entity.HasKey(e => e.LogId).HasName("PK__AccessLo__5E5499A8268D6DC2");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

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
<<<<<<< HEAD
                .HasConstraintName("FK__AccessLog__UserI__03F0984C");
=======
                .HasConstraintName("FK__AccessLog__UserI__01142BA1");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<Blog>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.BlogId).HasName("PK__Blogs__54379E5064AFEF88");
=======
            entity.HasKey(e => e.BlogId).HasName("PK__Blogs__54379E502B20E2C2");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsPublished).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Author).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorId)
<<<<<<< HEAD
                .HasConstraintName("FK__Blogs__AuthorID__0A9D95DB");
=======
                .HasConstraintName("FK__Blogs__AuthorID__07C12930");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<Booking>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951ACD5198530E");
=======
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951ACD4E26EC5F");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
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

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourId)
<<<<<<< HEAD
                .HasConstraintName("FK__Bookings__TourID__6B24EA82");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bookings__UserID__6A30C649");
=======
                .HasConstraintName("FK__Bookings__TourID__68487DD7");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bookings__UserID__6754599E");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<Companion>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.CompanionId).HasName("PK__Companio__8B53BE8BBB751F53");
=======
            entity.HasKey(e => e.CompanionId).HasName("PK__Companio__8B53BE8B79DA287E");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

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
<<<<<<< HEAD
                .HasConstraintName("FK__Companion__UserI__0F624AF8");
=======
                .HasConstraintName("FK__Companion__UserI__0C85DE4D");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<Destination>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.DestinationId).HasName("PK__Destinat__DB5FE4ACDD8BD4F4");
=======
            entity.HasKey(e => e.DestinationId).HasName("PK__Destinat__DB5FE4AC6E2DCA84");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.DestinationId).HasColumnName("DestinationID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DestinationName).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<DestinationImage>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.ImageId).HasName("PK__Destinat__7516F4ECBA6BF67E");
=======
            entity.HasKey(e => e.ImageId).HasName("PK__Destinat__7516F4ECD7B506A9");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

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
<<<<<<< HEAD
                .HasConstraintName("FK__Destinati__Desti__4D94879B");
=======
                .HasConstraintName("FK__Destinati__Desti__4AB81AF0");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => new { e.UserId, e.TourId }).HasName("PK__Favorite__018C020D2832C804");
=======
            entity.HasKey(e => new { e.UserId, e.TourId }).HasName("PK__Favorite__018C020DF23A5D7D");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.AddedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Tour).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.TourId)
<<<<<<< HEAD
                .HasConstraintName("FK__Favorites__TourI__7C4F7684");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Favorites__UserI__7B5B524B");
=======
                .HasConstraintName("FK__Favorites__TourI__797309D9");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Favorites__UserI__787EE5A0");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<Payment>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A587C203CAA");
=======
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A582EF92FAD");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
<<<<<<< HEAD
                .HasConstraintName("FK__Payments__Bookin__6EF57B66");
=======
                .HasConstraintName("FK__Payments__Bookin__6C190EBB");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<Review>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AE0122D364");
=======
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AEB1D6A7BD");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.SentimentScore).HasMaxLength(20);
            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Tour).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.TourId)
<<<<<<< HEAD
                .HasConstraintName("FK__Reviews__TourID__74AE54BC");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__UserID__73BA3083");
=======
                .HasConstraintName("FK__Reviews__TourID__71D1E811");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__UserID__70DDC3D8");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<Role>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A59A73633");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160E691D3AE").IsUnique();
=======
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A41AC85F1");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B616055EC6D29").IsUnique();
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B695C5B8344");
=======
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B6955E93A83");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.TourId).HasColumnName("TourID");

            entity.HasOne(d => d.Tour).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Cascade)
<<<<<<< HEAD
                .HasConstraintName("FK__Schedules__TourI__5FB337D6");
=======
                .HasConstraintName("FK__Schedules__TourI__5CD6CB2B");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<Tour>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.TourId).HasName("PK__Tours__604CEA100C21A0DA");
=======
            entity.HasKey(e => e.TourId).HasName("PK__Tours__604CEA1079FC5900");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.AdultPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ChildPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DestinationId).HasColumnName("DestinationID");
            entity.Property(e => e.TourName).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Tours)
                .HasForeignKey(d => d.CategoryId)
<<<<<<< HEAD
                .HasConstraintName("FK__Tours__CategoryI__5812160E");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Tours)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tours__CreatedBy__5629CD9C");

            entity.HasOne(d => d.Destination).WithMany(p => p.Tours)
                .HasForeignKey(d => d.DestinationId)
                .HasConstraintName("FK__Tours__Destinati__571DF1D5");
=======
                .HasConstraintName("FK__Tours__CategoryI__5535A963");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Tours)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tours__CreatedBy__534D60F1");

            entity.HasOne(d => d.Destination).WithMany(p => p.Tours)
                .HasForeignKey(d => d.DestinationId)
                .HasConstraintName("FK__Tours__Destinati__5441852A");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<TourCategory>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.CategoryId).HasName("PK__TourCate__19093A2B5F710F95");

            entity.HasIndex(e => e.CategoryName, "UQ__TourCate__8517B2E0D9E74DA8").IsUnique();
=======
            entity.HasKey(e => e.CategoryId).HasName("PK__TourCate__19093A2B32246B05");

            entity.HasIndex(e => e.CategoryName, "UQ__TourCate__8517B2E0BCA3B2CC").IsUnique();
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<TourImage>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.ImageId).HasName("PK__TourImag__7516F4EC50E2D473");
=======
            entity.HasKey(e => e.ImageId).HasName("PK__TourImag__7516F4ECCFB375E6");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

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
<<<<<<< HEAD
                .HasConstraintName("FK__TourImage__TourI__5CD6CB2B");
=======
                .HasConstraintName("FK__TourImage__TourI__59FA5E80");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<User>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC56B88BA3");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4611DBDB6").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053498A17232").IsUnique();
=======
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACCA4C1E31");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E408123488").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534BE8256FC").IsUnique();
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.Property(e => e.UserId).HasColumnName("UserID");
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
<<<<<<< HEAD
                .HasConstraintName("FK__Users__RoleID__440B1D61");
=======
                .HasConstraintName("FK__Users__RoleID__412EB0B6");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        modelBuilder.Entity<VippointHistory>(entity =>
        {
<<<<<<< HEAD
            entity.HasKey(e => e.HistoryId).HasName("PK__VIPPoint__4D7B4ADD80188E6B");
=======
            entity.HasKey(e => e.HistoryId).HasName("PK__VIPPoint__4D7B4ADDF4066AE7");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb

            entity.ToTable("VIPPointHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.VippointHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
<<<<<<< HEAD
                .HasConstraintName("FK__VIPPointH__UserI__00200768");
=======
                .HasConstraintName("FK__VIPPointH__UserI__7D439ABD");
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
