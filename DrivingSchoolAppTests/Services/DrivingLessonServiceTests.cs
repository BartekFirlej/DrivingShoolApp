using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Microsoft.Data.SqlClient;
using Moq;
using System.Runtime.Serialization;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class DrivingLessonServiceTests
    {
        private Mock<IDrivingLessonRepository> _drivingLessonRepositoryMock;
        private Mock<ICustomerService> _customerServiceMock;
        private Mock<ILecturerService> _lecturerServiceMock;
        private Mock<IAddressService> _addressServiceMock;
        private Mock<ICourseService> _courseServiceMock;
        private Fixture _fixture;
        private DrivingLessonService _service;

        public DrivingLessonServiceTests()
        {
            _fixture = new Fixture();
            _drivingLessonRepositoryMock = new Mock<IDrivingLessonRepository>();
            _customerServiceMock = new Mock<ICustomerService>();
            _lecturerServiceMock = new Mock<ILecturerService>();
            _addressServiceMock = new Mock<IAddressService>();
            _courseServiceMock = new Mock<ICourseService>();
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ReturnsDrivingLessons()
        {
            var drivingLesson = new DrivingLessonGetDTO();
            var drivingLessonsList = new PagedList<DrivingLessonGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<DrivingLessonGetDTO> { drivingLesson }, HasNextPage = false };
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLessons(1, 10)).ReturnsAsync(drivingLessonsList);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            var result = await _service.GetDrivingLessons(1, 10);

            Assert.AreEqual(drivingLessonsList, result);
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ThrowsNotFoundDrivingLessonException()
        {
            var drivingLessonsList = new PagedList<DrivingLessonGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<DrivingLessonGetDTO>(), HasNextPage = false };
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLessons(1, 10)).ReturnsAsync(drivingLessonsList);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.GetDrivingLessons(1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLessons_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLessons(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetDrivingLessons(-1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLessons_PropagatesSqlConnectionException()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLessons(1, 10)).ThrowsAsync(exception);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<SqlException>(async () => await _service.GetDrivingLessons(1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsDrivingLesson()
        {
            var drivingLesson = new DrivingLessonGetDTO();
            var idOfDrivingLessonToFind = 1;
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(idOfDrivingLessonToFind)).ReturnsAsync(drivingLesson);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            var result = await _service.GetDrivingLesson(idOfDrivingLessonToFind);

            Assert.AreEqual(drivingLesson, result);
        }

        [TestMethod] 
        public async Task Get_DrivingLesson_ThrowsNotFoundLectNotFoundDrivingLessomException()
        {
            var idOfDrivingLessonToFind = 1;
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(idOfDrivingLessonToFind)).ReturnsAsync((DrivingLessonGetDTO)null);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.GetDrivingLesson(idOfDrivingLessonToFind));
        }

        [TestMethod]
        public async Task Get_DrivingLesson_PropagatesSqlConnectionException()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var idOfDrivingLessonToFind = 1;
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(idOfDrivingLessonToFind)).ThrowsAsync(exception);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<SqlException>(async () => await _service.GetDrivingLesson(idOfDrivingLessonToFind));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ReturnsAddedDrivingLesson()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new CourseGetDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseType = courseTypeDTO, BeginDate = new DateTime(2023, 1, 1) };
            var addedDrivingLesson = new DrivingLesson { Id = 1, AddressId = address.Id, CustomerId = customer.Id,  LecturerId = lecturer.Id, LessonDate = new DateTime(2023,1,1)};
            var addedDrivingLessonDTO = new DrivingLessonGetDTO { Id = 1, AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id, LessonDate = new DateTime(2023, 1, 1) , CustomerName = "TestCustomer", LecturerName= "TestLecturer"};
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId=address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.GetLecturer(lecturer.Id)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.GetAddress(address.Id)).ReturnsAsync(address);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _drivingLessonRepositoryMock.Setup(repo => repo.PostDrivingLesson(drivingLessonToAdd)).ReturnsAsync(addedDrivingLesson);
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(addedDrivingLesson.Id)).ReturnsAsync(addedDrivingLessonDTO);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            var result = await _service.PostDrivingLesson(drivingLessonToAdd);

            Assert.AreEqual(addedDrivingLessonDTO, result);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsMissingLessonDateException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new CourseGetDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseType = courseTypeDTO, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonPostDTO { AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundCustomerException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new CourseGetDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseType = courseTypeDTO, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ThrowsAsync(new NotFoundCustomerException(customer.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundLecturerException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new CourseGetDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseType = courseTypeDTO, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.GetLecturer(lecturer.Id)).ThrowsAsync(new NotFoundLecturerException(lecturer.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundAddressException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new CourseGetDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseType = courseTypeDTO, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.GetLecturer(lecturer.Id)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.GetAddress(address.Id)).ThrowsAsync(new NotFoundAddressException(address.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_PropagatesSqlConnectionException()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new CourseGetDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseType = courseTypeDTO, BeginDate = new DateTime(2023, 1, 1) };
            var addedDrivingLesson = new DrivingLesson { Id = 1, AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, LessonDate = new DateTime(2023, 1, 1) };
            var addedDrivingLessonDTO = new DrivingLessonGetDTO { Id = 1, AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id, LessonDate = new DateTime(2023, 1, 1), CustomerName = "TestCustomer", LecturerName = "TestLecturer" };
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.GetLecturer(lecturer.Id)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.GetAddress(address.Id)).ReturnsAsync(address);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _drivingLessonRepositoryMock.Setup(repo => repo.PostDrivingLesson(drivingLessonToAdd)).ThrowsAsync(exception);
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(addedDrivingLesson.Id)).ReturnsAsync(addedDrivingLessonDTO);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<SqlException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundCourseException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new CourseGetDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseType = courseTypeDTO, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.GetLecturer(lecturer.Id)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.GetAddress(address.Id)).ReturnsAsync(address);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ThrowsAsync(new NotFoundCourseException(course.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_ReturnsDrivingLesson()
        {
            var drivingLesson = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3), CourseId = 1 };
            var idOfDrivingLesson = 1;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _drivingLessonRepositoryMock.Setup(repo => repo.DeleteDrivingLesson(drivingLesson)).ReturnsAsync(drivingLesson);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            var result = await _service.DeleteDrivingLesson(idOfDrivingLesson);

            Assert.AreEqual(drivingLesson, result);
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(10)).ReturnsAsync((DrivingLesson)null);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.DeleteDrivingLesson(idOfDrivingLesson));
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_PropagatesSqlConnectionException()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(idOfDrivingLesson)).ThrowsAsync(exception);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<SqlException>(async () => await _service.DeleteDrivingLesson(idOfDrivingLesson));
        }

        [TestMethod]
        public async Task Check_DrivingLesson_ReturnsDrivingLesson()
        {
            var drivingLesson = new DrivingLesson();
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(10)).ReturnsAsync(drivingLesson);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            var result = await _service.CheckDrivingLesson(idOfDrivingLesson);

            Assert.AreEqual(drivingLesson, result);
        }

        [TestMethod]
        public async Task Check_DrivingLesson_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(10)).ReturnsAsync((DrivingLesson)null);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.CheckDrivingLesson(idOfDrivingLesson));
        }

        [TestMethod]
        public async Task Check_DrivingLesson_PropagatesSqlConnectionException()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(idOfDrivingLesson)).ThrowsAsync(exception);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object);

            await Assert.ThrowsExceptionAsync<SqlException>(async () => await _service.CheckDrivingLesson(idOfDrivingLesson));
        }

    }
}
