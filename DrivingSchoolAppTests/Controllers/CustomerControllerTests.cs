using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public async Task Get_Customers_ReturnOk()
        {
            ICollection<CustomerGetDTO> usersList = new List<CustomerGetDTO>();
            _customerServiceMock.Setup(service => service.GetCustomers()).Returns(Task.FromResult(usersList));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomers();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Customers_ThrowsNotFoundUsers()
        {
            _customerServiceMock.Setup(service => service.GetCustomers()).Throws(new NotFoundCustomersException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomers();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Customer_ReturnOk()
        {
            var foundCustomer = new CustomerGetDTO();
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).Returns(Task.FromResult(foundCustomer));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomer(idOfCustomerToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Customer_ThrowsNotFoundUser()
        {
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).Throws(new NotFoundCustomerException(1));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomer(idOfCustomerToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ReturnOk()
        {
            ICollection<RegistrationGetDTO> customerRegistrations = new List<RegistrationGetDTO>();
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetUserRegistrations(idOfCustomerToFindHisRegistrations)).Returns(Task.FromResult(customerRegistrations));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CustomerRegistrations_ThrowsNotFoundUser()
        {
            ICollection<RegistrationGetDTO> customerRegistrations = new List<RegistrationGetDTO>();
            var idOfCustomerToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetUserRegistrations(idOfCustomerToFindHisRegistrations)).Throws(new NotFoundCustomerRegistrationsException(1));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomerRegistrations(idOfCustomerToFindHisRegistrations);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Customer_ReturnCreatedAtAction()
        {
            var customerToAdd = new CustomerPostDTO();
            var addedCustomer = new CustomerGetDTO();
            _customerServiceMock.Setup(service => service.AddCustomer(customerToAdd)).Returns(Task.FromResult(addedCustomer));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.AddCustomer(customerToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_CustomerForCourse_ReturnCreatedAtAction()
        {
            var registrationToAdd = new RegistrationPostDTO();
            var addedRegistration = new RegistrationGetDTO();
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Returns(Task.FromResult(addedRegistration));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_CustomerForCourse_ThrowsNotFoundUserException()
        {
            var registrationToAdd = new RegistrationPostDTO();
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new NotFoundCustomerException(1));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CustomerForCourse_ThrowsNotFoundCourseException()
        {
            var registrationToAdd = new RegistrationPostDTO();
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new NotFoundCourseException(1));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(404);
        }
    }
}