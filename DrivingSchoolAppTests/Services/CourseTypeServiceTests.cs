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
    public class CourseTypeServiceTests
    {
        private Mock<ICourseTypeRepository> _courseTypeRepositoryMock;
        private Mock<ILicenceCategoryService> _licenceCategoryServiceMock;
        private Fixture _fixture;
        private CourseTypeService _service;

        public CourseTypeServiceTests()
        {
            _fixture = new Fixture();
            _courseTypeRepositoryMock = new Mock<ICourseTypeRepository>();
            _licenceCategoryServiceMock = new Mock<ILicenceCategoryService>();
        }

        [TestMethod]
        public async Task Get_CourseTypes_ReturnsCourseTypes()
        {
            var courseType = new CourseTypeGetDTO();
            ICollection<CourseTypeGetDTO> courseTypesList = new List<CourseTypeGetDTO>() { courseType };
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseTypes()).Returns(Task.FromResult(courseTypesList));
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.GetCourseTypes();

            Assert.AreEqual(courseTypesList, result);
        }

        [TestMethod]
        public async Task Get_CourseTypes_ThrowsNotFoundCourseTypesException()
        {
            ICollection<CourseTypeGetDTO> courseTypesList = new List<CourseTypeGetDTO>();
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseTypes()).Returns(Task.FromResult(courseTypesList));
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.GetCourseTypes());
        }

        [TestMethod]
        public async Task Get_CourseType_ReturnsCourseType()
        {
            var courseType = new CourseTypeGetDTO();
            var idOfCourseTypeToFind = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseType(idOfCourseTypeToFind)).Returns(Task.FromResult(courseType));
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.GetCourseType(idOfCourseTypeToFind);

            Assert.AreEqual(courseType, result);
        }

        [TestMethod]
        public async Task Get_CourseType_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseTypeToFind = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseType(idOfCourseTypeToFind)).Returns(Task.FromResult<CourseTypeGetDTO>(null));
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.GetCourseType(idOfCourseTypeToFind));
        }

        [TestMethod]
        public async Task Post_CourseType_ReturnsAddedCourseType()
        {
            var addedCourseTypeDTO = new CourseTypeGetDTO { Id = 1, DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var addedCourseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var courseTypeToAdd = new CourseTypePostDTO { DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var licenceCategoryDTO = new LicenceCategoryGetDTO();
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(courseTypeToAdd.LicenceCategoryId)).Returns(Task.FromResult(licenceCategoryDTO));
            _courseTypeRepositoryMock.Setup(repo => repo.PostCourseType(courseTypeToAdd)).Returns(Task.FromResult(addedCourseType));
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseType(addedCourseType.Id)).Returns(Task.FromResult(addedCourseTypeDTO));
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            var result = await _service.PostCourseType(courseTypeToAdd);

            Assert.AreEqual(addedCourseTypeDTO, result);
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNotFoundLicenceCategoryException()
        {
            var courseTypeToAdd = new CourseTypePostDTO { DrivingHours = 10, LecturesHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            _licenceCategoryServiceMock.Setup(service => service.GetLicenceCategory(courseTypeToAdd.LicenceCategoryId)).Throws(new NotFoundLicenceCategoryException(courseTypeToAdd.LicenceCategoryId));
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.PostCourseType(courseTypeToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsLectureHoursMustBeGreaterThanZeroException()
        {
            var courseTypeToAdd = new CourseTypePostDTO { DrivingHours = 10, LecturesHours = -10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourseType(courseTypeToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsDrivingHoursMustBeGreaterThanZeroException()
        {
            var courseTypeToAdd = new CourseTypePostDTO { DrivingHours = -10, LecturesHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourseType(courseTypeToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsMinimumAgeMustBeGreaterThanZeroException()
        {
            var courseTypeToAdd = new CourseTypePostDTO { DrivingHours = 10, LecturesHours = 10, MinimumAge = 0, Name = "Kurs", LicenceCategoryId = 1 };
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourseType(courseTypeToAdd));
        }
    }
}
