using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;

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

            var resultList = await _repository.GetLicenceCategories(1,10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual("Test1", resultList.PagedItems[0].Name);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual("Test2", resultList.PagedItems[1].Name);
            Assert.IsFalse(resultList.HasNextPage);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.GetLicenceCategories(1,10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.IsFalse(result.HasNextPage);
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_LicenceCategories_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _dbContext = new DrivingSchoolDbContext();
            _repository = new LicenceCategoryRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetLicenceCategories(-1, 10));

            await _dbContext.DisposeAsync();
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

            await _dbContext.DisposeAsync();
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

            await _dbContext.DisposeAsync();
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
            var licenceCategoryToAdd = new LicenceCategoryRequestDTO { Name = "Test3" };
            _repository = new LicenceCategoryRepository(_dbContext);

            var addedLicenceCategory = await _repository.PostLicenceCategory(licenceCategoryToAdd);

            var retrievedLicenceCategory = await _repository.GetLicenceCategory(addedLicenceCategory.Id);
            Assert.IsNotNull(addedLicenceCategory);
            Assert.IsNotNull(retrievedLicenceCategory);
            Assert.AreEqual(addedLicenceCategory.Id, retrievedLicenceCategory.Id);
            Assert.AreEqual(licenceCategoryToAdd.Name, addedLicenceCategory.Name);
            Assert.AreEqual(licenceCategoryToAdd.Name, retrievedLicenceCategory.Name);
            Assert.AreEqual(3, await _dbContext.LicenceCategories.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Delete_LicenceCategory_ReturnsLicenceCategory()
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

            var result = await _repository.DeleteLicenceCategory(licenceCategory2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("Test2", result.Name);
            Assert.AreEqual(1, await _dbContext.LicenceCategories.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_LicenceCategory_ReturnsLicenceCategory()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategoryToCheck = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.SaveChangesAsync();
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.CheckLicenceCategory(idOfLicenceCategoryToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("Test2", result.Name);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_LicenceCategory_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategoryToCheck = 3;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.SaveChangesAsync();
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.CheckLicenceCategory(idOfLicenceCategoryToCheck);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_LicenceCategoryTracking_ReturnsLicenceCategory()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategoryToCheck = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.SaveChangesAsync();
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.CheckLicenceCategoryTracking(idOfLicenceCategoryToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("Test2", result.Name);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_LicenceCategoryTracking_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategoryToCheck = 3;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.SaveChangesAsync();
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.CheckLicenceCategoryTracking(idOfLicenceCategoryToCheck);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Update_LicenceCategory_ReturnsLicenceCategory()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2);
            await _dbContext.SaveChangesAsync();
            var licenceCategory1update = new LicenceCategoryRequestDTO { Name = "Test3" };
            _repository = new LicenceCategoryRepository(_dbContext);

            var result = await _repository.UpdateLicenceCategory(licenceCategory1, licenceCategory1update);

            var updatedLicenceCategory = await _repository.CheckLicenceCategory(licenceCategory1.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test3", result.Name);
            Assert.IsNotNull(updatedLicenceCategory);
            Assert.AreEqual(1, updatedLicenceCategory.Id);
            Assert.AreEqual("Test3", updatedLicenceCategory.Name);

            await _dbContext.DisposeAsync();
        }
    }
}