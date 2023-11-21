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
    public class CourseSubjectServiceTests
    {
        private Mock<ICourseSubjectRepository> _courseSubjectRepositoryMock;
        private Mock<ICourseService> _courseServiceMock;
        private Mock<ISubjectService> _subjectServiceMock;
        private Fixture _fixture;
        private CourseSubjectService _service;

        public CourseSubjectServiceTests()
        {
            _fixture = new Fixture();
            _courseSubjectRepositoryMock = new Mock<ICourseSubjectRepository>();
            _courseServiceMock = new Mock<ICourseService>();
            _subjectServiceMock = new Mock<ISubjectService>();
        }

        [TestMethod]
        public async Task Get_CoursesSubjects_ReturnsCoursesSubjects()
        {
            var courseSubject = new CourseSubjectGetDTO();
            ICollection<CourseSubjectGetDTO> courseSubjectList = new List<CourseSubjectGetDTO>() { courseSubject };
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCoursesSubjects()).ReturnsAsync(courseSubjectList);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            var result = await _service.GetCoursesSubjects();

            Assert.AreEqual(courseSubjectList, result);
        }

        [TestMethod]
        public async Task Get_CoursesSubjects_ThrowsNotFoundCourseSubjectException()
        {
            ICollection<CourseSubjectGetDTO> courseSubjectList = new List<CourseSubjectGetDTO>();
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCoursesSubjects()).ReturnsAsync(courseSubjectList);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.GetCoursesSubjects());
        }
        
        [TestMethod]
        public async Task Get_CourseSubject_ReturnsCourseSubject()
        {
            var courseSubject = new CourseSubjectGetDTO();
            var course = new CourseGetDTO();
            var subject = new SubjectGetDTO();
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(course.Id, subject.Id)).ReturnsAsync(courseSubject);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.GetSubject(subject.Id)).Returns(Task.FromResult(subject));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            var result = await _service.GetCourseSubject(course.Id, subject.Id);

            Assert.AreEqual(courseSubject, result);
        }

        [TestMethod]
        public async Task Get_CourseSubject_ThrowsNotFoundCourseException()
        {
            var idOfCourseTypeToFind = 1;
            var subject = new SubjectGetDTO();
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseTypeToFind)).ThrowsAsync(new NotFoundCourseException(idOfCourseTypeToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourseSubject(idOfCourseTypeToFind, subject.Id));
        }

        [TestMethod]
        public async Task Get_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            var course = new CourseGetDTO();
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.GetSubject(idOfSubjectToFind)).ThrowsAsync(new NotFoundSubjectException(idOfSubjectToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.GetCourseSubject(course.Id, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Get_CourseSubject_ThrowsNotFoundCourseSubjectException()
        {
            var course = new CourseGetDTO();
            var subject = new SubjectGetDTO();
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.GetSubject(subject.Id)).ReturnsAsync(subject);
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(course.Id, subject.Id)).ThrowsAsync(new NotFoundCourseSubjectException(course.Id, subject.Id));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.GetCourseSubject(course.Id, subject.Id));
        }

        [TestMethod]
        public async Task Get_CourseSubjects_ReturnsCourseSubjects()
        {
            var courseSubjects = new CourseSubjectsGetDTO();
            var course = new CourseGetDTO();
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubjects(course.Id)).ReturnsAsync(courseSubjects);
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).ReturnsAsync(course);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            var result = await _service.GetCourseSubjects(course.Id);

            Assert.AreEqual(courseSubjects, result);
        }

        [TestMethod]
        public async Task Get_CourseSubjects_ThrowsNotFoundCourseException()
        {
            var idOfCourseTypeToFind = 1;
            var subject = new SubjectGetDTO();
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseTypeToFind)).ThrowsAsync(new NotFoundCourseException(idOfCourseTypeToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourseSubjects(idOfCourseTypeToFind));
        }

        [TestMethod]
        public async Task Get_CourseSubjects_ThrowsNotFoundCourseSubjectException()
        {
            var idOfCourseTypeToFind = 1;
            var course = new CourseGetDTO();
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseTypeToFind)).ReturnsAsync(course);
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubjects(idOfCourseTypeToFind)).ReturnsAsync((CourseSubjectsGetDTO)null);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.GetCourseSubjects(idOfCourseTypeToFind));
        }

        [TestMethod]     
        public async Task Post_CourseSubject_ReturnsAddedCourseSubject()
        {
            var addedCourseSubjectDTO = new CourseSubjectGetDTO {CourseId = 1, SubjectId = 1, SubjectName = "Subject1", Duration = 5, CourseName ="Kurs" };
            var addedCourseSubject = new CourseSubject {CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectToAdd = new CourseSubjectPostDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var course = new CourseGetDTO { Id = 1 };
            var subject = new SubjectGetDTO { Id = 1 };
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.GetSubject(courseSubjectToAdd.SubjectId)).ReturnsAsync(subject);
            _courseSubjectRepositoryMock.SetupSequence(repo => repo.GetCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId))
                        .ReturnsAsync((CourseSubjectGetDTO)null)
                        .ReturnsAsync(addedCourseSubjectDTO);
            _courseSubjectRepositoryMock.Setup(repo => repo.PostCourseSubject(courseSubjectToAdd)).ReturnsAsync(addedCourseSubject);
            _courseSubjectRepositoryMock.Setup(repo => repo.TakenSeqNumber(courseSubjectToAdd.CourseId, courseSubjectToAdd.SequenceNumber)).ReturnsAsync(false);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            var result = await _service.PostCourseSubject(courseSubjectToAdd);

            Assert.AreEqual(addedCourseSubjectDTO, result);
        }
        
        [TestMethod]
        public async Task Post_CourseSubject_ThrowsSeqNumberMustBeGreaterThanZero()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO { CourseId = 1, SubjectId = 1, SequenceNumber = -10 };
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Post_CourseSubject_ThrowsNotFoundCourseException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO { CourseId = 10, SubjectId = 1, SequenceNumber = 10 };
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).ThrowsAsync(new NotFoundCourseException(courseSubjectToAdd.CourseId));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);
            
            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Post_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO { CourseId = 10, SubjectId = 1, SequenceNumber = 10 };
            var course = new CourseGetDTO { Id = 1 };
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.GetSubject(courseSubjectToAdd.SubjectId)).ThrowsAsync(new NotFoundSubjectException(courseSubjectToAdd.SubjectId));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Post_CourseSubject_ThrowsSubjectAlreadyAssignedToCourseException()
        {
            var addedCourseSubjectDTO = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1, SubjectName = "Subject1", Duration = 5, CourseName = "Kurs" };
            var addedCourseSubject = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectToAdd = new CourseSubjectPostDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var course = new CourseGetDTO { Id = 1 };
            var subject = new SubjectGetDTO { Id = 1 };
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.GetSubject(courseSubjectToAdd.SubjectId)).ReturnsAsync(subject);
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId)).ReturnsAsync(new CourseSubjectGetDTO());
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<SubjectAlreadyAssignedToCourseException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Post_CourseSubject_ThrowsTakenSeqNumberException()
        {
            var addedCourseSubjectDTO = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1, SubjectName = "Subject1", Duration = 5, CourseName = "Kurs" };
            var addedCourseSubject = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectToAdd = new CourseSubjectPostDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var course = new CourseGetDTO { Id = 1 };
            var subject = new SubjectGetDTO { Id = 1 };
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.GetSubject(courseSubjectToAdd.SubjectId)).ReturnsAsync(subject);
            _courseSubjectRepositoryMock.SetupSequence(repo => repo.GetCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId))
                        .ReturnsAsync((CourseSubjectGetDTO)null)
                        .ReturnsAsync(addedCourseSubjectDTO); 
            _courseSubjectRepositoryMock.Setup(repo => repo.TakenSeqNumber(courseSubjectToAdd.CourseId, courseSubjectToAdd.SequenceNumber)).ReturnsAsync(true);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<TakenSequenceNumberException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }
    }
}
