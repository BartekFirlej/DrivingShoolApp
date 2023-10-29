using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ICollection<ClassroomGetDTO> classroomsList = new List<ClassroomGetDTO>() { classroom };
            _classroomRepositoryMock.Setup(repo => repo.GetClassrooms()).Returns(Task.FromResult(classroomsList));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            var result = await _service.GetClassrooms();

            Assert.AreEqual(classroomsList, result);
        }

        [TestMethod]
        public async Task Get_Classrooms_ThrowsNotFoundClassroomsException()
        {
            ICollection<ClassroomGetDTO> classroomsList = new List<ClassroomGetDTO>();
            _classroomRepositoryMock.Setup(repo => repo.GetClassrooms()).Returns(Task.FromResult(classroomsList));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.GetClassrooms());
        }

        [TestMethod]
        public async Task Get_Classroom_ReturnsClassroom()
        {
            var classroom = new ClassroomGetDTO();
            var idOfClassroomToFind = 1;
            _classroomRepositoryMock.Setup(repo => repo.GetClassroom(idOfClassroomToFind)).Returns(Task.FromResult(classroom));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            var result = await _service.GetClassroom(idOfClassroomToFind);

            Assert.AreEqual(classroom, result);
        }

        [TestMethod]
        public async Task Get_Classroom_ThrowsNotFoundClassroomException()
        {
            var idOfClassroomToFind = 1;
            _classroomRepositoryMock.Setup(repo => repo.GetClassroom(idOfClassroomToFind)).Throws(new NotFoundClassroomException(idOfClassroomToFind));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundClassroomException>(async () => await _service.GetClassroom(idOfClassroomToFind));
        }

        [TestMethod]
        public async Task Post_Classroom_ReturnsAdded()
        {
            var addedAddressDTO = new AddressGetDTO { ID = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            var addedClassroom = new Classroom { Id = 1, AddressId = 1, Number = 10, Size = 10 };
            var addedClassroomGetDTO = new ClassroomGetDTO { Address = addedAddressDTO, Size = 10, ClassroomNumber = 10, ClassroomId = 1 };
            var classroomToAdd = new ClassroomPostDTO { AddressID=1, Size = 10, Number = 10};
            _addressServiceMock.Setup(service => service.GetAddress(classroomToAdd.AddressID)).Returns(Task.FromResult(addedAddressDTO));
            _classroomRepositoryMock.Setup(repo => repo.PostClassroom(classroomToAdd)).Returns(Task.FromResult(addedClassroom));
            _classroomRepositoryMock.Setup(repo => repo.GetClassroom(addedClassroom.Id)).Returns(Task.FromResult(addedClassroomGetDTO));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            var result = await _service.PostClassroom(classroomToAdd);

            Assert.AreEqual(addedClassroomGetDTO, result);
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNotFoundAddressException()
        {
            var classroomToAdd = new ClassroomPostDTO { AddressID = 1, Size = 10, Number = 10 };
            _addressServiceMock.Setup(service => service.GetAddress(classroomToAdd.AddressID)).Throws(new NotFoundAddressException(classroomToAdd.AddressID));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotFoundAddressException>(async () => await _service.PostClassroom(classroomToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsNumberMustBeGreaterThanZeroExceptionException()
        {
            var classroomToAdd = new ClassroomPostDTO { AddressID = 1, Size = 10, Number = -2 };
            var addedAddressDTO = new AddressGetDTO { ID = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            _addressServiceMock.Setup(service => service.GetAddress(classroomToAdd.AddressID)).Returns(Task.FromResult(addedAddressDTO));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostClassroom(classroomToAdd));
        }

        [TestMethod]
        public async Task Post_Classroom_ThrowsSizeMustBeGreaterThanZeroExceptionException()
        {
            var classroomToAdd = new ClassroomPostDTO { AddressID = 1, Size = -2, Number = 10 };
            var addedAddressDTO = new AddressGetDTO { ID = 1, Street = "Mazowiecka", City = "Warszawa", PostalCode = "11-111", Number = 1 };
            _addressServiceMock.Setup(service => service.GetAddress(classroomToAdd.AddressID)).Returns(Task.FromResult(addedAddressDTO));
            _service = new ClassroomService(_classroomRepositoryMock.Object, _addressServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _service.PostClassroom(classroomToAdd));
        }
    }
}
