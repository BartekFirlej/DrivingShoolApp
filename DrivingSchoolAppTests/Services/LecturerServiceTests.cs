using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Moq;

namespace DrivingSchoolAppTests.Services
{
    [TestClass]
    public class LecturerServiceTests
    {
        private Mock<ILecturerRepository> _lecturerRepositoryMock;
        private Fixture _fixture;
        private LecturerService _service;

        public LecturerServiceTests()
        {
            _fixture = new Fixture();
            _lecturerRepositoryMock = new Mock<ILecturerRepository>();
        }

        [TestMethod]
        public async Task Get_Lecturers_ReturnsLecturers()
        {
            var lecturer = new LecturerGetDTO();
            var lecturersList = new PagedList<LecturerGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<LecturerGetDTO> { lecturer }, HasNextPage = false };
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturers(1, 10)).Returns(Task.FromResult(lecturersList));
            _service = new LecturerService(_lecturerRepositoryMock.Object);

            var result = await _service.GetLecturers(1, 10);

            Assert.AreEqual(lecturersList, result);
        }

        [TestMethod]
        public async Task Get_Lecturers_ThrowsNotFoundLecturerException()
        {
            var lecturersList = new PagedList<LecturerGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<LecturerGetDTO>(), HasNextPage = false };
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturers(1, 10)).Returns(Task.FromResult(lecturersList));
            _service = new LecturerService(_lecturerRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.GetLecturers(1, 10));
        }

        [TestMethod]
        public async Task Get_Lecturers_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturers(-1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new LecturerService(_lecturerRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetLecturers(-1, 10));
        }

        [TestMethod]
        public async Task Get_Lecturer_ReturnsLecturer()
        {
            var lecturer = new LecturerGetDTO();
            var idOfLecturerToFind = 1;
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturer(idOfLecturerToFind)).Returns(Task.FromResult(lecturer));
            _service = new LecturerService(_lecturerRepositoryMock.Object);

            var result = await _service.GetLecturer(idOfLecturerToFind);

            Assert.AreEqual(lecturer, result);
        }

        [TestMethod]
        public async Task Get_Lecturer_ThrowsNotFoundLectNotFoundLecturerException()
        {
            var idOfLecturerToFind = 1;
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturer(idOfLecturerToFind)).Returns(Task.FromResult<LecturerGetDTO>(null));
            _service = new LecturerService(_lecturerRepositoryMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.GetLecturer(idOfLecturerToFind));
        }

        [TestMethod]
        public async Task Post_Lecturer_ReturnsAddedLecturer()
        {
            var addedLecturer = new Lecturer { Id = 1, Name = "TestName", SecondName = "TestSecondName" };
            var addedLecturerDTO = new LecturerGetDTO { Name = "TestName", SecondName = "TestSecondName" };
            var lecturerToAdd = new LecturerPostDTO { Name = "TestName", SecondName = "TestSecondName" };
            _lecturerRepositoryMock.Setup(repo => repo.PostLecturer(lecturerToAdd)).Returns(Task.FromResult(addedLecturer));
            _lecturerRepositoryMock.Setup(repo => repo.GetLecturer(addedLecturer.Id)).Returns(Task.FromResult(addedLecturerDTO));
            _service = new LecturerService(_lecturerRepositoryMock.Object);

            var result = await _service.PostLecturer(lecturerToAdd);

            Assert.AreEqual(addedLecturerDTO, result);
        }
    }
}
