using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ICollection<LectureGetDTO> lecturesList = new List<LectureGetDTO>() { lecture };
            _lectureRepositoryMock.Setup(repo => repo.GetLectures()).Returns(Task.FromResult(lecturesList));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.GetLectures();

            Assert.AreEqual(lecturesList, result);
        }

        [TestMethod]
        public async Task Get_Lectures_ThrowsNotFoundLecturesException()
        {
            ICollection<LectureGetDTO> lecturesList = new List<LectureGetDTO>();
            _lectureRepositoryMock.Setup(repo => repo.GetLectures()).Returns(Task.FromResult(lecturesList));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetLectures());
        }

        [TestMethod]
        public async Task Get_Lecture_ReturnsLecture()
        {
            var lecture = new LectureGetDTO();
            var idOfLectureToFind = 1;
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(idOfLectureToFind)).Returns(Task.FromResult(lecture));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.GetLecture(idOfLectureToFind);

            Assert.AreEqual(lecture, result);
        }

        [TestMethod]
        public async Task Get_Lecture_ThrowsNotFoundLectureException()
        {
            var idOfLectureToFind = 1;
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(idOfLectureToFind)).Returns(Task.FromResult<LectureGetDTO>(null));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLectureException>(async () => await _service.GetLecture(idOfLectureToFind));
        }

        [TestMethod]
        public async Task Get_CourseLectureSubject_ReturnsTrue()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            var foundLecture = new Lecture();
            _lectureRepositoryMock.Setup(repo => repo.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind)).Returns(Task.FromResult(foundLecture));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public async Task Get_CourseLectureSubject_ReturnsFalse()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            _lectureRepositoryMock.Setup(repo => repo.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind)).Returns(Task.FromResult<Lecture>(null));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.AreEqual(false, false);
        }

        [TestMethod]
        public async Task Post_Lecture_ReturnsAddedLecture()
        {
            var lecturer = new LecturerGetDTO { ID = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.ClassroomId, CourseSubjectsCourseId = courseSubject.CourseId, CourseSubjectsSubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).Returns(Task.FromResult(lecturer));
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).Returns(Task.FromResult(courseSubject));
            _classroomServiceMock.Setup(service => service.GetClassroom(lectureToAdd.ClassroomId)).Returns(Task.FromResult(classroom));
            _lectureRepositoryMock.Setup(repo => repo.PostLecture(lectureToAdd)).Returns(Task.FromResult(addedLecture));
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(addedLecture.Id)).Returns(Task.FromResult(addedLectureDTO));
            _lectureRepositoryMock.Setup(repo => repo.GetCourseLectureSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).Returns(Task.FromResult(new Lecture()));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            var result = await _service.PostLecture(lectureToAdd);

            Assert.AreEqual(addedLectureDTO, result);
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsLectureDateTimeException()
        {
            var lecturer = new LecturerGetDTO { ID = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var lectureToAdd = new LecturePostDTO { ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundLecturerException()
        {
            var lecturer = new LecturerGetDTO { ID = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).Throws(new NotFoundLecturerException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLecturerException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundCourseSubjectException()
        {
            var lecturer = new LecturerGetDTO { ID = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.ClassroomId, CourseSubjectsCourseId = courseSubject.CourseId, CourseSubjectsSubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).Returns(Task.FromResult(lecturer));
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).Throws(new NotFoundCourseSubjectException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowsNotFoundClassroomtException()
        {
            var lecturer = new LecturerGetDTO { ID = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.ClassroomId, CourseSubjectsCourseId = courseSubject.CourseId, CourseSubjectsSubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).Returns(Task.FromResult(lecturer));
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).Returns(Task.FromResult(courseSubject));
            _classroomServiceMock.Setup(service => service.GetClassroom(lectureToAdd.ClassroomId)).Throws(new NotFoundClassroomException());
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.PostLecture(lectureToAdd));
        }

        [TestMethod]
        public async Task Post_Lecture_ThrowSubjectAlreadyConductedLectureException()
        {
            var lecturer = new LecturerGetDTO { ID = 1, Name = "Test", SecondName = "Test" };
            var courseSubject = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1 };
            var classroom = new ClassroomGetDTO { ClassroomId = 1 };
            var addedLecture = new Lecture { Id = 1, ClassroomId = classroom.ClassroomId, CourseSubjectsCourseId = courseSubject.CourseId, CourseSubjectsSubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            var addedLectureDTO = new LectureGetDTO { Id = 1, ClassroomNumber = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            var lectureToAdd = new LecturePostDTO { LectureDate = new DateTime(2022, 1, 1), ClassroomId = classroom.ClassroomId, CourseId = courseSubject.CourseId, SubjectId = courseSubject.SubjectId, LecturerId = lecturer.ID };
            _lecturerServiceMock.Setup(service => service.GetLecturer(lectureToAdd.LecturerId)).Returns(Task.FromResult(lecturer));
            _courseSubjectServiceMock.Setup(service => service.GetCourseSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).Returns(Task.FromResult(courseSubject));
            _classroomServiceMock.Setup(service => service.GetClassroom(lectureToAdd.ClassroomId)).Returns(Task.FromResult(classroom));
            _lectureRepositoryMock.Setup(repo => repo.PostLecture(lectureToAdd)).Returns(Task.FromResult(addedLecture));
            _lectureRepositoryMock.Setup(repo => repo.GetLecture(addedLecture.Id)).Returns(Task.FromResult(addedLectureDTO));
            _lectureRepositoryMock.Setup(repo => repo.GetCourseLectureSubject(lectureToAdd.CourseId, lectureToAdd.SubjectId)).Returns(Task.FromResult<Lecture>(null));
            _service = new LectureService(_lectureRepositoryMock.Object, _lecturerServiceMock.Object, _courseSubjectServiceMock.Object, _classroomServiceMock.Object);

            await Assert.ThrowsExceptionAsync<SubjectAlreadyConductedLectureException>(async () => await _service.PostLecture(lectureToAdd));
        }
    }
}
