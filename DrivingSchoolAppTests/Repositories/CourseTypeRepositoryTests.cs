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
        }

        [TestMethod]
        public async Task Post_CourseType_ReturnsAddedCourseType()
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
            var courseTypeToAdd = new CourseTypePostDTO { Name = "TestCourseType3", DrivingHours = 30, LecturesHours = 30, MinimumAge = 30, LicenceCategoryId = 1 };
            _repository = new CourseTypeRepository(_dbContext);

            var addedCourseType = await _repository.PostCourseType(courseTypeToAdd);

            var retrievedCourseType = await _repository.GetCourseType(addedCourseType.Id);
            Assert.IsNotNull(addedCourseType);
            Assert.IsNotNull(retrievedCourseType);
            Assert.AreEqual(addedCourseType.Id, retrievedCourseType.Id);
            Assert.AreEqual(courseTypeToAdd.Name, addedCourseType.Name);
            Assert.AreEqual(courseTypeToAdd.DrivingHours, addedCourseType.DrivingHours);
            Assert.AreEqual(courseTypeToAdd.LecturesHours, addedCourseType.LectureHours);
            Assert.AreEqual(courseTypeToAdd.MinimumAge, addedCourseType.MinimumAge);
            Assert.AreEqual(courseTypeToAdd.LicenceCategoryId, addedCourseType.LicenceCategoryId);
            Assert.AreEqual(courseTypeToAdd.Name, retrievedCourseType.Name);
            Assert.AreEqual(courseTypeToAdd.DrivingHours, retrievedCourseType.DrivingHours);
            Assert.AreEqual(courseTypeToAdd.LecturesHours, retrievedCourseType.LecturesHours);
            Assert.AreEqual(courseTypeToAdd.MinimumAge, retrievedCourseType.MinimumAge);
            Assert.AreEqual(courseTypeToAdd.LicenceCategoryId, retrievedCourseType.LicenceCategoryId);
            Assert.AreEqual(licenceCategory1.Name, retrievedCourseType.LicenceCategoryName);
        }
    }
}