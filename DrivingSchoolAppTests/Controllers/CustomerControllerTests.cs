using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Models;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class CustomerControllerTests
    {
        private Mock<ICustomerService> _customerServiceMock;
        private Mock<IRegistrationService> _registrationServiceMock;
        private Mock<ICustomerLectureService> _customerLectureServiceMock;
        private Mock<IDrivingLicenceService> _drivingLicenceServiceMock;
        private Mock<IDateTimeHelper> _dateTimeHelperMock;
        private Fixture _fixture;
        private CustomerController _controller;

        public CustomerControllerTests()
        {
            _fixture = new Fixture();
            _customerServiceMock = new Mock<ICustomerService>();
            _registrationServiceMock = new Mock<IRegistrationService>();
            _customerLectureServiceMock = new Mock<ICustomerLectureService>();
            _drivingLicenceServiceMock = new Mock<IDrivingLicenceService>();
            _dateTimeHelperMock = new Mock<IDateTimeHelper>();
        }

        [TestMethod]
        public async Task Get_Customers_ReturnsOk()
        {
            var usersList = new PagedList<CustomerGetDTO>();
            _customerServiceMock.Setup(service => service.GetCustomers(1, 10)).ReturnsAsync(usersList);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomers(1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Customers_ThrowsNotFoundCustomersException()
        {
            _customerServiceMock.Setup(service => service.GetCustomers(1, 10)).ThrowsAsync(new NotFoundCustomerException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomers(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Customers_ThrowsPageIndexMustBeGreaterThanZeroException()
        {
            _customerServiceMock.Setup(service => service.GetCustomers(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetCustomers(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Customer_ReturnsOk()
        {
            var foundCustomer = new CustomerGetDTO();
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).ReturnsAsync(foundCustomer);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomer(idOfCustomerToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Customer_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(1));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomer(idOfCustomerToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ReturnsOk()
        {
            var customerRegistrations = new PagedList<RegistrationGetDTO>();
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10)).ReturnsAsync(customerRegistrations);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ThrowsNotFoundRegistrationException()
        {
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10)).ThrowsAsync(new NotFoundRegistrationException(idOfCustomerToFindHisRegistrations));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFindHisRegistrations));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ThrowsPageIndexMustBeGreaterThanZeroException()
        {
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, -1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, -1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ReturnsOk()
        {
            var customerLectures = new List<CustomerLectureGetDTO>();
            var idOfCustomerToFindHisLectures = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomerLectures(idOfCustomerToFindHisLectures)).ReturnsAsync(customerLectures);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomerLectures(idOfCustomerToFindHisLectures);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ThrowsNotFoundCustomerLecturesException()
        {
            var idOfCustomerToFindHisLectures = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomerLectures(idOfCustomerToFindHisLectures)).ThrowsAsync(new NotFoundCustomerLectureException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomerLectures(idOfCustomerToFindHisLectures);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerLectures_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFindHisLectures = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomerLectures(idOfCustomerToFindHisLectures)).ThrowsAsync(new NotFoundCustomerException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomerLectures(idOfCustomerToFindHisLectures);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerDrivingLicences_ReturnsOk()
        {
            var customerDrivingLicences = new List<DrivingLicenceGetDTO>();
            var idOfCustomerToFindHisDrivingLicences = 1;
            _dateTimeHelperMock.Setup(service => service.GetDateTimeNow()).Returns(new DateTime(2023,11,9));
            _drivingLicenceServiceMock.Setup(service => service.GetCustomerDrivingLicences(idOfCustomerToFindHisDrivingLicences)).ReturnsAsync(customerDrivingLicences);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomerLectures(idOfCustomerToFindHisDrivingLicences);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CustomerDrivingLicences_ThrowsNotFoundCustomerDrivingLicencesException()
        {
            var idOfCustomerToFindHisLectures = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomerLectures(idOfCustomerToFindHisLectures)).ThrowsAsync(new NotFoundCustomerLectureException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomerLectures(idOfCustomerToFindHisLectures);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerDrivingLicences_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFindHisLectures = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomerLectures(idOfCustomerToFindHisLectures)).ThrowsAsync(new NotFoundCustomerException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomerLectures(idOfCustomerToFindHisLectures);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Customer_ReturnsCreatedAtAction()
        {
            var customerToAdd = new CustomerRequestDTO();
            var addedCustomer = new CustomerResponseDTO();
            _customerServiceMock.Setup(service => service.PostCustomer(customerToAdd)).ReturnsAsync(addedCustomer);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCustomer(customerToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Customer_ThrowsDateTimeException()
        {
            var customerToAdd = new CustomerRequestDTO();
            var wrongDate = "begin date";
            _customerServiceMock.Setup(service => service.PostCustomer(customerToAdd)).ThrowsAsync(new DateTimeException(wrongDate));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCustomer(customerToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Delete_Customer_ReturnNoContent()
        {
            var deletedCustomer = new Customer();
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ReturnsAsync(deletedCustomer);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Customer_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToDelete));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Customer_ThrowsReferenceConstraintException()
        {
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Customer_ThrowsDbUpdateException()
        {
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Customer_ThrowsException()
        {
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ThrowsAsync(new Exception());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Update_Customer_ReturnsCustomer()
        {
            var customerUpdate = new CustomerRequestDTO();
            var updatedCustomer = new CustomerResponseDTO();
            var idOfCustomerToUpdate = 1;
            _customerServiceMock.Setup(service => service.UpdateCustomer(idOfCustomerToUpdate, customerUpdate)).ReturnsAsync(updatedCustomer);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (OkObjectResult)await _controller.UpdateCustomer(idOfCustomerToUpdate, customerUpdate);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Update_Customer_ThrowsNotFoundCustomer()
        {
            var customerUpdate = new CustomerRequestDTO();
            var idOfCustomerToUpdate = 1;
            _customerServiceMock.Setup(service => service.UpdateCustomer(idOfCustomerToUpdate, customerUpdate)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToUpdate));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateCustomer(idOfCustomerToUpdate, customerUpdate);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Update_Customer_ThrowsDateTimeException()
        {
            var customerUpdate = new CustomerRequestDTO();
            var idOfCustomerToUpdate = 1;
            _customerServiceMock.Setup(service => service.UpdateCustomer(idOfCustomerToUpdate, customerUpdate)).ThrowsAsync(new DateTimeException("birth date"));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object, _customerLectureServiceMock.Object, _drivingLicenceServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.UpdateCustomer(idOfCustomerToUpdate, customerUpdate);

            result.StatusCode.Should().Be(400);
        }
    }
} 