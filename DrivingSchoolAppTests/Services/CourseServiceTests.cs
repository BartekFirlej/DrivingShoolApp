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
    public class CourseServiceTests
    {
        private Mock<ICourseRepository> _courseRepositoryMock;
        private Mock<ICourseTypeService> _courseTypeServiceMock;
        private Fixture _fixture;
        private CourseService _service;

        public CourseServiceTests()
        {
            _fixture = new Fixture();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _courseTypeServiceMock = new Mock<ICourseTypeService>();
        }

        [TestMethod]
        public async Task Get_Courses_ReturnsCourses()
        {
            var course = new CourseGetDTO();
            var coursesList = new PagedList<CourseGetDTO>() { PageIndex = 1, PageSize = 10, HasNextPage = false, PagedItems = new List<CourseGetDTO> { course } };
            _courseRepositoryMock.Setup(repo => repo.GetCourses(1, 10)).Returns(Task.FromResult(coursesList));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            var result = await _service.GetCourses(1, 10);

            Assert.AreEqual(coursesList, result);
            Assert.AreEqual(coursesList.PagedItems.Count, result.PagedItems.Count);
        }

        [TestMethod]
        public async Task Get_Courses_ThrowsNotFoundCoursesException()
        {
            var coursesList = new PagedList<CourseGetDTO>() { PageIndex = 1, PageSize = 10, HasNextPage = false, PagedItems = new List<CourseGetDTO>()};
            _courseRepositoryMock.Setup(repo => repo.GetCourses(1, 10)).Returns(Task.FromResult(coursesList));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourses(1, 10));
        }

        [TestMethod]
        public async Task Get_Courses_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _courseRepositoryMock.Setup(repo => repo.GetCourses(-1, 10)).Throws(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetCourses(-1, 10));
        }

        [TestMethod]
        public async Task Get_Course_ReturnsCourse()
        {
            var course = new CourseGetDTO();
            var idOfCourseToFind = 1;
            _courseRepositoryMock.Setup(repo => repo.GetCourse(idOfCourseToFind)).Returns(Task.FromResult(course));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            var result = await _service.GetCourse(idOfCourseToFind);

            Assert.AreEqual(course, result);
        }

        [TestMethod]
        public async Task Get_Course_ThrowsNotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            _courseRepositoryMock.Setup(repo => repo.GetCourse(idOfCourseToFind)).Returns(Task.FromResult<CourseGetDTO>(null));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourse(idOfCourseToFind));
        }

        [TestMethod]
        public async Task Get_CourseAssignedPeople_ReturnsAssignedPeople()
        {
            var assignedPeople = 10;
            var idOfCourseToFind = 1;
            var course = new CourseGetDTO();
            _courseRepositoryMock.Setup(repo => repo.GetCourse(idOfCourseToFind)).Returns(Task.FromResult(course));
            _courseRepositoryMock.Setup(repo => repo.GetCourseAssignedPeopleCount(idOfCourseToFind)).Returns(Task.FromResult(assignedPeople));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            var result = await _service.GetCourseAssignedPeopleCount(idOfCourseToFind);

            Assert.AreEqual(assignedPeople, result);
        }

        [TestMethod]
        public async Task Get_CourseAssignedPeople_ThrowsNotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            _courseRepositoryMock.Setup(repo => repo.GetCourse(idOfCourseToFind)).Returns(Task.FromResult<CourseGetDTO>(null));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourseAssignedPeopleCount(idOfCourseToFind));
        }

        [TestMethod]
        public async Task Post_Course_ReturnsAddedCourse()
        {
            var courseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var addedCourseDTO = new CourseGetDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", CourseType = courseTypeDTO, BeginDate = new DateTime(2023, 1, 1) };
            var addedCourse = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = DateTime.Now, CourseTypeId = courseTypeDTO.Id };
            var courseToAdd = new CoursePostDTO { Limit = 10, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = 1 };
            _courseTypeServiceMock.Setup(service => service.GetCourseType(courseToAdd.CourseTypeId)).Returns(Task.FromResult(courseTypeDTO));
            _courseRepositoryMock.Setup(repo => repo.PostCourse(courseToAdd)).Returns(Task.FromResult(addedCourse));
            _courseRepositoryMock.Setup(repo => repo.GetCourse(addedCourse.Id)).Returns(Task.FromResult(addedCourseDTO));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            var result = await _service.PostCourse(courseToAdd);

            Assert.AreEqual(addedCourseDTO, result);
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNotFoundCourseTypeException()
        {
            var courseToAdd = new CoursePostDTO { Limit = 10, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = 1 };
            _courseTypeServiceMock.Setup(service => service.GetCourseType(courseToAdd.CourseTypeId)).Throws(new NotFoundCourseTypeException(courseToAdd.CourseTypeId));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.PostCourse(courseToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsPriceMustBeGreaterThanZeroException()
        {
            var courseToAdd = new CoursePostDTO { Limit = 10, Price = 0, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = 1 };
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourse(courseToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsLimitMustBeGreaterThanZeroException()
        {
            var courseToAdd = new CoursePostDTO { Limit = 0, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = 1 };
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourse(courseToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNotGivenBeinDateTimeException()
        {
            var courseToAdd = new CoursePostDTO { Limit = 0, Price = 10, Name = "Kurs kat. B", CourseTypeId = 1 };
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostCourse(courseToAdd));
        }
    }
}
