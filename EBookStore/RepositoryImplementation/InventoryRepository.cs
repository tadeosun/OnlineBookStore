using EBookStore.Common;
using EBookStore.Data;
using EBookStore.Models;
using EBookStore.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EBookStore.RepositoryImplementation
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DataContext _context;
        public InventoryRepository(DataContext context)    
        {
            _context = context;
        }
        public async Task<int> AddInventoryAsync(Books book)
        {
            await _context.Books.AddAsync(book);
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteInventoryAsync(Books book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Books?> GetInventoryByIdAsync(int Id)
        {
            long InventoryId = Id;
            return await _context.Books.FindAsync(InventoryId);
        }
        public async Task<Books?> GetInventoryByIdAsync(long Id)
        {
            return await _context.Books.FirstOrDefaultAsync(a => a.ID == Id);
        }

        public async Task<IList<Books>> SearchIBookByIdAsync(string searchTerm)
        {
            return await _context.Books.Where(b => b.Title.Equals(searchTerm)
                                                            || b.Author.Equals(searchTerm)
                                                            || b.ISBN.Equals(searchTerm)
                                                            || b.YearOfPublication.Equals(searchTerm)
                                                            || b.Genre.Equals(searchTerm)).ToListAsync();
        }

        public async Task<int> UpdateInventoryAsync(Books book)
        {
            _context.Books.Update(book);
            return await _context.SaveChangesAsync();
        }
        public async Task<Books?> GetBookByTitleAsync(string title)
        {
            return await _context.Books.FirstOrDefaultAsync(u => u.Title == title);
        }
        public async Task<bool> GetBookExistsAsync(string searchTerm)
        {
            return await _context.Books.AnyAsync(b => b.Title.Equals(searchTerm));
        }

        public async Task<IList<Books>> GetAllBooksAsync()
        {
            return await _context.Books.Where(a => a.InStock == Constants.Yes).ToListAsync();
        }
    }
}
