using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
            ICollection<LectureGetDTO> lecturesList = new List<LectureGetDTO>();
            _lectureServiceMock.Setup(service => service.GetLectures()).Returns(Task.FromResult(lecturesList));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLectures();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Lectures_ThrowsNotFoundLectureException()
        {
            _lectureServiceMock.Setup(service => service.GetLectures()).Throws(new NotFoundLectureException());
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLectures();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Lecture_ReturnsOk()
        {
            var lecture = new LectureGetDTO();
            var idOfLectureToFind = 1;
            _lectureServiceMock.Setup(service => service.GetLecture(idOfLectureToFind)).Returns(Task.FromResult(lecture));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            _lectureServiceMock.Setup(service => service.GetLecture(idOfLectureToFind)).Throws(new NotFoundLectureException(idOfLectureToFind));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ReturnsOk()
        {
            ICollection<CustomerLectureGetDTO> customersLecture = new List<CustomerLectureGetDTO>();
            var idOfLectureToFind = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomersLecture(idOfLectureToFind)).Returns(Task.FromResult(customersLecture));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetCustomersLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomersLecture(idOfLectureToFind)).Throws(new NotFoundLectureException(idOfLectureToFind));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomersLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_CustomersLecture_ThrowsNotFoundCustomersLectureException()
        {
            var idOfLectureToFind = 1;
            _customerLectureServiceMock.Setup(service => service.GetCustomersLecture(idOfLectureToFind)).Throws(new NotFoundCustomersLectureException(idOfLectureToFind));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetCustomersLecture(idOfLectureToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ReturnsCreatedAtAction()
        {
            var lectureToAdd = new LecturePostDTO();
            var addedLecture = new LectureGetDTO();
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).Returns(Task.FromResult(addedLecture));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundCourseException()
        {
            var lectureToAdd = new LecturePostDTO();
            var idOfCourse = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).Throws(new NotFoundCourseException(idOfCourse));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundSubjectException()
        {
            var lectureToAdd = new LecturePostDTO();
            var idOfSubject = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).Throws(new NotFoundSubjectException(idOfSubject));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundClassroomException()
        {
            var lectureToAdd = new LecturePostDTO();
            var idOfClassroom = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).Throws(new NotFoundClassroomException(idOfClassroom));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundCourseSubjectException()
        {
            var lectureToAdd = new LecturePostDTO();
            var idOfCourse = 1;
            var idOfSubject = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).Throws(new NotFoundCourseSubjectException(idOfCourse, idOfSubject));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundLecturerException()
        {
            var lectureToAdd = new LecturePostDTO();
            var idOfLecturer = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).Throws(new NotFoundLecturerException(idOfLecturer));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsDateTimeException()
        {
            var lectureToAdd = new LecturePostDTO();
            var nameOfProperty = "lecture date";
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).Throws(new DateTimeException(nameOfProperty));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsSubjectLareadyConductedLectureException()
        {
            var lectureToAdd = new LecturePostDTO();
            var idOfCourse = 1;
            var idOfSubject = 1;
            _lectureServiceMock.Setup(service => service.PostLecture(lectureToAdd)).Throws(new SubjectAlreadyConductedLectureException(idOfCourse, idOfSubject));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostLecture(lectureToAdd);

            result.StatusCode.Should().Be(409);
        }


        [TestMethod]
        public async Task Post_CustomerLecture_ReturnsCreatedAtAction()
        {
            var customerLectureToAdd = new CustomerLecturePostDTO();
            var addedCustomerLecture = new CustomerLectureGetDTO();
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).Returns(Task.FromResult(addedCustomerLecture));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundLectureException()
        {
            var customerLectureToAdd = new CustomerLecturePostDTO();
            var idOfLecture = 1;
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).Throws(new NotFoundLectureException(idOfLecture));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundCustomerException()
        {
            var customerLectureToAdd = new CustomerLecturePostDTO();
            var idOfCustomer = 1;
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).Throws(new NotFoundCustomerException(idOfCustomer));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsCustomerAlreadyAssignedToLectureException()
        {
            var customerLectureToAdd = new CustomerLecturePostDTO();
            var idOfCustomer = 1;
            var idOfLecture = 1;
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).Throws(new CustomerAlreadyAssignedToLectureException(idOfCustomer, idOfLecture));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (ConflictObjectResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(409);
        }

        [TestMethod]
        public async Task Post_CustomerLecture_ThrowsNotFoundRegistrationException()
        {
            var customerLectureToAdd = new CustomerLecturePostDTO();
            var idOfCustomer = 1;
            var idOfCourse = 1;
            _customerLectureServiceMock.Setup(service => service.PostCustomerLecture(customerLectureToAdd)).Throws(new NotFoundRegistrationException(idOfCustomer, idOfCourse));
            _controller = new LectureController(_lectureServiceMock.Object, _customerLectureServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.PostCustomerLecture(customerLectureToAdd);

            result.StatusCode.Should().Be(404);
        }
    }
}
