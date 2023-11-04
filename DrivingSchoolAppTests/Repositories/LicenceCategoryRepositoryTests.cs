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
    public class LicenceCategoryRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private LicenceCategoryRepository _repository;

        public LicenceCategoryRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ReturnsLicenceCategories()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.SaveChangesAsync();
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.GetLicenceCategories();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual("Test1", resultList[0].Name);
            Assert.AreEqual(2, resultList[1].Id);
            Assert.AreEqual("Test2", resultList[1].Name);
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.GetLicenceCategories();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ReturnsLicenceCategory()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategoryToFind = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.SaveChangesAsync();
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.GetLicenceCategory(idOfLicenceCategoryToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("Test2", result.Name);
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategoryToFind = 3;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.SaveChangesAsync();
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.GetLicenceCategory(idOfLicenceCategoryToFind);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Post_LicenceCategory_ReturnsAddedLicenceCategory()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.SaveChangesAsync();
            var licenceCategoryToAdd = new LicenceCategoryPostDTO { Name = "Test3" };
            _repository = new LicenceCategoryRepository(_dbContext);

            var addedLicenceCategory = await _repository.PostLicenceCategory(licenceCategoryToAdd);

            var retrievedLicenceCategory = await _repository.GetLicenceCategory(addedLicenceCategory.Id);
            Assert.IsNotNull(retrievedLicenceCategory);
            Assert.AreEqual(licenceCategoryToAdd.Name, addedLicenceCategory.Name);
            Assert.AreEqual(licenceCategoryToAdd.Name, retrievedLicenceCategory.Name);
        }
    }
}