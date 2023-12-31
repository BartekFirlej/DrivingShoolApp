﻿using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class RegistrationsRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private RegistrationRepository _repository;

        public RegistrationsRepositoryTests()
        {
            _fixture = new Fixture();
        }
        
        [TestMethod]
        public async Task Get_Registrations_ReturnsRegistrations()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 1, CustomerId = 2, RegistrationDate = new DateTime(2023, 2, 1) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var result = await _repository.GetRegistrations();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].CourseId);
            Assert.AreEqual(1, resultList[0].CustomerId);
            Assert.AreEqual(new DateTime(2023, 1, 1), resultList[0].RegistrationDate);
            Assert.AreEqual(1, resultList[1].CourseId);
            Assert.AreEqual(2, resultList[1].CustomerId);
            Assert.AreEqual(new DateTime(2023, 2, 1), resultList[1].RegistrationDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Registrations_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new RegistrationRepository(_dbContext);

            var result = await _repository.GetRegistrations();

            Assert.IsFalse(result.Any());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_ReturnsRegistrations()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 1, CustomerId = 2, RegistrationDate = new DateTime(2023, 2, 1) };
            var idOfCourseToFind = 1;
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var resultList = await _repository.GetCourseRegistrations(idOfCourseToFind, 1, 10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].CourseId);
            Assert.AreEqual(1, resultList.PagedItems[0].CustomerId);
            Assert.AreEqual(new DateTime(2023, 1, 1), resultList.PagedItems[0].RegistrationDate);
            Assert.AreEqual(1, resultList.PagedItems[1].CourseId);
            Assert.AreEqual(2, resultList.PagedItems[1].CustomerId);
            Assert.AreEqual(new DateTime(2023, 2, 1), resultList.PagedItems[1].RegistrationDate);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 1, CustomerId = 2, RegistrationDate = new DateTime(2023, 2, 1) };
            var idOfCourseToFind = 2;
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var result = await _repository.GetCourseRegistrations(idOfCourseToFind, 1 ,10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);
            Assert.IsFalse(result.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            var idOfCourseToFind = 2;
            _dbContext = new DrivingSchoolDbContext();
            _repository = new RegistrationRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetCourseRegistrations(idOfCourseToFind , - 1, 10));

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ReturnsRegistrations()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 2, CustomerId = 1, RegistrationDate = new DateTime(2023, 2, 1) };
            var idOfCustomerToFind = 1;
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var resultList = await _repository.GetCustomerRegistrations(idOfCustomerToFind, 1, 10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].CourseId);
            Assert.AreEqual(1, resultList.PagedItems[0].CustomerId);
            Assert.AreEqual(new DateTime(2023, 1, 1), resultList.PagedItems[0].RegistrationDate);
            Assert.AreEqual(2, resultList.PagedItems[1].CourseId);
            Assert.AreEqual(1, resultList.PagedItems[1].CustomerId);
            Assert.AreEqual(new DateTime(2023, 2, 1), resultList.PagedItems[1].RegistrationDate);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 2, CustomerId = 1, RegistrationDate = new DateTime(2023, 2, 1) };
            var idOfCustomerToFind = 2;
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var result = await _repository.GetCustomerRegistrations(idOfCustomerToFind, 1, 10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);
            Assert.IsFalse(result.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            var idOfCustomerToFind = 2;
            _dbContext = new DrivingSchoolDbContext();
            _repository = new RegistrationRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetCustomerRegistrations(idOfCustomerToFind, -1, 10));

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Registration_ReturnsRegistration()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 2, CustomerId = 1, RegistrationDate = new DateTime(2023, 2, 1) };
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var result = await _repository.GetRegistration(idOfCustomerToFind, idOfCourseToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual(1, result.CustomerId);
            Assert.AreEqual(new DateTime(2023, 1, 1), result.RegistrationDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Registration_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 2, CustomerId = 1, RegistrationDate = new DateTime(2023, 2, 1) };
            var idOfCustomerToFind = 2;
            var idOfCourseToFind = 2;
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var result = await _repository.GetRegistration(idOfCustomerToFind, idOfCourseToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_Registration_ReturnsRegistration()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 2, CustomerId = 1, RegistrationDate = new DateTime(2023, 2, 1) };
            var idOfCustomerToFind = 1;
            var idOfCourseToFind = 1;
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var result = await _repository.CheckRegistration(idOfCustomerToFind, idOfCourseToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual(1, result.CustomerId);
            Assert.AreEqual(new DateTime(2023, 1, 1), result.RegistrationDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_Registration_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 2, CustomerId = 1, RegistrationDate = new DateTime(2023, 2, 1) };
            var idOfCustomerToFind = 2;
            var idOfCourseToFind = 2;
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var result = await _repository.CheckRegistration(idOfCustomerToFind, idOfCourseToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Delete_Registration_ReturnsRegistration()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            var registration2 = new Registration { CourseId = 2, CustomerId = 1, RegistrationDate = new DateTime(2023, 2, 1) };

            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddRangeAsync(registration1, registration2);
            await _dbContext.SaveChangesAsync();
            _repository = new RegistrationRepository(_dbContext);

            var result = await _repository.DeleteRegistration(registration2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.CourseId);
            Assert.AreEqual(1, result.CustomerId);
            Assert.AreEqual(new DateTime(2023, 2, 1), result.RegistrationDate);
            Assert.AreEqual(1, await _dbContext.Registrations.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Post_Registration_ReturnsAddedRegistration()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            var registration1 = new Registration { CourseId = 1, CustomerId = 1, RegistrationDate = new DateTime(2023, 1, 1) };
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Registrations.AddAsync(registration1);
            await _dbContext.SaveChangesAsync();
            var registrationToAdd = new RegistrationRequestDTO { CourseId = 1, CustomerId = 2 };
            var registrationDate = new DateTime(2023, 1, 5);
            _repository = new RegistrationRepository(_dbContext);

            var addedRegistration = await _repository.PostRegistration(registrationToAdd, registrationDate);

            var retrievedRegistration = await _repository.GetRegistration(addedRegistration.CustomerId, addedRegistration.CourseId);
            Assert.IsNotNull(addedRegistration);
            Assert.IsNotNull(retrievedRegistration);
            Assert.AreEqual(registrationToAdd.CourseId, addedRegistration.CourseId);
            Assert.AreEqual(registrationToAdd.CustomerId, addedRegistration.CustomerId);
            Assert.AreEqual(registrationDate, addedRegistration.RegistrationDate);
            Assert.AreEqual(registrationToAdd.CourseId, retrievedRegistration.CourseId);
            Assert.AreEqual(registrationToAdd.CustomerId, retrievedRegistration.CustomerId);
            Assert.AreEqual(registrationDate, retrievedRegistration.RegistrationDate);
            Assert.AreEqual(addedRegistration.CourseId, retrievedRegistration.CourseId);
            Assert.AreEqual(addedRegistration.CustomerId, retrievedRegistration.CustomerId);
            Assert.AreEqual(addedRegistration.RegistrationDate, retrievedRegistration.RegistrationDate);

            Assert.AreEqual(2, await _dbContext.Registrations.CountAsync());

            await _dbContext.DisposeAsync();
        }
    }
}
