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
    public class CourseSubjectServiceTests
    {
        private Mock<ICourseSubjectRepository> _courseSubjectRepositoryMock;
        private Mock<ICourseService> _courseServiceMock;
        private Mock<ISubjectService> _subjectServiceMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private CourseSubjectService _service;

        public CourseSubjectServiceTests()
        {
            _fixture = new Fixture();
            _courseSubjectRepositoryMock = new Mock<ICourseSubjectRepository>();
            _courseServiceMock = new Mock<ICourseService>();
            _subjectServiceMock = new Mock<ISubjectService>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_CoursesSubjects_ReturnsCoursesSubjects()
        {
            var courseSubject = new CourseSubjectGetDTO();
            ICollection<CourseSubjectGetDTO> courseSubjectList = new List<CourseSubjectGetDTO>() { courseSubject };
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCoursesSubjects()).ReturnsAsync(courseSubjectList);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCoursesSubjects();

            Assert.AreEqual(courseSubjectList, result);
        }

        [TestMethod]
        public async Task Get_CoursesSubjects_ThrowsNotFoundCourseSubjectException()
        {
            ICollection<CourseSubjectGetDTO> courseSubjectList = new List<CourseSubjectGetDTO>();
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCoursesSubjects()).ReturnsAsync(courseSubjectList);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.GetCoursesSubjects());
        }
        
        [TestMethod]
        public async Task Get_CourseSubject_ReturnsCourseSubject()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            var courseSubject = new CourseSubjectGetDTO();
            var course = new Course();
            var subject = new Subject();
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(idOfCourseToFind, idOfSubjectToFind)).ReturnsAsync(courseSubject);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCourseSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.AreEqual(courseSubject, result);
        }

        [TestMethod]
        public async Task Get_CourseSubject_ThrowsNotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ThrowsAsync(new NotFoundCourseException(idOfCourseToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Get_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var course = new Course();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ThrowsAsync(new NotFoundSubjectException(idOfSubjectToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.GetCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Get_CourseSubject_ThrowsNotFoundCourseSubjectException()
        {
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var course = new Course();
            var subject = new Subject();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(idOfCourseToFind, idOfSubjectToFind)).ThrowsAsync(new NotFoundCourseSubjectException(course.Id, subject.Id));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.GetCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Get_CourseSubjects_ReturnsCourseSubjects()
        {
            var idOfCourseToFind = 1;
            var courseSubjects = new CourseSubjectsGetDTO();
            var course = new Course();
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubjects(idOfCourseToFind)).ReturnsAsync(courseSubjects);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCourseSubjects(idOfCourseToFind);

            Assert.AreEqual(courseSubjects, result);
        }

        [TestMethod]
        public async Task Get_CourseSubjects_ThrowsNotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            var subject = new SubjectGetDTO();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ThrowsAsync(new NotFoundCourseException(idOfCourseToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourseSubjects(idOfCourseToFind));
        }

        [TestMethod]
        public async Task Get_CourseSubjects_ThrowsNotFoundCourseSubjectException()
        {
            var idOfCourseToFind = 1;
            var course = new Course();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubjects(idOfCourseToFind)).ReturnsAsync((CourseSubjectsGetDTO)null);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.GetCourseSubjects(idOfCourseToFind));
        }

        [TestMethod]
        public async Task Check_CourseSubject_ReturnsCourseSubject()
        {
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var courseSubject = new CourseSubject();
            var course = new Course();
            var subject = new Subject();
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubject(idOfCourseToFind, idOfSubjectToFind)).ReturnsAsync(courseSubject);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckCourseSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.AreEqual(courseSubject, result);
        }

        [TestMethod]
        public async Task Check_CourseSubject_ThrowsNotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            var subject = new SubjectGetDTO();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ThrowsAsync(new NotFoundCourseException(idOfCourseToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.CheckCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Check_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var course = new Course();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ThrowsAsync(new NotFoundSubjectException(idOfSubjectToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.CheckCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Check_CourseSubject_ThrowsNotFoundCourseSubjectException()
        {
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var course = new Course();
            var subject = new Subject();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(course.Id, idOfSubjectToFind)).ThrowsAsync(new NotFoundCourseSubjectException(course.Id, subject.Id));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.GetCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]     
        public async Task Post_CourseSubject_ReturnsAddedCourseSubject()
        {
            var addedCourseSubjectDTO = new CourseSubjectResponseDTO {CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var addedCourseSubject = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectToAdd = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var course = new Course { Id = 1 };
            var subject = new Subject { Id = 1 };
            _courseServiceMock.Setup(service => service.CheckCourse(courseSubjectToAdd.CourseId)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(courseSubjectToAdd.SubjectId)).ReturnsAsync(subject);
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId)).ReturnsAsync((CourseSubject)null);
            _mapperMock.Setup(m => m.Map<CourseSubjectResponseDTO>(It.IsAny<CourseSubject>())).Returns(addedCourseSubjectDTO);
            _courseSubjectRepositoryMock.Setup(repo => repo.PostCourseSubject(courseSubjectToAdd)).ReturnsAsync(addedCourseSubject);
            _courseSubjectRepositoryMock.Setup(repo => repo.TakenSeqNumber(courseSubjectToAdd.CourseId, courseSubjectToAdd.SequenceNumber)).ReturnsAsync(false);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            var result = await _service.PostCourseSubject(courseSubjectToAdd);

            Assert.AreEqual(addedCourseSubjectDTO, result);
        }
        
        [TestMethod]
        public async Task Post_CourseSubject_ThrowsSeqNumberMustBeGreaterThanZeroException()
        {
            var courseSubjectToAdd = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = -10 };
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Post_CourseSubject_ThrowsNotFoundCourseException()
        {
            var courseSubjectToAdd = new CourseSubjectRequestDTO { CourseId = 10, SubjectId = 1, SequenceNumber = 10 };
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId)).ThrowsAsync(new NotFoundCourseException(courseSubjectToAdd.CourseId));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);
            
            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Post_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var courseSubjectToAdd = new CourseSubjectRequestDTO { CourseId = 10, SubjectId = 1, SequenceNumber = 10 };
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId)).ThrowsAsync(new NotFoundSubjectException(courseSubjectToAdd.SubjectId));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Post_CourseSubject_ThrowsSubjectAlreadyAssignedToCourseException()
        {
            var existringCourseSubject = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectToAdd = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId)).ReturnsAsync(existringCourseSubject);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<SubjectAlreadyAssignedToCourseException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Post_CourseSubject_ThrowsTakenSeqNumberException()
        {
            var addedCourseSubjectDTO = new CourseSubjectGetDTO { CourseId = 1, SubjectId = 1, SubjectName = "Subject1", Duration = 5, CourseName = "Kurs" };
            var addedCourseSubject = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectToAdd = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var course = new Course { Id = 1 };
            var subject = new Subject { Id = 1 };
            _courseServiceMock.Setup(service => service.CheckCourse(courseSubjectToAdd.CourseId)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(courseSubjectToAdd.SubjectId)).ReturnsAsync(subject);
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubject(courseSubjectToAdd.CourseId, courseSubjectToAdd.SubjectId)).ReturnsAsync((CourseSubject)null);
            _courseSubjectRepositoryMock.Setup(repo => repo.TakenSeqNumber(courseSubjectToAdd.CourseId, courseSubjectToAdd.SequenceNumber)).ReturnsAsync(true);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<TakenSequenceNumberException>(async () => await _service.PostCourseSubject(courseSubjectToAdd));
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ReturnsCourseSubject()
        {
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var courseSubject = new CourseSubject();
            var course = new Course();
            var subject = new Subject();
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubject(idOfCourseToFind, idOfSubjectToFind)).ReturnsAsync(courseSubject);
            _courseSubjectRepositoryMock.Setup(repo => repo.DeleteCourseSubject(courseSubject)).ReturnsAsync(courseSubject);
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            var result = await _service.DeleteCourseSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.AreEqual(courseSubject, result);
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ThrowsNotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 1;
            var subject = new SubjectGetDTO();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ThrowsAsync(new NotFoundCourseException(idOfCourseToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.DeleteCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var course = new Course();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ThrowsAsync(new NotFoundSubjectException(idOfSubjectToFind));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.DeleteCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ThrowsNotFoundCourseSubjectException()
        {
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var course = new Course();
            var subject = new Subject();
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _courseSubjectRepositoryMock.Setup(repo => repo.GetCourseSubject(course.Id, idOfSubjectToFind)).ThrowsAsync(new NotFoundCourseSubjectException(course.Id, subject.Id));
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.DeleteCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Delete_CourseSubject_PropagatesReferenceConstraintExceptionException()
        {
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var courseSubject = new CourseSubject();
            var course = new Course();
            var subject = new Subject();
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubject(idOfCourseToFind, idOfSubjectToFind)).ReturnsAsync(courseSubject);
            _courseSubjectRepositoryMock.Setup(repo => repo.DeleteCourseSubject(courseSubject)).ThrowsAsync(new ReferenceConstraintException());
            _courseServiceMock.Setup(service => service.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _subjectServiceMock.Setup(service => service.CheckSubject(idOfSubjectToFind)).ReturnsAsync(subject);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteCourseSubject(idOfCourseToFind, idOfSubjectToFind));
        }

        [TestMethod]
        public async Task Update_CourseSubject_ReturnsUpdatedCourseSubject()
        {
            var idOfCourse = 1;
            var idOfSubject = 1;
            var courseSubjectToUpdate = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectUpdate = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 3 };
            var updatedCourseSubject = new CourseSubjectResponseDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 3 };
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubjectTracking(courseSubjectUpdate.CourseId, courseSubjectUpdate.SubjectId)).ReturnsAsync(courseSubjectToUpdate);
            _courseSubjectRepositoryMock.Setup(repo => repo.TakenSeqNumber(courseSubjectUpdate.CourseId, courseSubjectUpdate.SequenceNumber)).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<CourseSubjectResponseDTO>(It.IsAny<CourseSubject>())).Returns(updatedCourseSubject);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            var result = await _service.UpdateCourseSubject(idOfCourse, idOfSubject, courseSubjectUpdate);

            Assert.AreEqual(updatedCourseSubject, result);
        }

        [TestMethod]
        public async Task Update_CourseSubject_ThrowsNotFoundCourseSubjectException()
        {
            var idOfCourse = 1;
            var idOfSubject = 1;
            var courseSubjectUpdate = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubjectTracking(courseSubjectUpdate.CourseId, courseSubjectUpdate.SubjectId)).ThrowsAsync(new NotFoundCourseSubjectException());
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseSubjectException>(async () => await _service.UpdateCourseSubject(idOfCourse, idOfSubject, courseSubjectUpdate));
        }

        [TestMethod]
        public async Task Update_CourseSubject_ThrowsSeqNumberMustBeGreaterThanZeroException()
        {
            var idOfCourse = 1;
            var idOfSubject = 1;
            var courseSubjectToUpdate = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectUpdate = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = -1 };
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubjectTracking(courseSubjectUpdate.CourseId, courseSubjectUpdate.SubjectId)).ReturnsAsync(courseSubjectToUpdate);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateCourseSubject(idOfCourse, idOfSubject, courseSubjectUpdate));
        }

        [TestMethod]
        public async Task Update_CourseSubject_ThrowsNotFoundCourseException()
        {
            var idOfCourse = 1;
            var idOfSubject = 1;
            var courseSubjectUpdate = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubjectTracking(courseSubjectUpdate.CourseId, courseSubjectUpdate.SubjectId)).ThrowsAsync(new NotFoundCourseException());
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.UpdateCourseSubject(idOfCourse, idOfSubject, courseSubjectUpdate));
        }

        [TestMethod]
        public async Task Update_CourseSubject_ThrowsNotFoundSubjectException()
        {
            var idOfCourse = 1;
            var idOfSubject = 1;
            var courseSubjectUpdate = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubjectTracking(courseSubjectUpdate.CourseId, courseSubjectUpdate.SubjectId)).ThrowsAsync(new NotFoundSubjectException());
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundSubjectException>(async () => await _service.UpdateCourseSubject(idOfCourse, idOfSubject, courseSubjectUpdate));
        }

        [TestMethod]
        public async Task Update_CourseSubject_ThrowsTakenSeqNumberException()
        {
            var idOfCourse = 1;
            var idOfSubject = 1;
            var courseSubjectToUpdate = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubjectUpdate = new CourseSubjectRequestDTO { CourseId = 1, SubjectId = 1, SequenceNumber = 3 };
            _courseSubjectRepositoryMock.Setup(repo => repo.CheckCourseSubjectTracking(courseSubjectUpdate.CourseId, courseSubjectUpdate.SubjectId)).ReturnsAsync(courseSubjectToUpdate);
            _courseSubjectRepositoryMock.Setup(repo => repo.TakenSeqNumber(courseSubjectUpdate.CourseId, courseSubjectUpdate.SequenceNumber)).ReturnsAsync(true);
            _service = new CourseSubjectService(_courseSubjectRepositoryMock.Object, _courseServiceMock.Object, _subjectServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<TakenSequenceNumberException>(async () => await _service.UpdateCourseSubject(idOfCourse, idOfSubject, courseSubjectUpdate));
        }
    }
}
