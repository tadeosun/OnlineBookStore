using EBookStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EBookStore.Data
{
    public interface IDataContext
    {
        DbSet<Orders> Orders { get; set; }
        DbSet<Users> Users { get; set; }
        DbSet<Books> Books { get; set; }
        DbSet<OrderItemDetails> OrderItemDetails { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        ValueTask<EntityEntry<T>> AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
    }
}
