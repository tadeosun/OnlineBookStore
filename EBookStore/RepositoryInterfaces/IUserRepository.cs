using System.Collections.Generic;
using EBookStore.Models;

public interface IUserRepository
{
    Task<Users?> GetUserByIdAsync(int userId);

    Task<Users?> GetUserByUsernameAsync(string username);

    Task<int> AddUserAsync(Users user);

    Task<int> UpdateUserAsync(Users user);

    Task DeleteUserAsync(Users user);
}