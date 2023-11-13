using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class CourseControllerTests
    {
        private Mock<ICourseService> _courseServiceMock;
        private Mock<ICourseSubjectService> _courseSubecjtServiceMock;
        private Mock<IRegistrationService> _registrationServiceMock;
        private Fixture _fixture;
        private CourseController _controller;

        public CourseControllerTests()
        {
            _fixture = new Fixture();
            _courseServiceMock = new Mock<ICourseService>();
            _courseSubecjtServiceMock = new Mock<ICourseSubjectService>();
            _registrationServiceMock = new Mock<IRegistrationService>();
        }

        [TestMethod]
        public async Task Get_Courses_ReturnsOk()
        {
            var coursesList = new PagedList<CourseGetDTO>();
            _courseServiceMock.Setup(service => service.GetCourses(1, 10)).Returns(Task.FromResult(coursesList));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourses(1,10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Courses_ThrowsNotFoundCourseException()
        {
            _courseServiceMock.Setup(service => service.GetCourses(1,10)).Throws(new NotFoundCourseException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourses(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Courses_ThrowsValueMustBeGreaterThanZeroException()
        {
            _courseServiceMock.Setup(service => service.GetCourses(-1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetCourses(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Course_ReturnsOk()
        {
            var foundCourse = new CourseGetDTO();
            var idOfCourseToFind = 1;
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseToFind)).Returns(Task.FromResult(foundCourse));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourse(idOfCourseToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Course_ThrowsNotFoundCourseException()
        {
            var foundCourse = new CourseGetDTO();
            var idOfCourseToFind = 1;
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseToFind)).Throws(new NotFoundCourseException(idOfCourseToFind));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourse(idOfCourseToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_ReturnsOk()
        {
            ICollection<RegistrationGetDTO> coursRegistrations = new List<RegistrationGetDTO>();
            var idOfCourseToFind = 1;
            _registrationServiceMock.Setup(service => service.GetCourseRegistrations(idOfCourseToFind)).Returns(Task.FromResult(coursRegistrations));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourseRegistrations(idOfCourseToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_NotFoundRegistrationException()
        {
            ICollection<RegistrationGetDTO> coursRegistrations = new List<RegistrationGetDTO>();
            var idOfCourseToFind = 1;
            _registrationServiceMock.Setup(service => service.GetCourseRegistrations(idOfCourseToFind)).Throws(new NotFoundRegistrationException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseRegistrations(idOfCourseToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_NotFoundCourseException()
        {
            ICollection<RegistrationGetDTO> coursRegistrations = new List<RegistrationGetDTO>();
            var idOfCourseToFind = 1;
            _registrationServiceMock.Setup(service => service.GetCourseRegistrations(idOfCourseToFind)).Throws(new NotFoundCourseException(idOfCourseToFind));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseRegistrations(idOfCourseToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Course_ReturnCreatedAtAction()
        {
            var courseToAdd = new CoursePostDTO();
            var addedCourse = new CourseGetDTO();
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).Returns(Task.FromResult(addedCourse));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Course_ThrowsNotFoundCourseTypeException()
        {
            var courseToAdd = new CoursePostDTO();
            var idOfCourseType = 1;
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).Throws(new NotFoundCourseTypeException(idOfCourseType));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Course_ThrowsLimitValueExceptionException()
        {
            var courseToAdd = new CoursePostDTO();
            var incorrectProperty = "limit";
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).Throws(new ValueMustBeGreaterThanZeroException(incorrectProperty));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_Course_ThrowsDateTimeExceptionException()
        {
            var courseToAdd = new CoursePostDTO();
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).Throws(new DateTimeException("begin date"));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_CourseSubject_ReturnCreatedAtAction()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            _courseSubecjtServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).Returns(Task.FromResult(addedCourseSubject));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_CourseSubject_NotFoundCourseException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            var idOfCourse = 1;
            _courseSubecjtServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).Throws(new NotFoundCourseException(idOfCourse));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CourseSubject_NotFoundSubjectException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            var idOfSubject = 1;
            _courseSubecjtServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).Throws(new NotFoundSubjectException(idOfSubject));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CourseSubject_SubjectAlreadyAssigenedToCourseException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            var idOfSubject = 1;
            var idOfCourse = 1;
            _courseSubecjtServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).Throws(new SubjectAlreadyAssignedToCourseException(idOfCourse,idOfSubject));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Post_CourseSubject_TakenSequenceNumber()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            var seqNumber = 1;
            var idOfCourse = 1;
            _courseSubecjtServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).Throws(new TakenSequenceNumberException(idOfCourse, seqNumber));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Post_CourseSubject_SeqNumberNotGreaterThanZeroException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            var propertyName = "sequence number";
            _courseSubecjtServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).Throws(new ValueMustBeGreaterThanZeroException(propertyName));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubecjtServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(400);
        }
    }
}