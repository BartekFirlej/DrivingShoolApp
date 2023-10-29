using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Repositories;

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
            ICollection<LicenceCategoryGetDTO> licenceCategoriesList = new List<LicenceCategoryGetDTO>();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategories()).Returns(Task.FromResult(licenceCategoriesList));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLicenceCategories();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ThrowsNotFoundLicenceCategoriesException()
        {
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategories()).Throws(new NotFoundLicenceCategoryException());
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLicenceCategories();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ReturnsOk()
        {
            var licenceCategory = new LicenceCategoryGetDTO();
            var idOfLicenceToFind = 1;
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(idOfLicenceToFind)).Returns(Task.FromResult(licenceCategory));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLicenceCategory(idOfLicenceToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_LicenceCategory_ThrowsNotFoundLicenceCategoriesException()
        {
            var idOfLicenceToFind = 1;
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(idOfLicenceToFind)).Throws(new NotFoundLicenceCategoryException(idOfLicenceToFind));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLicenceCategory(idOfLicenceToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_LicenceCategoryRequirements_ReturnsOk()
        {
            var idOfLicenceToFind = 1;
            ICollection<RequiredLicenceCategoryGetDTO> requiredLicences = new List<RequiredLicenceCategoryGetDTO>();
            _requiredLicenceCategoryServiceMock.Setup(service => service.GetRequirements(idOfLicenceToFind)).Returns(Task.FromResult(requiredLicences));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLicenceCategoryRequirements(idOfLicenceToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_LicenceCategoryRequirements_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfLicenceToFind = 1;
            _requiredLicenceCategoryServiceMock.Setup(service => service.GetRequirements(idOfLicenceToFind)).Throws(new NotFoundLicenceCategoryException(idOfLicenceToFind));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLicenceCategoryRequirements(idOfLicenceToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_LicenceCategoryRequirements_ThrowsNotFoundRequiredLicenceCategoryException()
        {
            var idOfLicenceToFind = 1;
            _requiredLicenceCategoryServiceMock.Setup(service => service.GetRequirements(idOfLicenceToFind)).Throws(new NotFoundRequiredLicenceCategoryException(idOfLicenceToFind));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLicenceCategoryRequirements(idOfLicenceToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_LicenceCategory_ReturnCreatedAtAction()
        {
            var licenceCategoryToAdd = new LicenceCategoryPostDTO();
            var addedLicenceCategory = new LicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.PostLicenceCategory(licenceCategoryToAdd)).Returns(Task.FromResult(addedLicenceCategory));
            _controller = new LicenceCategoryController(_licenceCategoryServiceMock.Object, _requiredLicenceCategoryServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostLicenceCategory(licenceCategoryToAdd);

            result.StatusCode.Should().Be(201);
        }
    }
}