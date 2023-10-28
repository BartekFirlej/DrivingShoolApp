using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;

namespace DrivingSchoolApp.Services
{
    public interface ICustomerService
    {
        public Task<ICollection<CustomerGetDTO>> GetCustomers();
        public Task<CustomerGetDTO> GetCustomer(int customerId);
        public Task<CustomerGetDTO> PostCustomer(CustomerPostDTO newCustomer);
        public bool CheckCustomerAgeRequirement(DateTime customerBirthDay, int requiredAge, DateTime assignDate);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository userRepository)
        {
            _customerRepository = userRepository;
        }

        public async Task<ICollection<CustomerGetDTO>> GetCustomers()
        {
            var customers = await _customerRepository.GetCustomers();
            if(!customers.Any())
                throw new NotFoundCustomerException();
            return customers;
        }

        public async Task<CustomerGetDTO> GetCustomer(int customerId)
        {
            var customer = await _customerRepository.GetCustomer(customerId);
            if(customer == null)
                throw new NotFoundCustomerException(customerId);
            return customer;
        }

        public async Task<CustomerGetDTO> PostCustomer(CustomerPostDTO newCustomer)
        {
            if (newCustomer.BirthDate == DateTime.MinValue)
                throw new DateTimeException("birth");
            var createdCustomer = await _customerRepository.PostCustomer(newCustomer);
            return await _customerRepository.GetCustomer(createdCustomer.Id);
        }

        public bool CheckCustomerAgeRequirement(DateTime customerBirthDay, int requiredAge, DateTime assignDate)
        {
            DateTime RequiredDate = customerBirthDay.AddYears(requiredAge);
            if (assignDate >= RequiredDate)
                return true;
            return false;
        }
    }
}
