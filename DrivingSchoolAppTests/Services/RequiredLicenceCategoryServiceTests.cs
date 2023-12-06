using AutoFixture;
using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Moq;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class RequiredLicenceCategoryServiceTests
    {
        private Mock<ILicenceCategoryService> _licenceCategoryServiceMock;
        private Mock<IRequiredLicenceCategoryRepository> _requiredLicenceCategoryRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private RequiredLicenceCategoryService _service;

        public RequiredLicenceCategoryServiceTests()
        {
            _fixture = new Fixture();
            _licenceCategoryServiceMock = new Mock<ILicenceCategoryService>();
            _requiredLicenceCategoryRepositoryMock = new Mock<IRequiredLicenceCategoryRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_Requirements_ReturnsRequirements()
        {
            var requirement = new RequiredLicenceCategoryGetDTO();
            ICollection<RequiredLicenceCategoryGetDTO> requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements()).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetRequirements();

            Assert.AreEqual(requirementsList, result);
        }

        [TestMethod]
        public async Task Get_Requirements_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            ICollection<RequiredLicenceCategoryGetDTO> requirementsList = new List<RequiredLicenceCategoryGetDTO>();
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements()).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.GetRequirements());
        }

        [TestMethod]
        public async Task Get_Requirement_ReturnsRequirement()
        {
            var licenceCategory = new LicenceCategory { Id = 1 };
            var requiredLicenceCategory = new LicenceCategory { Id = 2 };
            var requirement = new RequiredLicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(requiredLicenceCategory.Id)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirement(licenceCategory.Id, requiredLicenceCategory.Id)).ReturnsAsync(requirement);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetRequirement(licenceCategory.Id, requiredLicenceCategory.Id);

            Assert.AreEqual(requirement, result);
        }

        [TestMethod]
        public async Task Get_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var requirement = new RequiredLicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Get_Requirement_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory { Id = 1};
            var requirement = new RequiredLicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfRequiredLicenceCategory));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Get_Requirement_ThrowsNotFoundRequirementException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory { Id = 1 };
            var requiredLicenceCategory = new LicenceCategory { Id = 2 };
            var requirement = new RequiredLicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ReturnsAsync((RequiredLicenceCategoryGetDTO)null);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.GetRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Get_CategoryRequirements_ReturnsRequirements()
        {
            var idOfLicenceCategory = 1;
            var licenceCategory = new LicenceCategory { Id = 1 };
            var requirement = new RequiredLicenceCategoryGetDTO();
            ICollection<RequiredLicenceCategoryGetDTO> requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(idOfLicenceCategory)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetRequirements(idOfLicenceCategory);

            Assert.AreEqual(requirementsList, result);
        }

        [TestMethod]
        public async Task Get_CategoryRequirements_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetRequirements(idOfLicenceCategory));
        }

        [TestMethod]
        public async Task Get_CategoryRequirements_ThrowsNotFoundRequirementsException()
        {
            var idOfLicenceCategory = 1;
            var licenceCategory = new LicenceCategory { Id = 1 };
            ICollection<RequiredLicenceCategoryGetDTO> requirementsList = new List<RequiredLicenceCategoryGetDTO>();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(idOfLicenceCategory)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.GetRequirements(idOfLicenceCategory));
        }

        [TestMethod]
        public async Task Check_Requirement_ReturnsRequirement()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ReturnsAsync(requirement);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            Assert.AreEqual(requirement, result);
        }

        [TestMethod]
        public async Task Check_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Check_Requirement_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfRequiredLicenceCategory));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Check_Requirement_ThrowsNotFoundRequirementException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ReturnsAsync((RequiredLicenceCategory)null);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Check_RequirementTracking_ReturnsRequirement()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirementTracking(idOfLicenceCategory, idOfRequiredLicenceCategory)).ReturnsAsync(requirement);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckRequirementTracking(idOfLicenceCategory, idOfRequiredLicenceCategory);

            Assert.AreEqual(requirement, result);
        }

        [TestMethod]
        public async Task Check_RequirementTracking_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.CheckRequirementTracking(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Check_RequirementTracking_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfRequiredLicenceCategory));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.CheckRequirementTracking(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Check_RequirementTracking_ThrowsNotFoundRequirementException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirementTracking(idOfLicenceCategory, idOfRequiredLicenceCategory)).ReturnsAsync((RequiredLicenceCategory)null);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.CheckRequirementTracking(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Post_Requirement_ReturnsRequirement()
        {
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategory { Id = 2, Name = "Test" };
            var addedRequiredLicenceCategoryDTO = new RequiredLicenceCategoryResponseDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            var addedRequiredLicence = new RequiredLicenceCategory { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            var requireToAdd = new RequiredLicenceCategoryRequestDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(requiredLicenceCategory.Id)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirement(requireToAdd.LicenceCategoryId, requireToAdd.RequiredLicenceCategoryId)).ReturnsAsync((RequiredLicenceCategory)null);
            _requiredLicenceCategoryRepositoryMock.Setup(service => service.PostRequirement(requireToAdd)).ReturnsAsync(addedRequiredLicence);
            _mapperMock.Setup(m => m.Map<RequiredLicenceCategoryResponseDTO>(It.IsAny<RequiredLicenceCategory>())).Returns(addedRequiredLicenceCategoryDTO);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.PostRequirement(requireToAdd);

            Assert.AreEqual(addedRequiredLicenceCategoryDTO, result);
        }
        
        [TestMethod]
        public async Task Post_Requirement_ThrowsValueMustBeGreaterThanZeroException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2, Name = "Test" };
            var requireToAdd = new RequiredLicenceCategoryRequestDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 0 };
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostRequirement(requireToAdd));
        }

        [TestMethod]
        public async Task Post_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var licenceCategory = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2, Name = "Test" };
            var requireToAdd = new RequiredLicenceCategoryRequestDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(requireToAdd.LicenceCategoryId)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.PostRequirement(requireToAdd));
        }

        [TestMethod]
        public async Task Post_Requirement_ThrowsNotFoundRequireLicenceCategoryException()
        {
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategoryGetDTO { Id = 2, Name = "Test" };
            var requireToAdd = new RequiredLicenceCategoryRequestDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(requireToAdd.LicenceCategoryId)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(requiredLicenceCategory.Id)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.PostRequirement(requireToAdd));
        }

        [TestMethod]
        public async Task Post_Requirement_ThrowsRequirementAlreadyExistsException()
        {
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategory{ Id = 2, Name = "Test" };
            var requirement = new RequiredLicenceCategory();
            var requireToAdd = new RequiredLicenceCategoryRequestDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(requireToAdd.LicenceCategoryId)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(requiredLicenceCategory.Id)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirement(requireToAdd.LicenceCategoryId, requireToAdd.RequiredLicenceCategoryId)).ReturnsAsync(requirement);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<RequirementAlreadyExistsException>(async () => await _service.PostRequirement(requireToAdd));
        }

        [TestMethod]
        public async Task Meet_Requirement_NoRequirementsReturnsTrue()
        {
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var customerLicences = new List<DrivingLicence>();
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Meet_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var customerLicences = new List<DrivingLicence>();
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(licenceCategory.Id)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1)));
        }

        [TestMethod]
        public async Task Meet_Requirement_NoCustomerLicencesReturnsFalse()
        {
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requirement = new RequiredLicenceCategoryGetDTO();
            var customerLicences = new List<DrivingLicence>();
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Meet_Requirement_CustomerHasRequirementReturnsTrue()
        {
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requirement = new RequiredLicenceCategoryGetDTO { LicenceCategoryId = 2, RequiredLicenceCategoryId = 1, RequiredYears = 1 };
            var customerDrivingLicence = new DrivingLicence { LicenceCategoryId = 1, ReceivedDate = new DateTime(2015, 1, 1) };
            var customerLicences = new List<DrivingLicence> { customerDrivingLicence };            
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Meet_Requirement_CustomerHasntRequirementReturnsFalse()
        {
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requirement = new RequiredLicenceCategoryGetDTO { LicenceCategoryId = 2, RequiredLicenceCategoryId = 1, RequiredYears = 1 };
            var customerDrivingLicence = new DrivingLicence { LicenceCategoryId = 3, ReceivedDate = new DateTime(2015, 1, 1) };
            var customerLicences = new List<DrivingLicence> { customerDrivingLicence };
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Meet_Requirement_CustomerHasRequirementToShortReturnsFalse()
        {
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requirement = new RequiredLicenceCategoryGetDTO { LicenceCategoryId = 2, RequiredLicenceCategoryId = 1, RequiredYears = 1 };
            var customerDrivingLicence = new DrivingLicence { LicenceCategoryId = 1, ReceivedDate = new DateTime(2019, 11, 1) };
            var customerLicences = new List<DrivingLicence> { customerDrivingLicence };
            var requirementsList = new List<RequiredLicenceCategoryGetDTO>() { requirement };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(licenceCategory.Id)).ReturnsAsync(licenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.GetRequirements(licenceCategory.Id)).ReturnsAsync(requirementsList);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.MeetRequirements(customerLicences, licenceCategory.Id, new DateTime(2020, 1, 1));

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Delete_Requirement_ReturnsRequirement()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ReturnsAsync(requirement);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.DeleteRequirement(requirement)).ReturnsAsync(requirement);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            Assert.AreEqual(requirement, result);
        }

        [TestMethod]
        public async Task Delete_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Delete_Requirement_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfRequiredLicenceCategory));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Delete_Requirement_ThrowsNotFoundRequirementException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 2;
            var licenceCategory = new LicenceCategory();
            var requiredLicenceCategory = new LicenceCategory();
            var requirement = new RequiredLicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(idOfRequiredLicenceCategory)).ReturnsAsync(requiredLicenceCategory);
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ReturnsAsync((RequiredLicenceCategory)null);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory));
        }

        [TestMethod]
        public async Task Update_Requirement_ReturnsRequirement()
        {
            var licenceCategoryId = 1;
            var requiredLicenceCategoryId = 2;
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategory { Id = 2, Name = "Test" };
            var requirementToUpdate = new RequiredLicenceCategory { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            var requirementUpdate = new RequiredLicenceCategoryPutRequestDTO { RequiredYears = 3 };
            var updatedRequirement = new RequiredLicenceCategoryResponseDTO { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 3 };
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirementTracking(licenceCategoryId, requiredLicenceCategoryId)).ReturnsAsync(requirementToUpdate);
            _requiredLicenceCategoryRepositoryMock.Setup(service => service.UpdateRequirement(requirementToUpdate, requirementUpdate)).ReturnsAsync(requirementToUpdate);
            _mapperMock.Setup(m => m.Map<RequiredLicenceCategoryResponseDTO>(It.IsAny<RequiredLicenceCategory>())).Returns(updatedRequirement);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.UpdateRequirement(licenceCategoryId, requiredLicenceCategoryId, requirementUpdate);

            Assert.AreEqual(updatedRequirement, result);
        }

        [TestMethod]
        public async Task Update_Requirement_ThrowsNotFoundRequirementException()
        {
            var licenceCategoryId = 1;
            var requiredLicenceCategoryId = 2;
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategory { Id = 2, Name = "Test" };
            var requirementToUpdate = new RequiredLicenceCategory { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            var requirementUpdate = new RequiredLicenceCategoryPutRequestDTO { RequiredYears = 3 };
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirementTracking(licenceCategory.Id, requiredLicenceCategory.Id)).ThrowsAsync(new NotFoundRequiredLicenceCategoryException(licenceCategory.Id, requiredLicenceCategory.Id));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundRequiredLicenceCategoryException>(async () => await _service.UpdateRequirement(licenceCategoryId, requiredLicenceCategoryId, requirementUpdate));
        }

        [TestMethod]
        public async Task Update_Requirement_ThrowsValueMustBeGreaterThanZeroException()
        {
            var licenceCategoryId = 1;
            var requiredLicenceCategoryId = 2;
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategory { Id = 2, Name = "Test" };
            var requirementToUpdate = new RequiredLicenceCategory { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            var requirementUpdate = new RequiredLicenceCategoryPutRequestDTO { RequiredYears = -3 };
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirementTracking(licenceCategory.Id, requiredLicenceCategory.Id)).ReturnsAsync(requirementToUpdate);
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateRequirement(licenceCategoryId, requiredLicenceCategoryId, requirementUpdate));
        }

        [TestMethod]
        public async Task Update_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var licenceCategoryId = 1;
            var requiredLicenceCategoryId = 2;
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategory { Id = 2, Name = "Test" };
            var requirementToUpdate = new RequiredLicenceCategory { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            var requirementUpdate = new RequiredLicenceCategoryPutRequestDTO { RequiredYears = 3 };
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirementTracking(licenceCategory.Id, requiredLicenceCategory.Id)).ReturnsAsync(requirementToUpdate);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(licenceCategory.Id)).ThrowsAsync(new NotFoundLicenceCategoryException(licenceCategory.Id));
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.UpdateRequirement(licenceCategoryId, requiredLicenceCategoryId, requirementUpdate));
        }

        [TestMethod]
        public async Task Update_Requirement_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            var licenceCategoryId = 1;
            var requiredLicenceCategoryId = 2;
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var requiredLicenceCategory = new LicenceCategory { Id = 2, Name = "Test" };
            var requirementToUpdate = new RequiredLicenceCategory { LicenceCategoryId = licenceCategory.Id, RequiredLicenceCategoryId = requiredLicenceCategory.Id, RequiredYears = 2 };
            var requirementUpdate = new RequiredLicenceCategoryPutRequestDTO { RequiredYears = 3 };
            _requiredLicenceCategoryRepositoryMock.Setup(repo => repo.CheckRequirementTracking(licenceCategory.Id, requiredLicenceCategory.Id)).ReturnsAsync(requirementToUpdate);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(requiredLicenceCategory.Id)).Throws(new NotFoundLicenceCategoryException());
            _service = new RequiredLicenceCategoryService(_requiredLicenceCategoryRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.UpdateRequirement(licenceCategoryId, requiredLicenceCategoryId, requirementUpdate));
        }
    }
}
