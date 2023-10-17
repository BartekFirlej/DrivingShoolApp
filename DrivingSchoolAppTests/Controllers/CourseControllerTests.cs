using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class CourseControllerTests
    {
        private Mock<ICourseService> _courseServiceMock;
        private Fixture _fixture;
        private CourseController _controller;

        public CourseControllerTests()
        {
            _fixture = new Fixture();
            _courseServiceMock = new Mock<ICourseService>();
        }

        [TestMethod]
        public async Task Get_Courses_ReturnOk()
        {
            ICollection<CourseGetDTO> coursesList = new List<CourseGetDTO>();
            _courseServiceMock.Setup(service => service.GetCourses()).Returns(Task.FromResult(coursesList));
            _controller = new CourseController(_courseServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourses();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Courses_ThrowsNotFoundCourses()
        {
            _courseServiceMock.Setup(service => service.GetCourses()).Throws(new NotFoundCoursesException());
            _controller = new CourseController(_courseServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourses();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Course_ReturnOk()
        {
            var foundCourse = new CourseGetDTO();
            var idOfCourseToFind = 1;
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseToFind)).Returns(Task.FromResult(foundCourse));
            _controller = new CourseController(_courseServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourse(idOfCourseToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Course_ThrowsNotFoundCourse()
        {
            var foundCourse = new CourseGetDTO();
            var idOfCourseToFind = 1;
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseToFind)).Throws(new NotFoundCourseException(idOfCourseToFind));
            _controller = new CourseController(_courseServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourse(idOfCourseToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Course_ReturnCreatedAtAction()
        {
            var courseToAdd = new CoursePostDTO();
            var addedCourse = new CourseGetDTO();
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).Returns(Task.FromResult(addedCourse));
            _controller = new CourseController(_courseServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Course_ThrowsNotFoundCourseType()
        {
            var courseToAdd = new CoursePostDTO();
            var idOfCourseType = 1;
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).Throws(new NotFoundCourseTypeException(idOfCourseType));
            _controller = new CourseController(_courseServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(404);
        }
    }
}