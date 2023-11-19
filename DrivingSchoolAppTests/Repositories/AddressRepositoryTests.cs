using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using FluentAssertions.Common;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class AddressRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private AddressRepository _repository;

        public AddressRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Addresses_ReturnsAddresses()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.SaveChangesAsync();
            _repository = new AddressRepository(_dbContext);

            var resultList = await _repository.GetAddresses(1,10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual("TestCity1", resultList.PagedItems[0].City);
            Assert.AreEqual("TestStreet1", resultList.PagedItems[0].Street);
            Assert.AreEqual(10, resultList.PagedItems[0].Number);
            Assert.AreEqual("22-222", resultList.PagedItems[0].PostalCode);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual("TestCity2", resultList.PagedItems[1].City);
            Assert.AreEqual("TestStreet2", resultList.PagedItems[1].Street);
            Assert.AreEqual(20, resultList.PagedItems[1].Number);
            Assert.AreEqual("44-444", resultList.PagedItems[1].PostalCode);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);
        }

        [TestMethod]
        public async Task Get_Addresses_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new AddressRepository(_dbContext);

            var result = await _repository.GetAddresses(1,10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.IsFalse(result.HasNextPage);
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);
        }

        [TestMethod]
        public async Task Get_Addresses_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _dbContext = new DrivingSchoolDbContext();
            _repository = new AddressRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetAddresses(-1, 10));
        }

        [TestMethod]
        public async Task Get_Address_ReturnsAddress()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfAddressToFind = 2;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.SaveChangesAsync();
            _repository = new AddressRepository(_dbContext);

            var result = await _repository.GetAddress(idOfAddressToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCity2", result.City);
            Assert.AreEqual("TestStreet2", result.Street);
            Assert.AreEqual(20, result.Number);
            Assert.AreEqual("44-444", result.PostalCode);
        }

        [TestMethod]
        public async Task Get_Address_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfAddressToFind = 3;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.SaveChangesAsync();
            _repository = new AddressRepository(_dbContext);

            var result = await _repository.GetAddress(idOfAddressToFind);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Post_Address_ReturnsAddedAddress()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.SaveChangesAsync();
            var addressToAdd = new AddressPostDTO { City = "TestCity3", Number = 30, PostalCode = "66-666", Street = "TestStreet3" };
            _repository = new AddressRepository(_dbContext);

            var addedAddress = await _repository.PostAddress(addressToAdd);
            
            var retrievedAddress = await _repository.GetAddress(addedAddress.Id);
            Assert.IsNotNull(addedAddress);
            Assert.IsNotNull(retrievedAddress);
            Assert.AreEqual(addedAddress.Id, retrievedAddress.Id);
            Assert.AreEqual(addressToAdd.City, addedAddress.City);
            Assert.AreEqual(addressToAdd.Street, addedAddress.Street);
            Assert.AreEqual(addressToAdd.Number, addedAddress.Number);
            Assert.AreEqual(addressToAdd.PostalCode, addedAddress.PostalCode);
            Assert.AreEqual(addressToAdd.City, retrievedAddress.City);
            Assert.AreEqual(addressToAdd.Street, retrievedAddress.Street);
            Assert.AreEqual(addressToAdd.Number, retrievedAddress.Number);
            Assert.AreEqual(addressToAdd.PostalCode, retrievedAddress.PostalCode);
        }

        [TestMethod]
        public async Task Delete_Address_ReturnsAddress()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.SaveChangesAsync();
            _repository = new AddressRepository(_dbContext);

            var result = await _repository.DeleteAddress(address2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCity2", result.City);
            Assert.AreEqual(20, result.Number);
            Assert.AreEqual("44-444", result.PostalCode);
            Assert.AreEqual("TestStreet2", result.Street);
            Assert.AreEqual(1, await _dbContext.Addresses.CountAsync());
        }

        [TestMethod]
        public async Task Check_Address_ReturnsAddress()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfAddressToCheck = 2;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.SaveChangesAsync();
            _repository = new AddressRepository(_dbContext);

            var result = await _repository.CheckAddress(idOfAddressToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCity2", result.City);
            Assert.AreEqual(20, result.Number);
            Assert.AreEqual("44-444", result.PostalCode);
            Assert.AreEqual("TestStreet2", result.Street);
        }

        [TestMethod]
        public async Task Check_Address_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfAddressToCheck = 3;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.SaveChangesAsync();
            _repository = new AddressRepository(_dbContext);

            var result = await _repository.CheckAddress(idOfAddressToCheck);

            Assert.IsNull(result);
        }
    }
}