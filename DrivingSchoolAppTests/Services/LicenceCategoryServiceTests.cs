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
    public class LicenceCategoryServiceTests
    {
        private Mock<ILicenceCategoryRepository> _licenceCategoryRepositoryMock;
        private Fixture _fixture;
        private LicenceCategoryService _service;

        public LicenceCategoryServiceTests()
        {
            _fixture = new Fixture();
            _licenceCategoryRepositoryMock = new Mock<ILicenceCategoryRepository>();
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ReturnsLicenceCategories()
        {
            var licenceCategory = new LicenceCategoryGetDTO();
            var licenceCategoriesList = new PagedList<LicenceCategoryGetDTO> { PageIndex = 1, PageSize = 10, HasNextPage = false, PagedItems = new List<LicenceCategoryGetDTO> { licenceCategory } };
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategories(1,10)).Returns(Task.FromResult(licenceCategoriesList));
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object);

            var result = await _service.GetLicenceCategories(1,10);

            Assert.AreEqual(licenceCategoriesList, result);
            Assert.AreEqual(licenceCategoriesList.PagedItems.Count, result.PagedItems.Count);
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ThrowsNotFoundLicenceCategoriesException()
        {
            var licenceCategoriesList = new PagedList<LicenceCategoryGetDTO> { PageIndex = 1, PageSize = 10, HasNextPage = false, PagedItems = new List<LicenceCategoryGetDTO>() };
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategories(1, 10)).Returns(Task.FromResult(licenceCategoriesList));
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetLicenceCategories(1, 10));
        }

        [TestMethod]
        public async Task Get_LicenceCategories_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategories(-1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetLicenceCategories(-1, 10));
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ReturnsLicenceCategory()
        {
            var licenceCategory = new LicenceCategoryGetDTO();
            var idOfLicenceCategoryToFind = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategory(idOfLicenceCategoryToFind)).Returns(Task.FromResult(licenceCategory));
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object);

            var result = await _service.GetLicenceCategory(idOfLicenceCategoryToFind);

            Assert.AreEqual(licenceCategory, result);
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategoryToFind = 1;
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategory(idOfLicenceCategoryToFind)).Returns(Task.FromResult<LicenceCategoryGetDTO>(null));
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.GetLicenceCategory(idOfLicenceCategoryToFind));
        }

        [TestMethod]
        public async Task Post_LicenceCategory_ReturnsAddedLicenceCategory()
        {
            var addedLicenceCategory = new LicenceCategory { Id = 1, Name = "Test" };
            var addedLicenceCategoryDTO = new LicenceCategoryGetDTO { Id = 1, Name = "Test" };
            var licenceCategoryToAdd = new LicenceCategoryPostDTO { Name = "Test" };
            _licenceCategoryRepositoryMock.Setup(repo => repo.PostLicenceCategory(licenceCategoryToAdd)).Returns(Task.FromResult(addedLicenceCategory));
            _licenceCategoryRepositoryMock.Setup(repo => repo.GetLicenceCategory(addedLicenceCategory.Id)).Returns(Task.FromResult(addedLicenceCategoryDTO));
            _service = new LicenceCategoryService(_licenceCategoryRepositoryMock.Object);

            var result = await _service.PostLicenceCategory(licenceCategoryToAdd);

            Assert.AreEqual(addedLicenceCategoryDTO, result);
        }
    }
}
