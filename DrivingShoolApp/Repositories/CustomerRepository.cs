﻿using DrivingSchoolApp.Models;
using DrivingSchoolApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ICustomerRepository
    {
        public Task<PagedList<CustomerGetDTO>> GetCustomers(int page, int size);
        public Task<CustomerGetDTO> GetCustomer(int customerId);
        public Task<Customer> PostCustomer(CustomerRequestDTO customerDetails);
        public Task<Customer> CheckCustomer(int customerId);
        public Task<Customer> CheckCustomerTracking(int customerId);
        public Task<Customer> DeleteCustomer(Customer customerToDelete);
        public Task<Customer> UpdateCustomer(Customer customerToUpdate, CustomerRequestDTO customerUpdate);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public CustomerRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<CustomerGetDTO>> GetCustomers(int page, int size)
        {
            return await PagedList<CustomerGetDTO>.Create(_dbContext.Customers
                            .AsNoTracking()
                            .Select(c => new CustomerGetDTO
                            {
                                Id = c.Id,
                                Name = c.Name,
                                SecondName = c.SecondName,
                                BirthDate = c.BirthDate
                            }).OrderBy(c => c.Id),
                            page, size);
        }

        public async Task<CustomerGetDTO> GetCustomer(int customerId)
        {
            return await _dbContext.Customers
                            .AsNoTracking()
                            .Where(u => u.Id == customerId)
                            .Select(u => new CustomerGetDTO
                            {
                                Id = u.Id,
                                Name = u.Name,
                                SecondName = u.SecondName,
                                BirthDate = u.BirthDate
                            })
                            .FirstOrDefaultAsync();
        }

        public async Task<Customer> PostCustomer(CustomerRequestDTO customerDetails)
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

        public async Task<Customer> CheckCustomer(int customerId)
        {
            return await _dbContext.Customers
                            .AsNoTracking()
                            .Where(u => u.Id == customerId)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
        }

        public async Task<Customer> CheckCustomerTracking(int customerId)
        {
            return await _dbContext.Customers
                            .AsNoTracking()
                            .Where(u => u.Id == customerId)
                            .FirstOrDefaultAsync();
        }

        public async Task<Customer> DeleteCustomer(Customer customerToDelete)
        {
            var deletedCustomer = _dbContext.Customers.Remove(customerToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedCustomer.Entity;
        }

        public async Task<Customer> UpdateCustomer(Customer customerToUpdate, CustomerRequestDTO customerUpdate)
        {
            customerToUpdate.BirthDate = customerUpdate.BirthDate;
            customerToUpdate.Name = customerUpdate.Name;
            customerToUpdate.SecondName = customerUpdate.SecondName;
            await _dbContext.SaveChangesAsync();
            return customerToUpdate;
        }
    }
}
