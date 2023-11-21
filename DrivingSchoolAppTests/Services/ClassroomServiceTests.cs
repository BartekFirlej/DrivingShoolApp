﻿using AutoFixture;
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
    public class ClassroomServiceTests
    {
        private Mock<IAddressService> _addressServiceMock;
        private Mock<IClassroomRepository> _classroomRepositoryMock;
        private Fixture _fixture;
        private ClassroomService _service;

        public ClassroomServiceTests()
        {
            _fixture = new Fixture();
            _addressServiceMock = new Mock<IAddressService>();
            _classroomRepositoryMock = new Mock<IClassroomRepository>();
        }

        [TestMethod]
        public async Task Get_Classrooms_ReturnsClassrooms()
        {
            var classroom = new ClassroomGetDTO();
            var classroomsList = new PagedList<ClassroomGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<ClassroomGetDTO> { classroom }, HasNextPage = false };
            _classroomRepositoryMock.Setup(repo => repo.GetClassrooms(1,10)).ReturnsAsync(classroomsList);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            var result = await _service.GetClassrooms(1,10);

            Assert.AreEqual(classroomsList, result);
            Assert.AreEqual(classroomsList.PagedItems.Count, result.PagedItems.Count);
        }

        [TestMethod]
        public async Task Get_Classrooms_ThrowsNotFoundClassroomsException()
        {
            var classroomsList = new PagedList<ClassroomGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<ClassroomGetDTO> (), HasNextPage = false };
            _classroomRepositoryMock.Setup(repo => repo.GetClassrooms(1,10)).ReturnsAsync(classroomsList);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.GetClassrooms(1,10));
        }

        [TestMethod]
        public async Task Get_Classrooms_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _classroomRepositoryMock.Setup(repo => repo.GetClassrooms(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetClassrooms(-1, 10));
        }

        [TestMethod]
        public async Task Get_Classroom_ReturnsClassroom()
        {
            var classroom = new ClassroomGetDTO();
            var idOfClassroomToFind = 1;
            _classroomRepositoryMock.Setup(repo => repo.GetClassroom(idOfClassroomToFind)).ReturnsAsync(classroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            var result = await _service.GetClassroom(idOfClassroomToFind);

            Assert.AreEqual(classroom, result);
        }

        [TestMethod]
        public async Task Get_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroomToFind = 1;
            _classroomRepositoryMock.Setup(repo => repo.GetClassroom(idOfClassroomToFind)).ReturnsAsync((ClassroomGetDTO)null);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.GetClassroom(idOfClassroomToFind));
        }

        [TestMethod]
        public async Task Post_Classroom_ReturnsAdded()
        {
            var idOfAddressToFind = 1;
            var address = new Address();
            var addressDTO = new AddressGetDTO();
            var addedClassroom = new Classroom { Id = 1, AddressId = 1, Number = 10, Size = 10 };
            var addedClassroomGetDTO = new ClassroomGetDTO { Address = addressDTO, Size = 10, ClassroomNumber = 10, ClassroomId = 1 };
            var classroomToAdd = new ClassroomPostDTO { AddressID=1, Size = 10, Number = 10};
            _addressServiceMock.Setup(service => service.CheckAddress(idOfAddressToFind)).ReturnsAsync(address);
            _classroomRepositoryMock.Setup(repo => repo.PostClassroom(classroomToAdd)).ReturnsAsync(addedClassroom);
            _classroomRepositoryMock.Setup(repo => repo.GetClassroom(addedClassroom.Id)).ReturnsAsync(addedClassroomGetDTO);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            var result = await _service.PostClassroom(classroomToAdd);

            Assert.AreEqual(addedClassroomGetDTO, result);
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNotFoundAddressException()
        {
            var idOfAddressToFind = 1;
            var classroomToAdd = new ClassroomPostDTO { AddressID = 1, Size = 10, Number = 10 };
            _addressServiceMock.Setup(service => service.CheckAddress(idOfAddressToFind)).ThrowsAsync(new NotFoundAddressException(idOfAddressToFind));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.PostClassroom(classroomToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNumberMustBeGreaterThanZeroExceptionException()
        {
            var address = new Address();
            var idOfAddressToFind = 1;
            var classroomToAdd = new ClassroomPostDTO { AddressID = 1, Size = 10, Number = -2 };
            _addressServiceMock.Setup(service => service.CheckAddress(idOfAddressToFind)).ReturnsAsync(address);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostClassroom(classroomToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsSizeMustBeGreaterThanZeroExceptionException()
        {
            var address = new Address();
            var idOfAddressToFind = 1;
            var classroomToAdd = new ClassroomPostDTO { AddressID = 1, Size = -2, Number = 10 };
            _addressServiceMock.Setup(service => service.CheckAddress(idOfAddressToFind)).ReturnsAsync(address);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostClassroom(classroomToAdd));
        }

        [TestMethod]
        public async Task Delete_Classroom_ReturnsClassroom()
        {
            var deletedClassroom = new Classroom();
            var idOfClassroomToDelete = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroomToDelete)).ReturnsAsync(deletedClassroom);
            _classroomRepositoryMock.Setup(repo => repo.DeleteClassroom(deletedClassroom)).ReturnsAsync(deletedClassroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            var result = await _service.DeleteClassroom(idOfClassroomToDelete);

            Assert.AreEqual(deletedClassroom, result);
        }

        [TestMethod]
        public async Task Delete_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroomToDelete = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroomToDelete)).ReturnsAsync((Classroom)null);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.DeleteClassroom(idOfClassroomToDelete));
        }

        [TestMethod]
        public async Task Delete_Classroom_PropagatesReferenceConstraintExceptionException()
        {
            var deletedClassroom = new Classroom();
            var idOfClassroomToDelete = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroomToDelete)).ReturnsAsync(deletedClassroom);
            _classroomRepositoryMock.Setup(repo => repo.DeleteClassroom(deletedClassroom)).ThrowsAsync(new ReferenceConstraintException());
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteClassroom(idOfClassroomToDelete));
        }

        [TestMethod]
        public async Task Check_Classroom_ReturnsClassroom()
        {
            var classroom = new Classroom();
            var idOfClassroom = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroom)).ReturnsAsync(classroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            var result = await _service.CheckClassroom(idOfClassroom);

            Assert.AreEqual(classroom, result);
        }

        [TestMethod]
        public async Task Check_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroom = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroom)).ReturnsAsync((Classroom)null);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.CheckClassroom(idOfClassroom));
        }
    }
}
