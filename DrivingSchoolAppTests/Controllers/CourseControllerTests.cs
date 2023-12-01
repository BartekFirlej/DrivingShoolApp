using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Models;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class CourseControllerTests
    {
        private Mock<ICourseService> _courseServiceMock;
        private Mock<ICourseSubjectService> _courseSubjectServiceMock;
        private Mock<IRegistrationService> _registrationServiceMock;
        private Fixture _fixture;
        private CourseController _controller;

        public CourseControllerTests()
        {
            _fixture = new Fixture();
            _courseServiceMock = new Mock<ICourseService>();
            _courseSubjectServiceMock = new Mock<ICourseSubjectService>();
            _registrationServiceMock = new Mock<IRegistrationService>();
        }

        [TestMethod]
        public async Task Get_Courses_ReturnsOk()
        {
            var coursesList = new PagedList<CourseGetDTO>();
            _courseServiceMock.Setup(service => service.GetCourses(1, 10)).ReturnsAsync(coursesList);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourses(1,10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Courses_ThrowsNotFoundCourseException()
        {
            _courseServiceMock.Setup(service => service.GetCourses(1,10)).ThrowsAsync(new NotFoundCourseException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourses(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Courses_ThrowsValueMustBeGreaterThanZeroException()
        {
            _courseServiceMock.Setup(service => service.GetCourses(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetCourses(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Course_ReturnsOk()
        {
            var foundCourse = new CourseGetDTO();
            var idOfCourseToFind = 1;
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseToFind)).ReturnsAsync(foundCourse);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourse(idOfCourseToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Course_ThrowsNotFoundCourseException()
        {
            var foundCourse = new CourseGetDTO();
            var idOfCourseToFind = 1;
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseToFind)).ThrowsAsync(new NotFoundCourseException(idOfCourseToFind));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourse(idOfCourseToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_ReturnsOk()
        {
            var coursRegistrations = new PagedList<RegistrationGetDTO>();
            var idOfCourseToFind = 1;
            _registrationServiceMock.Setup(service => service.GetCourseRegistrations(idOfCourseToFind, 1, 10)).ReturnsAsync(coursRegistrations);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourseRegistrations(idOfCourseToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_NotFoundRegistrationException()
        {
            var idOfCourseToFind = 1;
            _registrationServiceMock.Setup(service => service.GetCourseRegistrations(idOfCourseToFind, 1 ,10)).ThrowsAsync(new NotFoundRegistrationException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseRegistrations(idOfCourseToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_NotFoundCourseException()
        {
            ICollection<RegistrationGetDTO> coursRegistrations = new List<RegistrationGetDTO>();
            var idOfCourseToFind = 1;
            _registrationServiceMock.Setup(service => service.GetCourseRegistrations(idOfCourseToFind, 1, 10)).ThrowsAsync(new NotFoundCourseException(idOfCourseToFind));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseRegistrations(idOfCourseToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CourseRegistrations_ThrowsValueMustBeGreaterThanZeroException()
        {
            var idOfCourseToFind = 1;
            _registrationServiceMock.Setup(service => service.GetCourseRegistrations(idOfCourseToFind, -1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetCourseRegistrations(idOfCourseToFind, - 1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_CourseSubjects_ReturnsOk()
        {
            var courseSubjects = new CourseSubjectsGetDTO();
            var idOfCourseToFind = 1;
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubjects(idOfCourseToFind)).ReturnsAsync(courseSubjects);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCourseSubjects(idOfCourseToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CourseSubjects_NotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubjects(idOfCourseToFind)).ThrowsAsync(new NotFoundCourseException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseSubjects(idOfCourseToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CourseSubjects_NotFoundCourseSubjectException()
        {
            var idOfCourseToFind = 1;
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubjects(idOfCourseToFind)).ThrowsAsync(new NotFoundCourseSubjectException(idOfCourseToFind));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCourseSubjects(idOfCourseToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Course_ReturnCreatedAtAction()
        {
            var courseToAdd = new CourseRequestDTO();
            var addedCourse = new CourseResponseDTO();
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).ReturnsAsync(addedCourse);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Course_ThrowsNotFoundCourseTypeException()
        {
            var courseToAdd = new CourseRequestDTO();
            var idOfCourseType = 1;
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).ThrowsAsync(new NotFoundCourseTypeException(idOfCourseType));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Course_ThrowsLimitValueExceptionException()
        {
            var courseToAdd = new CourseRequestDTO();
            var incorrectProperty = "limit";
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).ThrowsAsync(new ValueMustBeGreaterThanZeroException(incorrectProperty));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_Course_ThrowsDateTimeExceptionException()
        {
            var courseToAdd = new CourseRequestDTO();
            _courseServiceMock.Setup(service => service.PostCourse(courseToAdd)).ThrowsAsync(new DateTimeException("begin date"));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCourse(courseToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_CourseSubject_ReturnCreatedAtAction()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            _courseSubjectServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).ReturnsAsync(addedCourseSubject);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_CourseSubject_NotFoundCourseException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            var idOfCourse = 1;
            _courseSubjectServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).ThrowsAsync(new NotFoundCourseException(idOfCourse));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CourseSubject_NotFoundSubjectException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            var idOfSubject = 1;
            _courseSubjectServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).ThrowsAsync(new NotFoundSubjectException(idOfSubject));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

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
            _courseSubjectServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).ThrowsAsync(new SubjectAlreadyAssignedToCourseException(idOfCourse,idOfSubject));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

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
            _courseSubjectServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).ThrowsAsync(new TakenSequenceNumberException(idOfCourse, seqNumber));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Post_CourseSubject_SeqNumberNotGreaterThanZeroException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO();
            var addedCourseSubject = new CourseSubjectGetDTO();
            var propertyName = "sequence number";
            _courseSubjectServiceMock.Setup(service => service.PostCourseSubject(courseSubjectToAdd)).ThrowsAsync(new ValueMustBeGreaterThanZeroException(propertyName));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostCourseSubject(courseSubjectToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ReturnsCreatedAtAction()
        {
            var registrationToAdd = new RegistrationRequestDTO();
            var addedRegistration = new RegistrationResponseDTO();
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).ReturnsAsync(addedRegistration);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsNotFoundCustomerException()
        {
            var registrationToAdd = new RegistrationRequestDTO();
            var idOfCustomer = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new NotFoundCustomerException(idOfCustomer));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsNotFoundCourseException()
        {
            var registrationToAdd = new RegistrationRequestDTO();
            var idOfCourse = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new NotFoundCourseException(idOfCourse));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsCustomerAlreadyAssignedToCourseException()
        {
            var registrationToAdd = new RegistrationRequestDTO();
            var idOfCourse = 1;
            var idOfCustomer = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new CustomerAlreadyAssignedToCourseException(idOfCustomer, idOfCourse));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsCustomerDoesntMeetRequirementsException()
        {
            var registrationToAdd = new RegistrationRequestDTO();
            var idOfLicenceCategory = 1;
            var idOfCustomer = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new CustomerDoesntMeetRequirementsException(idOfCustomer, idOfLicenceCategory));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Register_CustomerForCourse_ThrowsAssignLimitReachedException()
        {
            var registrationToAdd = new RegistrationRequestDTO();
            var idOfCourse = 1;
            _registrationServiceMock.Setup(service => service.PostRegistration(registrationToAdd)).Throws(new AssignLimitReachedException(idOfCourse));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.RegisterCustomerForCourse(registrationToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Delete_Course_ReturnNoContent()
        {
            var deletedCourse = new Course();
            var idOfCourseToDelete = 1;
            _courseServiceMock.Setup(service => service.DeleteCourse(idOfCourseToDelete)).ReturnsAsync(deletedCourse);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteCourse(idOfCourseToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Course_ThrowsNotFoundCourseException()
        {
            var idOfCourseToDelete = 1;
            _courseServiceMock.Setup(service => service.DeleteCourse(idOfCourseToDelete)).ThrowsAsync(new NotFoundCourseException(idOfCourseToDelete));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCourse(idOfCourseToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Course_ThrowsReferenceConstraintException()
        {
            var idOfCourseToDelete = 1;
            _courseServiceMock.Setup(service => service.DeleteCourse(idOfCourseToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCourse(idOfCourseToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Course_ThrowsDbUpdateException()
        {
            var idOfCourseToDelete = 1; 
            _courseServiceMock.Setup(service => service.DeleteCourse(idOfCourseToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCourse(idOfCourseToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Course_ThrowsException()
        {
            var idOfCourseToDelete = 1;
            _courseServiceMock.Setup(service => service.DeleteCourse(idOfCourseToDelete)).ThrowsAsync(new Exception());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCourse(idOfCourseToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ReturnNoContent()
        {
            var deletedCourseSubject = new CourseSubject();
            var idOfCourseToDelete = 1;
            var idOfSubjectToDelete = 1;
            _courseSubjectServiceMock.Setup(service => service.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete)).ReturnsAsync(deletedCourseSubject);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ThrowsNotFoundCourseException()
        {
            var idOfCourseToDelete = 1;
            var idOfSubjectToDelete = 1;
            _courseSubjectServiceMock.Setup(service => service.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete)).ThrowsAsync(new NotFoundCourseException(idOfCourseToDelete));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var idOfCourseToDelete = 1;
            var idOfSubjectToDelete = 1;
            _courseSubjectServiceMock.Setup(service => service.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete)).ThrowsAsync(new NotFoundSubjectException(idOfSubjectToDelete));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ThrowsNotFoundCourseSubjectException()
        {
            var idOfCourseToDelete = 1;
            var idOfSubjectToDelete = 1;
            _courseSubjectServiceMock.Setup(service => service.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete)).ThrowsAsync(new NotFoundCourseSubjectException(idOfCourseToDelete, idOfSubjectToDelete));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ThrowsReferenceConstraintException()
        {
            var idOfCourseToDelete = 1;
            var idOfSubjectToDelete = 1;
            _courseSubjectServiceMock.Setup(service => service.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ThrowsDbUpdateException()
        {
            var idOfCourseToDelete = 1;
            var idOfSubjectToDelete = 1;
            _courseSubjectServiceMock.Setup(service => service.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ThrowsException()
        {
            var idOfCourseToDelete = 1;
            var idOfSubjectToDelete = 1;
            _courseSubjectServiceMock.Setup(service => service.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete)).ThrowsAsync(new Exception());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCourseSubject(idOfCourseToDelete, idOfSubjectToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Registration_ReturnNoContent()
        {
            var deletedRegistration = new Registration();
            var idOfCourseToDelete = 1;
            var idOfCustomerToDelete = 1;
            _registrationServiceMock.Setup(service => service.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete)).ReturnsAsync(deletedRegistration);
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Registration_ThrowsNotFoundCourseException()
        {
            var idOfCourseToDelete = 1;
            var idOfCustomerToDelete = 1;
            _registrationServiceMock.Setup(service => service.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete)).ThrowsAsync(new NotFoundCourseException(idOfCourseToDelete));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Registration_ThrowsNotFoundCustomerException()
        {
            var idOfCourseToDelete = 1;
            var idOfCustomerToDelete = 1;
            _registrationServiceMock.Setup(service => service.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete)).ThrowsAsync(new NotFoundCustomerException(idOfCustomerToDelete));
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Registration_ThrowsNotFoundRegistrationException()
        {
            var idOfCourseToDelete = 1;
            var idOfCustomerToDelete = 1;
            _registrationServiceMock.Setup(service => service.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete)).ThrowsAsync(new NotFoundRegistrationException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Registration_ThrowsDbUpdateException()
        {
            var idOfCourseToDelete = 1;
            var idOfCustomerToDelete = 1;
            _registrationServiceMock.Setup(service => service.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Registration_ThrowsException()
        {
            var idOfCourseToDelete = 1;
            var idOfCustomerToDelete = 1;
            _registrationServiceMock.Setup(service => service.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete)).ThrowsAsync(new Exception());
            _controller = new CourseController(_courseServiceMock.Object, _courseSubjectServiceMock.Object, _registrationServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteRegistration(idOfCourseToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }
    }
}