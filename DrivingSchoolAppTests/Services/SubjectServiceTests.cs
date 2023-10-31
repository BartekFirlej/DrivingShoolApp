using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class SubjectServiceTests
    {
        private Mock<ISubjectRepository> _subjectRepositoryMock;
        private Fixture _fixture;
        private SubjectService _service;

        public SubjectServiceTests()
        {
            _fixture = new Fixture();
            _subjectRepositoryMock = new Mock<ISubjectRepository>();
        }

        [TestMethod]
        public async Task Get_Subjects_ReturnsSubjects()
        {
            var subject = new SubjectGetDTO();
            ICollection<SubjectGetDTO> subjectsList = new List<SubjectGetDTO>() { subject };
            _subjectRepositoryMock.Setup(repo => repo.GetSubjects()).Returns(Task.FromResult(subjectsList));
            _service = new SubjectService(_subjectRepositoryMock.Object);

            var result = await _service.GetSubjects();

            Assert.AreEqual(subjectsList, result);
        }

        [TestMethod]
        public async Task Get_Subjects_ThrowsNotFoundSubjectsException()
        {
            ICollection<SubjectGetDTO> subjectsList = new List<SubjectGetDTO>();
            _subjectRepositoryMock.Setup(repo => repo.GetSubjects()).Returns(Task.FromResult(subjectsList));
            _service = new SubjectService(_subjectRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.GetSubjects());
        }

        [TestMethod]
        public async Task Get_Subject_ReturnsSubject()
        {
            var subject = new SubjectGetDTO();
            var idOfSubjectToFind = 1;
            _subjectRepositoryMock.Setup(repo => repo.GetSubject(idOfSubjectToFind)).Returns(Task.FromResult(subject));
            _service = new SubjectService(_subjectRepositoryMock.Object);

            var result = await _service.GetSubject(idOfSubjectToFind);

            Assert.AreEqual(subject, result);
        }

        [TestMethod]
        public async Task Get_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            _subjectRepositoryMock.Setup(repo => repo.GetSubject(idOfSubjectToFind)).Returns(Task.FromResult<SubjectGetDTO>(null));
            _service = new SubjectService(_subjectRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.GetSubject(idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Post_Subject_ReturnsAddedSubject()
        {
            var addedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var addedSubjectDTO = new SubjectGetDTO { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3};
            var subjectToAdd = new SubjectPostDTO { Code = "B01", Name = "Test Subject", Duration = 3 };
            _subjectRepositoryMock.Setup(repo => repo.PostSubject(subjectToAdd)).Returns(Task.FromResult(addedSubject));
            _subjectRepositoryMock.Setup(repo => repo.GetSubject(addedSubject.Id)).Returns(Task.FromResult(addedSubjectDTO));
            _service = new SubjectService(_subjectRepositoryMock.Object);

            var result = await _service.PostSubject(subjectToAdd);

            Assert.AreEqual(addedSubjectDTO, result);
        }

        [TestMethod]
        public async Task Post_Subject_ThrowsDurationMustBeGreaterThanZeroExceptionException()
        {
            var subjectToAdd = new SubjectPostDTO { Code = "B01", Name = "Test Subject", Duration = -3 };
            _service = new SubjectService(_subjectRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostSubject(subjectToAdd));
        }
    }
}
