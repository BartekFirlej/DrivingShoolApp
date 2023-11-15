using AutoFixture;
using Castle.Core.Resource;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Moq;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class CustomerServiceTests
    {
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Fixture _fixture;
        private CustomerService _service;

        public CustomerServiceTests()
        {
            _fixture = new Fixture();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
        }

        [TestMethod]
        public async Task Get_Customers_ReturnsCustomers()
        {
            var customer = new CustomerGetDTO();
            var customersList = new PagedList<CustomerGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<CustomerGetDTO> { customer }, HasNextPage = false };
            _customerRepositoryMock.Setup(repo => repo.GetCustomers(1, 10)).Returns(Task.FromResult(customersList));
            _service = new CustomerService(_customerRepositoryMock.Object);

            var result = await _service.GetCustomers(1, 10);

            Assert.AreEqual(customersList, result);
        }

        [TestMethod]
        public async Task Get_Customers_ThrowsNotFoundCustomersException()
        {
            var customersList = new PagedList<CustomerGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<CustomerGetDTO>(), HasNextPage = false };
            _customerRepositoryMock.Setup(repo => repo.GetCustomers(1, 10)).Returns(Task.FromResult(customersList));
            _service = new CustomerService(_customerRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomers(1, 10));
        }

        [TestMethod]
        public async Task Get_Addresses_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _customerRepositoryMock.Setup(repo => repo.GetCustomers(-1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new CustomerService(_customerRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetCustomers(-1, 10));
        }

        [TestMethod]
        public async Task Get_Customer_ReturnsCustomer()
        {
            var customer = new CustomerGetDTO();
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(customer.Id)).Returns(Task.FromResult(customer));
            _service = new CustomerService(_customerRepositoryMock.Object);

            var result = await _service.GetCustomer(customer.Id);

            Assert.AreEqual(customer, result);
        }

        [TestMethod]
        public async Task Get_Customer_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(idOfCustomerToFind)).Returns(Task.FromResult<CustomerGetDTO>(null));
            _service = new CustomerService(_customerRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomer(idOfCustomerToFind));
        }

        [TestMethod]
        public async Task Post_Customer_ReturnsAddedCustomer()
        {
            var addedCustomer = new Customer { Id = 1, Name = "Mathew", BirthDate = new DateTime(1999,10,1)};
            var addedCustomerDTO = new CustomerGetDTO { Id = 1, Name = "Mathew", BirthDate = new DateTime(1999, 10, 1) };
            var customerToAdd = new CustomerPostDTO { Name = "Mathew", BirthDate = new DateTime(1999, 10, 1) };
            _customerRepositoryMock.Setup(repo => repo.PostCustomer(customerToAdd)).Returns(Task.FromResult(addedCustomer));
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(addedCustomer.Id)).Returns(Task.FromResult(addedCustomerDTO));
            _service = new CustomerService(_customerRepositoryMock.Object);

            var result = await _service.PostCustomer(customerToAdd);

            Assert.AreEqual(addedCustomerDTO, result);
        }

        [TestMethod]
        public async Task Post_Address_ThrowsBirthDateTimeException()
        {
            var addedCustomer = new Customer { Id = 1, Name = "Mathew", };
            var addedCustomerDTO = new CustomerGetDTO { Id = 1, Name = "Mathew"};
            var customerToAdd = new CustomerPostDTO { Name = "Mathew"};
            _customerRepositoryMock.Setup(repo => repo.PostCustomer(customerToAdd)).Returns(Task.FromResult(addedCustomer));
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(addedCustomer.Id)).Returns(Task.FromResult(addedCustomerDTO));
            _service = new CustomerService(_customerRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostCustomer(customerToAdd));

        }

        [TestMethod]
        public async Task Check_Customer_AgeRequirementTrue()
        {
            var customerBirthDay = new DateTime(2000, 10, 10);
            int requiredAge = 18;
            var assignDate = new DateTime(2019, 10, 10);
            _service = new CustomerService(_customerRepositoryMock.Object);

            var result = _service.CheckCustomerAgeRequirement(customerBirthDay, requiredAge, assignDate);   

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Check_Customer_AgeRequirementFalse()
        {
            var customerBirthDay = new DateTime(2000, 10, 10);
            int requiredAge = 18;
            var assignDate = new DateTime(2017, 10, 10);
            _service = new CustomerService(_customerRepositoryMock.Object);

            var result = _service.CheckCustomerAgeRequirement(customerBirthDay, requiredAge, assignDate);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task Check_Customer_AgeRequirementEquals()
        {
            var customerBirthDay = new DateTime(2000, 10, 10);
            int requiredAge = 18;
            var assignDate = new DateTime(2018, 10, 10);
            _service = new CustomerService(_customerRepositoryMock.Object);

            var result = _service.CheckCustomerAgeRequirement(customerBirthDay, requiredAge, assignDate);

            Assert.AreEqual(true, result);
        }
    }
}
