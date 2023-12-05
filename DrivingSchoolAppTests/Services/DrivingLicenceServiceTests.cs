using AutoFixture;
using AutoMapper;
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
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private DrivingLicenceService _service;

        public DrivingLicenceServiceTests()
        {
            _fixture = new Fixture();
            _drivingLicenceRepositoryMock = new Mock<IDrivingLicenceRepository>();
            _customerServiceMock = new Mock<ICustomerService>();
            _licenceCategoryServiceMock = new Mock<ILicenceCategoryService>();
            _requiredLicenceCategoryServiceMock = new Mock<IRequiredLicenceCategoryService>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ReturnsDrivingLicences()
        {
            var drivingLicence = new DrivingLicenceGetDTO();
            var drivingLicencesList = new PagedList<DrivingLicenceGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<DrivingLicenceGetDTO> { drivingLicence }, HasNextPage = false };
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicences(1, 10)).ReturnsAsync(drivingLicencesList);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetDrivingLicences(1, 10);

            Assert.AreEqual(drivingLicencesList, result);
        }

        [TestMethod]
        public async Task Get_DrivingLicences_ThrowsNotFoundDrivingLicencesException()
        {
            var drivingLicencesList = new PagedList<DrivingLicenceGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<DrivingLicenceGetDTO>(), HasNextPage = false };
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicences(1, 10)).ReturnsAsync(drivingLicencesList);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.GetDrivingLicences(1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLicences_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicences(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetDrivingLicences(-1, 10));
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ReturnsDrivingLicence()
        {
            var drivingLicence = new DrivingLicenceGetDTO();
            var idOfDrivingLicenceToFind = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicence(idOfDrivingLicenceToFind)).ReturnsAsync(drivingLicence);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetDrivingLicence(idOfDrivingLicenceToFind);

            Assert.AreEqual(drivingLicence, result);
        }

        [TestMethod]
        public async Task Get_DrivingLicence_ThrowsNotFoundDrivingLicenceException()
        {
            var idOfDrivingLicenceToFind = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetDrivingLicence(idOfDrivingLicenceToFind)).ReturnsAsync((DrivingLicenceGetDTO)null);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.GetDrivingLicence(idOfDrivingLicenceToFind));
        }

        [TestMethod]
        public async Task Get_CustomerDrivingLicences_ReturnsDrivingLicences()
        {
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            var drivingLicence = new DrivingLicenceGetDTO();
            var drivingLicencesList = new List<DrivingLicenceGetDTO> { drivingLicence };
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetCustomerDrivingLicences(idOfCustomerToFind)).ReturnsAsync(drivingLicencesList);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCustomerDrivingLicences(idOfCustomerToFind);

            Assert.AreEqual(drivingLicencesList, result);
        }

        [TestMethod]
        public async Task Get_CustomerDrivingLicences_ThrowsNotFoundDrivingLicencesException()
        {
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            var drivingLicencesList = new List<DrivingLicenceGetDTO>();
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _drivingLicenceRepositoryMock.Setup(repo => repo.GetCustomerDrivingLicences(idOfCustomerToFind)).ReturnsAsync(drivingLicencesList);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.GetCustomerDrivingLicences(idOfCustomerToFind));
        }

        [TestMethod]
        public async Task Get_CustomerDrivingLicences_ThrowsNotFoundCustomerException()
        {
            var idOfCustomerToFind = 1;
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.GetCustomerDrivingLicences(idOfCustomerToFind));
        }

        [TestMethod]
        public async Task Check_CustomerDrivingLicences_ReturnsDrivingLicences()
        {
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            var drivingLicence = new DrivingLicence();
            var drivingLicencesList = new List<DrivingLicence> { drivingLicence };
            var checkDate = new DateTime(2023, 11, 9);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckCustomerDrivingLicences(idOfCustomerToFind, checkDate)).ReturnsAsync(drivingLicencesList);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckCustomerDrivingLicences(idOfCustomerToFind, checkDate);

            Assert.AreEqual(drivingLicencesList, result);
        }

        [TestMethod]
        public async Task Check_CustomerDrivingLicences_ThrowsNotFoundCustomer()
        {
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            var drivingLicence = new DrivingLicence();
            var drivingLicencesList = new List<DrivingLicence> { drivingLicence };
            var checkDate = new DateTime(2023, 11, 9);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToFind));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.CheckCustomerDrivingLicences(idOfCustomerToFind, checkDate));
        }

        [TestMethod]
        public async Task Check_CustomerDrivingLicences_ReturnsNull()
        {
            var idOfCustomerToFind = 1;
            var customer = new Customer();
            var drivingLicencesList = new List<DrivingLicence>();
            var checkDate = new DateTime(2023, 11, 9);
            _customerServiceMock.Setup(service => service.CheckCustomer(idOfCustomerToFind)).ReturnsAsync(customer);
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckCustomerDrivingLicences(idOfCustomerToFind, checkDate)).ReturnsAsync(drivingLicencesList);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckCustomerDrivingLicences(idOfCustomerToFind, checkDate);

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ReturnsAddedDrivingLicence()
        {
            var customer = new Customer { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000,1,1) };
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            ICollection<DrivingLicence> customerDrivingLicences = new List<DrivingLicence>();
            var addedDrivingLicence = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015,1,1), ReceivedDate = new DateTime(2014,1,1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id };
            var addedDrivingLicenceDTO = new DrivingLicenceResponseDTO { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id};
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLicenceToAdd.CustomerId)).ReturnsAsync(customer);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(drivingLicenceToAdd.LicenceCategoryId)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryServiceMock.Setup(service => service.MeetRequirements(customerDrivingLicences, drivingLicenceToAdd.LicenceCategoryId, drivingLicenceToAdd.ReceivedDate)).ReturnsAsync(true);
            _drivingLicenceRepositoryMock.Setup(repo => repo.PostDrivingLicence(drivingLicenceToAdd)).ReturnsAsync(addedDrivingLicence);
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckCustomerDrivingLicences(drivingLicenceToAdd.CustomerId, drivingLicenceToAdd.ReceivedDate)).ReturnsAsync(customerDrivingLicences);
            _mapperMock.Setup(m => m.Map<DrivingLicenceResponseDTO>(It.IsAny<DrivingLicence>())).Returns(addedDrivingLicenceDTO);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.PostDrivingLicence(drivingLicenceToAdd);

            Assert.AreEqual(addedDrivingLicenceDTO, result);
        }
        
        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsReceivedDateTimeException()
        {
            var customer = new Customer { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategory{ Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1) };
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsExpirationDateTimeException()
        {
            var customer = new Customer { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ReceivedDate = new DateTime(2015, 1, 1) };
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsReceivedDateBiggerThanExpirationDateTimeException()
        {
            var customer = new Customer { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ReceivedDate = new DateTime(2015, 1, 1), ExpirationDate = new DateTime(2014,1,1) };
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsNotFoundCustomerException()
        {
            var customer = new Customer { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLicenceToAdd.CustomerId)).ThrowsAsync(new NotFoundCustomerException(drivingLicenceToAdd.CustomerId));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsNotFoundLicenceCategoryException()
        {
            var customer = new Customer { Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLicenceToAdd.CustomerId)).ReturnsAsync(customer);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(drivingLicenceToAdd.LicenceCategoryId)).ThrowsAsync(new NotFoundLicenceCategoryException(drivingLicenceToAdd.LicenceCategoryId));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.PostDrivingLicence(drivingLicenceToAdd));
        }

        [TestMethod]
        public async Task Post_DrivingLicence_ThrowsDoesntMeetRequiremnts()
        {
            var customer = new Customer{ Id = 1, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            ICollection<DrivingLicence> customerDrivingLicences = new List<DrivingLicence>();
            var addedDrivingLicence = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id };
            var addedDrivingLicenceDTO = new DrivingLicenceGetDTO { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, LicenceCategoryName = "Test", CustomerName = "Test", CutomserSecondName = "Test" };
            var drivingLicenceToAdd = new DrivingLicenceRequestDTO { CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLicenceToAdd.CustomerId)).ReturnsAsync(customer);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(drivingLicenceToAdd.LicenceCategoryId)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryServiceMock.Setup(service => service.MeetRequirements(customerDrivingLicences, drivingLicenceToAdd.LicenceCategoryId, drivingLicenceToAdd.ReceivedDate)).ReturnsAsync(false);
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckCustomerDrivingLicences(drivingLicenceToAdd.CustomerId, drivingLicenceToAdd.ReceivedDate)).ReturnsAsync(customerDrivingLicences);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

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
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.DeleteDrivingLicence(idOfDrivingLicence);

            Assert.AreEqual(drivingLicence, result);
        }

        [TestMethod]
        public async Task Delete_DrivingLicence_ThrowsNotFoundDrivingLicenceException()
        {
            var idOfDrivingLicence = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicence(idOfDrivingLicence)).ReturnsAsync((DrivingLicence)null);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.DeleteDrivingLicence(idOfDrivingLicence));
        }

        [TestMethod]
        public async Task Check_DrivingLicence_ReturnsDrivingLicence()
        {
            var drivingLicence = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 1, LicenceCategoryId = 1 };
            var idOfDrivingLicence = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicence(idOfDrivingLicence)).ReturnsAsync(drivingLicence);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckDrivingLicence(idOfDrivingLicence);

            Assert.AreEqual(drivingLicence, result);
        }

        [TestMethod]
        public async Task Check_DrivingLicence_ThrowsDrivingLicenceException()
        {
            var idOfDrivingLicence = 10;
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicence(idOfDrivingLicence)).ReturnsAsync((DrivingLicence)null);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                  _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.CheckDrivingLicence(idOfDrivingLicence));
        }

        [TestMethod]
        public async Task Check_DrivingLicenceTracking_ReturnsDrivingLicence()
        {
            var drivingLicence = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 1, LicenceCategoryId = 1 };
            var idOfDrivingLicence = 1;
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ReturnsAsync(drivingLicence);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckDrivingLicenceTracking(idOfDrivingLicence);

            Assert.AreEqual(drivingLicence, result);
        }

        [TestMethod]
        public async Task Check_DrivingLicenceTracking_ThrowsDrivingLicenceException()
        {
            var idOfDrivingLicence = 10;
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ReturnsAsync((DrivingLicence)null);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                  _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.CheckDrivingLicenceTracking(idOfDrivingLicence));
        }

        [TestMethod]
        public async Task Update_DrivingLicence_ReturnsUpdatedDrivingLicence()
        {
            var idOfDrivingLicence = 1;
            var customer = new Customer { Id = 2, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategory { Id = 2, Name = "Test" };
            ICollection<DrivingLicence> customerDrivingLicences = new List<DrivingLicence>();
            var drivingLicenceToUpdate = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id };
            var updatedDrivingLicence = new DrivingLicenceResponseDTO { Id = 1, ExpirationDate = new DateTime(2016, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 2, LicenceCategoryId = 2 };
            var drivingLicenceUpdate = new DrivingLicenceRequestDTO { CustomerId = 2, LicenceCategoryId = 2, ExpirationDate = new DateTime(2016, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ReturnsAsync(drivingLicenceToUpdate);
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLicenceUpdate.CustomerId)).ReturnsAsync(customer);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(drivingLicenceUpdate.LicenceCategoryId)).ReturnsAsync(licenceCategory); 
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckCustomerDrivingLicences(drivingLicenceUpdate.CustomerId, drivingLicenceUpdate.ReceivedDate)).ReturnsAsync(customerDrivingLicences);
            _requiredLicenceCategoryServiceMock.Setup(service => service.MeetRequirements(customerDrivingLicences, drivingLicenceUpdate.LicenceCategoryId, drivingLicenceUpdate.ReceivedDate)).ReturnsAsync(true);
            _drivingLicenceRepositoryMock.Setup(repo => repo.UpdateDrivingLicence(drivingLicenceToUpdate, drivingLicenceUpdate)).ReturnsAsync(drivingLicenceToUpdate);
            _mapperMock.Setup(m => m.Map<DrivingLicenceResponseDTO>(It.IsAny<DrivingLicence>())).Returns(updatedDrivingLicence);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.UpdateDrivingLicence(idOfDrivingLicence, drivingLicenceUpdate);

            Assert.AreEqual(updatedDrivingLicence, result);
        }

        [TestMethod]
        public async Task Update_DrivingLicence_ThrowsNotFoundDrivingLicenceException()
        {
            var idOfDrivingLicence = 1;
            var drivingLicenceUpdate = new DrivingLicenceRequestDTO { CustomerId = 2, LicenceCategoryId = 2, ExpirationDate = new DateTime(2016, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ThrowsAsync(new NotFoundDrivingLicenceException(idOfDrivingLicence));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundDrivingLicenceException>(async () => await _service.UpdateDrivingLicence(idOfDrivingLicence, drivingLicenceUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLicence_ThrowsReceivedDateTimeException()
        {
            var idOfDrivingLicence = 1;
            var drivingLicenceToUpdate = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 1, LicenceCategoryId = 1 };
            var drivingLicenceUpdate = new DrivingLicenceRequestDTO { CustomerId = 2, LicenceCategoryId = 2, ExpirationDate = new DateTime(2016, 1, 1)};
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ReturnsAsync(drivingLicenceToUpdate);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.UpdateDrivingLicence(idOfDrivingLicence, drivingLicenceUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLicence_ThrowsExpirationDateTimeException()
        {
            var idOfDrivingLicence = 1;
            var drivingLicenceToUpdate = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 1, LicenceCategoryId = 1 };
            var drivingLicenceUpdate = new DrivingLicenceRequestDTO { CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2016, 1, 1) };
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ReturnsAsync(drivingLicenceToUpdate);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.UpdateDrivingLicence(idOfDrivingLicence, drivingLicenceUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLicence_ThrowsReceivedDateBiggerThanExpirationDateTimeException()
        {
            var idOfDrivingLicence = 1;
            var drivingLicenceToUpdate = new DrivingLicence { Id = 1, ReceivedDate = new DateTime(2015, 1, 1), ExpirationDate = new DateTime(2014, 1, 1), CustomerId = 1, LicenceCategoryId = 1 };
            var drivingLicenceUpdate = new DrivingLicenceRequestDTO { CustomerId = 2, LicenceCategoryId = 2, ReceivedDate = new DateTime(2016, 1, 1), ExpirationDate = new DateTime(2014,1,1) };
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ReturnsAsync(drivingLicenceToUpdate);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.UpdateDrivingLicence(idOfDrivingLicence, drivingLicenceUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLicence_ThrowsNotFoundCustomerException()
        {
            var idOfDrivingLicence = 1;
            var drivingLicenceToUpdate = new DrivingLicence { Id = 1,  ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 1, LicenceCategoryId = 1 };
            var drivingLicenceUpdate = new DrivingLicenceRequestDTO { CustomerId = 2, LicenceCategoryId = 2, ExpirationDate = new DateTime(2016, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ReturnsAsync(drivingLicenceToUpdate);
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLicenceUpdate.CustomerId)).ThrowsAsync(new NotFoundCustomerException(drivingLicenceUpdate.CustomerId));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCustomerException>(async () => await _service.UpdateDrivingLicence(idOfDrivingLicence, drivingLicenceUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLicence_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfDrivingLicence = 1;
            var drivingLicenceToUpdate = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 1, LicenceCategoryId = 1 };
            var drivingLicenceUpdate = new DrivingLicenceRequestDTO { CustomerId = 2, LicenceCategoryId = 2, ExpirationDate = new DateTime(2016, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            var customer = new Customer { Id = 2, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ReturnsAsync(drivingLicenceToUpdate);
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLicenceUpdate.CustomerId)).ReturnsAsync(customer);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(drivingLicenceUpdate.LicenceCategoryId)).ThrowsAsync(new NotFoundLicenceCategoryException(drivingLicenceUpdate.LicenceCategoryId));
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.UpdateDrivingLicence(idOfDrivingLicence, drivingLicenceUpdate));
        }

        [TestMethod]
        public async Task Update_DrivingLicence_ThrowsDoesntMeetRequiremnts()
        {
            var idOfDrivingLicence = 1;
            var customer = new Customer { Id = 2, Name = "Test", SecondName = "Test", BirthDate = new DateTime(2000, 1, 1) };
            var licenceCategory = new LicenceCategory { Id = 2, Name = "Test" };
            ICollection<DrivingLicence> customerDrivingLicences = new List<DrivingLicence>();
            var drivingLicenceToUpdate = new DrivingLicence { Id = 1, ExpirationDate = new DateTime(2015, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = customer.Id, LicenceCategoryId = licenceCategory.Id };
            var updatedDrivingLicence = new DrivingLicenceResponseDTO { Id = 1, ExpirationDate = new DateTime(2016, 1, 1), ReceivedDate = new DateTime(2014, 1, 1), CustomerId = 2, LicenceCategoryId = 2 };
            var drivingLicenceUpdate = new DrivingLicenceRequestDTO { CustomerId = 2, LicenceCategoryId = 2, ExpirationDate = new DateTime(2016, 1, 1), ReceivedDate = new DateTime(2014, 1, 1) };
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckDrivingLicenceTracking(idOfDrivingLicence)).ReturnsAsync(drivingLicenceToUpdate);
            _customerServiceMock.Setup(service => service.CheckCustomer(drivingLicenceUpdate.CustomerId)).ReturnsAsync(customer);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(drivingLicenceUpdate.LicenceCategoryId)).ReturnsAsync(licenceCategory);
            _drivingLicenceRepositoryMock.Setup(repo => repo.CheckCustomerDrivingLicences(drivingLicenceUpdate.CustomerId, drivingLicenceUpdate.ReceivedDate)).ReturnsAsync(customerDrivingLicences);
            _requiredLicenceCategoryServiceMock.Setup(service => service.MeetRequirements(customerDrivingLicences, drivingLicenceUpdate.LicenceCategoryId, drivingLicenceUpdate.ReceivedDate)).ReturnsAsync(false);
            _service = new DrivingLicenceService(_drivingLicenceRepositoryMock.Object, _customerServiceMock.Object,
                                                 _licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<CustomerDoesntMeetRequirementsException>(async () => await _service.UpdateDrivingLicence(idOfDrivingLicence, drivingLicenceUpdate));
        }
    }
}
