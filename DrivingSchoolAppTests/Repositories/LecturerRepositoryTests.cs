using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;

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

            var resultList = await _repository.GetLecturers(1, 10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual("TestName1", resultList.PagedItems[0].Name);
            Assert.AreEqual("TestSName1", resultList.PagedItems[0].SecondName);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual("TestName2", resultList.PagedItems[1].Name);
            Assert.AreEqual("TestSName2", resultList.PagedItems[1].SecondName);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Lecturers_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new LecturerRepository(_dbContext);

            var result = await _repository.GetLecturers(1, 10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.IsFalse(result.HasNextPage);
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Lecturers_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _dbContext = new DrivingSchoolDbContext();
            _repository = new LecturerRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetLecturers(-1, 10));

            await _dbContext.DisposeAsync();
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
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestName2", result.Name);
            Assert.AreEqual("TestSName2", result.SecondName);

            await _dbContext.DisposeAsync();
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
            
            await _dbContext.DisposeAsync();
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
            Assert.IsNotNull(addedLecturer);
            Assert.IsNotNull(retrievedLecturer);
            Assert.AreEqual(addedLecturer.Id, retrievedLecturer.Id);
            Assert.AreEqual(lecturerToAdd.Name, addedLecturer.Name);
            Assert.AreEqual(lecturerToAdd.SecondName, addedLecturer.SecondName);
            Assert.AreEqual(lecturerToAdd.Name, retrievedLecturer.Name);
            Assert.AreEqual(lecturerToAdd.SecondName, retrievedLecturer.SecondName);
            Assert.AreEqual(3, await _dbContext.Lecturers.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Delete_Lecturer_ReturnsLecturer()
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

            var result = await _repository.DeleteLecturer(lecturer2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestName2", result.Name);
            Assert.AreEqual("TestSName2", result.SecondName);
            Assert.AreEqual(1, await _dbContext.Lecturers.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_Lecturer_ReturnsLecturer()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLecturerToCheck = 2;
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.SaveChangesAsync();
            _repository = new LecturerRepository(_dbContext);

            var result = await _repository.CheckLecturer(idOfLecturerToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestName2", result.Name);
            Assert.AreEqual("TestSName2", result.SecondName);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_Lecturer_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLecturerToCheck = 3;
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.SaveChangesAsync();
            _repository = new LecturerRepository(_dbContext);

            var result = await _repository.CheckLecturer(idOfLecturerToCheck);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }
    }
}