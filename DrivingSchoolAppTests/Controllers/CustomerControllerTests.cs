using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class CustomerControllerTests
    {
        private Mock<ICustomerService> _customerServiceMock;
        private Mock<IRegistrationService> _registrationServiceMock;
        private Fixture _fixture;
        private CustomerController _controller;

        public CustomerControllerTests()
        {
            _fixture = new Fixture();
            _customerServiceMock = new Mock<ICustomerService>();
            _registrationServiceMock = new Mock<IRegistrationService>();
        }

        [TestMethod]
        public async Task Get_Customers_ReturnsOk()
        {
            var usersList = new PagedList<CustomerGetDTO>();
            _customerServiceMock.Setup(service => service.GetCustomers(1, 10)).Returns(Task.FromResult(usersList));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomers(1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Customers_ThrowsNotFoundCustomersException()
        {
            _customerServiceMock.Setup(service => service.GetCustomers(1, 10)).Throws(new NotFoundCustomerException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomers(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Customers_ThrowsPageIndexMustBeGreaterThanZeroException()
        {
            _customerServiceMock.Setup(service => service.GetCustomers(-1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetCustomers(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Customer_ReturnsOk()
        {
            var foundCustomer = new CustomerGetDTO();
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).Returns(Task.FromResult(foundCustomer));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomer(idOfCustomerToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Customer_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).Throws(new NotFoundCustomerException(1));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomer(idOfCustomerToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ReturnsOk()
        {
            var customerRegistrations = new PagedList<RegistrationGetDTO>();
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10)).Returns(Task.FromResult(customerRegistrations));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ThrowsNotFoundRegistrationException()
        {
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10)).Throws(new NotFoundRegistrationException(idOfCustomerToFindHisRegistrations));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10)).Throws(new NotFoundCustomerException(idOfCustomerToFindHisRegistrations));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, 1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ThrowsPageIndexMustBeGreaterThanZeroException()
        {
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, -1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations, -1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_Customer_ReturnsCreatedAtAction()
        {
            var customerToAdd = new CustomerPostDTO();
            var addedCustomer = new CustomerGetDTO();
            _customerServiceMock.Setup(service => service.PostCustomer(customerToAdd)).Returns(Task.FromResult(addedCustomer));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCustomer(customerToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Customer_ThrowsDateTimeException()
        {
            var customerToAdd = new CustomerPostDTO();
            var wrongDate = "begin date";
            _customerServiceMock.Setup(service => service.PostCustomer(customerToAdd)).Throws(new DateTimeException(wrongDate));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCustomer(customerToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task RegisterCustomerForCourse_ReturnsCreatedAtAction()
        {
            var registrationToAdd = new RegistrationPostDTO();
            var addedRegistration = new RegistrationGetDTO();
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Returns(Task.FromResult(addedRegistration));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsNotFoundCustomerException()
        {
            var registrationToAdd = new RegistrationPostDTO();
            var idOfCustomer = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new NotFoundCustomerException(idOfCustomer));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsNotFoundCourseException()
        {
            var registrationToAdd = new RegistrationPostDTO();
            var idOfCourse = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new NotFoundCourseException(idOfCourse));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsCustomerAlreadyAssignedToCourseException()
        {
            var registrationToAdd = new RegistrationPostDTO();
            var idOfCourse = 1;
            var idOfCustomer = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new CustomerAlreadyAssignedToCourseException(idOfCustomer, idOfCourse));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsCustomerDoesntMeetRequirementsException()
        {
            var registrationToAdd = new RegistrationPostDTO();
            var idOfLicenceCategory = 1;
            var idOfCustomer = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new CustomerDoesntMeetRequirementsException(idOfCustomer, idOfLicenceCategory));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsAssignLimitReachedException()
        {
            var registrationToAdd = new RegistrationPostDTO();
            var idOfCourse = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new AssignLimitReachedException(idOfCourse));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(409);
        }
    }
} 