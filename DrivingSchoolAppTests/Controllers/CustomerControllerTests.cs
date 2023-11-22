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
            _customerServiceMock.Setup(service => service.GetCustomers(1, 10)).ReturnsAsync(usersList);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomers(1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Customers_ThrowsNotFoundCustomersException()
        {
            _customerServiceMock.Setup(service => service.GetCustomers(1, 10)).ThrowsAsync(new NotFoundCustomerException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomers(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Customers_ThrowsPageIndexMustBeGreaterThanZeroException()
        {
            _customerServiceMock.Setup(service => service.GetCustomers(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetCustomers(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Customer_ReturnsOk()
        {
            var foundCustomer = new CustomerGetDTO();
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).ReturnsAsync(foundCustomer);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomer(idOfCustomerToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Customer_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.GetCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(1));
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
            _customerServiceMock.Setup(service => service.PostCustomer(customerToAdd)).ReturnsAsync(addedCustomer);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCustomer(customerToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Customer_ThrowsDateTimeException()
        {
            var customerToAdd = new CustomerPostDTO();
            var wrongDate = "begin date";
            _customerServiceMock.Setup(service => service.PostCustomer(customerToAdd)).ThrowsAsync(new DateTimeException(wrongDate));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCustomer(customerToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Delete_Customer_ReturnNoContent()
        {
            var deletedCustomer = new Customer();
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ReturnsAsync(deletedCustomer);
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Customer_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToDelete));
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Customer_ThrowsReferenceConstraintException()
        {
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Customer_ThrowsDbUpdateException()
        {
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Customer_ThrowsException()
        {
            var idOfCustomerToDelete = 1;
            _customerServiceMock.Setup(service => service.DeleteCustomer(idOfCustomerToDelete)).ThrowsAsync(new Exception());
            _controller = new CustomerController(_customerServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCustomer(idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }
    }
} 