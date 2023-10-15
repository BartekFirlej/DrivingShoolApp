using DrivingSchoolApp.Models;
using DrivingSchoolApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IUserRepository
    {
        public Task<ICollection<UserGetDTO>> GetUsers();
        public Task<UserGetDTO> GetUser(int userId);
        public Task<Customer> AddUser(UserPostDTO userDetails);
    }
    public class UserRepository : IUserRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public UserRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<UserGetDTO>> GetUsers()
        {
            return await _dbContext.Customers
                            .Select(u => new UserGetDTO
                            {
                                Id = u.Id,
                                Name = u.Name,
                                SecondName = u.SecondName,
                                BirthDate = u.BirthDate
                            })
                            .ToListAsync();
        }

        public async Task<UserGetDTO> GetUser(int userId)
        {
            return await _dbContext.Customers
                            .Where(u => u.Id == userId)
                            .Select(u => new UserGetDTO
                            {
                                Id = u.Id,
                                Name = u.Name,
                                SecondName = u.SecondName,
                                BirthDate = u.BirthDate
                            })
                            .FirstOrDefaultAsync();
        }

        public async Task<Customer> AddUser(UserPostDTO userDetails)
        {
            var userToAdd = new Customer
            {
                Name = userDetails.Name,
                SecondName = userDetails.SecondName,
                BirthDate = userDetails.BirthDate
            };
            await _dbContext.Customers.AddAsync(userToAdd);
            await _dbContext.SaveChangesAsync();
            return userToAdd;
        }
    }
}
