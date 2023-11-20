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
    public class LectureServiceTests
    {
        private Mock<ILectureRepository> _lectureRepositoryMock;
        private Mock<ILecturerService> _lecturerServiceMock;
        private Mock<ICourseSubjectService> _courseSubjectServiceMock;
        private Mock<IClassroomService> _classroomServiceMock;
        private Fixture _fixture;
        private LectureService _service;

        public LectureServiceTests()
        {
            _fixture = new Fixture();
            _lectureRepositoryMock = new Mock<ILectureRepository>();
            _lecturerServiceMock = new Mock<ILecturerService>();
            _courseSubjectServiceMock = new Mock<ICourseSubjectService>();
            _classroomServiceMock = new Mock<IClassroomService>();
        }

        [TestMethod]
        public async Task Get_Lectures_ReturnsLectures()
        {
            var lecture = new LectureGetDTO();
            var lecturesList = new PagedList<LectureGetDTO>() {PageIndex = 1, PageSize = 10, PagedItems = new List<LectureGetDTO> { lecture }, HasNextPage = false };
            _lectureRepositoryMock.Setup(repo => repo.GetLectures(1, 10)).ReturnsAsync(lecturesList);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.GetLectures(1, 10);

            Assert.AreEqual(lecturesList, result);
        }

        [TestMethod]
        public async Task Get_Lectures_ThrowsNotFoundLecturesException()
        {
            var lecturesList = new PagedList<LectureGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<LectureGetDTO> (), HasNextPage = false };
            _lectureRepositoryMock.Setup(repo => repo.GetLectures(1, 10)).ReturnsAsync(lecturesList);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetLectures(1, 10));
        }

        [TestMethod]
        public async Task Get_Lectures_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _lectureRepositoryMock.Setup(repo => repo.GetLectures(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetLectures(-1, 10));
        }

        [TestMethod]
        public async Task Get_Lecture_ReturnsLecture()
        {
            var lecture = new LectureGetDTO();
            var idOfLectureToFind = 1;
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(idOfLectureToFind)).ReturnsAsync(lecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.GetLecture(idOfLectureToFind);

            Assert.AreEqual(lecture, result);
        }

        [TestMethod]
        public async Task Get_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(idOfLectureToFind)).ReturnsAsync((LectureGetDTO)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetLecture(idOfLectureToFind));
        }

        [TestMethod]
        public async Task Get_CourseLectureSubject_ReturnsTrue()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            var foundLecture = new Lecture();
            _lectureRepositoryMock.Setup(repo => repo.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind)).ReturnsAsync(foundLecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public async Task Get_CourseLectureSubject_ReturnsFalse()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            _lectureRepositoryMock.Setup(repo => repo.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.AreEqual(false, false);
        }

        [TestMethod]
        public async Task Post_Lecture_ReturnsAddedLecture()
        {
            var lecturer = new LecturerGetDTO { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.ClassroomId, CourseSubjectsCourseId = courseSubject.CourseId, CourseSubjectsSubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync(courseSubject);
            _classroomServiceMock.Setup(service => service.GetClassroom(lectureToAdd.ClassroomId)).ReturnsAsync(classroom);
            _lectureRepositoryMock.Setup(repo => repo.PostLecture(lectureToAdd)).ReturnsAsync(addedLecture);
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(addedLecture.Id)).ReturnsAsync(addedLectureDTO);
            _lectureRepositoryMock.Setup(repo => repo.GetCourseLectureSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync(new Lecture());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.PostLecture(lectureToAdd);

            Assert.AreEqual(addedLectureDTO, result);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsLectureDateTimeException()
        {
            var lecturer = new LecturerGetDTO { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var lectureToAdd = new LecturePostDTO { ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundLecturerException()
        {
            var lecturer = new LecturerGetDTO { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).ThrowsAsync(new NotFoundLecturerException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundCourseSubjectException()
        {
            var lecturer = new LecturerGetDTO { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.ClassroomId, CourseSubjectsCourseId = courseSubject.CourseId, CourseSubjectsSubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ThrowsAsync(new NotFoundCourseSubjectException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundClassroomtException()
        {
            var lecturer = new LecturerGetDTO { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.ClassroomId, CourseSubjectsCourseId = courseSubject.CourseId, CourseSubjectsSubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync(courseSubject);
            _classroomServiceMock.Setup(service => service.GetClassroom(lectureToAdd.ClassroomId)).ThrowsAsync(new NotFoundClassroomException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowSubjectAlreadyConductedLectureException()
        {
            var lecturer = new LecturerGetDTO { Id = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.ClassroomId, CourseSubjectsCourseId = courseSubject.CourseId, CourseSubjectsSubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.Id };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).ReturnsAsync(lecturer);
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync(courseSubject);
            _classroomServiceMock.Setup(service => service.GetClassroom(lectureToAdd.ClassroomId)).ReturnsAsync(classroom);
            _lectureRepositoryMock.Setup(repo => repo.PostLecture(lectureToAdd)).ReturnsAsync(addedLecture);
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(addedLecture.Id)).ReturnsAsync(addedLectureDTO);
            _lectureRepositoryMock.Setup(repo => repo.GetCourseLectureSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<SubjectAlreadyConductedLectureException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Delete_Lecture_ReturnsLecture()
        {
            var idOfLectureToDelete = 1;
            var deletedlecture = new Lecture { Id = 1, ClassroomId = 1, CourseSubjectsCourseId = 2, CourseSubjectsSubjectId = 3, LecturerId = 4};
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLectureToDelete)).ReturnsAsync(deletedlecture);
            _lectureRepositoryMock.Setup(repo => repo.DeleteLecture(deletedlecture)).ReturnsAsync(deletedlecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.DeleteLecture(idOfLectureToDelete);

            Assert.AreEqual(deletedlecture, result);
        }

        [TestMethod]
        public async Task Delete_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToDelete = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLectureToDelete)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.DeleteLecture(idOfLectureToDelete));
        }

        [TestMethod]
        public async Task Delete_Lecture_PropagatesReferenceConstraintExceptionException()
        {
            var deletedlecture = new Lecture { Id = 1, ClassroomId = 1, CourseSubjectsCourseId = 2, CourseSubjectsSubjectId = 3, LecturerId = 4 };
            var idOfLectureToDelete = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLectureToDelete)).ReturnsAsync(deletedlecture);
            _lectureRepositoryMock.Setup(repo => repo.DeleteLecture(deletedlecture)).ThrowsAsync(new ReferenceConstraintException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteLecture(idOfLectureToDelete));
        }

        [TestMethod]
        public async Task Check_Lecture_ReturnsLecture()
        {
            var lecture = new Lecture { Id = 1, ClassroomId = 1, CourseSubjectsCourseId = 2, CourseSubjectsSubjectId = 3, LecturerId = 4 };
            var idOfLecture = 1; 
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLecture)).ReturnsAsync(lecture);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.CheckLecture(idOfLecture);

            Assert.AreEqual(lecture, result);
        }

        [TestMethod]
        public async Task Check_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLecture = 1;
            _lectureRepositoryMock.Setup(repo => repo.CheckLecture(idOfLecture)).ReturnsAsync((Lecture)null);
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.CheckLecture(idOfLecture));
        }
    }
}
