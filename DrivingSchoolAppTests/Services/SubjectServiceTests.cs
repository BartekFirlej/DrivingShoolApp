using AutoFixture;
using AutoMapper;
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
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private SubjectService _service;

        public SubjectServiceTests()
        {
            _fixture = new Fixture();
            _subjectRepositoryMock = new Mock<ISubjectRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_Subjects_ReturnsSubjects()
        {
            var subject = new SubjectGetDTO();
            var subjectsList = new PagedList<SubjectGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<SubjectGetDTO> { subject }, HasNextPage = false };
            _subjectRepositoryMock.Setup(repo => repo.GetSubjects(1, 10)).ReturnsAsync(subjectsList);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetSubjects(1, 10);

            Assert.AreEqual(subjectsList, result);
        }

        [TestMethod]
        public async Task Get_Subjects_ThrowsNotFoundSubjectsException()
        {
            var subjectsList = new PagedList<SubjectGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<SubjectGetDTO>(), HasNextPage = false };
            _subjectRepositoryMock.Setup(repo => repo.GetSubjects(1, 10)).ReturnsAsync(subjectsList);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.GetSubjects(1, 10));
        }

        [TestMethod]
        public async Task Get_Subjects_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _subjectRepositoryMock.Setup(repo => repo.GetSubjects(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetSubjects(-1, 10));
        }

        [TestMethod]
        public async Task Get_Subject_ReturnsSubject()
        {
            var subject = new SubjectGetDTO();
            var idOfSubjectToFind = 1;
            _subjectRepositoryMock.Setup(repo => repo.GetSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetSubject(idOfSubjectToFind);

            Assert.AreEqual(subject, result);
        }

        [TestMethod]
        public async Task Get_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            _subjectRepositoryMock.Setup(repo => repo.GetSubject(idOfSubjectToFind)).ReturnsAsync((SubjectGetDTO)null);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.GetSubject(idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Post_Subject_ReturnsAddedSubject()
        {
            var addedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var addedSubjectDTO = new SubjectResponseDTO { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3};
            var subjectToAdd = new SubjectRequestDTO { Code = "B01", Name = "Test Subject", Duration = 3 };
            _subjectRepositoryMock.Setup(repo => repo.PostSubject(subjectToAdd)).ReturnsAsync(addedSubject); 
            _mapperMock.Setup(m => m.Map<SubjectResponseDTO>(It.IsAny<Subject>())).Returns(addedSubjectDTO);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.PostSubject(subjectToAdd);

            Assert.AreEqual(addedSubjectDTO, result);
        }

        [TestMethod]
        public async Task Post_Subject_ThrowsDurationMustBeGreaterThanZeroExceptionException()
        {
            var subjectToAdd = new SubjectRequestDTO { Code = "B01", Name = "Test Subject", Duration = -3 };
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostSubject(subjectToAdd));
        }

        [TestMethod]
        public async Task Delete_Subject_ReturnsSubject()
        {
            var deletedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var idOfSubjectToDelete = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubjectToDelete)).ReturnsAsync(deletedSubject);
            _subjectRepositoryMock.Setup(repo => repo.DeleteSubject(deletedSubject)).ReturnsAsync(deletedSubject);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.DeleteSubject(idOfSubjectToDelete);

            Assert.AreEqual(deletedSubject, result);
        }

        [TestMethod]
        public async Task Delete_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToDelete = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubjectToDelete)).ReturnsAsync((Subject)null);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.DeleteSubject(idOfSubjectToDelete));
        }

        [TestMethod]
        public async Task Delete_Subject_PropagatesReferenceConstraintExceptionException()
        {
            var deletedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var idOfSubjectToDelete = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubjectToDelete)).ReturnsAsync(deletedSubject);
            _subjectRepositoryMock.Setup(repo => repo.DeleteSubject(deletedSubject)).ThrowsAsync(new ReferenceConstraintException());
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteSubject(idOfSubjectToDelete));
        }

        [TestMethod]
        public async Task Check_Subject_ReturnsSubject()
        {
            var deletedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var idOfSubject = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubject)).ReturnsAsync(deletedSubject);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.CheckSubject(idOfSubject);

            Assert.AreEqual(deletedSubject, result);
        }

        [TestMethod]
        public async Task Check_Subject_ThrowsNotFoundSubjectException()
        {
            var idOfSubject = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubject(idOfSubject)).ReturnsAsync((Subject)null);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.CheckSubject(idOfSubject));
        }

        [TestMethod]
        public async Task Check_SubjectTracking_ReturnsSubject()
        {
            var deletedSubject = new Subject { Id = 1, Code = "B01", Name = "Test Subject", Duration = 3 };
            var idOfSubject = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubjectTracking(idOfSubject)).ReturnsAsync(deletedSubject);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.CheckSubjectTracking(idOfSubject);

            Assert.AreEqual(deletedSubject, result);
        }

        [TestMethod]
        public async Task Check_SubjectTracking_ThrowsNotFoundSubjectException()
        {
            var idOfSubject = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubjectTracking(idOfSubject)).ReturnsAsync((Subject)null);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.CheckSubjectTracking(idOfSubject));
        }

        [TestMethod]
        public async Task Update_Subject_ReturnsSubject()
        {
            var subject = new Subject { Id = 1, Code = "A31", Duration = 2, Name = "Test" };
            var updateSubject = new SubjectRequestDTO { Code = "B31", Duration = 20, Name = "UpdatedTest" };
            var updatedSubject = new SubjectResponseDTO { Id = 1, Code = "B31", Duration = 20, Name = "UpdatedTest" };
            var idOfSubject = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubjectTracking(idOfSubject)).ReturnsAsync(subject);
            _mapperMock.Setup(m => m.Map<SubjectResponseDTO>(It.IsAny<Subject>())).Returns(updatedSubject);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.UpdateSubject(1, updateSubject);

            Assert.AreEqual(result, updatedSubject);
        }

        [TestMethod]
        public async Task Update_Subject_ThrowsNotFoundSubjectException()
        {
            var updateSubject = new SubjectRequestDTO { Code = "B31", Duration = 20, Name = "UpdatedTest" };
            var idOfSubject = 1;
            _subjectRepositoryMock.Setup(repo => repo.CheckSubjectTracking(idOfSubject)).ReturnsAsync((Subject)null);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.UpdateSubject(idOfSubject, updateSubject));
        }

        [TestMethod]
        public async Task Update_Subject_ThrowsValueMustBeGreaterThanZeroException()
        {
            var idOfSubject = 1; 
            var subject = new Subject { Id = 1, Code = "A31", Duration = 2, Name = "Test" };
            var updateSubject = new SubjectRequestDTO { Code = "B31", Duration = -20, Name = "UpdatedTest" };
            _subjectRepositoryMock.Setup(repo => repo.CheckSubjectTracking(idOfSubject)).ReturnsAsync(subject);
            _service = new SubjectService(_subjectRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateSubject(idOfSubject, updateSubject));
        }
    }
}
