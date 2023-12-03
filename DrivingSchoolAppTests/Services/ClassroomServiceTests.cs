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
    public class ClassroomServiceTests
    {
        private Mock<IAddressService> _addressServiceMock;
        private Mock<IClassroomRepository> _classroomRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Fixture _fixture;
        private ClassroomService _service;

        public ClassroomServiceTests()
        {
            _fixture = new Fixture();
            _addressServiceMock = new Mock<IAddressService>();
            _classroomRepositoryMock = new Mock<IClassroomRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Get_Classrooms_ReturnsClassrooms()
        {
            var classroom = new ClassroomGetDTO();
            var classroomsList = new PagedList<ClassroomGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<ClassroomGetDTO> { classroom }, HasNextPage = false };
            _classroomRepositoryMock.Setup(repo => repo.GetClassrooms(1,10)).ReturnsAsync(classroomsList);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetClassrooms(1,10);

            Assert.AreEqual(classroomsList, result);
            Assert.AreEqual(classroomsList.PagedItems.Count, result.PagedItems.Count);
        }

        [TestMethod]
        public async Task Get_Classrooms_ThrowsNotFoundClassroomsException()
        {
            var classroomsList = new PagedList<ClassroomGetDTO>() { PageIndex = 1, PageSize = 10, PagedItems = new List<ClassroomGetDTO> (), HasNextPage = false };
            _classroomRepositoryMock.Setup(repo => repo.GetClassrooms(1,10)).ReturnsAsync(classroomsList);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.GetClassrooms(1,10));
        }

        [TestMethod]
        public async Task Get_Classrooms_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _classroomRepositoryMock.Setup(repo => repo.GetClassrooms(-1, 10)).ThrowsAsync(new ValueMustBeGreaterThanZeroException("page index"));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.GetClassrooms(-1, 10));
        }

        [TestMethod]
        public async Task Get_Classroom_ReturnsClassroom()
        {
            var classroom = new ClassroomGetDTO();
            var idOfClassroomToFind = 1;
            _classroomRepositoryMock.Setup(repo => repo.GetClassroom(idOfClassroomToFind)).ReturnsAsync(classroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            var result = await _service.GetClassroom(idOfClassroomToFind);

            Assert.AreEqual(classroom, result);
        }

        [TestMethod]
        public async Task Get_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroomToFind = 1;
            _classroomRepositoryMock.Setup(repo => repo.GetClassroom(idOfClassroomToFind)).ReturnsAsync((ClassroomGetDTO)null);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.GetClassroom(idOfClassroomToFind));
        }

        [TestMethod]
        public async Task Post_Classroom_ReturnsAdded()
        {
            var idOfAddressToFind = 1;
            var address = new Address();
            var addedClassroom = new Classroom { Id = 1, AddressId = 1, Number = 10, Size = 10 };
            var addedClassroomDTO = new ClassroomResponseDTO { Id=1, Size = 10, Number = 10, AddressId = 1 };
            var classroomToAdd = new ClassroomRequestDTO { AddressId=1, Size = 10, Number = 10};
            _addressServiceMock.Setup(service => service.CheckAddress(idOfAddressToFind)).ReturnsAsync(address);
            _classroomRepositoryMock.Setup(repo => repo.PostClassroom(classroomToAdd)).ReturnsAsync(addedClassroom);
            _mapperMock.Setup(m => m.Map<ClassroomResponseDTO>(It.IsAny<Classroom>())).Returns(addedClassroomDTO);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            var result = await _service.PostClassroom(classroomToAdd);

            Assert.AreEqual(addedClassroomDTO, result);
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNotFoundAddressException()
        {
            var idOfAddressToFind = 1;
            var classroomToAdd = new ClassroomRequestDTO { AddressId = 1, Size = 10, Number = 10 };
            _addressServiceMock.Setup(service => service.CheckAddress(idOfAddressToFind)).ThrowsAsync(new NotFoundAddressException(idOfAddressToFind));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.PostClassroom(classroomToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNumberMustBeGreaterThanZeroExceptionException()
        {
            var address = new Address();
            var idOfAddressToFind = 1;
            var classroomToAdd = new ClassroomRequestDTO { AddressId = 1, Size = 10, Number = -2 };
            _addressServiceMock.Setup(service => service.CheckAddress(idOfAddressToFind)).ReturnsAsync(address);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostClassroom(classroomToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsSizeMustBeGreaterThanZeroExceptionException()
        {
            var address = new Address();
            var idOfAddressToFind = 1;
            var classroomToAdd = new ClassroomRequestDTO { AddressId = 1, Size = -2, Number = 10 };
            _addressServiceMock.Setup(service => service.CheckAddress(idOfAddressToFind)).ReturnsAsync(address);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostClassroom(classroomToAdd));
        }

        [TestMethod]
        public async Task Delete_Classroom_ReturnsClassroom()
        {
            var deletedClassroom = new Classroom();
            var idOfClassroomToDelete = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroomToDelete)).ReturnsAsync(deletedClassroom);
            _classroomRepositoryMock.Setup(repo => repo.DeleteClassroom(deletedClassroom)).ReturnsAsync(deletedClassroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            var result = await _service.DeleteClassroom(idOfClassroomToDelete);

            Assert.AreEqual(deletedClassroom, result);
        }

        [TestMethod]
        public async Task Delete_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroomToDelete = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroomToDelete)).ReturnsAsync((Classroom)null);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.DeleteClassroom(idOfClassroomToDelete));
        }

        [TestMethod]
        public async Task Delete_Classroom_PropagatesReferenceConstraintExceptionException()
        {
            var deletedClassroom = new Classroom();
            var idOfClassroomToDelete = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroomToDelete)).ReturnsAsync(deletedClassroom);
            _classroomRepositoryMock.Setup(repo => repo.DeleteClassroom(deletedClassroom)).ThrowsAsync(new ReferenceConstraintException());
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () => await _service.DeleteClassroom(idOfClassroomToDelete));
        }

        [TestMethod]
        public async Task Check_Classroom_ReturnsClassroom()
        {
            var classroom = new Classroom();
            var idOfClassroom = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroom)).ReturnsAsync(classroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckClassroom(idOfClassroom);

            Assert.AreEqual(classroom, result);
        }

        [TestMethod]
        public async Task Check_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroom = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroom(idOfClassroom)).ReturnsAsync((Classroom)null);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.CheckClassroom(idOfClassroom));
        }

        [TestMethod]
        public async Task Check_ClassroomTracking_ReturnsClassroom()
        {
            var classroom = new Classroom();
            var idOfClassroom = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroomTracking(idOfClassroom)).ReturnsAsync(classroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            var result = await _service.CheckClassroomTracking(idOfClassroom);

            Assert.AreEqual(classroom, result);
        }

        [TestMethod]
        public async Task Check_ClassroomTracking_ThrowsNotFoundClassroomException()
        {
            var idOfClassroom = 1;
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroomTracking(idOfClassroom)).ReturnsAsync((Classroom)null);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.CheckClassroomTracking(idOfClassroom));
        }

        [TestMethod]
        public async Task Update_Classroom_ReturnsClassroom()
        {
            var idOfClassroom = 1;
            var address = new Address();
            var classroom = new Classroom { Id = 1, AddressId = 1, Number = 10, Size = 10 };
            var updatedClassroom = new ClassroomResponseDTO { Id = 1, AddressId = 2, Size = 20, Number = 20 };
            var classroomUpdate = new ClassroomRequestDTO { AddressId = 2, Size = 20, Number = 20 };
            _addressServiceMock.Setup(service => service.CheckAddress(classroom.AddressId)).ReturnsAsync(address);
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroomTracking(idOfClassroom)).ReturnsAsync(classroom);
            _classroomRepositoryMock.Setup(repo => repo.UpdateClassroom(classroom, classroomUpdate)).ReturnsAsync(classroom);
            _mapperMock.Setup(m => m.Map<ClassroomResponseDTO>(It.IsAny<Classroom>())).Returns(updatedClassroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            var result = await _service.UpdateClassroom(idOfClassroom, classroomUpdate);

            Assert.AreEqual(updatedClassroom, result);
        }

        [TestMethod]
        public async Task Update_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfAddressToFind = 1;
            var classroomToAdd = new ClassroomRequestDTO { AddressId = 1, Size = 10, Number = 10 };
            _addressServiceMock.Setup(service => service.CheckAddress(idOfAddressToFind)).ThrowsAsync(new NotFoundAddressException(idOfAddressToFind));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.PostClassroom(classroomToAdd));
        }

        [TestMethod]
        public async Task Update_Classroom_ThrowsNumberMustBeGreaterThanZeroExceptionException()
        {
            var classroom = new Classroom();
            var idOfClassroom = 1;
            var classroomUpdate = new ClassroomRequestDTO { AddressId = 1, Size = 10, Number = -2 };
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroomTracking(idOfClassroom)).ReturnsAsync(classroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateClassroom(idOfClassroom, classroomUpdate));
        }

        [TestMethod]
        public async Task Update_Classroom_ThrowsSizeMustBeGreaterThanZeroExceptionException()
        {
            var classroom = new Classroom();
            var idOfClassroom = 1;
            var classroomUpdate = new ClassroomRequestDTO { AddressId = 1, Size = -10, Number = 2 };
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroomTracking(idOfClassroom)).ReturnsAsync(classroom);
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.UpdateClassroom(idOfClassroom, classroomUpdate));
        }

        [TestMethod]
        public async Task Update_Classroom_ThrowsNotFoundAddressExceptionException()
        {
            var classroom = new Classroom();
            var idOfClassroom = 1;
            var classroomUpdate = new ClassroomRequestDTO { AddressId = 1, Size = 10, Number = 2 };
            _classroomRepositoryMock.Setup(repo => repo.CheckClassroomTracking(idOfClassroom)).ReturnsAsync(classroom);
            _addressServiceMock.Setup(service => service.CheckAddress(classroomUpdate.AddressId)).ThrowsAsync(new NotFoundAddressException(classroomUpdate.AddressId));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object, _mapperMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.UpdateClassroom(idOfClassroom, classroomUpdate));
        }
    }
}
