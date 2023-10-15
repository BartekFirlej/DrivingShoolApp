using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;

namespace DrivingSchoolApp.Services
{
    public interface IUserService
    {
        public Task<ICollection<UserGetDTO>> GetUsers();
        public Task<UserGetDTO> GetUser(int userId);
        public Task<UserGetDTO> AddUser(UserPostDTO newUser);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ICollection<UserGetDTO>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            if(!users.Any())
                throw new NotFoundUsersException();
            return users;
        }

        public async Task<UserGetDTO> GetUser(int userId)
        {
            var user = await _userRepository.GetUser(userId);
            if(user == null)
                throw new NotFoundUserException(userId);
            return user;
        }

        public async Task<UserGetDTO> AddUser(UserPostDTO newUser)
        {
            var createdUser = await _userRepository.AddUser(newUser);
            return await _userRepository.GetUser(createdUser.Id);
        }
    }
}
