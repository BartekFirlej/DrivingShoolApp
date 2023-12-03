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
    public class LicenceCategoryServiceTests
    {
        private Mock<ILicenceCategoryRepository> _licenceCategoryRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private LicenceCategoryService _service;

        public LicenceCategoryServiceTests()
        {
            _fixture = new Fixture();
            _licenceCategoryRepositoryMock = new Mock<ILicenceCategoryRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ReturnsLicenceCategories()
        {
            var licenceCategory = new LicenceCategoryGetDTO();
            var licenceCategoriesList = new PagedList<LicenceCategoryGetDTO> { PageIndex = 1, PageSize = 10, HasNextPage = false, PagedItems = new List<LicenceCategoryGetDTO> { licenceCategory } };
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategories(1,10)).ReturnsAsync(licenceCategoriesList);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetLicenceCategories(1,10);

            Assert.AreEqual(licenceCategoriesList, result);
            Assert.AreEqual(licenceCategoriesList.PagedItems.Count, result.PagedItems.Count);
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ThrowsNotFoundLicenceCategoriesException()
        {
            var licenceCategoriesList = new PagedList<LicenceCategoryGetDTO> { PageIndex = 1, PageSize = 10, HasNextPage = false, PagedItems = new List<LicenceCategoryGetDTO>() };
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategories(1, 10)).ReturnsAsync(licenceCategoriesList);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetLicenceCategories(1, 10));
        }

        [TestMethod]
        public async Task Get_LicenceCategories_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategories(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetLicenceCategories(-1, 10));
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ReturnsLicenceCategory()
        {
            var licenceCategory = new LicenceCategoryGetDTO();
            var idOfLicenceCategoryToFind = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategory(idOfLicenceCategoryToFind)).ReturnsAsync(licenceCategory);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetLicenceCategory(idOfLicenceCategoryToFind);

            Assert.AreEqual(licenceCategory, result);
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategoryToFind = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategory(idOfLicenceCategoryToFind)).ReturnsAsync((LicenceCategoryGetDTO)null);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetLicenceCategory(idOfLicenceCategoryToFind));
        }

        [TestMethod]
        public async Task Post_LicenceCategory_ReturnsAddedLicenceCategory()
        {
            var addedLicenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var addedLicenceCategoryDTO = new LicenceCategoryResponseDTO { Id = 1, Name = "Test" };
            var licenceCategoryToAdd = new LicenceCategoryRequestDTO { Name = "Test" };
            _licenceCategoryRepositoryMock.Setup(repo => repo.PostLicenceCategory(licenceCategoryToAdd)).ReturnsAsync(addedLicenceCategory);
            _mapperMock.Setup(m => m.Map<LicenceCategoryResponseDTO>(It.IsAny<LicenceCategory>())).Returns(addedLicenceCategoryDTO);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.PostLicenceCategory(licenceCategoryToAdd);

            Assert.AreEqual(addedLicenceCategoryDTO, result);
        }

        [TestMethod]
        public async Task Delete_LicenceCategory_ReturnsLicenceCategory()
        {
            var idOfLicenceCategoryToDelete = 1;
            var deletedLicenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            _licenceCategoryRepositoryMock.Setup(repo => repo.CheckLicenceCategory(idOfLicenceCategoryToDelete)).ReturnsAsync(deletedLicenceCategory);
            _licenceCategoryRepositoryMock.Setup(repo => repo.DeleteLicenceCategory(deletedLicenceCategory)).ReturnsAsync(deletedLicenceCategory);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.DeleteLicenceCategory(idOfLicenceCategoryToDelete);

            Assert.AreEqual(deletedLicenceCategory, result);
        }

        [TestMethod]
        public async Task Delete_LicenceCategory_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategoryToDelete = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.CheckLicenceCategory(idOfLicenceCategoryToDelete)).ReturnsAsync((LicenceCategory)null);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.DeleteLicenceCategory(idOfLicenceCategoryToDelete));
        }

        [TestMethod]
        public async Task Delete_LicenceCategory_PropagatesReferenceConstraintExceptionException()
        {
            var deletedLicenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var idOfLicenceCategoryToDelete = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.CheckLicenceCategory(idOfLicenceCategoryToDelete)).ReturnsAsync(deletedLicenceCategory);
            _licenceCategoryRepositoryMock.Setup(repo => repo.DeleteLicenceCategory(deletedLicenceCategory)).ThrowsAsync(new ReferenceConstraintException());
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteLicenceCategory(idOfLicenceCategoryToDelete));
        }

        [TestMethod]
        public async Task Check_LicenceCategory_ReturnsLicenceCategory()
        {
            var deletedLicenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var idOfLicenceCategory = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync(deletedLicenceCategory);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.CheckLicenceCategory(idOfLicenceCategory);

            Assert.AreEqual(deletedLicenceCategory, result);
        }

        [TestMethod]
        public async Task Check_LicenceCategory_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.CheckLicenceCategory(idOfLicenceCategory)).ReturnsAsync((LicenceCategory)null);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.CheckLicenceCategory(idOfLicenceCategory));
        }

        [TestMethod]
        public async Task Check_LicenceCategoryTracking_ReturnsLicenceCategory()
        {
            var deletedLicenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var idOfLicenceCategory = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.CheckLicenceCategoryTracking(idOfLicenceCategory)).ReturnsAsync(deletedLicenceCategory);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.CheckLicenceCategoryTracking(idOfLicenceCategory);

            Assert.AreEqual(deletedLicenceCategory, result);
        }

        [TestMethod]
        public async Task Check_LicenceCategoryTracking_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.CheckLicenceCategoryTracking(idOfLicenceCategory)).ReturnsAsync((LicenceCategory)null);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.CheckLicenceCategoryTracking(idOfLicenceCategory));
        }

        [TestMethod]
        public async Task Update_LicenceCategory_ReturnsLicenceCategory()
        {
            var idOfLicenceCategory = 1;
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var updatedLicenceCategory = new LicenceCategoryResponseDTO { Id = 1, Name = "Test" };
            var licenceCategoryUpdate = new LicenceCategoryRequestDTO { Name = "Update" };
            _licenceCategoryRepositoryMock.Setup(repo => repo.CheckLicenceCategoryTracking(idOfLicenceCategory)).ReturnsAsync(licenceCategory);
            _licenceCategoryRepositoryMock.Setup(repo => repo.UpdateLicenceCategory(licenceCategory, licenceCategoryUpdate)).ReturnsAsync(licenceCategory);
            _mapperMock.Setup(m => m.Map<LicenceCategoryResponseDTO>(It.IsAny<LicenceCategory>())).Returns(updatedLicenceCategory);
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.UpdateLicenceCategory(idOfLicenceCategory, licenceCategoryUpdate);

            Assert.AreEqual(updatedLicenceCategory, result);
        }

        [TestMethod]
        public async Task Update_LicenceCategory_ThrowsNotFoundLicenceCategory()
        {
            var idOfLicenceCategory = 1;
            var licenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var licenceCategoryUpdate = new LicenceCategoryRequestDTO { Name = "Update" };
            _licenceCategoryRepositoryMock.Setup(repo => repo.CheckLicenceCategoryTracking(idOfLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.UpdateLicenceCategory(idOfLicenceCategory, licenceCategoryUpdate));
        }
    }
}
