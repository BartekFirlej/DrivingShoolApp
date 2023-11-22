using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class CourseRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private CourseRepository _repository;

        public CourseRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Courses_ReturnsCourses()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020,1,1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseRepository(_dbContext);

            var resultList = await _repository.GetCourses(1, 10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual("TestCourse1", resultList.PagedItems[0].Name);
            Assert.AreEqual(new DateTime(2020, 1, 1), resultList.PagedItems[0].BeginDate);
            Assert.AreEqual(10, resultList.PagedItems[0].Limit);
            Assert.AreEqual(100, resultList.PagedItems[0].Price);
            Assert.AreEqual(1, resultList.PagedItems[0].CourseType.Id);
            Assert.AreEqual(10, resultList.PagedItems[0].CourseType.DrivingHours);
            Assert.AreEqual(10, resultList.PagedItems[0].CourseType.LecturesHours);
            Assert.AreEqual(18, resultList.PagedItems[0].CourseType.MinimumAge);
            Assert.AreEqual(1, resultList.PagedItems[0].CourseType.LicenceCategoryId);
            Assert.AreEqual("TestLicence", resultList.PagedItems[0].CourseType.LicenceCategoryName);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual("TestCourse2", resultList.PagedItems[1].Name);
            Assert.AreEqual(new DateTime(2020, 2, 2), resultList.PagedItems[1].BeginDate);
            Assert.AreEqual(20, resultList.PagedItems[1].Limit);
            Assert.AreEqual(200, resultList.PagedItems[1].Price);
            Assert.AreEqual(1, resultList.PagedItems[1].CourseType.Id);
            Assert.AreEqual(10, resultList.PagedItems[1].CourseType.DrivingHours);
            Assert.AreEqual(10, resultList.PagedItems[1].CourseType.LecturesHours);
            Assert.AreEqual(18, resultList.PagedItems[1].CourseType.MinimumAge);
            Assert.AreEqual(1, resultList.PagedItems[1].CourseType.LicenceCategoryId);
            Assert.AreEqual("TestLicence", resultList.PagedItems[1].CourseType.LicenceCategoryName);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Courses_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new CourseRepository(_dbContext);

            var result = await _repository.GetCourses(1, 10);

            Assert.IsFalse(result.PagedItems.Any());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Courses_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _dbContext = new DrivingSchoolDbContext();
            _repository = new CourseRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetCourses(-1, 10));

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Course_ReturnsCourse()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToFind = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseRepository(_dbContext);

            var result = await _repository.GetCourse(idOfCourseToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCourse2", result.Name);
            Assert.AreEqual(new DateTime(2020, 2, 2), result.BeginDate);
            Assert.AreEqual(20, result.Limit);
            Assert.AreEqual(200, result.Price);
            Assert.AreEqual(1, result.CourseType.Id);
            Assert.AreEqual(10, result.CourseType.DrivingHours);
            Assert.AreEqual(10, result.CourseType.LecturesHours);
            Assert.AreEqual(18, result.CourseType.MinimumAge);
            Assert.AreEqual(1, result.CourseType.LicenceCategoryId);
            Assert.AreEqual("TestLicence", result.CourseType.LicenceCategoryName);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Course_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToFind = 3;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseRepository(_dbContext);

            var result = await _repository.GetCourse(idOfCourseToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseAssignedPeopleCount_ReturnsPeopleNumber()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToFind = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 1, CustomerId = 2, RegistrationDate = new DateTime(2023, 1, 1) };
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseRepository(_dbContext);

            var result = await _repository.GetCourseAssignedPeopleCount(idOfCourseToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseAssignedPeopleCount_ReturnsZeroNumber()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToFind = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 1, CustomerId = 2, RegistrationDate = new DateTime(2023, 1, 1) };
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseRepository(_dbContext);

            var result = await _repository.GetCourseAssignedPeopleCount(idOfCourseToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Post_Course_ReturnsAddedCourse()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.SaveChangesAsync();
            var courseToAdd = new CoursePostDTO { Name = "TestCourse3", BeginDate = new DateTime(2020, 3, 3), CourseTypeId = 1, Limit = 30, Price = 300 };
            _repository = new CourseRepository(_dbContext);

            var addedCourse = await _repository.PostCourse(courseToAdd);

            var retrievedCourse = await _repository.GetCourse(addedCourse.Id);
            Assert.IsNotNull(addedCourse);
            Assert.IsNotNull(retrievedCourse);
            Assert.AreEqual(addedCourse.Id, retrievedCourse.Id);
            Assert.AreEqual(courseToAdd.Name, addedCourse.Name);
            Assert.AreEqual(courseToAdd.BeginDate, addedCourse.BeginDate);
            Assert.AreEqual(courseToAdd.CourseTypeId, addedCourse.CourseTypeId);
            Assert.AreEqual(courseToAdd.Limit, addedCourse.Limit);
            Assert.AreEqual(courseToAdd.Price, addedCourse.Price);
            Assert.AreEqual(courseToAdd.Name, retrievedCourse.Name);
            Assert.AreEqual(courseToAdd.BeginDate, retrievedCourse.BeginDate);
            Assert.AreEqual(courseToAdd.CourseTypeId, retrievedCourse.CourseType.Id);
            Assert.AreEqual(courseToAdd.Limit, retrievedCourse.Limit);
            Assert.AreEqual(courseToAdd.Price, retrievedCourse.Price);
            Assert.AreEqual(courseType1.Name, retrievedCourse.CourseType.Name);
            Assert.AreEqual(courseType1.DrivingHours, retrievedCourse.CourseType.DrivingHours);
            Assert.AreEqual(courseType1.LectureHours, retrievedCourse.CourseType.LecturesHours);
            Assert.AreEqual(courseType1.MinimumAge, retrievedCourse.CourseType.MinimumAge);
            Assert.AreEqual(courseType1.LicenceCategoryId, retrievedCourse.CourseType.LicenceCategoryId);
            Assert.AreEqual(licenceCategory1.Name, retrievedCourse.CourseType.LicenceCategoryName);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Delete_Course_ReturnsCourse()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseRepository(_dbContext);

            var result = await _repository.DeleteCourse(course2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCourse2", result.Name);
            Assert.AreEqual(new DateTime(2020, 2, 2), result.BeginDate);
            Assert.AreEqual(1, result.CourseTypeId);
            Assert.AreEqual(20, result.Limit);
            Assert.AreEqual(200, result.Price);
            Assert.AreEqual(1, await _dbContext.Courses.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_Course_ReturnsCourse()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToCheck = 2;
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseRepository(_dbContext);

            var result = await _repository.CheckCourse(idOfCourseToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestCourse2", result.Name);
            Assert.AreEqual(new DateTime(2020, 2, 2), result.BeginDate);
            Assert.AreEqual(1, result.CourseTypeId);
            Assert.AreEqual(20, result.Limit);
            Assert.AreEqual(200, result.Price);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_Course_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToCheck = 3;
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseRepository(_dbContext);

            var result = await _repository.CheckCourse(idOfCourseToCheck);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }
    }
}