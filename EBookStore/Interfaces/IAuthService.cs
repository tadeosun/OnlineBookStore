using EBookStore.Models;
using EBookStore.RequestDto;

namespace EBookStore.Interfaces
{
    public interface IAuthService
    {
        Task<Users> AuthenticateUsers(LoginRequestDto request);
        Task<string> GenerateJwtToken(Users user);

    }
}
