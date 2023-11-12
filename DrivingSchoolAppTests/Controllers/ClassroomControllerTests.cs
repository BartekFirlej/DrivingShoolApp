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
            _classroomServiceMock.Setup(service => service.GetClassrooms(1,10)).Returns(Task.FromResult(classroomsList));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetClassrooms(1,10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Classrooms_ThrowsNotFoundClassroomException()
        {
            _classroomServiceMock.Setup(service => service.GetClassrooms(1,10)).Throws(new NotFoundClassroomException());
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetClassrooms();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Classrooms_ThrowsValueMustBeGreaterThanZeroException()
        {
            _classroomServiceMock.Setup(service => service.GetClassrooms(-1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetClassrooms(-1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Classroom_ReturnsOk()
        {
            var classroom = new ClassroomGetDTO();
            var idOfClassroomToFind = 1;
            _classroomServiceMock.Setup(service => service.GetClassroom(idOfClassroomToFind)).Returns(Task.FromResult(classroom));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetClassroom(idOfClassroomToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroomToFind = 1;
            _classroomServiceMock.Setup(service => service.GetClassroom(idOfClassroomToFind)).Throws(new NotFoundClassroomException(idOfClassroomToFind));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetClassroom(idOfClassroomToFind);


            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Classroom_ReturnCreatedAtAction()
        {
            var classroomToAdd = new ClassroomPostDTO();
            var addedClassroom = new ClassroomGetDTO();
            _classroomServiceMock.Setup(service => service.PostClassroom(classroomToAdd)).Returns(Task.FromResult(addedClassroom));
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
            _classroomServiceMock.Setup(service => service.PostClassroom(classroomToAdd)).Throws(new NotFoundAddressException(idOfAddress));
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
            _classroomServiceMock.Setup(service => service.PostClassroom(classroomToAdd)).Throws(new ValueMustBeGreaterThanZeroException(nameOfProperty));
            _controller = new ClassroomController(_classroomServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostClassroom(classroomToAdd);

            result.StatusCode.Should().Be(400);
        }
    }
}