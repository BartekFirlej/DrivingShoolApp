using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Models;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

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
            var subjectsList = new PagedList<SubjectGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<SubjectGetDTO>(), HasNextPage = false };
            _subjectServiceMock.Setup(service => service.GetSubjects(1, 10)).ReturnsAsync(subjectsList);
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetSubjects();

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Subjects_ThrowsNotFoundSubjectException()
        {
            _subjectServiceMock.Setup(service => service.GetSubjects(1, 10)).ThrowsAsync(new NotFoundSubjectException());
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetSubjects();

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Get_Subjects_ThrowsValueMustBeGreaterThanZeroException()
        {
            _subjectServiceMock.Setup(service => service.GetSubjects(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.GetSubjects(-1, 10);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Get_Subject_ReturnsOk()
        {
            var subject = new SubjectGetDTO();
            var idOfSubjectToFind = 1;
            _subjectServiceMock.Setup(service => service.GetSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (OkObjectResult)await _controller.GetSubject(idOfSubjectToFind);

            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public async Task Get_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            _subjectServiceMock.Setup(service => service.GetSubject(idOfSubjectToFind)).ThrowsAsync(new NotFoundSubjectException(idOfSubjectToFind));
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.GetSubject(idOfSubjectToFind);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Post_Subject_ReturnCreatedAtAction()
        {
            var subjectToAdd = new SubjectRequestDTO();
            var addedSubject = new SubjectResponseDTO();
            _subjectServiceMock.Setup(service => service.PostSubject(subjectToAdd)).ReturnsAsync(addedSubject);
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (CreatedAtActionResult)await _controller.PostSubject(subjectToAdd);

            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public async Task Post_Subject_ThrowsValueMustBeGreaterThanZero()
        {
            var subjectToAdd = new SubjectRequestDTO();
            var nameOfProperty = "duration";
            _subjectServiceMock.Setup(service => service.PostSubject(subjectToAdd)).ThrowsAsync(new ValueMustBeGreaterThanZeroException(nameOfProperty));
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (BadRequestObjectResult)await _controller.PostSubject(subjectToAdd);

            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task Delete_Subject_ReturnNoContent()
        {
            var deletedSubject = new Subject();
            var idOfSubjectToDelete = 1;
            _subjectServiceMock.Setup(service => service.DeleteSubject(idOfSubjectToDelete)).ReturnsAsync(deletedSubject);
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (NoContentResult)await _controller.DeleteSubject(idOfSubjectToDelete);

            result.StatusCode.Should().Be(204);
        }

        [TestMethod]
        public async Task Delete_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToDelete = 1;
            _subjectServiceMock.Setup(service => service.DeleteSubject(idOfSubjectToDelete)).ThrowsAsync(new NotFoundSubjectException(idOfSubjectToDelete));
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (NotFoundObjectResult)await _controller.DeleteSubject(idOfSubjectToDelete);

            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public async Task Delete_Subject_ThrowsReferenceConstraintException()
        {
            var idOfSubjectToDelete = 1;
            _subjectServiceMock.Setup(service => service.DeleteSubject(idOfSubjectToDelete)).ThrowsAsync(new ReferenceConstraintException());
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteSubject(idOfSubjectToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Subject_ThrowsDbUpdateException()
        {
            var idOfSubjectToDelete = 1;
            _subjectServiceMock.Setup(service => service.DeleteSubject(idOfSubjectToDelete)).ThrowsAsync(new DbUpdateException());
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteSubject(idOfSubjectToDelete);

            result.StatusCode.Should().Be(500);
        }

        [TestMethod]
        public async Task Delete_Subject_ThrowsException()
        {
            var idOfSubjectToDelete = 1;
            _subjectServiceMock.Setup(service => service.DeleteSubject(idOfSubjectToDelete)).ThrowsAsync(new Exception());
            _controller = new SubjectController(_subjectServiceMock.Object);

            var result = (ObjectResult)await _controller.DeleteSubject(idOfSubjectToDelete);

            result.StatusCode.Should().Be(500);
        }
    }
}