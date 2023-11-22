using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using EntityFramework.Exceptions.Common;
using Moq;

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
            var subjectsList = new PagedList<SubjectGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<SubjectGetDTO> { subject }, HasNextPage = false };
            _subjectRepositoryMock.Setup(repo => repo.GetSubjects(1, 10)).ReturnsAsync(subjectsList);
            _service = new SubjectService(_subjectRepositoryMock.Object);

            var result = await _service.GetSubjects(1, 10);

            Assert.AreEqual(subjectsList, result);
        }

        [TestMethod]
        public async Task Get_Subjects_ThrowsNotFoundSubjectsException()
        {
            var subjectsList = new PagedList<SubjectGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<SubjectGetDTO>(), HasNextPage = false };
            _subjectRepositoryMock.Setup(repo => repo.GetSubjects(1, 10)).ReturnsAsync(subjectsList);
            _service = new SubjectService(_subjectRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.GetSubjects(1, 10));
        }

        [TestMethod]
        public async Task Get_Subjects_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _subjectRepositoryMock.Setup(repo => repo.GetSubjects(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new SubjectService(_subjectRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetSubjects(-1, 10));
        }

        [TestMethod]
        public async Task Get_Subject_ReturnsSubject()
        {
            var subject = new SubjectGetDTO();
            var idOfSubjectToFind = 1;
            _subjectRepositoryMock.Setup(repo => repo.GetSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _service = new SubjectService(_subjectRepositoryMock.Object);

            var result = await _service.GetSubject(idOfSubjectToFind);

            Assert.AreEqual(subject, result);
        }

        [TestMethod]
        public async Task Get_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            _subjectRepositoryMock.Setup(repo => repo.GetSubject(idOfSubjectToFind)).ReturnsAsync((SubjectGetDTO)null);
            _service = new SubjectService(_subjectRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.GetSubject(idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Post_Subject_ReturnsAddedSubject()
        {
            var addedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var addedSubjectDTO = new SubjectGetDTO { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3};
            var subjectToAdd = new SubjectPostDTO { Code = "B01", Name = "Test Subject", Duration = 3 };
            _subjectRepositoryMock.Setup(repo => repo.PostSubject(subjectToAdd)).ReturnsAsync(addedSubject);
            _subjectRepositoryMock.Setup(repo => repo.GetSubject(addedSubject.Id)).ReturnsAsync(addedSubjectDTO);
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

        [TestMethod]
        public async Task Delete_Subject_ReturnsSubject()
        {
            var deletedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var idOfSubjectToDelete = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubjectToDelete)).ReturnsAsync(deletedSubject);
            _subjectRepositoryMock.Setup(repo => repo.DeleteSubject(deletedSubject)).ReturnsAsync(deletedSubject);
            _service = new SubjectService(_subjectRepositoryMock.Object);

            var result = await _service.DeleteSubject(idOfSubjectToDelete);

            Assert.AreEqual(deletedSubject, result);
        }

        [TestMethod]
        public async Task Delete_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToDelete = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubjectToDelete)).ReturnsAsync((Subject)null);
            _service = new SubjectService(_subjectRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.DeleteSubject(idOfSubjectToDelete));
        }

        [TestMethod]
        public async Task Delete_Subject_PropagatesReferenceConstraintExceptionException()
        {
            var deletedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var idOfSubjectToDelete = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubjectToDelete)).ReturnsAsync(deletedSubject);
            _subjectRepositoryMock.Setup(repo => repo.DeleteSubject(deletedSubject)).ThrowsAsync(new ReferenceConstraintException());
            _service = new SubjectService(_subjectRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteSubject(idOfSubjectToDelete));
        }

        [TestMethod]
        public async Task Check_Subject_ReturnsSubject()
        {
            var deletedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var idOfSubject = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubject)).ReturnsAsync(deletedSubject);
            _service = new SubjectService(_subjectRepositoryMock.Object);

            var result = await _service.CheckSubject(idOfSubject);

            Assert.AreEqual(deletedSubject, result);
        }

        [TestMethod]
        public async Task Check_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubject = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubject)).ReturnsAsync((Subject)null);
            _service = new SubjectService(_subjectRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.CheckSubject(idOfSubject));
        }
    }
}
