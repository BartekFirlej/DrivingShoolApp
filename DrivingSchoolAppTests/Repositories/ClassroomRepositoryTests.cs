﻿using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class ClassroomRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private ClassroomRepository _repository;

        public ClassroomRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Classrooms_ReturnsClassrooms()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var classroom2 = new Classroom { Id = 2, AddressId = 1, Number = 2, Size = 20 };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddRangeAsync(classroom1, classroom2);
            await _dbContext.SaveChangesAsync();
            _repository = new ClassroomRepository(_dbContext);

            var result = await _repository.GetClassrooms();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].ClassroomId);
            Assert.AreEqual(1, resultList[0].ClassroomNumber);
            Assert.AreEqual(10, resultList[0].Size);
            Assert.AreEqual(1, resultList[0].Address.Id);
            Assert.AreEqual(10, resultList[0].Address.Number);
            Assert.AreEqual("22-222", resultList[0].Address.PostalCode);
            Assert.AreEqual("TestStreet1", resultList[0].Address.Street);
            Assert.AreEqual("TestCity1", resultList[0].Address.City);
            Assert.AreEqual(2, resultList[1].ClassroomId);
            Assert.AreEqual(2, resultList[1].ClassroomNumber);
            Assert.AreEqual(20, resultList[1].Size);
            Assert.AreEqual(1, resultList[1].Address.Id);
            Assert.AreEqual(10, resultList[1].Address.Number);
            Assert.AreEqual("22-222", resultList[1].Address.PostalCode);
            Assert.AreEqual("TestStreet1", resultList[1].Address.Street);
            Assert.AreEqual("TestCity1", resultList[1].Address.City);
        }

        [TestMethod]
        public async Task Get_Classrooms_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options); 
            _repository = new ClassroomRepository(_dbContext);

            var result = await _repository.GetClassrooms();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Get_Classroom_ReturnsClassroom()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfClassroomToFind = 2;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var classroom2 = new Classroom { Id = 2, AddressId = 1, Number = 2, Size = 20 };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddRangeAsync(classroom1, classroom2);
            await _dbContext.SaveChangesAsync();
            _repository = new ClassroomRepository(_dbContext);

            var result = await _repository.GetClassroom(idOfClassroomToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ClassroomId);
            Assert.AreEqual(2, result.ClassroomNumber);
            Assert.AreEqual(20, result.Size);
            Assert.AreEqual(1, result.Address.Id);
            Assert.AreEqual(10, result.Address.Number);
            Assert.AreEqual("22-222", result.Address.PostalCode);
            Assert.AreEqual("TestStreet1", result.Address.Street);
            Assert.AreEqual("TestCity1", result.Address.City);
        }

        [TestMethod]
        public async Task Get_Classroom_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfClassroomToFind = 3;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var classroom2 = new Classroom { Id = 2, AddressId = 1, Number = 2, Size = 20 };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddRangeAsync(classroom1, classroom2);
            await _dbContext.SaveChangesAsync();
            _repository = new ClassroomRepository(_dbContext);

            var result = await _repository.GetClassroom(idOfClassroomToFind);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Post_Classroom_ReturnsAddedClassroom()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var classroom2 = new Classroom { Id = 2, AddressId = 1, Number = 2, Size = 20 };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddRangeAsync(classroom1, classroom2);
            await _dbContext.SaveChangesAsync();
            var classroomToAdd = new ClassroomPostDTO { AddressID = 1, Number = 3, Size = 30};
            _repository = new ClassroomRepository(_dbContext);

            var addedClassroom = await _repository.PostClassroom(classroomToAdd);

            var retrievedClassroom = await _repository.GetClassroom(addedClassroom.Id);
            Assert.IsNotNull(addedClassroom);
            Assert.IsNotNull(retrievedClassroom);
            Assert.AreEqual(addedClassroom.Id, retrievedClassroom.ClassroomId);
            Assert.AreEqual(classroomToAdd.Number, addedClassroom.Number);
            Assert.AreEqual(classroomToAdd.Size, addedClassroom.Size);
            Assert.AreEqual(classroomToAdd.AddressID, addedClassroom.AddressId);
            Assert.AreEqual(classroomToAdd.Number, retrievedClassroom.ClassroomNumber);
            Assert.AreEqual(classroomToAdd.Size, retrievedClassroom.Size);
            Assert.AreEqual(classroomToAdd.AddressID, retrievedClassroom.Address.Id);
            Assert.AreEqual(address1.Id, retrievedClassroom.Address.Id);
            Assert.AreEqual(address1.City, retrievedClassroom.Address.City);
            Assert.AreEqual(address1.Street, retrievedClassroom.Address.Street);
            Assert.AreEqual(address1.Number, retrievedClassroom.Address.Number);
            Assert.AreEqual(retrievedClassroom.Address.PostalCode, address1.PostalCode);
        }
    }
}