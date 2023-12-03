using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Services;
using EntityFramework.Exceptions.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class CourseTypeControllerTests
    {
        private Mock<ICourseTypeService> _courseTypeServiceMock;
        private Fixture _fixture;
        private CourseTypeController _controller;

        public CourseTypeControllerTests()
        {
            _fixture = new Fixture();
            _courseTypeServiceMock = new Mock<ICourseTypeService>();
        }

        [TestMethod]
        public async Task Get_CourseTypes_ReturnsOk()
        {
            var courseTypesList = new PagedList<CourseTypeGetDTO>();
            _courseTypeServiceMock.Setup(service => service.GetCourseTypes(1, 10)).ReturnsAsync(courseTypesList);
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourseTypes(1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CourseTypes_ThrowsNotFoundCourseTypeException()
        {
            _courseTypeServiceMock.Setup(service => service.GetCourseTypes(1, 10)).ThrowsAsync(new NotFoundCourseTypeException());
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseTypes(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CourseTypes_ThrowsValueMustBeGreaterThanZeroException()
        {
            _courseTypeServiceMock.Setup(service => service.GetCourseTypes(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetCourseTypes(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_CourseType_ReturnsOk()
        {
            var foundCourseType = new CourseTypeGetDTO();
            var idOfCourseTypeToFind = 1;
            _courseTypeServiceMock.Setup(service => service.GetCourseType(idOfCourseTypeToFind)).ReturnsAsync(foundCourseType);
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourseType(idOfCourseTypeToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CourseType_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseTypeToFind = 1;
            _courseTypeServiceMock.Setup(service => service.GetCourseType(idOfCourseTypeToFind)).ThrowsAsync(new NotFoundCourseTypeException(idOfCourseTypeToFind));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseType(idOfCourseTypeToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CourseType_ReturnCreatedAtAction()
        {
            var courseTypeToAdd = new CourseTypeRequestDTO();
            var addedCourseType = new CourseTypeResponseDTO();
            _courseTypeServiceMock.Setup(service => service.PostCourseType(courseTypeToAdd)).ReturnsAsync(addedCourseType);
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCourseType(courseTypeToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_CourseType_ThrowsNotFoundLicenceCategoryException()
        {
            var courseTypeToAdd = new CourseTypeRequestDTO();
            var idOfLicenceCategory = 1;
            _courseTypeServiceMock.Setup(service => service.PostCourseType(courseTypeToAdd)).ThrowsAsync(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCourseType(courseTypeToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CourseType_ThrowsValueMustBeGreaterThanZeroException()
        {
            var courseTypeToAdd = new CourseTypeRequestDTO();
            var nameOfProperty = "age";
            _courseTypeServiceMock.Setup(service => service.PostCourseType(courseTypeToAdd)).ThrowsAsync(new ValueMustBeGreaterThanZeroException(nameOfProperty));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCourseType(courseTypeToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Delete_CourseType_ReturnNoContent()
        {
            var deletedCourseType = new CourseType();
            var idOfCourseTypeToDelete = 1;
            _courseTypeServiceMock.Setup(service => service.DeleteCourseType(idOfCourseTypeToDelete)).ReturnsAsync(deletedCourseType);
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteCourseType(idOfCourseTypeToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_CourseType_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseTypeToDelete = 1;
            _courseTypeServiceMock.Setup(service => service.DeleteCourseType(idOfCourseTypeToDelete)).ThrowsAsync(new NotFoundCourseTypeException(idOfCourseTypeToDelete));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCourseType(idOfCourseTypeToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_CourseType_ThrowsReferenceConstraintException()
        {
            var idOfCourseTypeToDelete = 1;
            _courseTypeServiceMock.Setup(service => service.DeleteCourseType(idOfCourseTypeToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCourseType(idOfCourseTypeToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_CourseType_ThrowsDbUpdateException()
        {
            var idOfCourseTypeToDelete = 1;
            _courseTypeServiceMock.Setup(service => service.DeleteCourseType(idOfCourseTypeToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCourseType(idOfCourseTypeToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Course_ThrowsException()
        {
            var idOfCourseTypeToDelete = 1;
            _courseTypeServiceMock.Setup(service => service.DeleteCourseType(idOfCourseTypeToDelete)).ThrowsAsync(new Exception());
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCourseType(idOfCourseTypeToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Update_CourseType_ReturnOk()
        {
            var idOfCourseType = 1;
            var courseTypeUpdate = new CourseTypeRequestDTO();
            var updatedCourseType = new CourseTypeResponseDTO();
            _courseTypeServiceMock.Setup(service => service.UpdateCourseType(idOfCourseType, courseTypeUpdate)).ReturnsAsync(updatedCourseType);
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (OkObjectResult)await _controller.UpdateCourseType(idOfCourseType, courseTypeUpdate);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Update_CourseType_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseType = 1;
            var courseTypeUpdate = new CourseTypeRequestDTO();
            _courseTypeServiceMock.Setup(service => service.UpdateCourseType(idOfCourseType, courseTypeUpdate)).ThrowsAsync(new NotFoundCourseTypeException(idOfCourseType));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateCourseType(idOfCourseType, courseTypeUpdate);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Update_CourseType_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfCourseType = 1;
            var courseTypeUpdate = new CourseTypeRequestDTO();
            _courseTypeServiceMock.Setup(service => service.UpdateCourseType(idOfCourseType, courseTypeUpdate)).ThrowsAsync(new NotFoundLicenceCategoryException(1));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateCourseType(idOfCourseType, courseTypeUpdate);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Update_CourseType_ThrowsValueMustBeGreaterThanZeroException()
        {
            var idOfCourseType = 1;
            var courseTypeUpdate = new CourseTypeRequestDTO();
            var nameOfProperty = "age";
            _courseTypeServiceMock.Setup(service => service.UpdateCourseType(idOfCourseType, courseTypeUpdate)).ThrowsAsync(new ValueMustBeGreaterThanZeroException(nameOfProperty));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.UpdateCourseType(idOfCourseType, courseTypeUpdate);

            result.StatusCode.Should().Be(400);
        }
    }
}