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
    public class ClassroomControllerTests
    {
        private Mock<IClassroomService> _classroomServiceMock;
        private Fixture _fixture;
        private ClassroomController _controller;

        public ClassroomControllerTests()
        {
            _fixture = new Fixture();
            _classroomServiceMock = new Mock<IClassroomService>();
        }

        [TestMethod]
        public async Task Get_Classrooms_ReturnsOk()
        {
            PagedList<ClassroomGetDTO> classroomsList = new PagedList<ClassroomGetDTO>();
            _classroomServiceMock.Setup(service => service.GetClassrooms(1,10)).ReturnsAsync(classroomsList);
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetClassrooms(1,10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Classrooms_ThrowsNotFoundClassroomException()
        {
            _classroomServiceMock.Setup(service => service.GetClassrooms(1,10)).ThrowsAsync(new NotFoundClassroomException());
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetClassrooms();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Classrooms_ThrowsValueMustBeGreaterThanZeroException()
        {
            _classroomServiceMock.Setup(service => service.GetClassrooms(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetClassrooms(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Classroom_ReturnsOk()
        {
            var classroom = new ClassroomGetDTO();
            var idOfClassroomToFind = 1;
            _classroomServiceMock.Setup(service => service.GetClassroom(idOfClassroomToFind)).ReturnsAsync(classroom);
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetClassroom(idOfClassroomToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroomToFind = 1;
            _classroomServiceMock.Setup(service => service.GetClassroom(idOfClassroomToFind)).ThrowsAsync(new NotFoundClassroomException(idOfClassroomToFind));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetClassroom(idOfClassroomToFind);


            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Classroom_ReturnCreatedAtAction()
        {
            var classroomToAdd = new ClassroomPostDTO();
            var addedClassroom = new ClassroomGetDTO();
            _classroomServiceMock.Setup(service => service.PostClassroom(classroomToAdd)).ReturnsAsync(addedClassroom);
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostClassroom(classroomToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNotFoundAddressException()
        {
            var classroomToAdd = new ClassroomPostDTO();
            var addedClassroom = new ClassroomGetDTO();
            var idOfAddress = 1;
            _classroomServiceMock.Setup(service => service.PostClassroom(classroomToAdd)).ThrowsAsync(new NotFoundAddressException(idOfAddress));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostClassroom(classroomToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsValueMustBeGreaterThanZeroException()
        {
            var classroomToAdd = new ClassroomPostDTO();
            var addedClassroom = new ClassroomGetDTO();
            var nameOfProperty = "number";
            _classroomServiceMock.Setup(service => service.PostClassroom(classroomToAdd)).ThrowsAsync(new ValueMustBeGreaterThanZeroException(nameOfProperty));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostClassroom(classroomToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Delete_Classroom_ReturnNoContent()
        {
            var deletedClassroom = new Classroom();
            var idOfClassroomToDelete = 1;
            _classroomServiceMock.Setup(service => service.DeleteClassroom(idOfClassroomToDelete)).ReturnsAsync(deletedClassroom);
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteClassroom(idOfClassroomToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroomToDelete = 1;
            _classroomServiceMock.Setup(service => service.DeleteClassroom(idOfClassroomToDelete)).ThrowsAsync(new NotFoundClassroomException(idOfClassroomToDelete));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteClassroom(idOfClassroomToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Classroom_ThrowsReferenceConstraintException()
        {
            var idOfClassroomToDelete = 1;
            _classroomServiceMock.Setup(service => service.DeleteClassroom(idOfClassroomToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteClassroom(idOfClassroomToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Address_ThrowsDbUpdateException()
        {
            var idOfClassroomToDelete = 1;
            _classroomServiceMock.Setup(service => service.DeleteClassroom(idOfClassroomToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteClassroom(idOfClassroomToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Address_ThrowsException()
        {
            var idOfClassroomToDelete = 1;
            _classroomServiceMock.Setup(service => service.DeleteClassroom(idOfClassroomToDelete)).ThrowsAsync(new Exception());
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteClassroom(idOfClassroomToDelete);

            result.StatusCode.Should().Be(500);
        }
    }
}