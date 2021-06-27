using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FinalFantasu.Models
{
    public partial class AniFigure_DB : DbContext
    {
        public AniFigure_DB()
            : base("name=AniFigure_DB")
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Figure> Figures { get; set; }
        public virtual DbSet<FigureCategory> FigureCategories { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.Address)
                .HasForeignKey(e => e.idAddress);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.FigureCategories)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.idCategory);

            modelBuilder.Entity<Figure>()
                .HasMany(e => e.Carts)
                .WithOptional(e => e.Figure)
                .HasForeignKey(e => e.idFigure);

            modelBuilder.Entity<Figure>()
                .HasMany(e => e.FigureCategories)
                .WithOptional(e => e.Figure)
                .HasForeignKey(e => e.idFigure);

            modelBuilder.Entity<Figure>()
                .HasMany(e => e.Images)
                .WithOptional(e => e.Figure)
                .HasForeignKey(e => e.idFigure);

            modelBuilder.Entity<Figure>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.Figure)
                .HasForeignKey(e => e.idFigure);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.Order)
                .HasForeignKey(e => e.idOrder);

            modelBuilder.Entity<Publisher>()
                .HasMany(e => e.Figures)
                .WithOptional(e => e.Publisher)
                .HasForeignKey(e => e.idPublisher);

            modelBuilder.Entity<Type>()
                .HasMany(e => e.Figures)
                .WithOptional(e => e.Type)
                .HasForeignKey(e => e.idType);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Carts)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.idUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.idUser);
        }
    }
}
