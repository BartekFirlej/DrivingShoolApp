using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class CourseTypeRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private CourseTypeRepository _repository;

        public CourseTypeRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_CourseTypes_ReturnsCourseTypes()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicenceCategory1" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseTypeRepository(_dbContext);

            var resultList = await _repository.GetCourseTypes(1, 10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual("TestCourseType1", resultList.PagedItems[0].Name);
            Assert.AreEqual(10, resultList.PagedItems[0].DrivingHours);
            Assert.AreEqual(10, resultList.PagedItems[0].LecturesHours);
            Assert.AreEqual(18, resultList.PagedItems[0].MinimumAge);
            Assert.AreEqual(1, resultList.PagedItems[0].LicenceCategoryId);
            Assert.AreEqual("TestLicenceCategory1", resultList.PagedItems[0].LicenceCategoryName);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual("TestCourseType2", resultList.PagedItems[1].Name);
            Assert.AreEqual(20, resultList.PagedItems[1].DrivingHours);
            Assert.AreEqual(20, resultList.PagedItems[1].LecturesHours);
            Assert.AreEqual(21, resultList.PagedItems[1].MinimumAge);
            Assert.AreEqual(1, resultList.PagedItems[1].LicenceCategoryId);
            Assert.AreEqual("TestLicenceCategory1", resultList.PagedItems[1].LicenceCategoryName);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseTypes_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new CourseTypeRepository(_dbContext);

            var result = await _repository.GetCourseTypes(1, 10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.IsFalse(result.HasNextPage);
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseTypes_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _dbContext = new DrivingSchoolDbContext();
            _repository = new CourseTypeRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetCourseTypes(-1, 10));
        }

        [TestMethod]
        public async Task Get_CourseType_ReturnsCourseType()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseTypeToFind = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicenceCategory1" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseTypeRepository(_dbContext);

            var result = await _repository.GetCourseType(idOfCourseTypeToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCourseType2", result.Name);
            Assert.AreEqual(20, result.DrivingHours);
            Assert.AreEqual(20, result.LecturesHours);
            Assert.AreEqual(21, result.MinimumAge);
            Assert.AreEqual(1, result.LicenceCategoryId);
            Assert.AreEqual("TestLicenceCategory1", result.LicenceCategoryName);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseType_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseTypeToFind = 3;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicenceCategory1" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseTypeRepository(_dbContext);

            var result = await _repository.GetCourseType(idOfCourseTypeToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Post_CourseType_ReturnsAddedCourseType()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicenceCategory1" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            var courseTypeToAdd = new CourseTypeRequestDTO { Name = "TestCourseType3", DrivingHours = 30, LectureHours = 30, MinimumAge = 30, LicenceCategoryId = 1 };
            _repository = new CourseTypeRepository(_dbContext);

            var addedCourseType = await _repository.PostCourseType(courseTypeToAdd);

            var retrievedCourseType = await _repository.GetCourseType(addedCourseType.Id);
            Assert.IsNotNull(addedCourseType);
            Assert.IsNotNull(retrievedCourseType);
            Assert.AreEqual(addedCourseType.Id, retrievedCourseType.Id);
            Assert.AreEqual(courseTypeToAdd.Name, addedCourseType.Name);
            Assert.AreEqual(courseTypeToAdd.DrivingHours, addedCourseType.DrivingHours);
            Assert.AreEqual(courseTypeToAdd.LectureHours, addedCourseType.LectureHours);
            Assert.AreEqual(courseTypeToAdd.MinimumAge, addedCourseType.MinimumAge);
            Assert.AreEqual(courseTypeToAdd.LicenceCategoryId, addedCourseType.LicenceCategoryId);
            Assert.AreEqual(courseTypeToAdd.Name, retrievedCourseType.Name);
            Assert.AreEqual(courseTypeToAdd.DrivingHours, retrievedCourseType.DrivingHours);
            Assert.AreEqual(courseTypeToAdd.LectureHours, retrievedCourseType.LecturesHours);
            Assert.AreEqual(courseTypeToAdd.MinimumAge, retrievedCourseType.MinimumAge);
            Assert.AreEqual(courseTypeToAdd.LicenceCategoryId, retrievedCourseType.LicenceCategoryId);
            Assert.AreEqual(licenceCategory1.Name, retrievedCourseType.LicenceCategoryName);
            Assert.AreEqual(3, await _dbContext.CourseTypes.CountAsync());

            await _dbContext.DisposeAsync();
        }


        [TestMethod]
        public async Task Delete_CourseType_ReturnsCourseType()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseTypeRepository(_dbContext);

            var result = await _repository.DeleteCourseType(courseType2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCourseType2", result.Name);
            Assert.AreEqual(20, result.DrivingHours);
            Assert.AreEqual(20, result.LectureHours);
            Assert.AreEqual(21, result.MinimumAge);
            Assert.AreEqual(1, result.LicenceCategoryId);
            Assert.AreEqual(1, await _dbContext.CourseTypes.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_CourseType_ReturnsCourseType()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseTypeToCheck = 2;
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseTypeRepository(_dbContext);

            var result = await _repository.CheckCourseType(idOfCourseTypeToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCourseType2", result.Name);
            Assert.AreEqual(20, result.DrivingHours);
            Assert.AreEqual(20, result.LectureHours);
            Assert.AreEqual(21, result.MinimumAge);
            Assert.AreEqual(1, result.LicenceCategoryId);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_CourseType_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseTypeToCheck = 3;
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseTypeRepository(_dbContext);

            var result = await _repository.CheckCourseType(idOfCourseTypeToCheck);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_CourseTypeTracking_ReturnsCourseType()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseTypeToCheck = 2;
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseTypeRepository(_dbContext);

            var result = await _repository.CheckCourseTypeTracking(idOfCourseTypeToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCourseType2", result.Name);
            Assert.AreEqual(20, result.DrivingHours);
            Assert.AreEqual(20, result.LectureHours);
            Assert.AreEqual(21, result.MinimumAge);
            Assert.AreEqual(1, result.LicenceCategoryId);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_CourseTypeTracking_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseTypeToCheck = 3;
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseTypeRepository(_dbContext);

            var result = await _repository.CheckCourseTypeTracking(idOfCourseTypeToCheck);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Update_CourseType_ReturnsCourseType()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var courseType2 = new CourseType { Id = 2, Name = "TestCourseType2", DrivingHours = 20, LectureHours = 20, MinimumAge = 21, LicenceCategoryId = 1 };
            var courseType1Update = new CourseTypeRequestDTO { Name = "UpdatedName", DrivingHours = 30, LectureHours = 30, MinimumAge = 25, LicenceCategoryId = 1 };
            await _dbContext.CourseTypes.AddRangeAsync(courseType1, courseType2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseTypeRepository(_dbContext);

            var result = await _repository.UpdateCourseType(courseType1, courseType1Update);

            var updatedCourseType = await _repository.CheckCourseType(courseType1.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("UpdatedName", result.Name);
            Assert.AreEqual(30, result.DrivingHours);
            Assert.AreEqual(30, result.LectureHours);
            Assert.AreEqual(25, result.MinimumAge);
            Assert.AreEqual(1, result.LicenceCategoryId);
            Assert.IsNotNull(updatedCourseType);
            Assert.AreEqual(1, updatedCourseType.Id);
            Assert.AreEqual("UpdatedName", updatedCourseType.Name);
            Assert.AreEqual(30, updatedCourseType.DrivingHours);
            Assert.AreEqual(30, updatedCourseType.LectureHours);
            Assert.AreEqual(25, updatedCourseType.MinimumAge);
            Assert.AreEqual(1, updatedCourseType.LicenceCategoryId);

            await _dbContext.DisposeAsync();
        }
    }
}