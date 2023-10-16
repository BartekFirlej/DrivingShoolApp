using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.DTOs;

namespace DrivingSchoolApp.Services
{
    public interface ICustomerService
    {
        public Task<ICollection<UserGetDTO>> GetCustomers();
        public Task<UserGetDTO> GetCustomer(int customerId);
        public Task<UserGetDTO> AddCustomer(UserPostDTO newCustomer);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository userRepository)
        {
            _customerRepository = userRepository;
        }

        public async Task<ICollection<UserGetDTO>> GetCustomers()
        {
            var customers = await _customerRepository.GetCustomers();
            if(!customers.Any())
                throw new NotFoundUsersException();
            return customers;
        }

        public async Task<UserGetDTO> GetCustomer(int customerId)
        {
            var customer = await _customerRepository.GetCustomer(customerId);
            if(customer == null)
                throw new NotFoundUserException(customerId);
            return customer;
        }

        public async Task<UserGetDTO> AddCustomer(UserPostDTO newCustomer)
        {
            var createdCustomer = await _customerRepository.AddCustomer(newCustomer);
            return await _customerRepository.GetCustomer(createdCustomer.Id);
        }
    }
}
