using DrivingSchoolApp.Models;
using DrivingSchoolApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ICustomerRepository
    {
        public Task<ICollection<UserGetDTO>> GetCustomers();
        public Task<UserGetDTO> GetCustomer(int customerId);
        public Task<Customer> AddCustomer(UserPostDTO customerDetails);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public CustomerRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<UserGetDTO>> GetCustomers()
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

        public async Task<UserGetDTO> GetCustomer(int customerId)
        {
            return await _dbContext.Customers
                            .Where(u => u.Id == customerId)
                            .Select(u => new UserGetDTO
                            {
                                Id = u.Id,
                                Name = u.Name,
                                SecondName = u.SecondName,
                                BirthDate = u.BirthDate
                            })
                            .FirstOrDefaultAsync();
        }

        public async Task<Customer> AddCustomer(UserPostDTO customerDetails)
        {
            var customerToAdd = new Customer
            {
                Name = customerDetails.Name,
                SecondName = customerDetails.SecondName,
                BirthDate = customerDetails.BirthDate
            };
            await _dbContext.Customers.AddAsync(customerToAdd);
            await _dbContext.SaveChangesAsync();
            return customerToAdd;
        }
    }
}
