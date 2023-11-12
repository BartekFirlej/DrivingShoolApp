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
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCoursesSubjects()).Returns(Task.FromResult(courseSubjectList));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            var result = await _service.GetCoursesSubjects();

            Assert.AreEqual(courseSubjectList, result);
        }

        [TestMethod]
        public async Task Get_CoursesSubjects_ThrowsNotFoundCourseSubjectException()
        {
            ICollection<CourseSubjectGetDTO> courseSubjectList = new List<CourseSubjectGetDTO>();
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCoursesSubjects()).Returns(Task.FromResult(courseSubjectList));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.GetCoursesSubjects());
        }
        
        [TestMethod]
        public async Task Get_CourseSubject_ReturnsCourseSubject()
        {
            var courseSubject = new CourseSubjectGetDTO();
            var course = new CourseGetDTO();
            var subject = new SubjectGetDTO();
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(course.Id, subject.Id)).Returns(Task.FromResult(courseSubject));
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).Returns(Task.FromResult(course));
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
            _courseServiceMock.Setup(service => service.GetCourse(idOfCourseTypeToFind)).Throws(new NotFoundCourseException(idOfCourseTypeToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourseSubject(idOfCourseTypeToFind, subject.Id));
        }

        [TestMethod]
        public async Task Get_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            var course = new CourseGetDTO();
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).Returns(Task.FromResult(course));
            _subjectServiceMock.Setup(service => service.GetSubject(idOfSubjectToFind)).Throws(new NotFoundSubjectException(idOfSubjectToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.GetCourseSubject(course.Id, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Get_CourseSubject_ThrowsNotFoundCourseSubjectException()
        {
            var course = new CourseGetDTO();
            var subject = new SubjectGetDTO();
            _courseServiceMock.Setup(service => service.GetCourse(course.Id)).Returns(Task.FromResult(course));
            _subjectServiceMock.Setup(service => service.GetSubject(subject.Id)).Returns(Task.FromResult(subject));
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(course.Id, subject.Id)).Throws(new NotFoundCourseSubjectException(course.Id, subject.Id));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.GetCourseSubject(course.Id, subject.Id));
        }

        
        
        [TestMethod]     
        public async Task Post_CourseSubject_ReturnsAddedCourseSubject()
        {
            var addedCourseSubjectDTO = new CourseSubjectGetDTO {CourseId = 1, SubjectId = 1, SubjectName = "Subject1", Duration = 5, CourseName ="Kurs" };
            var addedCourseSubject = new CourseSubject {CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectToAdd = new CourseSubjectPostDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var course = new CourseGetDTO { Id = 1 };
            var subject = new SubjectGetDTO { Id = 1 };
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).Returns(Task.FromResult(course));
            _subjectServiceMock.Setup(service => service.GetSubject(courseSubjectToAdd.SubjectId)).Returns(Task.FromResult(subject));
            _courseSubjectRepositoryMock.SetupSequence(repo => repo.GetCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId))
                        .Returns(Task.FromResult<CourseSubjectGetDTO>(null))
                        .Returns(Task.FromResult(addedCourseSubjectDTO));
            _courseSubjectRepositoryMock.Setup(repo => repo.PostCourseSubject(courseSubjectToAdd)).Returns(Task.FromResult(addedCourseSubject));
            _courseSubjectRepositoryMock.Setup(repo => repo.TakenSeqNumber(courseSubjectToAdd.CourseId, courseSubjectToAdd.SequenceNumber)).Returns(Task.FromResult(false));
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
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).Throws(new NotFoundCourseException(courseSubjectToAdd.CourseId));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);
            
            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Post_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var courseSubjectToAdd = new CourseSubjectPostDTO { CourseId = 10, SubjectId = 1, SequenceNumber = 10 };
            var course = new CourseGetDTO { Id = 1 };
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).Returns(Task.FromResult(course));
            _subjectServiceMock.Setup(service => service.GetSubject(courseSubjectToAdd.SubjectId)).Throws(new NotFoundSubjectException(courseSubjectToAdd.SubjectId));
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
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).Returns(Task.FromResult(course));
            _subjectServiceMock.Setup(service => service.GetSubject(courseSubjectToAdd.SubjectId)).Returns(Task.FromResult(subject));
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId)).Returns(Task.FromResult(new CourseSubjectGetDTO()));
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
            _courseServiceMock.Setup(service => service.GetCourse(courseSubjectToAdd.CourseId)).Returns(Task.FromResult(course));
            _subjectServiceMock.Setup(service => service.GetSubject(courseSubjectToAdd.SubjectId)).Returns(Task.FromResult(subject));
            _courseSubjectRepositoryMock.SetupSequence(repo => repo.GetCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId))
                        .Returns(Task.FromResult<CourseSubjectGetDTO>(null))
                        .Returns(Task.FromResult(addedCourseSubjectDTO)); 
            _courseSubjectRepositoryMock.Setup(repo => repo.TakenSeqNumber(courseSubjectToAdd.CourseId, courseSubjectToAdd.SequenceNumber)).Returns(Task.FromResult(true));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object);

            await Assert.ThrowsExceptionAsync<TakenSequenceNumberException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }
    }
}
