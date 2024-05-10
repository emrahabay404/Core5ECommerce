using Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataAccess.Concrete
{
   public class Context : IdentityDbContext<AppUser, AppRole, int>
   {

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseSqlServer("Server=MYPC\\SQLEXPRESS;Database=core5etic;Trusted_Connection=true;TrustServerCertificate=True;");
         //"Server=MYPC\\SQLEXPRESS;Database=Shift_Db33;Trusted_Connection=true;TrustServerCertificate=True;";
      }


      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");


         modelBuilder.Entity<Favourite>(entity =>
         {
            entity.Property(e => e.Id).HasColumnName("_Id");

            entity.Property(e => e.ProductId).HasColumnName("Product_Id");

            entity.HasOne(d => d.AppUser)
                .WithMany(p => p._Favourites)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK_Assignments_Shifts");

            entity.HasOne(d => d.Product)
                .WithMany(p => p._Favourites)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Assignments_Teams");
         });


         base.OnModelCreating(modelBuilder);
         modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
      }


      public DbSet<Product> Products { get; set; }
      public DbSet<Comment> Comments { get; set; }
      public DbSet<Category> Categories { get; set; }
      public DbSet<Photo> Photos { get; set; }
      public DbSet<City> Cities { get; set; }
      public DbSet<Cargo> Cargos { get; set; }
      public DbSet<SubCategory> SubCategories { get; set; }
      public DbSet<Brand> Brands { get; set; }
      public DbSet<Favourite> Favourites { get; set; }
      public DbSet<SupportRequest> SupportRequests { get; set; }
      public DbSet<Wallet> Wallets { get; set; }
      public DbSet<Address> Addresses { get; set; }
      public DbSet<Basket> Baskets { get; set; }
      public DbSet<Order> Orders { get; set; }
      public DbSet<OrderDetail> OrderDetails { get; set; }
      public DbSet<OrderProcess> OrderProcesses { get; set; }
      public DbSet<RequestSeller> RequestSellers { get; set; }
      public DbSet<UserBankAcco> UserBankAccos { get; set; }
      public DbSet<Notification> Notifications { get; set; }
      public DbSet<NotifiType> NotifiTypes { get; set; }
   }
}