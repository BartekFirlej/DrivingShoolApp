using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class SubjectRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private SubjectRepository _repository;

        public SubjectRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Subjects_ReturnsSubjects()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var subject1 = new Subject { Id = 1, Name = "TestName1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestName2", Code = "TestCode2", Duration = 2 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.SaveChangesAsync();
            _repository = new SubjectRepository(_dbContext);

            var result = await _repository.GetSubjects();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual("TestName1", resultList[0].Name);
            Assert.AreEqual("TestCode1", resultList[0].Code);
            Assert.AreEqual(1, resultList[0].Duration);
            Assert.AreEqual(2, resultList[1].Id);
            Assert.AreEqual("TestName2", resultList[1].Name);
            Assert.AreEqual("TestCode2", resultList[1].Code);
            Assert.AreEqual(2, resultList[1].Duration);
        }

        [TestMethod]
        public async Task Get_Subjects_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new SubjectRepository(_dbContext);

            var result = await _repository.GetSubjects();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Get_Subject_ReturnsSubject()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfSubjectToFind = 2;
            var subject1 = new Subject { Id = 1, Name = "TestName1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestName2", Code = "TestCode2", Duration = 2 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.SaveChangesAsync();
            _repository = new SubjectRepository(_dbContext);

            var result = await _repository.GetSubject(idOfSubjectToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("TestName2", result.Name);
            Assert.AreEqual("TestCode2", result.Code);
            Assert.AreEqual(2, result.Duration);
        }

        [TestMethod]
        public async Task Get_Subject_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfSubjectToFind = 3;
            var subject1 = new Subject { Id = 1, Name = "TestName1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestName2", Code = "TestCode2", Duration = 2 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.SaveChangesAsync();
            _repository = new SubjectRepository(_dbContext);

            var result = await _repository.GetSubject(idOfSubjectToFind);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Post_Subject_ReturnsAddedSubject()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var subject1 = new Subject { Id = 1, Name = "TestName1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestName2", Code = "TestCode2", Duration = 2 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.SaveChangesAsync();
            var subjectToAdd = new SubjectPostDTO { Code = "TestCode3", Name = "TestName3", Duration = 3 };
            _repository = new SubjectRepository(_dbContext);

            var addedSubject = await _repository.PostSubject(subjectToAdd);

            var retrievedSubject = await _repository.GetSubject(addedSubject.Id);
            Assert.IsNotNull(addedSubject);
            Assert.IsNotNull(retrievedSubject);
            Assert.AreEqual(addedSubject.Id, retrievedSubject.Id);
            Assert.AreEqual(subjectToAdd.Name, addedSubject.Name);
            Assert.AreEqual(subjectToAdd.Code, addedSubject.Code);
            Assert.AreEqual(subjectToAdd.Duration, addedSubject.Duration);
            Assert.AreEqual(subjectToAdd.Name, retrievedSubject.Name);
            Assert.AreEqual(subjectToAdd.Code, retrievedSubject.Code);
            Assert.AreEqual(subjectToAdd.Duration, retrievedSubject.Duration);
        }
    }
}