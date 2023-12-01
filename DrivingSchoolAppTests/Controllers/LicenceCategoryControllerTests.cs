using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
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
    public class LicenceCategoryControllerTests
    {
        private Mock<ILicenceCategoryService> _licenceCategoryServiceMock;
        private Mock<IRequiredLicenceCategoryService> _requiredLicenceCategoryServiceMock;
        private Fixture _fixture;
        private LicenceCategoryController _controller;

        public LicenceCategoryControllerTests()
        {
            _fixture = new Fixture();
            _licenceCategoryServiceMock = new Mock<ILicenceCategoryService>();
            _requiredLicenceCategoryServiceMock = new Mock<IRequiredLicenceCategoryService>();
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ReturnsOk()
        {
            var licenceCategoriesList = new PagedList<LicenceCategoryGetDTO>();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategories(1, 10)).ReturnsAsync(licenceCategoriesList);
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLicenceCategories(1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ThrowsNotFoundLicenceCategoriesException()
        {
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategories(1, 10)).ThrowsAsync(new NotFoundLicenceCategoryException());
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLicenceCategories(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Addresses_ThrowsValueMustBeGreaterThanZeroException()
        {
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategories(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetLicenceCategories(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ReturnsOk()
        {
            var licenceCategory = new LicenceCategoryGetDTO();
            var idOfLicenceToFind = 1;
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(idOfLicenceToFind)).ReturnsAsync(licenceCategory);
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLicenceCategory(idOfLicenceToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ThrowsNotFoundLicenceCategoriesException()
        {
            var idOfLicenceToFind = 1;
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(idOfLicenceToFind)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceToFind));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLicenceCategory(idOfLicenceToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_LicenceCategoryRequirements_ReturnsOk()
        {
            var idOfLicenceToFind = 1;
            ICollection<RequiredLicenceCategoryGetDTO> requiredLicences = new List<RequiredLicenceCategoryGetDTO>();
            _requiredLicenceCategoryServiceMock.Setup(service => service.GetRequirements(idOfLicenceToFind)).ReturnsAsync(requiredLicences);
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLicenceCategoryRequirements(idOfLicenceToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_LicenceCategoryRequirements_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceToFind = 1;
            _requiredLicenceCategoryServiceMock.Setup(service => service.GetRequirements(idOfLicenceToFind)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceToFind));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLicenceCategoryRequirements(idOfLicenceToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_LicenceCategoryRequirements_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            var idOfLicenceToFind = 1;
            _requiredLicenceCategoryServiceMock.Setup(service => service.GetRequirements(idOfLicenceToFind)).ThrowsAsync(new NotFoundRequiredLicenceCategoryException(idOfLicenceToFind));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLicenceCategoryRequirements(idOfLicenceToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_LicenceCategory_ReturnCreatedAtAction()
        {
            var licenceCategoryToAdd = new LicenceCategoryRequestDTO();
            var addedLicenceCategory = new LicenceCategoryResponseDTO();
            _licenceCategoryServiceMock.Setup(service => service.PostLicenceCategory(licenceCategoryToAdd)).ReturnsAsync(addedLicenceCategory);
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostLicenceCategory(licenceCategoryToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_RequiredLicenceCategory_ReturnsCreatedAtAction()
        {
            var requiredLicenceCategoryToAdd = new RequiredLicenceCategoryPostDTO();
            var addedRequiredLicenceCategory = new RequiredLicenceCategoryGetDTO();
            _requiredLicenceCategoryServiceMock.Setup(service => service.PostRequirement(requiredLicenceCategoryToAdd)).ReturnsAsync(addedRequiredLicenceCategory);
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostRequiredLicenceCategory(requiredLicenceCategoryToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_RequiredLicenceCategory_ThrowsNotFoundLicenceCategoryException()
        {
            var requiredLicenceCategoryToAdd = new RequiredLicenceCategoryPostDTO();
            _requiredLicenceCategoryServiceMock.Setup(service => service.PostRequirement(requiredLicenceCategoryToAdd)).ThrowsAsync(new NotFoundLicenceCategoryException());
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostRequiredLicenceCategory(requiredLicenceCategoryToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_RequiredLicenceCategory_ThrowsValueMustBeGreaterThanZeroException()
        {
            var requiredLicenceCategoryToAdd = new RequiredLicenceCategoryPostDTO();
            _requiredLicenceCategoryServiceMock.Setup(service => service.PostRequirement(requiredLicenceCategoryToAdd)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("years"));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostRequiredLicenceCategory(requiredLicenceCategoryToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_RequiredLicenceCategory_ThrowsRequirementAlreadyExistsException()
        {
            var requiredLicenceCategoryToAdd = new RequiredLicenceCategoryPostDTO();
            _requiredLicenceCategoryServiceMock.Setup(service => service.PostRequirement(requiredLicenceCategoryToAdd)).ThrowsAsync(new RequirementAlreadyExistsException(1, 2));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostRequiredLicenceCategory(requiredLicenceCategoryToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Delete_LicenceCategory_ReturnNoContent()
        {
            var deletedLicenceCategory = new LicenceCategory();
            var idOfLicenceCategoryToDelete = 1;
            _licenceCategoryServiceMock.Setup(service => service.DeleteLicenceCategory(idOfLicenceCategoryToDelete)).ReturnsAsync(deletedLicenceCategory);
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteLicenceCategory(idOfLicenceCategoryToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_LicenceCategory_ThrowsNotFoundLecturerException()
        {
            var idOfLicenceCategoryToDelete = 1;
            _licenceCategoryServiceMock.Setup(service => service.DeleteLicenceCategory(idOfLicenceCategoryToDelete)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceCategoryToDelete));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteLicenceCategory(idOfLicenceCategoryToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_LicenceCategory_ThrowsReferenceConstraintException()
        {
            var idOfLicenceCategoryToDelete = 1;
            _licenceCategoryServiceMock.Setup(service => service.DeleteLicenceCategory(idOfLicenceCategoryToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteLicenceCategory(idOfLicenceCategoryToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_LicenceCategory_ThrowsDbUpdateException()
        {
            var idOfLicenceCategoryToDelete = 1;
            _licenceCategoryServiceMock.Setup(service => service.DeleteLicenceCategory(idOfLicenceCategoryToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteLicenceCategory(idOfLicenceCategoryToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_LicenceCategory_ThrowsException()
        {
            var idOfLicenceCategoryToDelete = 1;
            _licenceCategoryServiceMock.Setup(service => service.DeleteLicenceCategory(idOfLicenceCategoryToDelete)).ThrowsAsync(new Exception());
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteLicenceCategory(idOfLicenceCategoryToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Requiremnt_ReturnNoContent()
        {
            var deletedRequirement = new RequiredLicenceCategory();
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 1;
            _requiredLicenceCategoryServiceMock.Setup(service => service.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ReturnsAsync(deletedRequirement);
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Requirement_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 1;
            _requiredLicenceCategoryServiceMock.Setup(service => service.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Requirement_ThrowsNotFoundRequirementException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 1;
            _requiredLicenceCategoryServiceMock.Setup(service => service.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ThrowsAsync(new NotFoundRequiredLicenceCategoryException());
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Requirement_ThrowsDbUpdateException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 1;
            _requiredLicenceCategoryServiceMock.Setup(service => service.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ThrowsAsync(new DbUpdateException());
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Requirement_ThrowsException()
        {
            var idOfLicenceCategory = 1;
            var idOfRequiredLicenceCategory = 1;
            _requiredLicenceCategoryServiceMock.Setup(service => service.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory)).ThrowsAsync(new Exception());
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteRequirement(idOfLicenceCategory, idOfRequiredLicenceCategory);

            result.StatusCode.Should().Be(500);
        }
    }
}