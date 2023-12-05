using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class CustomerLectureRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private CustomerLectureRepository _repository;

        public CustomerLectureRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_GetCustomersLectures_ReturnsCustomersLectures()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture2.Customers.Add(customer2);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.GetCustomersLectures();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].CustomerId);
            Assert.AreEqual("TestName1", resultList[0].CustomerName);
            Assert.AreEqual(1, resultList[0].LectureId);
            Assert.AreEqual(new DateTime(2023, 10, 10), resultList[0].LectureDate);
            Assert.AreEqual(2, resultList[1].CustomerId);
            Assert.AreEqual("TestName2", resultList[1].CustomerName);
            Assert.AreEqual(2, resultList[1].LectureId);
            Assert.AreEqual(new DateTime(2023, 11, 11), resultList[1].LectureDate);

            await _dbContext.DisposeAsync();
        }

        
        [TestMethod]
        public async Task Get_GetCustomersLectures_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.GetCustomersLectures();

            Assert.IsFalse(result.Any());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ReturnsCustomersLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture1.Customers.Add(customer2);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var idOfLectureToFind = 1;
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.GetCustomersLecture(idOfLectureToFind);

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].CustomerId);
            Assert.AreEqual("TestName1", resultList[0].CustomerName);
            Assert.AreEqual(1, resultList[0].LectureId);
            Assert.AreEqual(new DateTime(2023, 10, 10), resultList[0].LectureDate);
            Assert.AreEqual(2, resultList[1].CustomerId);
            Assert.AreEqual("TestName2", resultList[1].CustomerName);
            Assert.AreEqual(1, resultList[1].LectureId);
            Assert.AreEqual(new DateTime(2023, 10, 10), resultList[1].LectureDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture1.Customers.Add(customer2);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var idOfLectureToFind = 2;
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.GetCustomersLecture(idOfLectureToFind);

            Assert.IsFalse(result.Any());

            await _dbContext.DisposeAsync();
        }
        
        [TestMethod]
        public async Task Get_CustomerLectures_ReturnsCustomerLectures()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture2.Customers.Add(customer1);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var idOfCustomerToFind = 1;
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.GetCustomerLectures(idOfCustomerToFind);

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].CustomerId);
            Assert.AreEqual("TestName1", resultList[0].CustomerName);
            Assert.AreEqual(1, resultList[0].LectureId);
            Assert.AreEqual(new DateTime(2023, 10, 10), resultList[0].LectureDate);
            Assert.AreEqual(1, resultList[1].CustomerId);
            Assert.AreEqual("TestName1", resultList[1].CustomerName);
            Assert.AreEqual(2, resultList[1].LectureId);
            Assert.AreEqual(new DateTime(2023, 11, 11), resultList[1].LectureDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture2.Customers.Add(customer1);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var idOfCustomerToFind = 2;
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.GetCustomerLectures(idOfCustomerToFind);

            Assert.IsFalse(result.Any());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CustomerLecture_ReturnsCustomerLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture2.Customers.Add(customer1);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var idOfCustomerToFind = 1;
            var idOfLectureToFind = 1;
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.GetCustomerLecture(idOfCustomerToFind, idOfLectureToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CustomerId);
            Assert.AreEqual("TestName1", result.CustomerName);
            Assert.AreEqual(1, result.LectureId);
            Assert.AreEqual(new DateTime(2023, 10, 10), result.LectureDate);

            await _dbContext.DisposeAsync();
        }

        public async Task Get_CustomerLecture_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture2.Customers.Add(customer1);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var idOfCustomerToFind = 2;
            var idOfLectureToFind = 2;
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.GetCustomerLecture(idOfCustomerToFind, idOfLectureToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_CustomerLecture_ReturnsCustomerLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture2.Customers.Add(customer1);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var idOfCustomerToFind = 1;
            var idOfLectureToFind = 1;
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CustomerId);
            Assert.AreEqual(1, result.LectureId);

            await _dbContext.DisposeAsync();
        }

        public async Task Check_CustomerLecture_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture2.Customers.Add(customer1);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var idOfCustomerToFind = 2;
            var idOfLectureToFind = 2;
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.CheckCustomerLecture(idOfCustomerToFind, idOfLectureToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ReturnsAddedCustomerLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture2.Customers.Add(customer1);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var customerLectureToAdd = new CustomerLectureRequestDTO { CustomerId = 2, LectureId = 1 };
            _repository = new CustomerLectureRepository(_dbContext);

            var addedCustomerLecture = await _repository.PostCustomerLecture(customerLectureToAdd);

            Assert.IsNotNull(addedCustomerLecture);
            Assert.AreEqual(customerLectureToAdd.CustomerId, addedCustomerLecture.CustomerId);
            Assert.AreEqual(customerLectureToAdd.LectureId, addedCustomerLecture.LectureId);
            Assert.AreEqual(2, await _dbContext.Lectures.Where(l => l.Id == 1).Include(l => l.Customers).SelectMany(l => l.Customers).CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ReturnsCustomerLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var customer1 = new Customer { Id = 1, Name = "TestName1", SecondName = "TestSName1", BirthDate = new DateTime(2000, 1, 1) };
            var customer2 = new Customer { Id = 2, Name = "TestName2", SecondName = "TestSName2", BirthDate = new DateTime(1990, 1, 1) };
            lecture1.Customers.Add(customer1);
            lecture2.Customers.Add(customer1);
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.Customers.AddRangeAsync(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            _repository = new CustomerLectureRepository(_dbContext);

            var result = await _repository.DeleteCustomerLecture(customer1, lecture1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CustomerId);
            Assert.AreEqual(1, result.LectureId);
            Assert.AreEqual(0, await _dbContext.Lectures.Where(l => l.Id == 1).Include(l => l.Customers).SelectMany(l => l.Customers).CountAsync());

            await _dbContext.DisposeAsync();
        }
    }
}