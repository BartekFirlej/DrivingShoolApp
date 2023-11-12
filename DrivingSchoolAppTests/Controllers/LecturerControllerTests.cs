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
            ICollection<LecturerGetDTO> lecturersList = new List<LecturerGetDTO>();
            _lecturerServiceMock.Setup(service => service.GetLecturers()).Returns(Task.FromResult(lecturersList));
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLecturers();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Lecturers_ThrowsNotFoundLecturerException()
        {
            _lecturerServiceMock.Setup(service => service.GetLecturers()).Throws(new NotFoundLecturerException());
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLecturers();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Lecturer_ReturnsOk()
        {
            var lecturer = new LecturerGetDTO();
            var idOfLecturerToFind = 1;
            _lecturerServiceMock.Setup(service => service.GetLecturer(idOfLecturerToFind)).Returns(Task.FromResult(lecturer));
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLecturer(idOfLecturerToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Lecturer_ThrowsNotFoundLecturerException()
        {
            var idOfLecturerToFind = 1;
            _lecturerServiceMock.Setup(service => service.GetLecturer(idOfLecturerToFind)).Throws(new NotFoundLecturerException(idOfLecturerToFind));
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLecturer(idOfLecturerToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecturer_ReturnCreatedAtAction()
        {
            var lecturerToAdd = new LecturerPostDTO();
            var addedLecturer = new LecturerGetDTO();
            _lecturerServiceMock.Setup(service => service.PostLecturer(lecturerToAdd)).Returns(Task.FromResult(addedLecturer));
            _controller = new LecturerController(_lecturerServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostLecturer(lecturerToAdd);

            result.StatusCode.Should().Be(201);
        }
    }
}