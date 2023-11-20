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
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicences(1, 10)).ReturnsAsync(drivingLicencesList);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = await _service.GetDrivingLicences(1, 10);

            Assert.AreEqual(drivingLicencesList, result);
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ThrowsNotFoundDrivingLicencesException()
        {
            var drivingLicencesList = new PagedList<DrivingLicenceGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<DrivingLicenceGetDTO>(), HasNextPage = false };
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicences(1, 10)).ReturnsAsync(drivingLicencesList);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.GetDrivingLicences(1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLicences_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicences(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetDrivingLicences(-1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ReturnsDrivingLicence()
        {
            var drivingLicence = new DrivingLicenceGetDTO();
            var idOfDrivingLicenceToFind = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicence(idOfDrivingLicenceToFind)).ReturnsAsync(drivingLicence);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = await _service.GetDrivingLicence(idOfDrivingLicenceToFind);

            Assert.AreEqual(drivingLicence, result);
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ThrowsNotFoundDrivingLicenceException()
        {
            var idOfDrivingLicenceToFind = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicence(idOfDrivingLicenceToFind)).ReturnsAsync((DrivingLicenceGetDTO)null);
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
            _customerServiceMock.Setup(service => service.GetCustomer(drivingLicenceToAdd.CustomerId)).ReturnsAsync(customer);
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(drivingLicenceToAdd.LicenceCategoryId)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryServiceMock.Setup(service => service.MeetRequirements(customerDrivingLicences, drivingLicenceToAdd.LicenceCategoryId, drivingLicenceToAdd.ReceivedDate)).ReturnsAsync(true);
            _drivingLicenceRepositoryMock.Setup(repo => repo.PostDrivingLicence(drivingLicenceToAdd)).ReturnsAsync(addedDrivingLicence);
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicence(addedDrivingLicence.Id)).ReturnsAsync(addedDrivingLicenceDTO);
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetCustomerDrivingLicences(drivingLicenceToAdd.CustomerId, drivingLicenceToAdd.ReceivedDate)).ReturnsAsync(customerDrivingLicences);
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
            _customerServiceMock.Setup(service => service.GetCustomer(drivingLicenceToAdd.CustomerId)).ThrowsAsync(new NotFoundCustomerException(drivingLicenceToAdd.CustomerId));
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
            _customerServiceMock.Setup(service => service.GetCustomer(drivingLicenceToAdd.CustomerId)).ReturnsAsync(customer);
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(drivingLicenceToAdd.LicenceCategoryId)).ThrowsAsync(new NotFoundLicenceCategoryException(drivingLicenceToAdd.LicenceCategoryId));
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
            _customerServiceMock.Setup(service => service.GetCustomer(drivingLicenceToAdd.CustomerId)).ReturnsAsync(customer);
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(drivingLicenceToAdd.LicenceCategoryId)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryServiceMock.Setup(service => service.MeetRequirements(customerDrivingLicences, drivingLicenceToAdd.LicenceCategoryId, drivingLicenceToAdd.ReceivedDate)).ReturnsAsync(false);
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetCustomerDrivingLicences(drivingLicenceToAdd.CustomerId, drivingLicenceToAdd.ReceivedDate)).ReturnsAsync(customerDrivingLicences);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<CustomerDoesntMeetRequirementsException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Delete_DrivingLicence_ReturnsDrivingLicence()
        {
            var drivingLicence = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 1, LicenceCategoryId = 1 };
            var idOfDrivingLicence = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicence(idOfDrivingLicence)).ReturnsAsync(drivingLicence);
            _drivingLicenceRepositoryMock.Setup(repo => repo.DeleteDrivingLicence(drivingLicence)).ReturnsAsync(drivingLicence);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = await _service.DeleteDrivingLicence(idOfDrivingLicence);

            Assert.AreEqual(drivingLicence, result);
        }

        [TestMethod]
        public async Task Delete_DrivingLicence_ThrowsNotFoundDrivingLicenceException()
        {
            var idOfDrivingLicence = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicence(idOfDrivingLicence)).ReturnsAsync((DrivingLicence)null);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.DeleteDrivingLicence(idOfDrivingLicence));
        }

        [TestMethod]
        public async Task Check_DrivingLicence_ReturnsDrivingLicence()
        {
            var drivingLicence = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 1, LicenceCategoryId = 1 };
            var idOfDrivingLicence = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicence(idOfDrivingLicence)).ReturnsAsync(drivingLicence);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = await _service.CheckDrivingLicence(idOfDrivingLicence);

            Assert.AreEqual(drivingLicence, result);
        }

        [TestMethod]
        public async Task Check_DrivingLicence_ThrowsDrivingLicenceException()
        {
            var idOfDrivingLicence = 10;
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicence(idOfDrivingLicence)).ReturnsAsync((DrivingLicence)null);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                  _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.DeleteDrivingLicence(idOfDrivingLicence));
        }
    }
}
