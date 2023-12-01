using AutoFixture;
using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using EntityFramework.Exceptions.Common;
using Moq;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class CustomerServiceTests
    {
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private CustomerService _service;

        public CustomerServiceTests()
        {
            _fixture = new Fixture();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_Customers_ReturnsCustomers()
        {
            var customer = new CustomerGetDTO();
            var customersList = new PagedList<CustomerGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<CustomerGetDTO> { customer }, HasNextPage = false };
            _customerRepositoryMock.Setup(repo => repo.GetCustomers(1, 10)).ReturnsAsync(customersList);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetCustomers(1, 10);

            Assert.AreEqual(customersList, result);
        }

        [TestMethod]
        public async Task Get_Customers_ThrowsNotFoundCustomersException()
        {
            var customersList = new PagedList<CustomerGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<CustomerGetDTO>(), HasNextPage = false };
            _customerRepositoryMock.Setup(repo => repo.GetCustomers(1, 10)).ReturnsAsync(customersList);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomers(1, 10));
        }

        [TestMethod]
        public async Task Get_Addresses_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _customerRepositoryMock.Setup(repo => repo.GetCustomers(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetCustomers(-1, 10));
        }

        [TestMethod]
        public async Task Get_Customer_ReturnsCustomer()
        {
            var customer = new CustomerGetDTO();
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(customer.Id)).ReturnsAsync(customer);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetCustomer(customer.Id);

            Assert.AreEqual(customer, result);
        }

        [TestMethod]
        public async Task Get_Customer_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(idOfCustomerToFind)).ReturnsAsync((CustomerGetDTO)null);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomer(idOfCustomerToFind));
        }

        [TestMethod]
        public async Task Post_Customer_ReturnsAddedCustomer()
        {
            var addedCustomer = new Customer { Id = 1, Name = "Mathew", BirthDate = new DateTime(1999,10,1)};
            var addedCustomerDTO = new CustomerResponseDTO { Id = 1, Name = "Mathew", BirthDate = new DateTime(1999, 10, 1) };
            var customerToAdd = new CustomerRequestDTO { Name = "Mathew", BirthDate = new DateTime(1999, 10, 1) };
            _customerRepositoryMock.Setup(repo => repo.PostCustomer(customerToAdd)).ReturnsAsync(addedCustomer);
            _mapperMock.Setup(m => m.Map<CustomerResponseDTO>(It.IsAny<Customer>())).Returns(addedCustomerDTO);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.PostCustomer(customerToAdd);

            Assert.AreEqual(addedCustomerDTO, result);
        }

        [TestMethod]
        public async Task Post_Address_ThrowsBirthDateTimeException()
        {
            var addedCustomer = new Customer { Id = 1, Name = "Mathew", };
            var addedCustomerDTO = new CustomerGetDTO { Id = 1, Name = "Mathew"};
            var customerToAdd = new CustomerRequestDTO { Name = "Mathew"};
            _customerRepositoryMock.Setup(repo => repo.PostCustomer(customerToAdd)).ReturnsAsync(addedCustomer);
            _customerRepositoryMock.Setup(repo => repo.GetCustomer(addedCustomer.Id)).ReturnsAsync(addedCustomerDTO);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostCustomer(customerToAdd));

        }

        [TestMethod]
        public async Task Check_Customer_AgeRequirementTrue()
        {
            var customerBirthDay = new DateTime(2000, 10, 10);
            int requiredAge = 18;
            var assignDate = new DateTime(2019, 10, 10);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            var result = _service.CheckCustomerAgeRequirement(customerBirthDay, requiredAge, assignDate);   

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Check_Customer_AgeRequirementFalse()
        {
            var customerBirthDay = new DateTime(2000, 10, 10);
            int requiredAge = 18;
            var assignDate = new DateTime(2017, 10, 10);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            var result = _service.CheckCustomerAgeRequirement(customerBirthDay, requiredAge, assignDate);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task Check_Customer_AgeRequirementEquals()
        {
            var customerBirthDay = new DateTime(2000, 10, 10);
            int requiredAge = 18;
            var assignDate = new DateTime(2018, 10, 10);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            var result = _service.CheckCustomerAgeRequirement(customerBirthDay, requiredAge, assignDate);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Delete_Customer_ReturnsCustomer()
        {
            var deletedCustomer = new Customer { Id = 1, Name = "Mathew", };
            var idOfCustomerToDelete = 1;
            _customerRepositoryMock.Setup(repo => repo.CheckCustomer(idOfCustomerToDelete)).ReturnsAsync(deletedCustomer);
            _customerRepositoryMock.Setup(repo => repo.DeleteCustomer(deletedCustomer)).ReturnsAsync(deletedCustomer);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.DeleteCustomer(idOfCustomerToDelete);

            Assert.AreEqual(deletedCustomer, result);
        }

        [TestMethod]
        public async Task Delete_Customer_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToDelete = 1;
            _customerRepositoryMock.Setup(repo => repo.CheckCustomer(idOfCustomerToDelete)).ReturnsAsync((Customer)null);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.DeleteCustomer(idOfCustomerToDelete));
        }

        [TestMethod]
        public async Task Delete_Customer_PropagatesReferenceConstraintExceptionException()
        {
            var deletedCustomer = new Customer { Id = 1, Name = "Mathew", };
            var idOfCustomerToDelete = 1;
            _customerRepositoryMock.Setup(repo => repo.CheckCustomer(idOfCustomerToDelete)).ReturnsAsync(deletedCustomer);
            _customerRepositoryMock.Setup(repo => repo.DeleteCustomer(deletedCustomer)).ThrowsAsync(new ReferenceConstraintException());
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteCustomer(idOfCustomerToDelete));
        }

        [TestMethod]
        public async Task Check_Customer_ReturnsCustomer()
        {
            var customer = new Customer { Id = 1, Name = "Mathew", };
            var idOfCustomer = 1;
            _customerRepositoryMock.Setup(repo => repo.CheckCustomer(idOfCustomer)).ReturnsAsync(customer);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.CheckCustomer(idOfCustomer);

            Assert.AreEqual(customer, result);
        }

        [TestMethod]
        public async Task Check_Customer_ThrowsNotFoundCustomerException()
        {
            var idOfCustomer = 1;
            _customerRepositoryMock.Setup(repo => repo.CheckCustomer(idOfCustomer)).ReturnsAsync((Customer)null);
            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.CheckCustomer(idOfCustomer));
        }
    }
}
