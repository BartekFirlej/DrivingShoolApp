using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class RequiredLicenceCategoryRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private RequiredLicenceCategoryRepository _repository;

        public RequiredLicenceCategoryRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Requirements_ReturnsRequirements()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var licenceCategory3 = new LicenceCategory { Id = 3, Name = "Test3" };
            var requirement1 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 1, RequiredYears = 2 };
            var requirement2 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 2, RequiredYears = 3 };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2, licenceCategory3);
            await _dbContext.RequiredLicenceCategories.AddRangeAsync(requirement1, requirement2);
            await _dbContext.SaveChangesAsync();
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var result = await _repository.GetRequirements();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(3, resultList[0].LicenceCategoryId);
            Assert.AreEqual("Test3", resultList[0].LicenceCategoryName);
            Assert.AreEqual(1, resultList[0].RequiredLicenceCategoryId);
            Assert.AreEqual("Test1", resultList[0].RequiredLicenceCategoryName);
            Assert.AreEqual(2, resultList[0].RequiredYears);
            Assert.AreEqual(3, resultList[1].LicenceCategoryId);
            Assert.AreEqual("Test3", resultList[1].LicenceCategoryName);
            Assert.AreEqual(2, resultList[1].RequiredLicenceCategoryId);
            Assert.AreEqual("Test2", resultList[1].RequiredLicenceCategoryName);
            Assert.AreEqual(3, resultList[1].RequiredYears);
        }

        [TestMethod]
        public async Task Get_Requirements_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var result = await _repository.GetRequirements();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Get_RequirementsOfLicenceCategory_ReturnsRequirements()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategory = 3;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var licenceCategory3 = new LicenceCategory { Id = 3, Name = "Test3" };
            var requirement1 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 1, RequiredYears = 2 };
            var requirement2 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 2, RequiredYears = 3 };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2, licenceCategory3);
            await _dbContext.RequiredLicenceCategories.AddRangeAsync(requirement1, requirement2);
            await _dbContext.SaveChangesAsync();
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var result = await _repository.GetRequirements(idOfLicenceCategory);

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(3, resultList[0].LicenceCategoryId);
            Assert.AreEqual("Test3", resultList[0].LicenceCategoryName);
            Assert.AreEqual(1, resultList[0].RequiredLicenceCategoryId);
            Assert.AreEqual("Test1", resultList[0].RequiredLicenceCategoryName);
            Assert.AreEqual(2, resultList[0].RequiredYears);
            Assert.AreEqual(3, resultList[1].LicenceCategoryId);
            Assert.AreEqual("Test3", resultList[1].LicenceCategoryName);
            Assert.AreEqual(2, resultList[1].RequiredLicenceCategoryId);
            Assert.AreEqual("Test2", resultList[1].RequiredLicenceCategoryName);
            Assert.AreEqual(3, resultList[1].RequiredYears);
        }

        [TestMethod]
        public async Task Get_RequirementsOfLicenceCategory_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategory = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var licenceCategory3 = new LicenceCategory { Id = 3, Name = "Test3" };
            var requirement1 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 1, RequiredYears = 2 };
            var requirement2 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 2, RequiredYears = 3 };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2, licenceCategory3);
            await _dbContext.RequiredLicenceCategories.AddRangeAsync(requirement1, requirement2);
            await _dbContext.SaveChangesAsync();
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var result = await _repository.GetRequirements(idOfLicenceCategory);

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Get_Requirement_ReturnsRequirement()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategory = 3;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var licenceCategory3 = new LicenceCategory { Id = 3, Name = "Test3" };
            var requirement1 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 1, RequiredYears = 2 };
            var requirement2 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 2, RequiredYears = 3 };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2, licenceCategory3);
            await _dbContext.RequiredLicenceCategories.AddRangeAsync(requirement1, requirement2);
            await _dbContext.SaveChangesAsync();
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var result = await _repository.GetRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.LicenceCategoryId);
            Assert.AreEqual("Test3", result.LicenceCategoryName);
            Assert.AreEqual(2, result.RequiredLicenceCategoryId);
            Assert.AreEqual("Test2", result.RequiredLicenceCategoryName);
            Assert.AreEqual(3, result.RequiredYears);
        }

        [TestMethod]
        public async Task Get_Requirement_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategory = 2;
            var idOfRequiredLicenceCategory = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var licenceCategory3 = new LicenceCategory { Id = 3, Name = "Test3" };
            var requirement1 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 1, RequiredYears = 2 };
            var requirement2 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 2, RequiredYears = 3 };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2, licenceCategory3);
            await _dbContext.RequiredLicenceCategories.AddRangeAsync(requirement1, requirement2);
            await _dbContext.SaveChangesAsync();
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var result = await _repository.GetRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Check_Requirement_ReturnsRequirement()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategory = 3;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var licenceCategory3 = new LicenceCategory { Id = 3, Name = "Test3" };
            var requirement1 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 1, RequiredYears = 2 };
            var requirement2 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 2, RequiredYears = 3 };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2, licenceCategory3);
            await _dbContext.RequiredLicenceCategories.AddRangeAsync(requirement1, requirement2);
            await _dbContext.SaveChangesAsync();
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var result = await _repository.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.LicenceCategoryId);
            Assert.AreEqual(2, result.RequiredLicenceCategoryId);
            Assert.AreEqual(3, result.RequiredYears);
        }

        [TestMethod]
        public async Task Check_Requirement_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategory = 2;
            var idOfRequiredLicenceCategory = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var licenceCategory3 = new LicenceCategory { Id = 3, Name = "Test3" };
            var requirement1 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 1, RequiredYears = 2 };
            var requirement2 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 2, RequiredYears = 3 };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2, licenceCategory3);
            await _dbContext.RequiredLicenceCategories.AddRangeAsync(requirement1, requirement2);
            await _dbContext.SaveChangesAsync();
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var result = await _repository.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Post_Requirement_ReturnsAddedRequirement()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategory = 2;
            var idOfRequiredLicenceCategory = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var licenceCategory3 = new LicenceCategory { Id = 3, Name = "Test3" };
            var requirement1 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 1, RequiredYears = 2 };
            var requirement2 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 2, RequiredYears = 3 };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2, licenceCategory3);
            await _dbContext.RequiredLicenceCategories.AddRangeAsync(requirement1, requirement2);
            await _dbContext.SaveChangesAsync();
            var requirementToAdd = new RequiredLicenceCategoryPostDTO { LicenceCategoryId = 2, RequiredLicenceCategoryId = 1, RequiredYears = 5 };
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var addedRequirement = await _repository.PostRequirement(requirementToAdd);

            var retrievedRequirement = await _repository.GetRequirement(addedRequirement.LicenceCategoryId, addedRequirement.RequiredLicenceCategoryId);
            Assert.IsNotNull(addedRequirement);
            Assert.IsNotNull(retrievedRequirement);
            Assert.AreEqual(addedRequirement.LicenceCategoryId, retrievedRequirement.LicenceCategoryId);
            Assert.AreEqual(addedRequirement.RequiredLicenceCategoryId, retrievedRequirement.RequiredLicenceCategoryId);
            Assert.AreEqual(requirementToAdd.LicenceCategoryId, addedRequirement.LicenceCategoryId);
            Assert.AreEqual(requirementToAdd.RequiredLicenceCategoryId, addedRequirement.RequiredLicenceCategoryId);
            Assert.AreEqual(requirementToAdd.RequiredYears, addedRequirement.RequiredYears);
            Assert.AreEqual(requirementToAdd.LicenceCategoryId, retrievedRequirement.LicenceCategoryId);
            Assert.AreEqual(requirementToAdd.RequiredLicenceCategoryId, retrievedRequirement.RequiredLicenceCategoryId);
            Assert.AreEqual(requirementToAdd.RequiredYears, retrievedRequirement.RequiredYears);
            Assert.AreEqual("Test2", retrievedRequirement.LicenceCategoryName);
            Assert.AreEqual("Test1", retrievedRequirement.RequiredLicenceCategoryName);
        }

        [TestMethod]
        public async Task Delete_Requirement_ReturnsRequirement()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLicenceCategory = 3;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "Test1" };
            var licenceCategory2 = new LicenceCategory { Id = 2, Name = "Test2" };
            var licenceCategory3 = new LicenceCategory { Id = 3, Name = "Test3" };
            var requirement1 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 1, RequiredYears = 2 };
            var requirement2 = new RequiredLicenceCategory { LicenceCategoryId = 3, RequiredLicenceCategoryId = 2, RequiredYears = 3 };
            await _dbContext.LicenceCategories.AddRangeAsync(licenceCategory1, licenceCategory2, licenceCategory3);
            await _dbContext.RequiredLicenceCategories.AddRangeAsync(requirement1, requirement2);
            await _dbContext.SaveChangesAsync();
            _repository = new RequiredLicenceCategoryRepository(_dbContext);

            var result = await _repository.DeleteRequirement(requirement2);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.LicenceCategoryId);
            Assert.AreEqual(2, result.RequiredLicenceCategoryId);
            Assert.AreEqual(3, result.RequiredYears);
            Assert.AreEqual(1, _dbContext.RequiredLicenceCategories.Count());
        }
    }
}