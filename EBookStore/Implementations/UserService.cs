using EBookStore.Common;
using EBookStore.Data;
using EBookStore.Interfaces;
using EBookStore.Models;
using EBookStore.RequestDto;
using EBookStore.ResponseDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static EBookStore.Common.Constants;

namespace EBookStore.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AddUserResponse> AddUser(UserRequestDto userRequest, string loggedinUser)
        {
            try
            {
                var response = new AddUserResponse();

                bool isUserValid = ValidateUserRequest(userRequest, response);
                if (!isUserValid) { return response; }

                var existingUser = await GetUserByUsername(userRequest.Username);

                if (existingUser != null)
                {
                    response.ResponseCode = ResponseMapping.ResponseCode06;
                    response.ResponseMessage = string.Format("Username", ResponseMapping.ResponseCode06Message);
                    return response;
                }

                var insertRequest = new Users()
                {
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password),
                    Email = userRequest.Email,
                    Username = userRequest.Username,
                    MobileNumber = userRequest.MobileNumber,
                    CreatedBy = loggedinUser,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Role = userRequest.Role = Constants.Role.User ?? Constants.Role.Admin
                };


                int rowID = await _userRepository.AddUserAsync(insertRequest);
                if (rowID > 0)
                {
                    var userInfo = new UserResponseDto()
                    { 
                        Email = userRequest.Email,
                        MobileNumber = userRequest.MobileNumber,
                        Role = userRequest.Role,
                        Username= userRequest.Username,
                        
                    };
                    response.UserDetails = userInfo;
                    response.ResponseCode = ResponseMapping.ResponseCode00;
                    response.ResponseMessage = ResponseMapping.ResponseCode00Message;
                }
                else
                {
                    response.ResponseCode = ResponseMapping.ResponseCode09;
                    response.ResponseMessage = string.Format(ResponseMapping.ResponseCode09Message, "User");
                }

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GetUserResponse> GetUserById(int id)
        {
            try
            {
                var response = new GetUserResponse();
                var userInfo = await _userRepository.GetUserByIdAsync(id);
                if (userInfo != null)
                {
                    var user = new UserResponseDto()
                    {
                        Email = userInfo.Email,
                        Username = userInfo.Username,
                        MobileNumber = userInfo.MobileNumber,
                        Role = userInfo.Role
                    };
                    response.UserDetails = user;
                    response.ResponseCode = ResponseMapping.ResponseCode00;
                    response.ResponseMessage = ResponseMapping.ResponseCode00Message;
                    return response;
                }
                response.ResponseCode = ResponseMapping.ResponseCode07;
                response.ResponseMessage = string.Format(ResponseMapping.ResponseCode07Message, "User");
                return response;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Users?> GetUserByUsername(string username)
        {
            try
            {
                var users = await _userRepository.GetUserByUsernameAsync(username);
                return users;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UpdateUserResponse> UpdateUser(UserRequestDto userRequest, string loggedinUser)
        {
            try
            {
                var response = new UpdateUserResponse();

                bool isUserValid = ValidateUserRequest(userRequest, response);
                if (!isUserValid) { return response; }

                Users existingUser = await GetUserByUsername(userRequest.Username);
                if (existingUser == null)
                {
                    response.ResponseCode = ResponseMapping.ResponseCode07;
                    response.ResponseMessage = string.Format(ResponseMapping.ResponseCode07Message, "User");
                    return response;
                }
                else
                {
                    existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
                    existingUser.Email = userRequest.Email;
                    existingUser.Username = userRequest.Username;
                    existingUser.MobileNumber = userRequest.MobileNumber;
                    existingUser.ModifiedBy = loggedinUser;
                    existingUser.DateModified = DateTime.Now;
                    existingUser.Role = userRequest.Role;
                }


                int rowAffected = await _userRepository.UpdateUserAsync(existingUser);
                if (rowAffected > 0)
                {
                    var updateddetails = new UserResponseDto()
                    {
                        Email = existingUser.Email,
                        MobileNumber = existingUser.MobileNumber,
                        Username = existingUser.Username,
                        Role = existingUser.Role
                    };
                    response.UserDetails = updateddetails;
                    response.ResponseCode = ResponseMapping.ResponseCode00;
                    response.ResponseMessage = ResponseMapping.ResponseCode00Message;
                }
                else
                {
                    response.ResponseCode = ResponseMapping.ResponseCode10;
                    response.ResponseMessage = string.Format(ResponseMapping.ResponseCode10Message, "User");
                }

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static bool ValidateUserRequest(UserRequestDto request, Response response)
        {
            bool isrequestvalid = true;
            if (request == null || (string.IsNullOrEmpty(request.Email)
             || string.IsNullOrEmpty(request.MobileNumber) || string.IsNullOrEmpty(request.Username)
             || string.IsNullOrEmpty(request.Role) || string.IsNullOrEmpty(request.Password)))
            {
                response.ResponseCode = ResponseMapping.ResponseCode02;
                response.ResponseMessage = ResponseMapping.ResponseCode02Message;
                isrequestvalid = false;
                return isrequestvalid;
            }

            if (!Helper.IsValidEmail(request.Email))
            {
                response.ResponseCode = ResponseMapping.ResponseCode13;
                response.ResponseMessage = ResponseMapping.ResponseCode13Message;
                isrequestvalid = false;
                return isrequestvalid;
            }

            if (request.Username.Length > 10)
            {
                response.ResponseCode = ResponseMapping.ResponseCode12;
                response.ResponseMessage = ResponseMapping.ResponseCode12Message;
                isrequestvalid = false;
                return isrequestvalid;
            }

            if (!Helper.IsValidPassword(request.Password))
            {
                response.ResponseCode = ResponseMapping.ResponseCode11;
                response.ResponseMessage = ResponseMapping.ResponseCode11Message;
                isrequestvalid = false;
                return isrequestvalid;
            }

            return isrequestvalid;
        }

    }
}
