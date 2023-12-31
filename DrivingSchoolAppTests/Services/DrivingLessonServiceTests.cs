﻿using AutoFixture;
using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Moq;

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
        private Mock<IMapper> _mapperMock;
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
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ReturnsDrivingLessons()
        {
            var drivingLesson = new DrivingLessonGetDTO();
            var drivingLessonsList = new PagedList<DrivingLessonGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<DrivingLessonGetDTO> { drivingLesson }, HasNextPage = false };
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLessons(1, 10)).ReturnsAsync(drivingLessonsList);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetDrivingLessons(1, 10);

            Assert.AreEqual(drivingLessonsList, result);
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ThrowsNotFoundDrivingLessonException()
        {
            var drivingLessonsList = new PagedList<DrivingLessonGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<DrivingLessonGetDTO>(), HasNextPage = false };
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLessons(1, 10)).ReturnsAsync(drivingLessonsList);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.GetDrivingLessons(1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLessons_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLessons(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetDrivingLessons(-1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsDrivingLesson()
        {
            var drivingLesson = new DrivingLessonGetDTO();
            var idOfDrivingLessonToFind = 1;
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(idOfDrivingLessonToFind)).ReturnsAsync(drivingLesson);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetDrivingLesson(idOfDrivingLessonToFind);

            Assert.AreEqual(drivingLesson, result);
        }

        [TestMethod] 
        public async Task Get_DrivingLesson_ThrowsNotFoundLectNotFoundDrivingLessomException()
        {
            var idOfDrivingLessonToFind = 1;
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(idOfDrivingLessonToFind)).ReturnsAsync((DrivingLessonGetDTO)null);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.GetDrivingLesson(idOfDrivingLessonToFind));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ReturnsAddedDrivingLesson()
        {
            var customer = new Customer { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new Lecturer { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new Address { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseTypeId = courseTypeDTO.Id, BeginDate = new DateTime(2023, 1, 1) };
            var addedDrivingLesson = new DrivingLesson { Id = 1, AddressId = address.Id, CustomerId = customer.Id,  LecturerId = lecturer.Id, LessonDate = new DateTime(2023,1,1)};
            var addedDrivingLessonDTO = new DrivingLessonResponseDTO { Id = 1, AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id, LessonDate = new DateTime(2023, 1, 1)};
            var drivingLessonToAdd = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 1, 1), AddressId=address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.CheckCustomer(customer.Id)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lecturer.Id)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.CheckAddress(address.Id)).ReturnsAsync(address);
            _courseServiceMock.Setup(service => service.CheckCourse(course.Id)).ReturnsAsync(course);
            _drivingLessonRepositoryMock.Setup(repo => repo.PostDrivingLesson(drivingLessonToAdd)).ReturnsAsync(addedDrivingLesson);
            _mapperMock.Setup(m => m.Map<DrivingLessonResponseDTO>(It.IsAny<DrivingLesson>())).Returns(addedDrivingLessonDTO);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            var result = await _service.PostDrivingLesson(drivingLessonToAdd);

            Assert.AreEqual(addedDrivingLessonDTO, result);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsMissingLessonDateException()
        {
            var customer = new Customer { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new Lecturer { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new Address { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseTypeId = courseTypeDTO.Id, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonRequestDTO { AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundCustomerException()
        {
            var customer = new Customer { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new Lecturer { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new Address { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseTypeId = courseTypeDTO.Id, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.CheckCustomer(customer.Id)).ThrowsAsync(new NotFoundCustomerException(customer.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundLecturerException()
        {
            var customer = new Customer { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new Lecturer { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new Address { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseTypeId = courseTypeDTO.Id, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.CheckCustomer(customer.Id)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lecturer.Id)).ThrowsAsync(new NotFoundLecturerException(lecturer.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundAddressException()
        {
            var customer = new Customer { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new Lecturer { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new Address { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseTypeId = courseTypeDTO.Id, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.CheckCustomer(customer.Id)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lecturer.Id)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.CheckAddress(address.Id)).ThrowsAsync(new NotFoundAddressException(address.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundCourseException()
        {
            var customer = new Customer { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new Lecturer { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new Address { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseTypeId = courseTypeDTO.Id, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLessonToAdd = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, CourseId = course.Id };
            _customerServiceMock.Setup(service => service.CheckCustomer(customer.Id)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lecturer.Id)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.CheckAddress(address.Id)).ReturnsAsync(address);
            _courseServiceMock.Setup(service => service.CheckCourse(course.Id)).ThrowsAsync(new NotFoundCourseException(course.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_ReturnsDrivingLesson()
        {
            var drivingLesson = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 4, 3), CourseId = 1 };
            var idOfDrivingLesson = 1;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _drivingLessonRepositoryMock.Setup(repo => repo.DeleteDrivingLesson(drivingLesson)).ReturnsAsync(drivingLesson);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            var result = await _service.DeleteDrivingLesson(idOfDrivingLesson);

            Assert.AreEqual(drivingLesson, result);
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(10)).ReturnsAsync((DrivingLesson)null);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.DeleteDrivingLesson(idOfDrivingLesson));
        }

        [TestMethod]
        public async Task Check_DrivingLesson_ReturnsDrivingLesson()
        {
            var drivingLesson = new DrivingLesson();
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckDrivingLesson(idOfDrivingLesson);

            Assert.AreEqual(drivingLesson, result);
        }

        [TestMethod]
        public async Task Check_DrivingLesson_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLesson(idOfDrivingLesson)).ReturnsAsync((DrivingLesson)null);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.CheckDrivingLesson(idOfDrivingLesson));
        }

        [TestMethod]
        public async Task Check_DrivingLessonTracking_ReturnsDrivingLesson()
        {
            var drivingLesson = new DrivingLesson();
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLessonTracking(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckDrivingLessonTracking(idOfDrivingLesson);

            Assert.AreEqual(drivingLesson, result);
        }

        [TestMethod]
        public async Task Check_DrivingLessonTracking_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLesson = 10;
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLessonTracking(idOfDrivingLesson)).ReturnsAsync((DrivingLesson)null);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.CheckDrivingLessonTracking(idOfDrivingLesson));
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ReturnsUpdatedDrivingLesson()
        {
            var idOfDrivingLesson = 1;
            var customer = new Customer { Id = 2, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new Lecturer { Id = 2, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new Address { Id = 2, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var course = new Course { Id = 2, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseTypeId = courseTypeDTO.Id, BeginDate = new DateTime(2023, 1, 1) };
            var drivingLesson = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 1, 1) };
            var updatedDrivingLesson = new DrivingLessonResponseDTO { Id = 1, AddressId = 2, CustomerId = 2, LecturerId = 2, CourseId = 2, LessonDate = new DateTime(2023, 10, 10) };
            var drivingLessonUpdate = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 10, 10), AddressId = 2, CustomerId = 2, LecturerId = 2, CourseId = 2 };
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLessonTracking(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLessonUpdate.CustomerId)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(drivingLessonUpdate.LecturerId)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.CheckAddress(drivingLessonUpdate.AddressId)).ReturnsAsync(address);
            _courseServiceMock.Setup(service => service.CheckCourse(drivingLessonUpdate.CourseId)).ReturnsAsync(course);
            _drivingLessonRepositoryMock.Setup(repo => repo.UpdateDrivingLesson(drivingLesson, drivingLessonUpdate)).ReturnsAsync(drivingLesson);
            _mapperMock.Setup(m => m.Map<DrivingLessonResponseDTO>(It.IsAny<DrivingLesson>())).Returns(updatedDrivingLesson);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            var result = await _service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate);

            Assert.AreEqual(updatedDrivingLesson, result);
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLesson = 1;
            var drivingLessonUpdate = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 10, 10), AddressId = 2, CustomerId = 2, LecturerId = 2, CourseId = 2 };
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLessonTracking(idOfDrivingLesson)).ThrowsAsync(new NotFoundDrivingLessonException(idOfDrivingLesson));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsDateTimeException()
        {
            var idOfDrivingLesson = 1;
            var drivingLesson = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 1, 1) };
            var drivingLessonUpdate = new DrivingLessonRequestDTO { AddressId = 2, CustomerId = 2, LecturerId = 2, CourseId = 2 };
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLessonTracking(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundCustomerException()
        {
            var idOfDrivingLesson = 1;
            var drivingLesson = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 1, 1) };
            var drivingLessonUpdate = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 10, 10), AddressId = 2, CustomerId = 2, LecturerId = 2, CourseId = 2 };
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLessonTracking(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLessonUpdate.CustomerId)).ThrowsAsync(new NotFoundCustomerException(drivingLessonUpdate.CustomerId));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundLecturerException()
        {
            var idOfDrivingLesson = 1;
            var customer = new Customer { Id = 2, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var drivingLesson = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 1, 1) };
            var drivingLessonUpdate = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 10, 10), AddressId = 2, CustomerId = 2, LecturerId = 2, CourseId = 2 };
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLessonTracking(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLessonUpdate.CustomerId)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(drivingLessonUpdate.LecturerId)).ThrowsAsync(new NotFoundLecturerException(drivingLessonUpdate.LecturerId));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundAddressException()
        {
            var idOfDrivingLesson = 1;
            var customer = new Customer { Id = 2, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" }; 
            var lecturer = new Lecturer { Id = 2, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var drivingLesson = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 1, 1) };
            var drivingLessonUpdate = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 10, 10), AddressId = 2, CustomerId = 2, LecturerId = 2, CourseId = 2 };
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLessonTracking(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLessonUpdate.CustomerId)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(drivingLessonUpdate.LecturerId)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.CheckAddress(drivingLessonUpdate.AddressId)).ThrowsAsync(new NotFoundAddressException(drivingLessonUpdate.AddressId));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundCourseException()
        {
            var idOfDrivingLesson = 1;
            var customer = new Customer { Id = 2, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new Lecturer { Id = 2, Name = "LecturerName", SecondName = "LecturerSecondName" }; 
            var address = new Address { Id = 2, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var drivingLesson = new DrivingLesson { Id = 1, AddressId = 1, CustomerId = 1, LecturerId = 1, LessonDate = new DateTime(2023, 1, 1) };
            var drivingLessonUpdate = new DrivingLessonRequestDTO { LessonDate = new DateTime(2023, 10, 10), AddressId = 2, CustomerId = 2, LecturerId = 2, CourseId = 2 };
            _drivingLessonRepositoryMock.Setup(repo => repo.CheckDrivingLessonTracking(idOfDrivingLesson)).ReturnsAsync(drivingLesson);
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLessonUpdate.CustomerId)).ReturnsAsync(customer);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(drivingLessonUpdate.LecturerId)).ReturnsAsync(lecturer);
            _addressServiceMock.Setup(service => service.CheckAddress(drivingLessonUpdate.AddressId)).ReturnsAsync(address);
            _courseServiceMock.Setup(service => service.CheckCourse(drivingLessonUpdate.CourseId)).ThrowsAsync(new NotFoundCourseException(drivingLessonUpdate.CourseId));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object, _courseServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate));
        }
    }
}
