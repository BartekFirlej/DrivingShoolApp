using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class CustomerLectureTests
    {
        private Mock<ICustomerLectureRepository> _customerLectureRepositoryMock;
        private Mock<ICustomerService> _customerServiceMock;
        private Mock<ILectureService> _lectureServiceMock;
        private Mock<IRegistrationService> _registrationServiceMock;
        private Fixture _fixture;
        private CustomerLectureService _service;

        public CustomerLectureTests()
        {
            _fixture = new Fixture();
            _customerLectureRepositoryMock = new Mock<ICustomerLectureRepository>();
            _customerServiceMock = new Mock<ICustomerService>();
            _lectureServiceMock = new Mock<ILectureService>();
            _registrationServiceMock = new Mock<IRegistrationService>();
        }

        [TestMethod]
        public async Task Get_CustomersLectures_ReturnsCustomersLectures()
        {
            var customerLecture = new CustomerLectureGetDTO();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() { customerLecture };
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomersLectures()).Returns(Task.FromResult(customersLecturesList));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            var result = await _service.GetCustomersLectures();

            Assert.AreEqual(customersLecturesList, result);
        }

        [TestMethod]
        public async Task Get_CustomersLectures_ThrowsNotFoundCustomerLectureException()
        {
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>();
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomersLectures()).Returns(Task.FromResult(customersLecturesList));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerLectureException>(async () => await _service.GetCustomersLectures());
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ReturnsCustomerLectures()
        {
            var customer = new CustomerGetDTO();
            var customerLecture = new CustomerLectureGetDTO();
            ICollection<CustomerLectureGetDTO> customerLecturesList = new List<CustomerLectureGetDTO>() { customerLecture };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Returns(Task.FromResult(customer));
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLectures(customer.Id)).Returns(Task.FromResult(customerLecturesList));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            var result = await _service.GetCustomerLectures(customer.Id);

            Assert.AreEqual(customerLecturesList, result);
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ThrowsNotFoundCustomerLecturesException()
        {
            var customer = new CustomerGetDTO();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>();
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Returns(Task.FromResult(customer));
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLectures(customer.Id)).Returns(Task.FromResult(customersLecturesList));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerLectureException>(async () => await _service.GetCustomerLectures(customer.Id));
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ThrowsNotFoundCustomerException()
        {
            var customer = new CustomerGetDTO();
            var customerLecture = new CustomerLectureGetDTO();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() { customerLecture };
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Throws(new NotFoundCustomerException(customer.Id));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomerLectures(customer.Id));
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ReturnsCustomersLecture()
        {
            var lecture = new LectureGetDTO();
            var customerLecture = new CustomerLectureGetDTO();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() { customerLecture };
            _lectureServiceMock.Setup(service => service.GetLecture(lecture.Id)).Returns(Task.FromResult(lecture));
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomersLecture(lecture.Id)).Returns(Task.FromResult(customersLecturesList));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            var result = await _service.GetCustomersLecture(lecture.Id);

            Assert.AreEqual(customersLecturesList, result);
        }
        
        [TestMethod]
        public async Task Get_CustomersLecture_ThrowsNotFoundCustomersLectureException()
        {
            var lecture = new LectureGetDTO();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() {};
            _lectureServiceMock.Setup(service => service.GetLecture(lecture.Id)).Returns(Task.FromResult(lecture));
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomersLecture(lecture.Id)).Returns(Task.FromResult(customersLecturesList));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomersLectureException>(async () => await _service.GetCustomersLecture(lecture.Id));
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ThrowsNotFoundLectureException()
        {
            var lecture = new LectureGetDTO();
            ICollection<CustomerLectureGetDTO> customersLecturesList = new List<CustomerLectureGetDTO>() { };
            _lectureServiceMock.Setup(service => service.GetLecture(lecture.Id)).Throws(new NotFoundLectureException(lecture.Id));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetCustomersLecture(lecture.Id));

        }

        [TestMethod]
        public async Task Get_CustomerLecture_ReturnsCustomerLecture()
        {
            var lecture = new LectureGetDTO();
            var customer = new CustomerGetDTO();
            var customerLecture = new CustomerLectureGetDTO();
            _lectureServiceMock.Setup(service => service.GetLecture(lecture.Id)).Returns(Task.FromResult(lecture));
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Returns(Task.FromResult(customer));
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLecture(customer.Id, lecture.Id)).Returns(Task.FromResult(customerLecture));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            var result = await _service.GetCustomerLecture(customer.Id, lecture.Id);

            Assert.AreEqual(customerLecture, result);
        }

        [TestMethod]
        public async Task Get_CustomerLecture_NotFoundLectureException()
        {
            var lecture = new LectureGetDTO();
            var customer = new CustomerGetDTO();
            _lectureServiceMock.Setup(service => service.GetLecture(lecture.Id)).Throws(new NotFoundLectureException(lecture.Id));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetCustomerLecture(customer.Id, lecture.Id));
        }

        [TestMethod]
        public async Task Get_CustomerLecture_NotFoundCustomerException()
        {
            var lecture = new LectureGetDTO();
            var customer = new CustomerGetDTO();
            _lectureServiceMock.Setup(service => service.GetLecture(lecture.Id)).Returns(Task.FromResult(lecture));
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Throws(new NotFoundCustomerException(customer.Id));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomerLecture(customer.Id, lecture.Id));
        }

        [TestMethod]
        public async Task Get_CustomerLecture_NotFoundCustomerLectureException()
        {
            var lecture = new LectureGetDTO();
            var customer = new CustomerGetDTO();
            _lectureServiceMock.Setup(service => service.GetLecture(lecture.Id)).Returns(Task.FromResult(lecture));
            _customerServiceMock.Setup(service => service.GetCustomer(customer.Id)).Returns(Task.FromResult(customer));
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLecture(customer.Id, lecture.Id)).Returns(Task.FromResult<CustomerLectureGetDTO>(null));
            
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerLectureException>(async () => await _service.GetCustomerLecture(customer.Id, lecture.Id));
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ReturnsAddedCustomerLecture()
        {
            var course = new CourseGetDTO { Id = 1 };
            var registration = new RegistrationGetDTO { CourseId = course.Id, CustomerId = 1};
            var lecture = new LectureGetDTO { Id = 1};
            var customer = new CustomerGetDTO { Id = 1};
            var customerLectureToAdd = new CustomerLecturePostDTO { CustomerId =1 , LectureId = 1};
            var addedCustomerLecture = new CustomerLectureGetDTO { CustomerId = 1, LectureId = 1, CustomerName = "Name"};
            _customerServiceMock.Setup(service => service.GetCustomer(customerLectureToAdd.CustomerId)).Returns(Task.FromResult(customer));
            _lectureServiceMock.Setup(service => service.GetLecture(customerLectureToAdd.LectureId)).Returns(Task.FromResult(lecture));
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLecture(customer.Id, lecture.Id)).Returns(Task.FromResult<CustomerLectureGetDTO>(null));
            _customerLectureRepositoryMock.Setup(repo => repo.PostCustomerLecture(customerLectureToAdd)).Returns(Task.FromResult(addedCustomerLecture));
            _registrationServiceMock.Setup(service => service.GetRegistration(registration.CustomerId, registration.CourseId))
                    .Returns(Task.FromResult(registration));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            var result = await _service.PostCustomerLecture(customerLectureToAdd);

            Assert.AreEqual(addedCustomerLecture, result);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundCustomerException()
        {
            var course = new CourseGetDTO { Id = 1 };
            var registration = new RegistrationGetDTO { CourseId = course.Id, CustomerId = 1 };
            var lecture = new LectureGetDTO { Id = 1 };
            var customer = new CustomerGetDTO { Id = 1 };
            var customerLectureToAdd = new CustomerLecturePostDTO { CustomerId = 1, LectureId = 1 };
            var addedCustomerLecture = new CustomerLectureGetDTO { CustomerId = 1, LectureId = 1, CustomerName = "Name" };
            _customerServiceMock.Setup(service => service.GetCustomer(customerLectureToAdd.CustomerId)).Throws(new NotFoundCustomerException(customerLectureToAdd.CustomerId));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.PostCustomerLecture(customerLectureToAdd));
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundLectureException()
        {
            var course = new CourseGetDTO { Id = 1 };
            var registration = new RegistrationGetDTO { CourseId = course.Id, CustomerId = 1 };
            var lecture = new LectureGetDTO { Id = 1 };
            var customer = new CustomerGetDTO { Id = 1 };
            var customerLectureToAdd = new CustomerLecturePostDTO { CustomerId = 1, LectureId = 1 };
            var addedCustomerLecture = new CustomerLectureGetDTO { CustomerId = 1, LectureId = 1, CustomerName = "Name" };
            _customerServiceMock.Setup(service => service.GetCustomer(customerLectureToAdd.CustomerId)).Returns(Task.FromResult(customer));
            _lectureServiceMock.Setup(service => service.GetLecture(customerLectureToAdd.LectureId)).Throws(new NotFoundLectureException(customerLectureToAdd.LectureId));
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.PostCustomerLecture(customerLectureToAdd));
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsCustomerAlreadyAssignedToLectureException()
        {
            var course = new CourseGetDTO { Id = 1 };
            var registration = new RegistrationGetDTO { CourseId = course.Id, CustomerId = 1 };
            var lecture = new LectureGetDTO { Id = 1 };
            var customer = new CustomerGetDTO { Id = 1 };
            var customerLecture = new CustomerLectureGetDTO();
            var customerLectureToAdd = new CustomerLecturePostDTO { CustomerId = 1, LectureId = 1 };
            var addedCustomerLecture = new CustomerLectureGetDTO { CustomerId = 1, LectureId = 1, CustomerName = "Name" };
            _customerServiceMock.Setup(service => service.GetCustomer(customerLectureToAdd.CustomerId)).Returns(Task.FromResult(customer));
            _lectureServiceMock.Setup(service => service.GetLecture(customerLectureToAdd.LectureId)).Returns(Task.FromResult(lecture));
            _customerLectureRepositoryMock.Setup(repo => repo.GetCustomerLecture(customer.Id, lecture.Id)).Returns(Task.FromResult(customerLecture)); 
            _service = new CustomerLectureService(_customerLectureRepositoryMock.Object, _customerServiceMock.Object, _lectureServiceMock.Object, _registrationServiceMock.Object);

            await Assert.ThrowsExceptionAsync<CustomerAlreadyAssignedToLectureException>(async () => await _service.PostCustomerLecture(customerLectureToAdd));
        }
    }
}
