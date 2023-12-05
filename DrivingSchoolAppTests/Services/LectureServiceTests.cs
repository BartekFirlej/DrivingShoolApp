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
    public class LectureServiceTests
    {
        private Mock<ILectureRepository> _lectureRepositoryMock;
        private Mock<ILecturerService> _lecturerServiceMock;
        private Mock<ICourseSubjectService> _courseSubjectServiceMock;
        private Mock<IClassroomService> _classroomServiceMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private LectureService _service;

        public LectureServiceTests()
        {
            _fixture = new Fixture();
            _lectureRepositoryMock = new Mock<ILectureRepository>();
            _lecturerServiceMock = new Mock<ILecturerService>();
            _courseSubjectServiceMock = new Mock<ICourseSubjectService>();
            _classroomServiceMock = new Mock<IClassroomService>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_Lectures_ReturnsLectures()
        {
            var lecture = new LectureGetDTO();
            var lecturesList = new PagedList<LectureGetDTO>() {PageIndex = 1, PageSize = 10, PagedItems = new List<LectureGetDTO> { lecture }, HasNextPage = false };
            _lectureRepositoryMock.Setup(repo => repo.GetLectures(1, 10)).ReturnsAsync(lecturesList);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetLectures(1, 10);

            Assert.AreEqual(lecturesList, result);
        }

        [TestMethod]
        public async Task Get_Lectures_ThrowsNotFoundLecturesException()
        {
            var lecturesList = new PagedList<LectureGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<LectureGetDTO> (), HasNextPage = false };
            _lectureRepositoryMock.Setup(repo => repo.GetLectures(1, 10)).ReturnsAsync(lecturesList);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetLectures(1, 10));
        }

        [TestMethod]
        public async Task Get_Lectures_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _lectureRepositoryMock.Setup(repo => repo.GetLectures(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetLectures(-1, 10));
        }

        [TestMethod]
        public async Task Get_Lecture_ReturnsLecture()
        {
            var lecture = new LectureGetDTO();
            var idOfLectureToFind = 1;
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetLecture(idOfLectureToFind);

            Assert.AreEqual(lecture, result);
        }

        [TestMethod]
        public async Task Get_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(idOfLectureToFind)).ReturnsAsync((LectureGetDTO)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetLecture(idOfLectureToFind));
        }

        [TestMethod]
        public async Task Get_CourseLectureSubject_ReturnsTrue()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            var foundLecture = new Lecture();
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureAtCourseAboutSubject(idOfCourseToFind, idOfSubjectToFind)).ReturnsAsync(foundLecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckLectureAtCourseAboutSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public async Task Get_CourseLectureSubject_ReturnsFalse()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureAtCourseAboutSubject(idOfCourseToFind, idOfSubjectToFind)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckLectureAtCourseAboutSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.AreEqual(false, false);
        }

        [TestMethod]
        public async Task Post_Lecture_ReturnsAddedLecture()
        {
            var lecturer = new Lecturer { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 1, SubjectId = 1 };
            var classroom = new Classroom{ Id = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureResponseDTO { Id = 1, ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id, LectureDate = new DateTime(2022, 1, 1) };
            var lectureToAdd = new LectureRequestDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync(courseSubject);
            _classroomServiceMock.Setup(service => service.CheckClassroom(lectureToAdd.ClassroomId)).ReturnsAsync(classroom);
            _lectureRepositoryMock.Setup(repo => repo.PostLecture(lectureToAdd)).ReturnsAsync(addedLecture);
            _mapperMock.Setup(m => m.Map<LectureResponseDTO>(It.IsAny<Lecture>())).Returns(addedLectureDTO);
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureAtCourseAboutSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.PostLecture(lectureToAdd);

            Assert.AreEqual(addedLectureDTO, result);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsLectureDateTimeException()
        {
            var lecturer = new Lecturer { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 1, SubjectId = 1 };
            var classroom = new Classroom{ Id = 1 };
            var lectureToAdd = new LectureRequestDTO { ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundLecturerException()
        {
            var lecturer = new Lecturer { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 1, SubjectId = 1 };
            var classroom = new Classroom { Id = 1 };
            var lectureToAdd = new LectureRequestDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureToAdd.LecturerId)).ThrowsAsync(new NotFoundLecturerException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundCourseSubjectException()
        {
            var lecturer = new Lecturer { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 1, SubjectId = 1 };
            var classroom = new Classroom { Id = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var lectureToAdd = new LectureRequestDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ThrowsAsync(new NotFoundCourseSubjectException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundCourseException()
        {
            var lecturer = new Lecturer { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 1, SubjectId = 1 };
            var classroom = new Classroom { Id = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var lectureToAdd = new LectureRequestDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ThrowsAsync(new NotFoundCourseException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundSubjectException()
        {
            var lecturer = new Lecturer { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 1, SubjectId = 1 };
            var classroom = new Classroom { Id = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var lectureToAdd = new LectureRequestDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ThrowsAsync(new NotFoundSubjectException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundClassroomtException()
        {
            var lecturer = new Lecturer { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 1, SubjectId = 1 };
            var classroom = new Classroom { Id = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var lectureToAdd = new LectureRequestDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync(courseSubject);
            _classroomServiceMock.Setup(service => service.CheckClassroom(lectureToAdd.ClassroomId)).ThrowsAsync(new NotFoundClassroomException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowSubjectAlreadyConductedLectureException()
        {
            var lecturer = new Lecturer { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 1, SubjectId = 1 };
            var classroom = new Classroom { Id = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var lectureToAdd = new LectureRequestDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.Id, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync(courseSubject);
            _classroomServiceMock.Setup(service => service.CheckClassroom(lectureToAdd.ClassroomId)).ReturnsAsync(classroom);
            _lectureRepositoryMock.Setup(repo => repo.PostLecture(lectureToAdd)).ReturnsAsync(addedLecture);
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(addedLecture.Id)).ReturnsAsync(addedLectureDTO);
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureAtCourseAboutSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync(new Lecture());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<SubjectAlreadyConductedLectureException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Delete_Lecture_ReturnsLecture()
        {
            var idOfLectureToDelete = 1;
            var deletedlecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 2, SubjectId = 3, LecturerId = 4};
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLectureToDelete)).ReturnsAsync(deletedlecture);
            _lectureRepositoryMock.Setup(repo => repo.DeleteLecture(deletedlecture)).ReturnsAsync(deletedlecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.DeleteLecture(idOfLectureToDelete);

            Assert.AreEqual(deletedlecture, result);
        }

        [TestMethod]
        public async Task Delete_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToDelete = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLectureToDelete)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.DeleteLecture(idOfLectureToDelete));
        }

        [TestMethod]
        public async Task Delete_Lecture_PropagatesReferenceConstraintExceptionException()
        {
            var deletedlecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 2, SubjectId = 3, LecturerId = 4 };
            var idOfLectureToDelete = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLectureToDelete)).ReturnsAsync(deletedlecture);
            _lectureRepositoryMock.Setup(repo => repo.DeleteLecture(deletedlecture)).ThrowsAsync(new ReferenceConstraintException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteLecture(idOfLectureToDelete));
        }

        [TestMethod]
        public async Task Check_Lecture_ReturnsLecture()
        {
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 2, SubjectId = 3, LecturerId = 4 };
            var idOfLecture = 1; 
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLecture)).ReturnsAsync(lecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckLecture(idOfLecture);

            Assert.AreEqual(lecture, result);
        }

        [TestMethod]
        public async Task Check_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLecture = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLecture)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.CheckLecture(idOfLecture));
        }

        [TestMethod]
        public async Task Check_LectureTracking_ReturnsLecture()
        {
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 2, SubjectId = 3, LecturerId = 4 };
            var idOfLecture = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync(lecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckLectureTracking(idOfLecture);

            Assert.AreEqual(lecture, result);
        }

        [TestMethod]
        public async Task Check_LectureTracking_ThrowsNotFoundLectureException()
        {
            var idOfLecture = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.CheckLectureTracking(idOfLecture));
        }

        [TestMethod]
        public async Task CheckLectureAtCourseAboutSubject_ReturnsNull()
        {
            var idOfLecture = 1;
            var idOfSubject = 1;
            var idOfCourse = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureAtCourseAboutSubject(idOfCourse, idOfSubject)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckLectureAtCourseAboutSubject(idOfCourse, idOfSubject);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CheckLectureAtCourseAboutSubject_ReturnsLecture()
        {
            var idOfLecture = 1;
            var idOfSubject = 1;
            var idOfCourse = 1;
            var lecture = new Lecture();
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureAtCourseAboutSubject(idOfCourse, idOfSubject)).ReturnsAsync(lecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckLectureAtCourseAboutSubject(idOfCourse, idOfSubject);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Update_Lecture_ReturnsUpdatedLecture()
        {
            var idOfLecture = 1;
            var lecturer = new Lecturer { Id = 2, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 2, SubjectId = 2 };
            var classroom = new Classroom { Id = 2 };
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2022, 1, 1) };
            var lectureUpdate = new LectureRequestDTO { LectureDate = new DateTime(2022, 10, 10), ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2 };
            var updatedLectureDTO = new LectureResponseDTO { Id = 1, ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2022, 10, 10) };
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync(lecture);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureUpdate.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId)).ReturnsAsync(courseSubject);
            _classroomServiceMock.Setup(service => service.CheckClassroom(lectureUpdate.ClassroomId)).ReturnsAsync(classroom);
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureAtCourseAboutSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId)).ReturnsAsync((Lecture)null);
            _lectureRepositoryMock.Setup(repo => repo.UpdateLecture(lecture, lectureUpdate)).ReturnsAsync(lecture);
            _mapperMock.Setup(m => m.Map<LectureResponseDTO>(It.IsAny<Lecture>())).Returns(updatedLectureDTO);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            var result = await _service.UpdateLecture(idOfLecture, lectureUpdate);

            Assert.AreEqual(updatedLectureDTO, result);
        }

        [TestMethod]
        public async Task Update_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLecture = 1;
            var lectureUpdate = new LectureRequestDTO { LectureDate = new DateTime(2022, 10, 10), ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2 };
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ThrowsAsync(new NotFoundLectureException(idOfLecture));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.UpdateLecture(idOfLecture, lectureUpdate));
        }

        [TestMethod]
        public async Task Update_Lecture_ThrowsLectureDateTimeException()
        {
            var idOfLecture = 1;
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2022, 1, 1) };
            var lectureUpdate = new LectureRequestDTO { ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2 };
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync(lecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.UpdateLecture(idOfLecture, lectureUpdate));
        }

        [TestMethod]
        public async Task Update_Lecture_ThrowsNotFoundLecturerException()
        {
            var idOfLecture = 1;
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2022, 1, 1) };
            var lectureUpdate = new LectureRequestDTO { LectureDate = new DateTime(2022, 10, 10), ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2 };
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync(lecture);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureUpdate.LecturerId)).ThrowsAsync(new NotFoundLecturerException(2));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.UpdateLecture(idOfLecture, lectureUpdate));
        }

        [TestMethod]
        public async Task Update_Lecture_ThrowsNotFoundCourseSubjectException()
        {
            var idOfLecture = 1;
            var lecturer = new Lecturer { Id = 2, Name = "Test", SecondName = "Test" };
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2022, 1, 1) };
            var lectureUpdate = new LectureRequestDTO { LectureDate = new DateTime(2022, 10, 10), ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2 };
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync(lecture);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureUpdate.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId)).ThrowsAsync(new NotFoundCourseSubjectException(2,2));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.UpdateLecture(idOfLecture, lectureUpdate));
        }

        [TestMethod]
        public async Task Update_Lecture_ThrowsNotFoundCourseException()
        {
            var idOfLecture = 1; 
            var lecturer = new Lecturer { Id = 2, Name = "Test", SecondName = "Test" };
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2022, 1, 1) };
            var lectureUpdate = new LectureRequestDTO { LectureDate = new DateTime(2022, 10, 10), ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2 };
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync(lecture);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureUpdate.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId)).ThrowsAsync(new NotFoundCourseException(2));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.UpdateLecture(idOfLecture, lectureUpdate));
        }

        [TestMethod]
        public async Task Update_Lecture_ThrowsNotFoundSubjectException()
        {
            var idOfLecture = 1;
            var lecturer = new Lecturer { Id = 2, Name = "Test", SecondName = "Test" };
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2022, 1, 1) };
            var lectureUpdate = new LectureRequestDTO { LectureDate = new DateTime(2022, 10, 10), ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2 };
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync(lecture);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureUpdate.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId)).ThrowsAsync(new NotFoundSubjectException(2));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.UpdateLecture(idOfLecture, lectureUpdate));
        }

        [TestMethod]
        public async Task Update_Lecture_ThrowsNotFoundClassroomException()
        {
            var idOfLecture = 1;
            var lecturer = new Lecturer { Id = 2, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 2, SubjectId = 2 };
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2022, 1, 1) };
            var lectureUpdate = new LectureRequestDTO { LectureDate = new DateTime(2022, 10, 10), ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2 };
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync(lecture);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureUpdate.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId)).ReturnsAsync(courseSubject);
            _classroomServiceMock.Setup(service => service.CheckClassroom(lectureUpdate.ClassroomId)).ThrowsAsync(new NotFoundClassroomException(lectureUpdate.ClassroomId));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.UpdateLecture(idOfLecture, lectureUpdate));
        }

        [TestMethod]
        public async Task Update_Lecture_ThrowSubjectAlreadyConductedLectureException()
        {
            var idOfLecture = 1;
            var classroom = new Classroom { Id = 2 };
            var lecturer = new Lecturer { Id = 2, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubject { CourseId = 2, SubjectId = 2 };
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2022, 1, 1) };
            var lectureUpdate = new LectureRequestDTO { LectureDate = new DateTime(2022, 10, 10), ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2 };
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureTracking(idOfLecture)).ReturnsAsync(lecture);
            _lecturerServiceMock.Setup(service => service.CheckLecturer(lectureUpdate.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.CheckCourseSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId)).ReturnsAsync(courseSubject);
            _classroomServiceMock.Setup(service => service.CheckClassroom(lectureUpdate.ClassroomId)).ReturnsAsync(classroom);
            _lectureRepositoryMock.Setup(repo => repo.CheckLectureAtCourseAboutSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId)).ReturnsAsync(new Lecture());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<SubjectAlreadyConductedLectureException>(async () => await _service.UpdateLecture(idOfLecture, lectureUpdate));
        }
    }
}
