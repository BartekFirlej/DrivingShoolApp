using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
            ICollection<CourseTypeGetDTO> courseTypesList = new List<CourseTypeGetDTO>();
            _courseTypeServiceMock.Setup(service => service.GetCourseTypes()).Returns(Task.FromResult(courseTypesList));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourseTypes();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CourseTypes_ThrowsNotFoundCourseTypeException()
        {
            _courseTypeServiceMock.Setup(service => service.GetCourseTypes()).Throws(new NotFoundCourseTypeException());
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseTypes();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CourseType_ReturnsOk()
        {
            var foundCourseType = new CourseTypeGetDTO();
            var idOfCourseTypeToFind = 1;
            _courseTypeServiceMock.Setup(service => service.GetCourseType(idOfCourseTypeToFind)).Returns(Task.FromResult(foundCourseType));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourseType(idOfCourseTypeToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CourseType_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseTypeToFind = 1;
            _courseTypeServiceMock.Setup(service => service.GetCourseType(idOfCourseTypeToFind)).Throws(new NotFoundCourseTypeException(idOfCourseTypeToFind));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseType(idOfCourseTypeToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CourseType_ReturnCreatedAtAction()
        {
            var courseTypeToAdd = new CourseTypePostDTO();
            var addedCourseType = new CourseTypeGetDTO();
            _courseTypeServiceMock.Setup(service => service.PostCourseType(courseTypeToAdd)).Returns(Task.FromResult(addedCourseType));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCourseType(courseTypeToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_CourseType_ThrowsNotFoundLicenceCategoryException()
        {
            var courseTypeToAdd = new CourseTypePostDTO();
            var idOfLicenceCategory = 1;
            _courseTypeServiceMock.Setup(service => service.PostCourseType(courseTypeToAdd)).Throws(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCourseType(courseTypeToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CourseType_ThrowsValueMustBeGreaterThanZeroException()
        {
            var courseTypeToAdd = new CourseTypePostDTO();
            var nameOfProperty = "age";
            _courseTypeServiceMock.Setup(service => service.PostCourseType(courseTypeToAdd)).Throws(new ValueMustBeGreaterThanZeroException(nameOfProperty));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCourseType(courseTypeToAdd);

            result.StatusCode.Should().Be(400);
        }
    }
}