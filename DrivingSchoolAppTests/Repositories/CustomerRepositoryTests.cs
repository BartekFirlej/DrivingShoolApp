using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;

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

            var resultList = await _repository.GetCustomers(1, 10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual("TestName1", resultList.PagedItems[0].Name);
            Assert.AreEqual("TestSName1", resultList.PagedItems[0].SecondName);
            Assert.AreEqual(new DateTime(2000, 1, 1), resultList.PagedItems[0].BirthDate);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual("TestName2", resultList.PagedItems[1].Name);
            Assert.AreEqual("TestSName2", resultList.PagedItems[1].SecondName);
            Assert.AreEqual(new DateTime(1990, 1, 1), resultList.PagedItems[1].BirthDate);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);
        }

        [TestMethod]
        public async Task Get_Customers_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new CustomerRepository(_dbContext);

            var result = await _repository.GetCustomers(1, 10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.IsFalse(result.HasNextPage);
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);
        }

        [TestMethod]
        public async Task Get_Customers_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _dbContext = new DrivingSchoolDbContext();
            _repository = new CustomerRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetCustomers(-1, 10));
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
        public async Task Post_Customer_ReturnsAddedCustomer()
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

        [TestMethod]
        public async Task Delete_Customer_ReturnsCustomer()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            _repository = new CustomerRepository(_dbContext);

            var result = await _repository.DeleteCustomer(customer2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestName2", result.Name);
            Assert.AreEqual("TestSName2", result.SecondName);
            Assert.AreEqual(new DateTime(1990, 1, 1), result.BirthDate);
            Assert.AreEqual(1, await _dbContext.Customers.CountAsync());
        }

        [TestMethod]
        public async Task Check_Customer_ReturnsCustomer()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCustomerToCheck = 2;
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            _repository = new CustomerRepository(_dbContext);

            var result = await _repository.CheckCustomer(idOfCustomerToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestName2", result.Name);
            Assert.AreEqual("TestSName2", result.SecondName);
            Assert.AreEqual(new DateTime(1990, 1, 1), result.BirthDate);
        }

        [TestMethod]
        public async Task Check_Customer_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCustomerToCheck = 3;
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            _repository = new CustomerRepository(_dbContext);

            var result = await _repository.CheckCustomer(idOfCustomerToCheck);

            Assert.IsNull(result);
        }
    }
}