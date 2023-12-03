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
    public class LecturerControllerTests
    {
        private Mock<ILecturerService> _lecturerServiceMock;
        private Fixture _fixture;
        private LecturerController _controller;

        public LecturerControllerTests()
        {
            _fixture = new Fixture();
            _lecturerServiceMock = new Mock<ILecturerService>();
        }

        [TestMethod]
        public async Task Get_Lecturers_ReturnsOk()
        {
            var lecturersList = new PagedList<LecturerGetDTO>();
            _lecturerServiceMock.Setup(service => service.GetLecturers(1, 10)).ReturnsAsync(lecturersList);
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLecturers(1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Lecturers_ThrowsNotFoundLecturerException()
        {
            _lecturerServiceMock.Setup(service => service.GetLecturers(1, 10)).ThrowsAsync(new NotFoundLecturerException());
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLecturers(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Lecturers_ThrowsValueMustBeGreaterThanZeroException()
        {
            _lecturerServiceMock.Setup(service => service.GetLecturers(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetLecturers(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Lecturer_ReturnsOk()
        {
            var lecturer = new LecturerGetDTO();
            var idOfLecturerToFind = 1;
            _lecturerServiceMock.Setup(service => service.GetLecturer(idOfLecturerToFind)).ReturnsAsync(lecturer);
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLecturer(idOfLecturerToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Lecturer_ThrowsNotFoundLecturerException()
        {
            var idOfLecturerToFind = 1;
            _lecturerServiceMock.Setup(service => service.GetLecturer(idOfLecturerToFind)).ThrowsAsync(new NotFoundLecturerException(idOfLecturerToFind));
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLecturer(idOfLecturerToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecturer_ReturnCreatedAtAction()
        {
            var lecturerToAdd = new LecturerRequestDTO();
            var addedLecturer = new LecturerResponseDTO();
            _lecturerServiceMock.Setup(service => service.PostLecturer(lecturerToAdd)).ReturnsAsync(addedLecturer);
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostLecturer(lecturerToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Delete_Lecturer_ReturnNoContent()
        {
            var deletedLecturer = new Lecturer();
            var idOfLecturerToDelete = 1;
            _lecturerServiceMock.Setup(service => service.DeleteLecturer(idOfLecturerToDelete)).ReturnsAsync(deletedLecturer);
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteLecturer(idOfLecturerToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Lecturer_ThrowsNotFoundLecturerException()
        {
            var idOfLecturerToDelete = 1;
            _lecturerServiceMock.Setup(service => service.DeleteLecturer(idOfLecturerToDelete)).ThrowsAsync(new NotFoundLecturerException(idOfLecturerToDelete));
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteLecturer(idOfLecturerToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Lecturer_ThrowsReferenceConstraintException()
        {
            var idOfLecturerToDelete = 1;
            _lecturerServiceMock.Setup(service => service.DeleteLecturer(idOfLecturerToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteLecturer(idOfLecturerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Lecturer_ThrowsDbUpdateException()
        {
            var idOfLecturerToDelete = 1;
            _lecturerServiceMock.Setup(service => service.DeleteLecturer(idOfLecturerToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteLecturer(idOfLecturerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Lecturer_ThrowsException()
        {
            var idOfLecturerToDelete = 1;
            _lecturerServiceMock.Setup(service => service.DeleteLecturer(idOfLecturerToDelete)).ThrowsAsync(new Exception());
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteLecturer(idOfLecturerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Update_Lecturer_ReturnNoContent()
        {
            var updateLecturer = new LecturerRequestDTO();
            var updatedLecturer = new LecturerResponseDTO();
            var idOfLecturerToUpdate = 1;
            _lecturerServiceMock.Setup(service => service.UpdateLecturer(idOfLecturerToUpdate, updateLecturer)).ReturnsAsync(updatedLecturer);
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (OkObjectResult)await _controller.UpdateLecturer(idOfLecturerToUpdate, updateLecturer);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Update_Lecturer_ThrowsNotFoundLecturerException()
        {
            var updateLecturer = new LecturerRequestDTO(); 
            var idOfLecturerToUpdate = 1;
            _lecturerServiceMock.Setup(service => service.UpdateLecturer(idOfLecturerToUpdate, updateLecturer)).ThrowsAsync(new NotFoundLecturerException(idOfLecturerToUpdate));
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.UpdateLecturer(idOfLecturerToUpdate, updateLecturer);

            result.StatusCode.Should().Be(404);
        }
    }
}