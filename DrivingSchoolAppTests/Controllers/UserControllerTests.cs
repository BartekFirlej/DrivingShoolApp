using AutoFixture;
using DrivingSchool.Controllers;
using DrivingSchool.DTOs;
using DrivingSchool.Exceptions;
using DrivingSchool.Repositories;
using DrivingSchool.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace DrivingSchoolTests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IRegistrationService> _registrationServiceMock;
        private Fixture _fixture;
        private UserController _controller;

        public UserControllerTests()
        {
            _fixture = new Fixture();
            _userServiceMock = new Mock<IUserService>();
            _registrationServiceMock = new Mock<IRegistrationService>();
        }

        [TestMethod]
        public async Task Get_Users_ReturnOk()
        {
            ICollection<UserGetDTO> usersList = new List<UserGetDTO>();
            _userServiceMock.Setup(service => service.GetUsers()).Returns(Task.FromResult(usersList));
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetUsers();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Users_ThrowsNotFoundUsers()
        {
            _userServiceMock.Setup(service => service.GetUsers()).Throws(new NotFoundUsersException());
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetUsers();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_User_ReturnOk()
        {
            var foundUser = new UserGetDTO();
            var idOfUserToFind = 1;
            _userServiceMock.Setup(service => service.GetUser(idOfUserToFind)).Returns(Task.FromResult(foundUser));
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetUser(idOfUserToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_User_ThrowsNotFoundUser()
        {
            var idOfUserToFind = 1;
            _userServiceMock.Setup(service => service.GetUser(idOfUserToFind)).Throws(new NotFoundUserException(1));
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetUser(idOfUserToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_UserRegistrations_ReturnOk()
        {
            ICollection<RegistrationGetDTO> userRegistrations = new List<RegistrationGetDTO>();
            var idOfUserToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetUserRegistrations(idOfUserToFindHisRegistrations)).Returns(Task.FromResult(userRegistrations));
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetUserRegistrations(idOfUserToFindHisRegistrations);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_UserRegistrations_ThrowsNotFoundUser()
        {
            ICollection<RegistrationGetDTO> userRegistrations = new List<RegistrationGetDTO>();
            var idOfUserToFindHisRegistrations = 1;
            _registrationServiceMock.Setup(service => service.GetUserRegistrations(idOfUserToFindHisRegistrations)).Throws(new NotFoundUserRegistrationsException(1));
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetUserRegistrations(idOfUserToFindHisRegistrations);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_User_ReturnCreatedAtAction()
        {
            var userToAdd = new UserPostDTO();
            var addedUser = new UserGetDTO();
            _userServiceMock.Setup(service => service.AddUser(userToAdd)).Returns(Task.FromResult(addedUser));
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.AddUser(userToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_UserForCourse_ReturnCreatedAtAction()
        {
            var registrationToAdd = new RegistrationPostDTO();
            var addedRegistration = new RegistrationGetDTO();
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Returns(Task.FromResult(addedRegistration));
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.RegisterUserForCourse(registrationToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_UserForCourse_ThrowsNotFoundUserException()
        {
            var registrationToAdd = new RegistrationPostDTO();
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new NotFoundUserException(1));
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.RegisterUserForCourse(registrationToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_UserForCourse_ThrowsNotFoundCourseException()
        {
            var registrationToAdd = new RegistrationPostDTO();
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new NotFoundCourseException(1));
            _controller = new UserController(_userServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.RegisterUserForCourse(registrationToAdd);

            result.StatusCode.Should().Be(404);
        }
    }
}
