using AutoFixture;
using DrivingSchoolApp.Controllers;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using Azure.Core;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class LecturerRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private LecturerRepository _repository;

        public LecturerRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Lecturers_ReturnsLecturers()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.SaveChangesAsync();
            _repository = new LecturerRepository(_dbContext);

            var result = await _repository.GetLecturers();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].ID);
            Assert.AreEqual("TestName1", resultList[0].Name);
            Assert.AreEqual("TestSName1", resultList[0].SecondName);
            Assert.AreEqual(2, resultList[1].ID);
            Assert.AreEqual("TestName2", resultList[1].Name);
            Assert.AreEqual("TestSName2", resultList[1].SecondName);
        }

        [TestMethod]
        public async Task Get_Lecturers_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new LecturerRepository(_dbContext);

            var result = await _repository.GetLecturers();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Get_Lecturer_ReturnsLecturer()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLecturerToFind = 2;
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.SaveChangesAsync();
            _repository = new LecturerRepository(_dbContext);

            var result = await _repository.GetLecturer(idOfLecturerToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ID);
            Assert.AreEqual("TestName2", result.Name);
            Assert.AreEqual("TestSName2", result.SecondName);
        }

        [TestMethod]
        public async Task Get_Lecturer_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLecturerToFind = 3;
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.SaveChangesAsync();
            _repository = new LecturerRepository(_dbContext);

            var result = await _repository.GetLecturer(idOfLecturerToFind);


            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Post_Lecturer_ReturnsAddedLecturer()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.SaveChangesAsync();
            var lecturerToAdd = new LecturerPostDTO { Name = "TestName3", SecondName = "TestSName3" };
            _repository = new LecturerRepository(_dbContext);

            var addedLecturer = await _repository.PostLecturer(lecturerToAdd);

            var retrievedLecturer = await _repository.GetLecturer(addedLecturer.Id);
            Assert.IsNotNull(retrievedLecturer);
            Assert.AreEqual(lecturerToAdd.Name, addedLecturer.Name);
            Assert.AreEqual(lecturerToAdd.SecondName, addedLecturer.SecondName);
            Assert.AreEqual(lecturerToAdd.Name, retrievedLecturer.Name);
            Assert.AreEqual(lecturerToAdd.SecondName, retrievedLecturer.SecondName);
        }
    }
}