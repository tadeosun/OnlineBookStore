using EBookStore.Models;
using EBookStore.RequestDto;
using EBookStore.ResponseDto;

namespace EBookStore.Interfaces
{
    public interface IUserService
    {
        Task<GetUserResponse> GetUserById(int id);
        Task<Users?> GetUserByUsername(string name);
        Task<AddUserResponse> AddUser(UserRequestDto userRequest, string userName);
        Task<UpdateUserResponse> UpdateUser(UserRequestDto user, string userName);
    }
}
