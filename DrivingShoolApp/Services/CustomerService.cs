using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using AutoMapper;

namespace DrivingSchoolApp.Services
{
    public interface ICustomerService
    {
        public Task<PagedList<CustomerGetDTO>> GetCustomers(int page, int size);
        public Task<CustomerGetDTO> GetCustomer(int customerId);
        public Task<CustomerResponseDTO> PostCustomer(CustomerRequestDTO newCustomer);
        public bool CheckCustomerAgeRequirement(DateTime customerBirthDay, int requiredAge, DateTime assignDate);
        public Task<Customer> CheckCustomer(int customerId);
        public Task<Customer> DeleteCustomer(int customerId);
        public Task<CustomerGetDTO> UpdateCustomer(int customerId, CustomerRequestDTO customerUpdate);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository userRepository, IMapper mapper)
        {
            _customerRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<CustomerGetDTO>> GetCustomers(int page, int size)
        {
            var customers = await _customerRepository.GetCustomers(page, size);
            if(!customers.PagedItems.Any())
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

        public async Task<CustomerResponseDTO> PostCustomer(CustomerRequestDTO newCustomer)
        {
            if (newCustomer.BirthDate == DateTime.MinValue)
                throw new DateTimeException("birth");
            var addedCustomer = await _customerRepository.PostCustomer(newCustomer);
            return _mapper.Map<CustomerResponseDTO>(addedCustomer);
        }

        public bool CheckCustomerAgeRequirement(DateTime customerBirthDay, int requiredAge, DateTime assignDate)
        {
            DateTime RequiredDate = customerBirthDay.AddYears(requiredAge);
            if (assignDate >= RequiredDate)
                return true;
            return false;
        }

        public async Task<Customer> CheckCustomer(int customerId)
        {
            var customer = await _customerRepository.CheckCustomer(customerId);
            if (customer == null)
                throw new NotFoundCustomerException(customerId);
            return customer;
        }

        public async Task<Customer> DeleteCustomer(int customerId)
        {
            var customerToDelete = await CheckCustomer(customerId);
            return await _customerRepository.DeleteCustomer(customerToDelete);
        }

        public async Task<CustomerGetDTO> UpdateCustomer(int customerId, CustomerRequestDTO customerUpdate)
        {
            await CheckCustomer(customerId);
            if (customerUpdate.BirthDate == DateTime.MinValue)
                throw new DateTimeException("birth");
            await _customerRepository.UpdateCustomer(customerId, customerUpdate);
            return await _customerRepository.GetCustomer(customerId);
        }
    }
}
