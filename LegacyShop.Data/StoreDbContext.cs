using System.Data.Entity;
using LegacyShop.Data.Entities;

namespace LegacyShop.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext()
            : this("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=LegacyShop;Integrated Security=True;MultipleActiveResultSets=True")
        {
        }

        public StoreDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithRequired(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithRequired(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithRequired(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
