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
    public class LecturerServiceTests
    {
        private Mock<ILecturerRepository> _lecturerRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private LecturerService _service;

        public LecturerServiceTests()
        {
            _fixture = new Fixture();
            _lecturerRepositoryMock = new Mock<ILecturerRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_Lecturers_ReturnsLecturers()
        {
            var lecturer = new LecturerGetDTO();
            var lecturersList = new PagedList<LecturerGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<LecturerGetDTO> { lecturer }, HasNextPage = false };
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturers(1, 10)).ReturnsAsync(lecturersList);
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetLecturers(1, 10);

            Assert.AreEqual(lecturersList, result);
        }

        [TestMethod]
        public async Task Get_Lecturers_ThrowsNotFoundLecturerException()
        {
            var lecturersList = new PagedList<LecturerGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<LecturerGetDTO>(), HasNextPage = false };
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturers(1, 10)).ReturnsAsync(lecturersList);
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.GetLecturers(1, 10));
        }

        [TestMethod]
        public async Task Get_Lecturers_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturers(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetLecturers(-1, 10));
        }

        [TestMethod]
        public async Task Get_Lecturer_ReturnsLecturer()
        {
            var lecturer = new LecturerGetDTO();
            var idOfLecturerToFind = 1;
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturer(idOfLecturerToFind)).ReturnsAsync(lecturer);
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.GetLecturer(idOfLecturerToFind);

            Assert.AreEqual(lecturer, result);
        }

        [TestMethod]
        public async Task Get_Lecturer_ThrowsNotFoundLectNotFoundLecturerException()
        {
            var idOfLecturerToFind = 1;
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturer(idOfLecturerToFind)).ReturnsAsync((LecturerGetDTO)null);
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.GetLecturer(idOfLecturerToFind));
        }

        [TestMethod]
        public async Task Post_Lecturer_ReturnsAddedLecturer()
        {
            var addedLecturer = new Lecturer { Id = 1, Name = "TestName", SecondName = "TestSecondName" };
            var addedLecturerDTO = new LecturerResponseDTO {Id = 1, Name = "TestName", SecondName = "TestSecondName" };
            var lecturerToAdd = new LecturerRequestDTO { Name = "TestName", SecondName = "TestSecondName" };
            _lecturerRepositoryMock.Setup(repo => repo.PostLecturer(lecturerToAdd)).ReturnsAsync(addedLecturer);
            _mapperMock.Setup(m => m.Map<LecturerResponseDTO>(It.IsAny<Lecturer>())).Returns(addedLecturerDTO);
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.PostLecturer(lecturerToAdd);

            Assert.AreEqual(addedLecturerDTO, result);
        }

        [TestMethod]
        public async Task Delete_Lecturer_ReturnsLecturer()
        {
            var idOfLecturerToDelete = 1;
            var deletedLecturer = new Lecturer { Id = 1, Name = "TestName", SecondName = "TestSecondName" };
            _lecturerRepositoryMock.Setup(repo => repo.CheckLecturer(idOfLecturerToDelete)).ReturnsAsync(deletedLecturer);
            _lecturerRepositoryMock.Setup(repo => repo.DeleteLecturer(deletedLecturer)).ReturnsAsync(deletedLecturer);
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.DeleteLecturer(idOfLecturerToDelete);

            Assert.AreEqual(deletedLecturer, result);
        }

        [TestMethod]
        public async Task Delete_Lecturer_ThrowsNotFoundLecturerException()
        {
            var idOfLectureToDelete = 1;
            _lecturerRepositoryMock.Setup(repo => repo.CheckLecturer(idOfLectureToDelete)).ReturnsAsync((Lecturer)null);
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.DeleteLecturer(idOfLectureToDelete));
        }

        [TestMethod]
        public async Task Delete_Lecturer_PropagatesReferenceConstraintExceptionException()
        {
            var idOfLecturerToDelete = 1;
            var deletedLecturer = new Lecturer { Id = 1, Name = "TestName", SecondName = "TestSecondName" };
            _lecturerRepositoryMock.Setup(repo => repo.CheckLecturer(idOfLecturerToDelete)).ReturnsAsync(deletedLecturer);
            _lecturerRepositoryMock.Setup(repo => repo.DeleteLecturer(deletedLecturer)).ThrowsAsync(new ReferenceConstraintException());
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteLecturer(idOfLecturerToDelete));
        }

        [TestMethod]
        public async Task Check_Lecturer_ReturnsLecturer()
        {
            var lecture = new Lecturer { Id = 1, Name = "TestName", SecondName = "TestSecondName" };
            var idOfLecturerToCheck = 1;
            _lecturerRepositoryMock.Setup(repo => repo.CheckLecturer(idOfLecturerToCheck)).ReturnsAsync(lecture);
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            var result = await _service.CheckLecturer(idOfLecturerToCheck);

            Assert.AreEqual(lecture, result);
        }

        [TestMethod]
        public async Task Check_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLecturerToCheck = 1;
            _lecturerRepositoryMock.Setup(repo => repo.CheckLecturer(idOfLecturerToCheck)).ReturnsAsync((Lecturer)null);
            _service = new LecturerService(_lecturerRepositoryMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.CheckLecturer(idOfLecturerToCheck));
        }
    }
}
