using EBookStore.RequestDto;

namespace EBookStore.ResponseDto
{
    public class GetUserResponse : Response
    {
        public UserResponseDto UserDetails { get; set; }
    }

    public class AddUserResponse : Response
    {
        public UserResponseDto UserDetails { get; set; }
    }

    public class UpdateUserResponse : Response
    {
        public UserResponseDto UserDetails { get; set; }
    }

    public class UserResponseDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Role { get; set; }
    }
}
