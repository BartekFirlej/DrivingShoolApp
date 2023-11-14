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
    public class DrivingLicenceServiceTests
    {
        private Mock<IDrivingLicenceRepository> _drivingLicenceRepositoryMock;
        private Mock<ICustomerService> _customerServiceMock;
        private Mock<ILicenceCategoryService> _licenceCategoryServiceMock;
        private Mock<IRequiredLicenceCategoryService> _requiredLicenceCategoryServiceMock;
        private Fixture _fixture;
        private DrivingLicenceService _service;

        public DrivingLicenceServiceTests()
        {
            _fixture = new Fixture();
            _drivingLicenceRepositoryMock = new Mock<IDrivingLicenceRepository>();
            _customerServiceMock = new Mock<ICustomerService>();
            _licenceCategoryServiceMock = new Mock<ILicenceCategoryService>();
            _requiredLicenceCategoryServiceMock = new Mock<IRequiredLicenceCategoryService>();
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ReturnsDrivingLicences()
        {
            var drivingLicence = new DrivingLicenceGetDTO();
            var drivingLicencesList = new PagedList<DrivingLicenceGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<DrivingLicenceGetDTO> { drivingLicence }, HasNextPage = false };
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicences(1, 10)).Returns(Task.FromResult(drivingLicencesList));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = await _service.GetDrivingLicences(1, 10);

            Assert.AreEqual(drivingLicencesList, result);
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ThrowsNotFoundDrivingLicencesException()
        {
            var drivingLicencesList = new PagedList<DrivingLicenceGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<DrivingLicenceGetDTO>(), HasNextPage = false };
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicences(1, 10)).Returns(Task.FromResult(drivingLicencesList));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.GetDrivingLicences(1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ReturnsDrivingLicence()
        {
            var drivingLicence = new DrivingLicenceGetDTO();
            var idOfDrivingLicenceToFind = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicence(idOfDrivingLicenceToFind)).Returns(Task.FromResult(drivingLicence));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = await _service.GetDrivingLicence(idOfDrivingLicenceToFind);

            Assert.AreEqual(drivingLicence, result);
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ThrowsNotFoundDrivingLicenceException()
        {
            var idOfDrivingLicenceToFind = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicence(idOfDrivingLicenceToFind)).Returns(Task.FromResult<DrivingLicenceGetDTO>(null));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.GetDrivingLicence(idOfDrivingLicenceToFind));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ReturnsAddedDrivingLicence()
        {
            var customer = new CustomerGetDTO { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000,1,1) };
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            ICollection<DrivingLicenceGetDTO> customerDrivingLicences = new List<DrivingLicenceGetDTO>();
            var addedDrivingLicence = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015,1,1), ReceivedDate = new DateTime(2014,1,1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id };
            var addedDrivingLicenceDTO = new DrivingLicenceGetDTO { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, LicenceCategoryName = "Test", CustomerName = "Test", CutomserSecondName = "Test" };
            var drivingLicenceToAdd = new DrivingLicencePostDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _customerServiceMock.Setup(service => service.GetCustomer(drivingLicenceToAdd.CustomerId)).Returns(Task.FromResult(customer));
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(drivingLicenceToAdd.LicenceCategoryId)).Returns(Task.FromResult(licenceCategory));
            _requiredLicenceCategoryServiceMock.Setup(service => service.MeetRequirements(customerDrivingLicences, drivingLicenceToAdd.LicenceCategoryId, drivingLicenceToAdd.ReceivedDate)).Returns(Task.FromResult(true));
            _drivingLicenceRepositoryMock.Setup(repo => repo.PostDrivingLicence(drivingLicenceToAdd)).Returns(Task.FromResult(addedDrivingLicence));
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicence(addedDrivingLicence.Id)).Returns(Task.FromResult(addedDrivingLicenceDTO));
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetCustomerDrivingLicences(drivingLicenceToAdd.CustomerId, drivingLicenceToAdd.ReceivedDate)).Returns(Task.FromResult(customerDrivingLicences));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = await _service.PostDrivingLicence(drivingLicenceToAdd);

            Assert.AreEqual(addedDrivingLicenceDTO, result);
        }
        

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsReceivedDateTimeException()
        {
            var customer = new CustomerGetDTO { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicencePostDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1) };
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsExpirationDateTimeException()
        {
            var customer = new CustomerGetDTO { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicencePostDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ReceivedDate = new DateTime(2015, 1, 1) };
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsReceivedDateBiggerThanExpirationDateTimeException()
        {
            var customer = new CustomerGetDTO { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicencePostDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ReceivedDate = new DateTime(2015, 1, 1), ExpirationDate = new DateTime(2014,1,1) };
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsNotFoundCustomerException()
        {
            var customer = new CustomerGetDTO { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicencePostDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _customerServiceMock.Setup(service => service.GetCustomer(drivingLicenceToAdd.CustomerId)).Throws(new NotFoundCustomerException(drivingLicenceToAdd.CustomerId));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsNotFoundLicenceCategoryException()
        {
            var customer = new CustomerGetDTO { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicencePostDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _customerServiceMock.Setup(service => service.GetCustomer(drivingLicenceToAdd.CustomerId)).Returns(Task.FromResult(customer));
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(drivingLicenceToAdd.LicenceCategoryId)).Throws(new NotFoundLicenceCategoryException(drivingLicenceToAdd.LicenceCategoryId));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsDoesntMeetRequiremnts()
        {
            var customer = new CustomerGetDTO { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            ICollection<DrivingLicenceGetDTO> customerDrivingLicences = new List<DrivingLicenceGetDTO>();
            var addedDrivingLicence = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id };
            var addedDrivingLicenceDTO = new DrivingLicenceGetDTO { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, LicenceCategoryName = "Test", CustomerName = "Test", CutomserSecondName = "Test" };
            var drivingLicenceToAdd = new DrivingLicencePostDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _customerServiceMock.Setup(service => service.GetCustomer(drivingLicenceToAdd.CustomerId)).Returns(Task.FromResult(customer));
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(drivingLicenceToAdd.LicenceCategoryId)).Returns(Task.FromResult(licenceCategory));
            _requiredLicenceCategoryServiceMock.Setup(service => service.MeetRequirements(customerDrivingLicences, drivingLicenceToAdd.LicenceCategoryId, drivingLicenceToAdd.ReceivedDate)).Returns(Task.FromResult(false));
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetCustomerDrivingLicences(drivingLicenceToAdd.CustomerId, drivingLicenceToAdd.ReceivedDate)).Returns(Task.FromResult(customerDrivingLicences));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<CustomerDoesntMeetRequirementsException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }
    }
}
