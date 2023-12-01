using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class DrivingLessonRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private DrivingLessonRepository _repository;

        public DrivingLessonRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsDrivingLessons()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3), CourseId = 1 };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6), CourseId = 2 };
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLessonRepository(_dbContext);

            var resultList = await _repository.GetDrivingLessons(1, 10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual(new DateTime(2023, 4, 3), resultList.PagedItems[0].LessonDate);
            Assert.AreEqual(1, resultList.PagedItems[0].AddressId);
            Assert.AreEqual(1, resultList.PagedItems[0].LecturerId);
            Assert.AreEqual("TestLecturerName1", resultList.PagedItems[0].LecturerName);
            Assert.AreEqual(1, resultList.PagedItems[0].CustomerId);
            Assert.AreEqual("TestCustomerName1", resultList.PagedItems[0].CustomerName);
            Assert.AreEqual(1, resultList.PagedItems[0].CourseId);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual(new DateTime(2023, 5, 6), resultList.PagedItems[1].LessonDate);
            Assert.AreEqual(2, resultList.PagedItems[1].AddressId);
            Assert.AreEqual(2, resultList.PagedItems[1].LecturerId);
            Assert.AreEqual("TestLecturerName2", resultList.PagedItems[1].LecturerName);
            Assert.AreEqual(2, resultList.PagedItems[1].CustomerId);
            Assert.AreEqual("TestCustomerName2", resultList.PagedItems[1].CustomerName);
            Assert.AreEqual(2, resultList.PagedItems[1].CourseId);
            Assert.IsFalse(resultList.HasNextPage);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.GetDrivingLessons(1, 10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.IsFalse(result.HasNextPage);
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_DrivingLessons_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _dbContext = new DrivingSchoolDbContext();
            _repository = new DrivingLessonRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetDrivingLessons(-1, 10));

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsDrivingLesson()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLessonToFind = 2;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3), CourseId = 1 };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6), CourseId = 2 };
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2); 
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.GetDrivingLesson(idOfDrivingLessonToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(new DateTime(2023, 5, 6), result.LessonDate);
            Assert.AreEqual("TestLecturerName2", result.LecturerName);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual("TestCustomerName2", result.CustomerName);
            Assert.AreEqual(2, result.CustomerId);
            Assert.AreEqual(2, result.AddressId);
            Assert.AreEqual(2, result.CourseId);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLessonToFind = 3;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3), CourseId = 1 };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6), CourseId = 2 };
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.GetDrivingLesson(idOfDrivingLessonToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ReturnsAddedDrivingLesson()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3), CourseId = 1 };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6), CourseId = 2 };
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            var drivingLessonToAdd = new DrivingLessonRequestDTO { AddressId = 2, CustomerId = 1, LecturerId = 2, LessonDate = new DateTime(2023, 10, 10), CourseId = 2 };
            _repository = new DrivingLessonRepository(_dbContext);

            var addedDrivingLesson = await _repository.PostDrivingLesson(drivingLessonToAdd);

            var retrievedDrivingLesson = await _repository.GetDrivingLesson(addedDrivingLesson.Id);
            Assert.IsNotNull(addedDrivingLesson);
            Assert.IsNotNull(retrievedDrivingLesson);
            Assert.AreEqual(addedDrivingLesson.Id, addedDrivingLesson.Id);
            Assert.AreEqual(drivingLessonToAdd.LecturerId, addedDrivingLesson.LecturerId);
            Assert.AreEqual(drivingLessonToAdd.CustomerId, addedDrivingLesson.CustomerId);
            Assert.AreEqual(drivingLessonToAdd.AddressId, addedDrivingLesson.AddressId);
            Assert.AreEqual(drivingLessonToAdd.CourseId, addedDrivingLesson.CourseId);
            Assert.AreEqual(drivingLessonToAdd.LessonDate, addedDrivingLesson.LessonDate);
            Assert.AreEqual(drivingLessonToAdd.LecturerId, retrievedDrivingLesson.LecturerId);
            Assert.AreEqual(drivingLessonToAdd.CustomerId, retrievedDrivingLesson.CustomerId);
            Assert.AreEqual(drivingLessonToAdd.AddressId, retrievedDrivingLesson.AddressId);
            Assert.AreEqual(drivingLessonToAdd.CourseId, retrievedDrivingLesson.CourseId);
            Assert.AreEqual(drivingLessonToAdd.LessonDate, retrievedDrivingLesson.LessonDate);
            Assert.AreEqual(customer1.Name, retrievedDrivingLesson.CustomerName);
            Assert.AreEqual(lecturer2.Name, retrievedDrivingLesson.LecturerName);
            Assert.AreEqual(3, await _dbContext.DrivingLessons.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_ReturnsDrivingLesson()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3), CourseId = 1 };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6), CourseId = 2 };
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.DeleteDrivingLesson(drivingLesson2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(new DateTime(2023, 5, 6), result.LessonDate);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual(2, result.CustomerId);
            Assert.AreEqual(2, result.AddressId);
            Assert.AreEqual(2, result.CourseId);
            Assert.AreEqual(1, await _dbContext.DrivingLessons.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_DrivingLesson_ReturnsDrivingLesson()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLessonToFind = 2;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3), CourseId = 1 };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6), CourseId = 2 };
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.CheckDrivingLesson(idOfDrivingLessonToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(new DateTime(2023, 5, 6), result.LessonDate);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual(2, result.CustomerId);
            Assert.AreEqual(2, result.AddressId);
            Assert.AreEqual(2, result.CourseId);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_DrivingLesson_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfDrivingLessonToFind = 3;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var address2 = new Address { Id = 2, City = "TestCity2", Number = 20, PostalCode = "44-444", Street = "TestStreet2" };
            var customer1 = new Customer { Id = 1, Name = "TestCustomerName1", SecondName = "TestCustomerSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestCustomerName2", SecondName = "TestCustomerSName2", BirthDate = new DateTime(1990, 1, 1) };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestLecturerName1", SecondName = "TestLecturerSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestLecturerName2", SecondName = "TestLecturerSName2" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var drivingLesson1 = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3), CourseId = 1 };
            var drivingLesson2 = new DrivingLesson { Id = 2, AddressId = 2, CustomerId = 2, LecturerId = 2, LessonDate = new DateTime(2023, 5, 6), CourseId = 2 };
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.Addresses.AddRangeAsync(address1, address2);
            await _dbContext.DrivingLessons.AddRangeAsync(drivingLesson1, drivingLesson2);
            await _dbContext.SaveChangesAsync();
            _repository = new DrivingLessonRepository(_dbContext);

            var result = await _repository.CheckDrivingLesson(idOfDrivingLessonToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }
    }
}