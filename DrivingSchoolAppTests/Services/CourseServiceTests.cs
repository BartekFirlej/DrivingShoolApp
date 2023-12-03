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
    public class CourseServiceTests
    {
        private Mock<ICourseRepository> _courseRepositoryMock;
        private Mock<ICourseTypeService> _courseTypeServiceMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private CourseService _service;

        public CourseServiceTests()
        {
            _fixture = new Fixture();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _courseTypeServiceMock = new Mock<ICourseTypeService>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_Courses_ReturnsCourses()
        {
            var course = new CourseGetDTO();
            var coursesList = new PagedList<CourseGetDTO>() { PageIndex = 1, PageSize = 10, HasNextPage = false, PagedItems = new List<CourseGetDTO> { course } };
            _courseRepositoryMock.Setup(repo => repo.GetCourses(1, 10)).ReturnsAsync(coursesList);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCourses(1, 10);

            Assert.AreEqual(coursesList, result);
            Assert.AreEqual(coursesList.PagedItems.Count, result.PagedItems.Count);
        }

        [TestMethod]
        public async Task Get_Courses_ThrowsNotFoundCoursesException()
        {
            var coursesList = new PagedList<CourseGetDTO>() { PageIndex = 1, PageSize = 10, HasNextPage = false, PagedItems = new List<CourseGetDTO>()};
            _courseRepositoryMock.Setup(repo => repo.GetCourses(1, 10)).ReturnsAsync(coursesList);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourses(1, 10));
        }

        [TestMethod]
        public async Task Get_Courses_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _courseRepositoryMock.Setup(repo => repo.GetCourses(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetCourses(-1, 10));
        }

        [TestMethod]
        public async Task Get_Course_ReturnsCourse()
        {
            var course = new CourseGetDTO();
            var idOfCourseToFind = 1;
            _courseRepositoryMock.Setup(repo => repo.GetCourse(idOfCourseToFind)).ReturnsAsync(course);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCourse(idOfCourseToFind);

            Assert.AreEqual(course, result);
        }

        [TestMethod]
        public async Task Get_Course_ThrowsNotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            _courseRepositoryMock.Setup(repo => repo.GetCourse(idOfCourseToFind)).ReturnsAsync((CourseGetDTO)null);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourse(idOfCourseToFind));
        }

        [TestMethod]
        public async Task Get_CourseAssignedPeople_ReturnsAssignedPeople()
        {
            var assignedPeople = 10;
            var idOfCourseToFind = 1;
            var course = new Course();
            _courseRepositoryMock.Setup(repo => repo.CheckCourse(idOfCourseToFind)).ReturnsAsync(course);
            _courseRepositoryMock.Setup(repo => repo.GetCourseAssignedPeopleCount(idOfCourseToFind)).ReturnsAsync(assignedPeople);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCourseAssignedPeopleCount(idOfCourseToFind);

            Assert.AreEqual(assignedPeople, result);
        }

        [TestMethod]
        public async Task Get_CourseAssignedPeople_ThrowsNotFoundCourseException()
        {
            var idOfCourseToFind = 1;
            _courseRepositoryMock.Setup(repo => repo.CheckCourse(idOfCourseToFind)).ReturnsAsync((Course)null);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.GetCourseAssignedPeopleCount(idOfCourseToFind));
        }

        [TestMethod]
        public async Task Post_Course_ReturnsAddedCourse()
        {
            var courseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var addedCourseDTO = new CourseResponseDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = new DateTime(2023, 1, 1), CourseTypeId = 1 };
            var addedCourse = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = DateTime.Now, CourseTypeId = courseType.Id };
            var courseToAdd = new CourseRequestDTO { Limit = 10, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = 1 };
            _courseTypeServiceMock.Setup(service => service.CheckCourseType(courseToAdd.CourseTypeId)).ReturnsAsync(courseType);
            _courseRepositoryMock.Setup(repo => repo.PostCourse(courseToAdd)).ReturnsAsync(addedCourse);
            _mapperMock.Setup(m => m.Map<CourseResponseDTO>(It.IsAny<Course>())).Returns(addedCourseDTO);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            var result = await _service.PostCourse(courseToAdd);

            Assert.AreEqual(addedCourseDTO, result);
        }

        [TestMethod]
        public async Task Post_Course_ThrowsNotFoundCourseTypeException()
        {
            var courseToAdd = new CourseRequestDTO { Limit = 10, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = 1 };
            _courseTypeServiceMock.Setup(service => service.CheckCourseType(courseToAdd.CourseTypeId)).ThrowsAsync(new NotFoundCourseTypeException(courseToAdd.CourseTypeId));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.PostCourse(courseToAdd));
        }

        [TestMethod]
        public async Task Post_Course_ThrowsPriceMustBeGreaterThanZeroException()
        {
            var courseToAdd = new CourseRequestDTO { Limit = 10, Price = 0, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = 1 };
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourse(courseToAdd));
        }

        [TestMethod]
        public async Task Post_Course_ThrowsLimitMustBeGreaterThanZeroException()
        {
            var courseToAdd = new CourseRequestDTO { Limit = 0, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = 1 };
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourse(courseToAdd));
        }

        [TestMethod]
        public async Task Post_Course_ThrowsNotGivenBeinDateTimeException()
        {
            var courseToAdd = new CourseRequestDTO { Limit = 0, Price = 10, Name = "Kurs kat. B", CourseTypeId = 1 };
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.PostCourse(courseToAdd));
        }

        [TestMethod]
        public async Task Delete_Course_ReturnsCourse()
        {
            var deletedCourse = new Course();
            var idOfCourseToDelete = 1;
            _courseRepositoryMock.Setup(repo => repo.CheckCourse(idOfCourseToDelete)).ReturnsAsync(deletedCourse);
            _courseRepositoryMock.Setup(repo => repo.DeleteCourse(deletedCourse)).ReturnsAsync(deletedCourse);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            var result = await _service.DeleteCourse(idOfCourseToDelete);

            Assert.AreEqual(deletedCourse, result);
        }

        [TestMethod]
        public async Task Delete_Course_ThrowsNotFoundCourseException()
        {
            var idOfCourseToDelete = 1;
            _courseRepositoryMock.Setup(repo => repo.CheckCourse(idOfCourseToDelete)).ReturnsAsync((Course)null);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.DeleteCourse(idOfCourseToDelete));
        }

        [TestMethod]
        public async Task Delete_Course_PropagatesReferenceConstraintExceptionException()
        {
            var deletedCourse = new Course();
            var idOfCourseToDelete = 1;
            _courseRepositoryMock.Setup(repo => repo.CheckCourse(idOfCourseToDelete)).ReturnsAsync(deletedCourse);
            _courseRepositoryMock.Setup(repo => repo.DeleteCourse(deletedCourse)).ThrowsAsync(new ReferenceConstraintException());
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteCourse(idOfCourseToDelete));
        }

        [TestMethod]
        public async Task Check_Course_ReturnsCourse()
        {
            var course = new Course();
            var idOfCourse = 1;
            _courseRepositoryMock.Setup(repo => repo.CheckCourse(idOfCourse)).ReturnsAsync(course);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckCourse(idOfCourse);

            Assert.AreEqual(course, result);
        }

        [TestMethod]
        public async Task Check_Course_ThrowsNotFoundCourseException()
        {
            var idOfCourse = 1;
            _courseRepositoryMock.Setup(repo => repo.CheckCourse(idOfCourse)).ReturnsAsync((Course)null);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.CheckCourse(idOfCourse));
        }

        [TestMethod]
        public async Task Check_CourseTracking_ReturnsCourse()
        {
            var course = new Course();
            var idOfCourse = 1;
            _courseRepositoryMock.Setup(repo => repo.CheckCourseTracking(idOfCourse)).ReturnsAsync(course);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckCourseTracking(idOfCourse);

            Assert.AreEqual(course, result);
        }

        [TestMethod]
        public async Task Check_CourseTracking_ThrowsNotFoundCourseException()
        {
            var idOfCourse = 1;
            _courseRepositoryMock.Setup(repo => repo.CheckCourseTracking(idOfCourse)).ReturnsAsync((Course)null);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.CheckCourse(idOfCourse));
        }

        [TestMethod]
        public async Task Update_Course_ReturnsCourse()
        {
            var idOfCourse = 1;
            var courseType = new CourseType { Id = 1, Name = "Test" };
            var updatedCourseDTO = new CourseResponseDTO { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = new DateTime(2023, 1, 1), CourseTypeId = courseType.Id };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = DateTime.Now, CourseTypeId = courseType.Id };
            var courseUpdate = new CourseRequestDTO { Limit = 2, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = courseType.Id };
            _courseRepositoryMock.Setup(repo => repo.CheckCourseTracking(idOfCourse)).ReturnsAsync(course);
            _courseTypeServiceMock.Setup(service => service.CheckCourseType(courseType.Id)).ReturnsAsync(courseType);
            _courseRepositoryMock.Setup(repo => repo.UpdateCourse(course, courseUpdate)).ReturnsAsync(course);
            _mapperMock.Setup(m => m.Map<CourseResponseDTO>(It.IsAny<Course>())).Returns(updatedCourseDTO);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            var result = await _service.UpdateCourse(idOfCourse, courseUpdate);

            Assert.AreEqual(updatedCourseDTO, result);
        }

        [TestMethod]
        public async Task Update_Course_ThrowsNotFoundCourseException()
        {
            var idOfCourse = 1;
            var courseType = new CourseType { Id = 1, Name = "Test" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = DateTime.Now, CourseTypeId = courseType.Id };
            var courseUpdate = new CourseRequestDTO { Limit = 2, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = courseType.Id };
            _courseRepositoryMock.Setup(repo => repo.CheckCourseTracking(idOfCourse)).ReturnsAsync((Course)null);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseException>(async () => await _service.UpdateCourse(idOfCourse, courseUpdate));
        }

        [TestMethod]
        public async Task Update_Course_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourse = 1;
            var courseType = new CourseType { Id = 1, Name = "Test" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = DateTime.Now, CourseTypeId = courseType.Id };
            var courseUpdate = new CourseRequestDTO { Limit = 2, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = courseType.Id };
            _courseRepositoryMock.Setup(repo => repo.CheckCourseTracking(idOfCourse)).ReturnsAsync(course);
            _courseTypeServiceMock.Setup(service => service.CheckCourseType(courseType.Id)).ThrowsAsync(new NotFoundCourseTypeException(courseType.Id));
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.UpdateCourse(idOfCourse, courseUpdate));
        }

        [TestMethod]
        public async Task Update_Course_ThrowsPriceMustBeGreaterThanZeroException()
        {
            var idOfCourse = 1;
            var courseType = new CourseType { Id = 1, Name = "Test" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = DateTime.Now, CourseTypeId = courseType.Id };
            var courseUpdate = new CourseRequestDTO { Limit = 2, Price = -10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = courseType.Id };
            _courseRepositoryMock.Setup(repo => repo.CheckCourseTracking(idOfCourse)).ReturnsAsync(course);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateCourse(idOfCourse, courseUpdate));
        }

        [TestMethod]
        public async Task Update_Course_ThrowsLimitMustBeGreaterThanZeroException()
        {
            var idOfCourse = 1;
            var courseType = new CourseType { Id = 1, Name = "Test" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = DateTime.Now, CourseTypeId = courseType.Id };
            var courseUpdate = new CourseRequestDTO { Limit = -2, Price = 10, BeginDate = new DateTime(2023, 1, 1), Name = "Kurs kat. B", CourseTypeId = courseType.Id };
            _courseRepositoryMock.Setup(repo => repo.CheckCourseTracking(idOfCourse)).ReturnsAsync(course);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateCourse(idOfCourse, courseUpdate));
        }

        [TestMethod]
        public async Task Update_Course_ThrowsNotGivenBeinDateTimeException()
        {
            var idOfCourse = 1;
            var courseType = new CourseType { Id = 1, Name = "Test" };
            var course = new Course { Id = 1, Limit = 10, Price = 1000, Name = "Kurs kat. B", BeginDate = DateTime.Now, CourseTypeId = courseType.Id };
            var courseUpdate = new CourseRequestDTO { Limit = -2, Price = 10, Name = "Kurs kat. B", CourseTypeId = courseType.Id };
            _courseRepositoryMock.Setup(repo => repo.CheckCourseTracking(idOfCourse)).ReturnsAsync(course);
            _service = new CourseService(_courseRepositoryMock.Object, _courseTypeServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<DateTimeException>(async () => await _service.UpdateCourse(idOfCourse, courseUpdate));
        }
    }
}
