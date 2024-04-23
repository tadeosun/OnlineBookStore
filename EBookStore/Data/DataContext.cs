using EBookStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EBookStore.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<OrderItemDetails> OrderItemDetails { get; set; }

        public override int SaveChanges() => base.SaveChanges();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        public override ValueTask<EntityEntry<T>> AddAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            return base.AddAsync(entity, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemDetails>()
                .HasKey(ord => new { ord.ID, ord.BookID, ord.OrderID });

            base.OnModelCreating(modelBuilder);
        }
    }
}
