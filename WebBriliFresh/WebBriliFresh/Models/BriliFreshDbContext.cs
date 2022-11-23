using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebBriliFresh.Models;

public partial class BriliFreshDbContext : DbContext
{
    public BriliFreshDbContext()
    {
    }

    public BriliFreshDbContext(DbContextOptions<BriliFreshDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DiscountAll> DiscountAlls { get; set; }

    public virtual DbSet<DiscountOrder> DiscountOrders { get; set; }

    public virtual DbSet<DiscountProduct> DiscountProducts { get; set; }

    public virtual DbSet<DiscountStore> DiscountStores { get; set; }

    public virtual DbSet<DiscountType> DiscountTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<FeedbackImage> FeedbackImages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<QnA> QnAs { get; set; }

    public virtual DbSet<QnAImage> QnAImages { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=34.87.162.37;Database=BriliFreshDB;user id=brilifreshdb;password=brilifreshdb;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddId).HasName("PK__Address__A0E1ADEEE8CA1C6F");

            entity.ToTable("Address");

            entity.Property(e => e.AddId).HasColumnName("AddID");
            entity.Property(e => e.CusId).HasColumnName("CusID");

            entity.HasOne(d => d.Cus).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("fk_Address_1");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CusId).HasName("PK__Customer__2F18713084D2A8B4");

            entity.ToTable("Customer");

            entity.Property(e => e.CusId).HasColumnName("CusID");
            entity.Property(e => e.RewardId).HasColumnName("RewardID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Reward).WithMany(p => p.Customers)
                .HasForeignKey(d => d.RewardId)
                .HasConstraintName("fk_Customer_2");

            entity.HasOne(d => d.User).WithMany(p => p.Customers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_Customer_1");
        });

        modelBuilder.Entity<DiscountAll>(entity =>
        {
            entity.HasKey(e => e.DisId).HasName("PK__Discount__E2AA7E643E202296");

            entity.ToTable("Discount_All");

            entity.Property(e => e.DisId).HasColumnName("DisID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DiscountOrder>(entity =>
        {
            entity.HasKey(e => e.DisId).HasName("PK__Discount__E2AA7E640395556D");

            entity.ToTable("Discount_Order");

            entity.Property(e => e.DisId).HasColumnName("DisID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.MaxDis).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DiscountProduct>(entity =>
        {
            entity.HasKey(e => e.DisId).HasName("PK__Discount__E2AA7E64CB3BFD90");

            entity.ToTable("Discount_Product");

            entity.Property(e => e.DisId).HasColumnName("DisID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ProId).HasColumnName("ProID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Pro).WithMany(p => p.DiscountProducts)
                .HasForeignKey(d => d.ProId)
                .HasConstraintName("fk_Discount_Product_1");
        });

        modelBuilder.Entity<DiscountStore>(entity =>
        {
            entity.HasKey(e => e.DisId).HasName("PK__Discount__E2AA7E648DB79EB2");

            entity.ToTable("Discount_Store");

            entity.Property(e => e.DisId).HasColumnName("DisID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");

            entity.HasOne(d => d.Store).WithMany(p => p.DiscountStores)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("fk_Discount_Store_1");
        });

        modelBuilder.Entity<DiscountType>(entity =>
        {
            entity.HasKey(e => e.DisId).HasName("PK__Discount__E2AA7E64BEEF3275");

            entity.ToTable("Discount_Type");

            entity.Property(e => e.DisId).HasColumnName("DisID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Type).WithMany(p => p.DiscountTypes)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("fk_Discount_Type_1");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK__Employee__AF2DBA791CAD9740");

            entity.ToTable("Employee");

            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Store).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("fk_Employee_2");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_Employee_1");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FbId).HasName("PK__Feedback__36769D6C81066F7D");

            entity.ToTable("Feedback");

            entity.Property(e => e.FbId).HasColumnName("FbID");
            entity.Property(e => e.CusId).HasColumnName("CusID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProId).HasColumnName("ProID");
            entity.Property(e => e.ReplyId).HasColumnName("ReplyID");
            entity.Property(e => e.SendDate).HasColumnType("datetime");

            entity.HasOne(d => d.Cus).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("fk_Feedback_3");

            entity.HasOne(d => d.Order).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_Feedback_4");

            entity.HasOne(d => d.Pro).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProId)
                .HasConstraintName("fk_Feedback_2");

            entity.HasOne(d => d.Reply).WithMany(p => p.InverseReply)
                .HasForeignKey(d => d.ReplyId)
                .HasConstraintName("fk_Feedback_1");
        });

        modelBuilder.Entity<FeedbackImage>(entity =>
        {
            entity.HasKey(e => e.FbImgId).HasName("PK__Feedback__05FB6A30E6C37DE9");

            entity.ToTable("Feedback_Image");

            entity.Property(e => e.FbImgId).HasColumnName("FbImgID");
            entity.Property(e => e.FbId).HasColumnName("FbID");

            entity.HasOne(d => d.Fb).WithMany(p => p.FeedbackImages)
                .HasForeignKey(d => d.FbId)
                .HasConstraintName("fk_Feedback_Image_1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF3DFB581C");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.AddId).HasColumnName("AddID");
            entity.Property(e => e.DisId).HasColumnName("DisID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransId).HasColumnName("TransID");

            entity.HasOne(d => d.Add).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AddId)
                .HasConstraintName("fk_Order_1");

            entity.HasOne(d => d.Dis).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DisId)
                .HasConstraintName("fk_Order_3");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("fk_Order_4");

            entity.HasOne(d => d.Trans).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TransId)
                .HasConstraintName("fk_Order_2");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProId }).HasName("PK__Order_De__D5B072F09429B746");

            entity.ToTable("Order_Details");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProId).HasColumnName("ProID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_Details_1");

            entity.HasOne(d => d.Pro).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_Details_2");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProId).HasName("PK__Product__620295F0060AE041");

            entity.ToTable("Product");

            entity.Property(e => e.ProId).HasColumnName("ProID");
            entity.Property(e => e.IsDelet).HasColumnName("isDelet");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Type).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("fk_Product_1");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImgId).HasName("PK__Product___352F541307DD260F");

            entity.ToTable("Product_Image");

            entity.Property(e => e.ImgId).HasColumnName("ImgID");
            entity.Property(e => e.ProId).HasColumnName("ProID");

            entity.HasOne(d => d.Pro).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProId)
                .HasConstraintName("fk_Product_Image_1");
        });

        modelBuilder.Entity<QnA>(entity =>
        {
            entity.HasKey(e => e.QnAid).HasName("PK__QnA__C4DF8B295216EC48");

            entity.ToTable("QnA");

            entity.Property(e => e.QnAid).HasColumnName("QnAID");
            entity.Property(e => e.CusId).HasColumnName("CusID");
            entity.Property(e => e.ProId).HasColumnName("ProID");
            entity.Property(e => e.ReplyId).HasColumnName("ReplyID");
            entity.Property(e => e.SendDate).HasColumnType("datetime");

            entity.HasOne(d => d.Cus).WithMany(p => p.QnAs)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("fk_QnA_3");

            entity.HasOne(d => d.Pro).WithMany(p => p.QnAs)
                .HasForeignKey(d => d.ProId)
                .HasConstraintName("fk_QnA_2");

            entity.HasOne(d => d.Reply).WithMany(p => p.InverseReply)
                .HasForeignKey(d => d.ReplyId)
                .HasConstraintName("fk_QnA_1");
        });

        modelBuilder.Entity<QnAImage>(entity =>
        {
            entity.HasKey(e => e.QnAimgId).HasName("PK__QnA_Imag__1924FF462AC0BF71");

            entity.ToTable("QnA_Image");

            entity.Property(e => e.QnAimgId).HasColumnName("QnAImgID");
            entity.Property(e => e.QnAid).HasColumnName("QnAID");

            entity.HasOne(d => d.QnA).WithMany(p => p.QnAImages)
                .HasForeignKey(d => d.QnAid)
                .HasConstraintName("fk_QnA_Image_1");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PK__Reward__825015994E6C8F56");

            entity.ToTable("Reward");

            entity.Property(e => e.RewardId).HasColumnName("RewardID");
            entity.Property(e => e.Point).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.ProId }).HasName("PK__Stock__2DA2D9BE355B9887");

            entity.ToTable("Stock");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.ProId).HasColumnName("ProID");

            entity.HasOne(d => d.Pro).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Stock_2");

            entity.HasOne(d => d.Store).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Stock_1");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__Store__3B82F0E1A0C23954");

            entity.ToTable("Store");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.TransId).HasName("PK__Transpor__9E5DDB1CD09307FB");

            entity.ToTable("Transport");

            entity.Property(e => e.TransId).HasColumnName("TransID");
            entity.Property(e => e.Fee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ShippingDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Type__516F039576E880F3");

            entity.ToTable("Type");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC1901C7F0");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
