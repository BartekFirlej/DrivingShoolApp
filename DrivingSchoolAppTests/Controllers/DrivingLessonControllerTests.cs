using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class DrivingLessonControllerTests
    {
        private Mock<IDrivingLessonService> _drivingLessonServiceMock;
        private Fixture _fixture;
        private DrivingLessonController _controller;

        public DrivingLessonControllerTests()
        {
            _fixture = new Fixture();
            _drivingLessonServiceMock = new Mock<IDrivingLessonService>();
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ReturnsOk()
        {
            var drivingLessonsList = new PagedList<DrivingLessonGetDTO>();
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLessons(1, 10)).Returns(Task.FromResult(drivingLessonsList));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetDrivingLessons(1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ThrowsNotFoundDrivingLessonsException()
        {
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLessons(1, 10)).Throws(new NotFoundDrivingLessonException());
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetDrivingLessons(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ThrowsValueMustBeGreaterThanZeroException()
        {
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLessons(-1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetDrivingLessons(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsOk()
        {
            var foundDrivingLesson = new DrivingLessonGetDTO();
            var idOfDrivingLesson = 1;
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLesson(idOfDrivingLesson)).Returns(Task.FromResult(foundDrivingLesson));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetDrivingLesson(idOfDrivingLesson);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLesson = 1;
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLesson(idOfDrivingLesson)).Throws(new NotFoundDrivingLessonException(idOfDrivingLesson));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetDrivingLesson(idOfDrivingLesson);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ReturnsCreatedAtAction()
        {
            var drivingLessonToAdd = new DrivingLessonPostDTO();
            var addedDrivingLesson = new DrivingLessonGetDTO();
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).Returns(Task.FromResult(addedDrivingLesson));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundCustomerException()
        {
            var drivingLessonToAdd = new DrivingLessonPostDTO();
            var idOfCustomer = 1;
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).Throws(new NotFoundCustomerException(idOfCustomer));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundLecturerException()
        {
            var drivingLessonToAdd = new DrivingLessonPostDTO();
            var idOfLecturer = 1;
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).Throws(new NotFoundLecturerException(idOfLecturer));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundAddressException()
        {
            var drivingLessonToAdd = new DrivingLessonPostDTO();
            var idOfAddress = 1;
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).Throws(new NotFoundAddressException(idOfAddress));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundCourseException()
        {
            var drivingLessonToAdd = new DrivingLessonPostDTO();
            var idOfCourse = 1;
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).Throws(new NotFoundCourseException(idOfCourse));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsDateTimeException()
        {
            var drivingLessonToAdd = new DrivingLessonPostDTO();
            var propertyName = "lesson date";
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).Throws(new DateTimeException(propertyName));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(400);
        }
    }
}