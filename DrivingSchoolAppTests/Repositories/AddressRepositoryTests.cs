﻿using AutoFixture;
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

            var result = await _repository.GetAddresses();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].ID);
            Assert.AreEqual("TestCity1", resultList[0].City);
            Assert.AreEqual("TestStreet1", resultList[0].Street);
            Assert.AreEqual(10, resultList[0].Number);
            Assert.AreEqual("22-222", resultList[0].PostalCode);
            Assert.AreEqual(2, resultList[1].ID);
            Assert.AreEqual("TestCity2", resultList[1].City);
            Assert.AreEqual("TestStreet2", resultList[1].Street);
            Assert.AreEqual(20, resultList[1].Number);
            Assert.AreEqual("44-444", resultList[1].PostalCode);
        }

        [TestMethod]
        public async Task Get_Addresses_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new AddressRepository(_dbContext);

            var result = await _repository.GetAddresses();

            Assert.IsFalse(result.Any());
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
            Assert.AreEqual(2, result.ID);
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
            Assert.IsNotNull(retrievedAddress);
            Assert.AreEqual(addressToAdd.City, addedAddress.City);
            Assert.AreEqual(addressToAdd.Street, addedAddress.Street);
            Assert.AreEqual(addressToAdd.Number, addedAddress.Number);
            Assert.AreEqual(addressToAdd.PostalCode, addedAddress.PostalCode);
        }
    }
}