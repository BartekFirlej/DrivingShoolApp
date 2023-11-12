using AutoFixture;
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
        private Fixture _fixture;
        private DrivingLessonService _service;

        public DrivingLessonServiceTests()
        {
            _fixture = new Fixture();
            _drivingLessonRepositoryMock = new Mock<IDrivingLessonRepository>();
            _customerServiceMock = new Mock<ICustomerService>();
            _lecturerServiceMock = new Mock<ILecturerService>();
            _addressServiceMock = new Mock<IAddressService>();
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ReturnsDrivingLessons()
        {
            var drivingLesson = new DrivingLessonGetDTO();
            ICollection<DrivingLessonGetDTO> drivingLessonsList = new List<DrivingLessonGetDTO>() { drivingLesson };
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLessons()).Returns(Task.FromResult(drivingLessonsList));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object);

            var result = await _service.GetDrivingLessons();

            Assert.AreEqual(drivingLessonsList, result);
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ThrowsNotFoundDrivingLessonException()
        {
            ICollection<DrivingLessonGetDTO> drivingLessonsList = new List<DrivingLessonGetDTO>();
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLessons()).Returns(Task.FromResult(drivingLessonsList));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object);


            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.GetDrivingLessons());
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsDrivingLesson()
        {
            var drivingLesson = new DrivingLessonGetDTO();
            var idOfDrivingLessonToFind = 1;
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(idOfDrivingLessonToFind)).Returns(Task.FromResult(drivingLesson));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object);

            var result = await _service.GetDrivingLesson(idOfDrivingLessonToFind);

            Assert.AreEqual(drivingLesson, result);
        }

        [TestMethod] 
        public async Task Get_DrivingLesson_ThrowsNotFoundLectNotFoundDrivingLessomException()
        {
            var idOfDrivingLessonToFind = 1;
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(idOfDrivingLessonToFind)).Returns(Task.FromResult<DrivingLessonGetDTO>(null));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLessonException>(async () => await _service.GetDrivingLesson(idOfDrivingLessonToFind));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ReturnsAddedDrivingLesson()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var addedDrivingLesson = new DrivingLesson { Id = 1, AddressId = address.Id, CustomerId = customer.Id,  LecturerId = lecturer.Id, LessonDate = new DateTime(2023,1,1)};
            var addedDrivingLessonDTO = new DrivingLessonGetDTO { Id = 1, AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id, LessonDate = new DateTime(2023, 1, 1) , CustomerName = "TestCustomer", LecturerName= "TestLecturer"};
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId=address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Returns(Task.FromResult(customer));
            _lecturerServiceMock.Setup(service => service.GetLecturer(lecturer.Id)).Returns(Task.FromResult(lecturer));
            _addressServiceMock.Setup(service => service.GetAddress(address.Id)).Returns(Task.FromResult(address));
            _drivingLessonRepositoryMock.Setup(repo => repo.PostDrivingLesson(drivingLessonToAdd)).Returns(Task.FromResult(addedDrivingLesson));
            _drivingLessonRepositoryMock.Setup(repo => repo.GetDrivingLesson(addedDrivingLesson.Id)).Returns(Task.FromResult(addedDrivingLessonDTO));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object);

            var result = await _service.PostDrivingLesson(drivingLessonToAdd);

            Assert.AreEqual(addedDrivingLessonDTO, result);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsMissingLessonDateException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var drivingLessonToAdd = new DrivingLessonPostDTO { AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id };
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundCustomerException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Throws(new NotFoundCustomerException(customer.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundLecturerException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Returns(Task.FromResult(customer));
            _lecturerServiceMock.Setup(service => service.GetLecturer(lecturer.Id)).Throws(new NotFoundLecturerException(lecturer.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundAddressException()
        {
            var customer = new CustomerGetDTO { Id = 1, BirthDate = new DateTime(2000, 1, 1), Name = "CustomerName", SecondName = "CustomerSecondName" };
            var lecturer = new LecturerGetDTO { Id = 1, Name = "LecturerName", SecondName = "LecturerSecondName" };
            var address = new AddressGetDTO { Id = 1, City = "TestCity", PostalCode = "22-222", Street = "TestStreet", Number = 1 };
            var drivingLessonToAdd = new DrivingLessonPostDTO { LessonDate = new DateTime(2023, 1, 1), AddressId = address.Id, CustomerId = customer.Id, LecturerId = lecturer.Id };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Returns(Task.FromResult(customer));
            _lecturerServiceMock.Setup(service => service.GetLecturer(lecturer.Id)).Returns(Task.FromResult(lecturer));
            _addressServiceMock.Setup(service => service.GetAddress(address.Id)).Throws(new NotFoundAddressException(address.Id));
            _service = new DrivingLessonService(_drivingLessonRepositoryMock.Object, _customerServiceMock.Object, _lecturerServiceMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.PostDrivingLesson(drivingLessonToAdd));
        }
    }
}
