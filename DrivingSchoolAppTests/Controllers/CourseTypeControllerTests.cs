using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public async Task Get_CourseTypes_ReturnOk()
        {
            ICollection<CourseTypeGetDTO> courseTypesList = new List<CourseTypeGetDTO>();
            _courseTypeServiceMock.Setup(service => service.GetCourseTypes()).Returns(Task.FromResult(courseTypesList));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourseTypes();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CourseTypes_ThrowsNotFoundCourseTypes()
        {
            _courseTypeServiceMock.Setup(service => service.GetCourseTypes()).Throws(new NotFoundCourseTypesException());
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseTypes();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CourseType_ReturnOk()
        {
            var foundCourseType = new CourseTypeGetDTO();
            var idOfCourseTypeToFind = 1;
            _courseTypeServiceMock.Setup(service => service.GetCourseType(idOfCourseTypeToFind)).Returns(Task.FromResult(foundCourseType));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourseType(idOfCourseTypeToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CourseType_ThrowsNotFoundCourseType()
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
        public async Task Post_CourseType_ThrowsNotFoundLicenceCategory()
        {
            var courseTypeToAdd = new CourseTypePostDTO();
            var idOfLicenceCategory = 1;
            _courseTypeServiceMock.Setup(service => service.PostCourseType(courseTypeToAdd)).Throws(new NotFoundLicenceCategoryException(idOfLicenceCategory));
            _controller = new CourseTypeController(_courseTypeServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCourseType(courseTypeToAdd);

            result.StatusCode.Should().Be(404);
        }
    }
}