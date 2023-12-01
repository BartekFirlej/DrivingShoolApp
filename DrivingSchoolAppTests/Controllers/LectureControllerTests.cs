using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Services;
using EntityFramework.Exceptions.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DrivingSchoolAppTests.Controllers
{
    [TestClass]
    public class LectureControllerTests
    {
        private Mock<ILectureService> _lectureServiceMock;
        private Mock<ICustomerLectureService> _customerLectureServiceMock;
        private Fixture _fixture;
        private LectureController _controller;

        public LectureControllerTests()
        {
            _fixture = new Fixture();
            _lectureServiceMock = new Mock<ILectureService>();
            _customerLectureServiceMock = new Mock<ICustomerLectureService>();
        }

        [TestMethod]
        public async Task Get_Lectures_ReturnsOk()
        {
            var lecturesList = new PagedList<LectureGetDTO>();
            _lectureServiceMock.Setup(service => service.GetLectures(1, 10)).ReturnsAsync(lecturesList);
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLectures(1, 10);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Lectures_ThrowsNotFoundLectureException()
        {
            _lectureServiceMock.Setup(service => service.GetLectures(1, 10)).ThrowsAsync(new NotFoundLectureException());
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLectures(1, 10);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Lectures_ThrowsValueMustBeGreaterThanZeroException()
        {
            _lectureServiceMock.Setup(service => service.GetLectures(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetLectures(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Lecture_ReturnsOk()
        {
            var lecture = new LectureGetDTO();
            var idOfLectureToFind = 1;
            _lectureServiceMock.Setup(service => service.GetLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            _lectureServiceMock.Setup(service => service.GetLecture(idOfLectureToFind)).ThrowsAsync(new NotFoundLectureException(idOfLectureToFind));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ReturnsOk()
        {
            ICollection<CustomerLectureGetDTO> customersLecture = new List<CustomerLectureGetDTO>();
            var idOfLectureToFind = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomersLecture(idOfLectureToFind)).ReturnsAsync(customersLecture);
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomersLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomersLecture(idOfLectureToFind)).ThrowsAsync(new NotFoundLectureException(idOfLectureToFind));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomersLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ThrowsNotFoundCustomersLectureException()
        {
            var idOfLectureToFind = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomersLecture(idOfLectureToFind)).ThrowsAsync(new NotFoundCustomersLectureException(idOfLectureToFind));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomersLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ReturnsCreatedAtAction()
        {
            var lectureToAdd = new LectureRequestDTO();
            var addedLecture = new LectureResponseDTO();
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).ReturnsAsync(addedLecture);
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundCourseException()
        {
            var lectureToAdd = new LectureRequestDTO();
            var idOfCourse = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).ThrowsAsync(new NotFoundCourseException(idOfCourse));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundSubjectException()
        {
            var lectureToAdd = new LectureRequestDTO();
            var idOfSubject = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).ThrowsAsync (new NotFoundSubjectException(idOfSubject));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundClassroomException()
        {
            var lectureToAdd = new LectureRequestDTO();
            var idOfClassroom = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).ThrowsAsync(new NotFoundClassroomException(idOfClassroom));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundCourseSubjectException()
        {
            var lectureToAdd = new LectureRequestDTO();
            var idOfCourse = 1;
            var idOfSubject = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).ThrowsAsync(new NotFoundCourseSubjectException(idOfCourse, idOfSubject));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundLecturerException()
        {
            var lectureToAdd = new LectureRequestDTO();
            var idOfLecturer = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).ThrowsAsync(new NotFoundLecturerException(idOfLecturer));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsDateTimeException()
        {
            var lectureToAdd = new LectureRequestDTO();
            var nameOfProperty = "lecture date";
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).ThrowsAsync(new DateTimeException(nameOfProperty));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsSubjectLareadyConductedLectureException()
        {
            var lectureToAdd = new LectureRequestDTO();
            var idOfCourse = 1;
            var idOfSubject = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).ThrowsAsync(new SubjectAlreadyConductedLectureException(idOfCourse, idOfSubject));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(409);
        }


        [TestMethod]
        public async Task Post_CustomerLecture_ReturnsCreatedAtAction()
        {
            var customerLectureToAdd = new CustomerLectureRequestDTO();
            var addedCustomerLecture = new CustomerLectureResponseDTO();
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).ReturnsAsync(addedCustomerLecture);
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundLectureException()
        {
            var customerLectureToAdd = new CustomerLectureRequestDTO();
            var idOfLecture = 1;
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).ThrowsAsync(new NotFoundLectureException(idOfLecture));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundCustomerException()
        {
            var customerLectureToAdd = new CustomerLectureRequestDTO();
            var idOfCustomer = 1;
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).ThrowsAsync(new NotFoundCustomerException(idOfCustomer));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsCustomerAlreadyAssignedToLectureException()
        {
            var customerLectureToAdd = new CustomerLectureRequestDTO();
            var idOfCustomer = 1;
            var idOfLecture = 1;
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).ThrowsAsync(new CustomerAlreadyAssignedToLectureException(idOfCustomer, idOfLecture));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundRegistrationException()
        {
            var customerLectureToAdd = new CustomerLectureRequestDTO();
            var idOfCustomer = 1;
            var idOfCourse = 1;
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).ThrowsAsync(new NotFoundRegistrationException(idOfCustomer, idOfCourse));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Lecture_ReturnNoContent()
        {
            var deletedLecture = new Lecture();
            var idOfLectureToDelete = 1;
            _lectureServiceMock.Setup(service => service.DeleteLecture(idOfLectureToDelete)).ReturnsAsync(deletedLecture);
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteLecture(idOfLectureToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToDelete = 1;
            _lectureServiceMock.Setup(service => service.DeleteLecture(idOfLectureToDelete)).ThrowsAsync(new NotFoundLectureException(idOfLectureToDelete));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteLecture(idOfLectureToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Lecture_ThrowsReferenceConstraintException()
        {
            var idOfLectureToDelete = 1;
            _lectureServiceMock.Setup(service => service.DeleteLecture(idOfLectureToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteLecture(idOfLectureToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Lecture_ThrowsDbUpdateException()
        {
            var idOfLectureToDelete = 1;
            _lectureServiceMock.Setup(service => service.DeleteLecture(idOfLectureToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteLecture(idOfLectureToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Lecture_ThrowsException()
        {
            var idOfLectureToDelete = 1;
            _lectureServiceMock.Setup(service => service.DeleteLecture(idOfLectureToDelete)).ThrowsAsync(new Exception());
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteLecture(idOfLectureToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ReturnNoContent()
        {
            var deletedCustomerLecture = new CustomerLectureCheckDTO();
            var idOfLectureToDelete = 1;
            var idOfCustomerToDelete = 1;
            _customerLectureServiceMock.Setup(service => service.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete)).ReturnsAsync(deletedCustomerLecture);
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToDelete = 1;
            var idOfCustomerToDelete = 1;
            _customerLectureServiceMock.Setup(service => service.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete)).ThrowsAsync(new NotFoundLectureException(idOfLectureToDelete));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ThrowsNotFoundCustomerException()
        {
            var idOfLectureToDelete = 1;
            var idOfCustomerToDelete = 1;
            _customerLectureServiceMock.Setup(service => service.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete)).ThrowsAsync(new NotFoundCustomerException(idOfLectureToDelete));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ThrowsNotFoundCustomerLectureException()
        {
            var idOfLectureToDelete = 1;
            var idOfCustomerToDelete = 1;
            _customerLectureServiceMock.Setup(service => service.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete)).ThrowsAsync(new NotFoundCustomerLectureException(idOfLectureToDelete));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ThrowsDbUpdateException()
        {
            var idOfLectureToDelete = 1;
            var idOfCustomerToDelete = 1;
            _customerLectureServiceMock.Setup(service => service.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_CustomerLecture_ThrowsException()
        {
            var idOfLectureToDelete = 1;
            var idOfCustomerToDelete = 1;
            _customerLectureServiceMock.Setup(service => service.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete)).ThrowsAsync(new Exception());
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteCustomerLecture(idOfLectureToDelete, idOfCustomerToDelete);

            result.StatusCode.Should().Be(500);
        }
    }
}
