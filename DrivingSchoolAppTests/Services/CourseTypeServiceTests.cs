﻿using AutoFixture;
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
    public class CourseTypeServiceTests
    {
        private Mock<ICourseTypeRepository> _courseTypeRepositoryMock;
        private Mock<ILicenceCategoryService> _licenceCategoryServiceMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private CourseTypeService _service;

        public CourseTypeServiceTests()
        {
            _fixture = new Fixture();
            _courseTypeRepositoryMock = new Mock<ICourseTypeRepository>();
            _licenceCategoryServiceMock = new Mock<ILicenceCategoryService>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_CourseTypes_ReturnsCourseTypes()
        {
            var courseType = new CourseTypeGetDTO();
            var courseTypesList = new PagedList<CourseTypeGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<CourseTypeGetDTO> { courseType }, HasNextPage = false };
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseTypes(1, 10)).ReturnsAsync(courseTypesList);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCourseTypes(1, 10);

            Assert.AreEqual(courseTypesList, result);
        }

        [TestMethod]
        public async Task Get_CourseTypes_ThrowsNotFoundCourseTypesException()
        {
            var courseTypesList = new PagedList<CourseTypeGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<CourseTypeGetDTO>(), HasNextPage = false };
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseTypes(1, 10)).ReturnsAsync(courseTypesList);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.GetCourseTypes(1, 10));
        }

        [TestMethod]
        public async Task Get_CourseTypes_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseTypes(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetCourseTypes(-1, 10));
        }

        [TestMethod]
        public async Task Get_CourseType_ReturnsCourseType()
        {
            var courseType = new CourseTypeGetDTO();
            var idOfCourseTypeToFind = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseType(idOfCourseTypeToFind)).ReturnsAsync(courseType);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetCourseType(idOfCourseTypeToFind);

            Assert.AreEqual(courseType, result);
        }

        [TestMethod]
        public async Task Get_CourseType_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseTypeToFind = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.GetCourseType(idOfCourseTypeToFind)).ReturnsAsync((CourseTypeGetDTO)null);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.GetCourseType(idOfCourseTypeToFind));
        }

        [TestMethod]
        public async Task Post_CourseType_ReturnsAddedCourseType()
        {
            var addedCourseTypeDTO = new CourseTypeResponseDTO { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var addedCourseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var courseTypeToAdd = new CourseTypeRequestDTO { DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var licenceCategoryDTO = new LicenceCategory();
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(courseTypeToAdd.LicenceCategoryId)).ReturnsAsync(licenceCategoryDTO);
            _courseTypeRepositoryMock.Setup(repo => repo.PostCourseType(courseTypeToAdd)).ReturnsAsync(addedCourseType);
            _mapperMock.Setup(m => m.Map<CourseTypeResponseDTO>(It.IsAny<CourseType>())).Returns(addedCourseTypeDTO);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.PostCourseType(courseTypeToAdd);

            Assert.AreEqual(addedCourseTypeDTO, result);
        }

        [TestMethod]
        public async Task Post_CourseType_ThrowsNotFoundLicenceCategoryException()
        {
            var courseTypeToAdd = new CourseTypeRequestDTO { DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(courseTypeToAdd.LicenceCategoryId)).ThrowsAsync(new NotFoundLicenceCategoryException(courseTypeToAdd.LicenceCategoryId));
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.PostCourseType(courseTypeToAdd));
        }

        [TestMethod]
        public async Task Post_CourseType_ThrowsLectureHoursMustBeGreaterThanZeroException()
        {
            var courseTypeToAdd = new CourseTypeRequestDTO { DrivingHours = 10, LectureHours = -10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourseType(courseTypeToAdd));
        }

        [TestMethod]
        public async Task Post_CourseType_ThrowsDrivingHoursMustBeGreaterThanZeroException()
        {
            var courseTypeToAdd = new CourseTypeRequestDTO { DrivingHours = -10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourseType(courseTypeToAdd));
        }

        [TestMethod]
        public async Task Post_CourseType_ThrowsMinimumAgeMustBeGreaterThanZeroException()
        {
            var courseTypeToAdd = new CourseTypeRequestDTO { DrivingHours = 10, LectureHours = 10, MinimumAge = 0, Name = "Kurs", LicenceCategoryId = 1 };
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostCourseType(courseTypeToAdd));
        }

        [TestMethod]
        public async Task Delete_CourseType_ReturnsCourseType()
        {
            var deletedCourseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var idOfCourseTypeToDelete = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseType(idOfCourseTypeToDelete)).ReturnsAsync(deletedCourseType);
            _courseTypeRepositoryMock.Setup(repo => repo.DeleteCourseType(deletedCourseType)).ReturnsAsync(deletedCourseType);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.DeleteCourseType(idOfCourseTypeToDelete);

            Assert.AreEqual(deletedCourseType, result);
        }

        [TestMethod]
        public async Task Delete_CourseType_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseTypeToDelete = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseType(idOfCourseTypeToDelete)).ReturnsAsync((CourseType)null);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.DeleteCourseType(idOfCourseTypeToDelete));
        }

        [TestMethod]
        public async Task Delete_CourseType_PropagatesReferenceConstraintExceptionException()
        {
            var deletedCourseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var idOfCourseTypeToDelete = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseType(idOfCourseTypeToDelete)).ReturnsAsync(deletedCourseType);
            _courseTypeRepositoryMock.Setup(repo => repo.DeleteCourseType(deletedCourseType)).ThrowsAsync(new ReferenceConstraintException());
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteCourseType(idOfCourseTypeToDelete));
        }

        [TestMethod]
        public async Task Check_CourseType_ReturnsCourseType()
        {
            var courseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var idOfCourseType = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseType(idOfCourseType)).ReturnsAsync(courseType);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckCourseType(idOfCourseType);

            Assert.AreEqual(courseType, result);
        }

        [TestMethod]
        public async Task Check_CourseType_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseType = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseType(idOfCourseType)).ReturnsAsync((CourseType)null);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.CheckCourseType(idOfCourseType));
        }

        [TestMethod]
        public async Task Check_CourseTypeTracking_ReturnsCourseType()
        {
            var courseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var idOfCourseType = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseTypeTracking(idOfCourseType)).ReturnsAsync(courseType);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckCourseTypeTracking(idOfCourseType);

            Assert.AreEqual(courseType, result);
        }

        [TestMethod]
        public async Task Check_CourseTypeTracking_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseType = 1;
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseTypeTracking(idOfCourseType)).ReturnsAsync((CourseType)null);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.CheckCourseTypeTracking(idOfCourseType));
        }

        [TestMethod]
        public async Task Update_CourseType_ReturnsCourseType()
        {
            var idOfCourseType = 1;
            var updatedCourseTypeDTO = new CourseTypeResponseDTO { Id = 1, DrivingHours = 20, LectureHours = 20, MinimumAge = 18, LicenceCategoryId = 1, Name = "Kurs" };
            var courseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var courseTypeUpdate = new CourseTypeRequestDTO { DrivingHours = 20, LectureHours = 20, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var licenceCategoryDTO = new LicenceCategory();
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseTypeTracking(idOfCourseType)).ReturnsAsync(courseType);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(courseTypeUpdate.LicenceCategoryId)).ReturnsAsync(licenceCategoryDTO);
            _courseTypeRepositoryMock.Setup(repo => repo.UpdateCourseType(courseType, courseTypeUpdate)).ReturnsAsync(courseType);
            _mapperMock.Setup(m => m.Map<CourseTypeResponseDTO>(It.IsAny<CourseType>())).Returns(updatedCourseTypeDTO);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            var result = await _service.UpdateCourseType(idOfCourseType, courseTypeUpdate);

            Assert.AreEqual(updatedCourseTypeDTO, result);
        }

        [TestMethod]
        public async Task Update_CourseType_ThrowsNotFoundCourseTypeException()
        {
            var idOfCourseType = 1;
            var courseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var courseTypeUpdate = new CourseTypeRequestDTO { DrivingHours = 20, LectureHours = 20, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseTypeTracking(idOfCourseType)).ReturnsAsync((CourseType)null);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundCourseTypeException>(async () => await _service.UpdateCourseType(idOfCourseType, courseTypeUpdate));
        }

        [TestMethod]
        public async Task Update_CourseType_ThrowsNotFoundLicenceCategoryException()
        {
            var idOfCourseType = 1;
            var courseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var courseTypeUpdate = new CourseTypeRequestDTO { DrivingHours = 20, LectureHours = 20, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseTypeTracking(idOfCourseType)).ReturnsAsync(courseType);
            _licenceCategoryServiceMock.Setup(service => service.CheckLicenceCategory(courseTypeUpdate.LicenceCategoryId)).ThrowsAsync(new NotFoundLicenceCategoryException(1));
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundLicenceCategoryException>(async () => await _service.UpdateCourseType(idOfCourseType, courseTypeUpdate));
        }

        [TestMethod]
        public async Task Update_CourseType_ThrowsLectureHoursMustBeGreaterThanZeroException()
        {
            var idOfCourseType = 1;
            var courseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var courseTypeUpdate = new CourseTypeRequestDTO { DrivingHours = 20, LectureHours = -20, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var licenceCategoryDTO = new LicenceCategory();
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseTypeTracking(idOfCourseType)).ReturnsAsync(courseType);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateCourseType(idOfCourseType, courseTypeUpdate));
        }

        [TestMethod]
        public async Task Update_CourseType_ThrowsDrivingHoursMustBeGreaterThanZeroException()
        {
            var idOfCourseType = 1;
            var courseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var courseTypeUpdate = new CourseTypeRequestDTO { DrivingHours = -20, LectureHours = 20, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var licenceCategoryDTO = new LicenceCategory();
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseTypeTracking(idOfCourseType)).ReturnsAsync(courseType);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateCourseType(idOfCourseType, courseTypeUpdate));
        }

        [TestMethod]
        public async Task Update_CourseType_ThrowsMinimumAgeMustBeGreaterThanZeroException()
        {
            var idOfCourseType = 1;
            var courseType = new CourseType { Id = 1, DrivingHours = 10, LectureHours = 10, MinimumAge = 18, Name = "Kurs", LicenceCategoryId = 1 };
            var courseTypeUpdate = new CourseTypeRequestDTO { DrivingHours = 20, LectureHours = 20, MinimumAge = -18, Name = "Kurs", LicenceCategoryId = 1 };
            var licenceCategoryDTO = new LicenceCategory();
            _courseTypeRepositoryMock.Setup(repo => repo.CheckCourseTypeTracking(idOfCourseType)).ReturnsAsync(courseType);
            _service = new CourseTypeService(_courseTypeRepositoryMock.Object, _licenceCategoryServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateCourseType(idOfCourseType, courseTypeUpdate));
        }
    }
}
