using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using Azure.Core;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private CustomerRepository _repository;

        public CustomerRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Customers_ReturnsCustomers()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000,1,1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990,1,1) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            _repository = new CustomerRepository(_dbContext);

            var result = await _repository.GetCustomers();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual("TestName1", resultList[0].Name);
            Assert.AreEqual("TestSName1", resultList[0].SecondName);
            Assert.AreEqual(new DateTime(2000, 1, 1), resultList[0].BirthDate);
            Assert.AreEqual(2, resultList[1].Id);
            Assert.AreEqual("TestName2", resultList[1].Name);
            Assert.AreEqual("TestSName2", resultList[1].SecondName);
            Assert.AreEqual(new DateTime(1990, 1, 1), resultList[1].BirthDate);
        }

        [TestMethod]
        public async Task Get_Customers_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new CustomerRepository(_dbContext);

            var result = await _repository.GetCustomers();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Get_Customer_ReturnsCsutomer()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCustomerToFind = 2;
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            _repository = new CustomerRepository(_dbContext);

            var result = await _repository.GetCustomer(idOfCustomerToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestName2", result.Name);
            Assert.AreEqual("TestSName2", result.SecondName);
            Assert.AreEqual(new DateTime(1990, 1, 1), result.BirthDate);
        }

        [TestMethod]
        public async Task Get_Customer_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCustomerToFind = 3;
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            _repository = new CustomerRepository(_dbContext);

            var result = await _repository.GetCustomer(idOfCustomerToFind);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Post_Custoemr_ReturnsAddedCustomer()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var customerToAdd = new CustomerPostDTO { Name = "TestName3", SecondName = "TestSName3", BirthDate = new DateTime(1980, 1, 1) };
            _repository = new CustomerRepository(_dbContext);

            var addedCustomer = await _repository.PostCustomer(customerToAdd);

            var retrievedCustomer = await _repository.GetCustomer(addedCustomer.Id);
            Assert.IsNotNull(addedCustomer);
            Assert.IsNotNull(retrievedCustomer);
            Assert.AreEqual(addedCustomer.Id, retrievedCustomer.Id);
            Assert.AreEqual(customerToAdd.Name, addedCustomer.Name);
            Assert.AreEqual(customerToAdd.SecondName, addedCustomer.SecondName);
            Assert.AreEqual(customerToAdd.BirthDate, addedCustomer.BirthDate);
            Assert.AreEqual(customerToAdd.Name, retrievedCustomer.Name);
            Assert.AreEqual(customerToAdd.SecondName, retrievedCustomer.SecondName);
            Assert.AreEqual(customerToAdd.BirthDate, retrievedCustomer.BirthDate);
        }
    }
}