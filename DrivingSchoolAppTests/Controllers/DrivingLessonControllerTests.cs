using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using DrivingSchoolApp.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

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
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLessons(1, 10)).ReturnsAsync(drivingLessonsList);
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetDrivingLessons(1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ThrowsNotFoundDrivingLessonsException()
        {
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLessons(1, 10)).ThrowsAsync(new NotFoundDrivingLessonException());
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetDrivingLessons(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_DrivingLessons_ThrowsValueMustBeGreaterThanZeroException()
        {
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLessons(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetDrivingLessons(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ReturnsOk()
        {
            var foundDrivingLesson = new DrivingLessonGetDTO();
            var idOfDrivingLesson = 1;
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLesson(idOfDrivingLesson)).ReturnsAsync(foundDrivingLesson);
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetDrivingLesson(idOfDrivingLesson);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_DrivingLesson_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLesson = 1;
            _drivingLessonServiceMock.Setup(service => service.GetDrivingLesson(idOfDrivingLesson)).ThrowsAsync(new NotFoundDrivingLessonException(idOfDrivingLesson));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetDrivingLesson(idOfDrivingLesson);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ReturnsCreatedAtAction()
        {
            var drivingLessonToAdd = new DrivingLessonRequestDTO();
            var addedDrivingLesson = new DrivingLessonResponseDTO();
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).ReturnsAsync(addedDrivingLesson);
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundCustomerException()
        {
            var drivingLessonToAdd = new DrivingLessonRequestDTO();
            var idOfCustomer = 1;
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).ThrowsAsync(new NotFoundCustomerException(idOfCustomer));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundLecturerException()
        {
            var drivingLessonToAdd = new DrivingLessonRequestDTO();
            var idOfLecturer = 1;
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).ThrowsAsync(new NotFoundLecturerException(idOfLecturer));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundAddressException()
        {
            var drivingLessonToAdd = new DrivingLessonRequestDTO();
            var idOfAddress = 1;
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).ThrowsAsync(new NotFoundAddressException(idOfAddress));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsNotFoundCourseException()
        {
            var drivingLessonToAdd = new DrivingLessonRequestDTO();
            var idOfCourse = 1;
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).ThrowsAsync(new NotFoundCourseException(idOfCourse));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_DrivingLesson_ThrowsDateTimeException()
        {
            var drivingLessonToAdd = new DrivingLessonRequestDTO();
            var propertyName = "lesson date";
            _drivingLessonServiceMock.Setup(service => service.PostDrivingLesson(drivingLessonToAdd)).ThrowsAsync(new DateTimeException(propertyName));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostDrivingLesson(drivingLessonToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_ReturnsNoContent()
        {
            var idOfDrivingLesson = 5;
            var deletedDrivingLesson = new DrivingLesson();
            _drivingLessonServiceMock.Setup(service => service.DeleteDrivingLesson(idOfDrivingLesson)).ReturnsAsync(deletedDrivingLesson);
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteDrivingLesson(idOfDrivingLesson);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLessonToDelete = 5;
            _drivingLessonServiceMock.Setup(service => service.DeleteDrivingLesson(idOfDrivingLessonToDelete)).ThrowsAsync(new NotFoundDrivingLessonException(idOfDrivingLessonToDelete));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteDrivingLesson(idOfDrivingLessonToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_ThrowsDbUpdateException()
        {
            var idOfDrivingLessonToDelete = 1;
            _drivingLessonServiceMock.Setup(service => service.DeleteDrivingLesson(idOfDrivingLessonToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteDrivingLesson(idOfDrivingLessonToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_DrivingLesson_ThrowsException()
        {
            var idOfDrivingLessonToDelete = 1;
            _drivingLessonServiceMock.Setup(service => service.DeleteDrivingLesson(idOfDrivingLessonToDelete)).ThrowsAsync(new Exception());
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteDrivingLesson(idOfDrivingLessonToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ReturnsOk()
        {
            var idOfDrivingLesson = 1;
            var drivingLessonUpdate = new DrivingLessonRequestDTO();
            var updatedDrivingLesson = new DrivingLessonResponseDTO();
            _drivingLessonServiceMock.Setup(service => service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate)).ReturnsAsync(updatedDrivingLesson);
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (OkObjectResult)await _controller.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundDrivingLessonException()
        {
            var idOfDrivingLesson = 1;
            var drivingLessonUpdate = new DrivingLessonRequestDTO();
            _drivingLessonServiceMock.Setup(service => service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate)).ThrowsAsync(new NotFoundDrivingLessonException(idOfDrivingLesson));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundCustomerException()
        {
            var idOfDrivingLesson = 1;
            var drivingLessonUpdate = new DrivingLessonRequestDTO();
            _drivingLessonServiceMock.Setup(service => service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate)).ThrowsAsync(new NotFoundCustomerException(1));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundLecturerException()
        {
            var idOfDrivingLesson = 1;
            var drivingLessonUpdate = new DrivingLessonRequestDTO();
            _drivingLessonServiceMock.Setup(service => service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate)).ThrowsAsync(new NotFoundLecturerException(1));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundAddressException()
        {
            var idOfDrivingLesson = 1;
            var drivingLessonUpdate = new DrivingLessonRequestDTO();
            _drivingLessonServiceMock.Setup(service => service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate)).ThrowsAsync(new NotFoundAddressException(1));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsNotFoundCourseException()
        {
            var idOfDrivingLesson = 1;
            var drivingLessonUpdate = new DrivingLessonRequestDTO();
            _drivingLessonServiceMock.Setup(service => service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate)).ThrowsAsync(new NotFoundCourseException(1));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Update_DrivingLesson_ThrowsDateTimeException()
        {

            var idOfDrivingLesson = 1;
            var drivingLessonUpdate = new DrivingLessonRequestDTO();
            _drivingLessonServiceMock.Setup(service => service.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate)).ThrowsAsync(new DateTimeException("lesson date"));
            _controller = new DrivingLessonController(_drivingLessonServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.UpdateDrivingLesson(idOfDrivingLesson, drivingLessonUpdate);

            result.StatusCode.Should().Be(400);
        }
    }
}