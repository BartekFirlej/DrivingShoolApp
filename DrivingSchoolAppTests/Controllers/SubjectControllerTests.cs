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
    public class SubjectControllerTests
    {
        private Mock<ISubjectService> _subjectServiceMock;
        private Fixture _fixture;
        private SubjectController _controller;

        public SubjectControllerTests()
        {
            _fixture = new Fixture();
            _subjectServiceMock = new Mock<ISubjectService>();
        }

        [TestMethod]
        public async Task Get_Subjects_ReturnsOk()
        {
            ICollection<SubjectGetDTO> subjectsList = new List<SubjectGetDTO>();
            _subjectServiceMock.Setup(service => service.GetSubjects()).Returns(Task.FromResult(subjectsList));
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetSubjects();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_LicenceCategories_ThrowsNotFoundSubjectException()
        {
            _subjectServiceMock.Setup(service => service.GetSubjects()).Throws(new NotFoundSubjectException());
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetSubjects();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Subject_ReturnsOk()
        {
            var subject = new SubjectGetDTO();
            var idOfSubjectToFind = 1;
            _subjectServiceMock.Setup(service => service.GetSubject(idOfSubjectToFind)).Returns(Task.FromResult(subject));
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetSubject(idOfSubjectToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            _subjectServiceMock.Setup(service => service.GetSubject(idOfSubjectToFind)).Throws(new NotFoundSubjectException(idOfSubjectToFind));
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetSubject(idOfSubjectToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Subject_ReturnCreatedAtAction()
        {
            var subjectToAdd = new SubjectPostDTO();
            var addedSubject = new SubjectGetDTO();
            _subjectServiceMock.Setup(service => service.PostSubject(subjectToAdd)).Returns(Task.FromResult(addedSubject));
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostSubject(subjectToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Subject_ThrowsValueMustBeGreaterThanZero()
        {
            var subjectToAdd = new SubjectPostDTO();
            var nameOfProperty = "duration";
            _subjectServiceMock.Setup(service => service.PostSubject(subjectToAdd)).Throws(new ValueMustBeGreaterThanZeroException(nameOfProperty));
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostSubject(subjectToAdd);

            result.StatusCode.Should().Be(400);
        }
    }
}