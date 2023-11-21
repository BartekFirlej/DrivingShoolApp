using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class CourseSubjectRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private CourseSubjectRepository _repository;

        public CourseSubjectRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_CoursesSubject_ReturnsCoursesSubjects()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 2, SubjectId = 2, SequenceNumber = 1 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.GetCoursesSubjects();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].CourseId);
            Assert.AreEqual(1, resultList[0].SubjectId);
            Assert.AreEqual(1, resultList[0].SequenceNumber);
            Assert.AreEqual("TestCourse1", resultList[0].CourseName);
            Assert.AreEqual("TestSubject1", resultList[0].SubjectName);
            Assert.AreEqual(2, resultList[1].CourseId);
            Assert.AreEqual(2, resultList[1].SubjectId);
            Assert.AreEqual(1, resultList[1].SequenceNumber);
            Assert.AreEqual("TestCourse2", resultList[1].CourseName);
            Assert.AreEqual("TestSubject2", resultList[1].SubjectName);
        }

        [TestMethod]
        public async Task Get_CoursesSubjects_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.GetCoursesSubjects();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task Get_CourseSubject_ReturnsCourseSubject()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfSubjectToFind = 1;
            var idOfCourseToFind = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 2, SubjectId = 2, SequenceNumber = 1 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.GetCourseSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual(1, result.SubjectId);
            Assert.AreEqual(1, result.SequenceNumber);
            Assert.AreEqual("TestCourse1", result.CourseName);
            Assert.AreEqual("TestSubject1", result.SubjectName);
        }

        [TestMethod]
        public async Task Get_CourseSubject_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfSubjectToFind = 3;
            var idOfCourseToFind = 3;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 2, SubjectId = 2, SequenceNumber = 1 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.GetCourseSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Get_CourseSubjects_ReturnsCourseSubjects()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToFind = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.GetCourseSubjects(idOfCourseToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(100, result.Price);
            Assert.AreEqual(10, result.Limit);
            Assert.AreEqual(1, result.CourseTypeId);
            Assert.AreEqual("TestCourseType1", result.CourseTypeName);
            Assert.AreEqual("TestCourse1", result.Name);
            Assert.AreEqual(new DateTime(2020, 1, 1), result.BeginDate);
            Assert.AreEqual(2, result.CourseSubjects.Count());
            Assert.AreEqual(1, result.CourseSubjects[0].Subject.Id);
            Assert.AreEqual(1, result.CourseSubjects[0].Subject.Duration);
            Assert.AreEqual("TestSubject1", result.CourseSubjects[0].Subject.Name);
            Assert.AreEqual("TestCode1", result.CourseSubjects[0].Subject.Code);
            Assert.AreEqual(1, result.CourseSubjects[0].Sequence); 
            Assert.AreEqual(2, result.CourseSubjects[1].Subject.Id);
            Assert.AreEqual(2, result.CourseSubjects[1].Subject.Duration);
            Assert.AreEqual("TestSubject2", result.CourseSubjects[1].Subject.Name);
            Assert.AreEqual("TestCode2", result.CourseSubjects[1].Subject.Code);
            Assert.AreEqual(2, result.CourseSubjects[1].Sequence);
        }

        [TestMethod]
        public async Task Get_CourseSubjects_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToFind = 2;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.GetCourseSubjects(idOfCourseToFind);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Check_CourseSubject_ReturnsCourseSubject()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToCheck = 1;
            var idOfSubjectToCheck = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 2, SubjectId = 2, SequenceNumber = 1 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.CheckCourseSubject(idOfCourseToCheck, idOfSubjectToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual(1, result.SubjectId);
            Assert.AreEqual(1, result.SequenceNumber);
        }

        [TestMethod]
        public async Task Check_CourseSubject_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToCheck = 2;
            var idOfSubjectToCheck = 3;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 2, SubjectId = 2, SequenceNumber = 1 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.CheckCourseSubject(idOfCourseToCheck, idOfSubjectToCheck);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task TakenSeqNumber_ReturnsTrue()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var sequenceNumberToFind = 1;
            var idOfCourseToFind = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 2, SubjectId = 2, SequenceNumber = 1 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.TakenSeqNumber(idOfCourseToFind, sequenceNumberToFind);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task TakenSeqNumber_ReturnsFalse()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var sequenceNumberToFind = 10;
            var idOfCourseToFind = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 2, SubjectId = 2, SequenceNumber = 1 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.TakenSeqNumber(idOfCourseToFind, sequenceNumberToFind);

            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Post_CourseSubject_ReturnsAddedCourseSubject()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 2, SubjectId = 2, SequenceNumber = 1 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            var courseSubjectToAdd = new CourseSubjectPostDTO { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            _repository = new CourseSubjectRepository(_dbContext);

            var addedCourseSubject = await _repository.PostCourseSubject(courseSubjectToAdd);

            var retrievedCourseSubject = await _repository.GetCourseSubject(addedCourseSubject.CourseId, addedCourseSubject.SubjectId);
            Assert.IsNotNull(addedCourseSubject);
            Assert.IsNotNull(retrievedCourseSubject);
            Assert.AreEqual(addedCourseSubject.CourseId, retrievedCourseSubject.CourseId);
            Assert.AreEqual(addedCourseSubject.SubjectId, retrievedCourseSubject.SubjectId);
            Assert.AreEqual(addedCourseSubject.SequenceNumber, retrievedCourseSubject.SequenceNumber);
            Assert.AreEqual(courseSubjectToAdd.CourseId, addedCourseSubject.CourseId);
            Assert.AreEqual(courseSubjectToAdd.SubjectId, addedCourseSubject.SubjectId);
            Assert.AreEqual(courseSubjectToAdd.SequenceNumber, addedCourseSubject.SequenceNumber);
            Assert.AreEqual(courseSubjectToAdd.CourseId, retrievedCourseSubject.CourseId);
            Assert.AreEqual(courseSubjectToAdd.SubjectId, retrievedCourseSubject.SubjectId);
            Assert.AreEqual(courseSubjectToAdd.SequenceNumber, retrievedCourseSubject.SequenceNumber);
        }

        [TestMethod]
        public async Task Delete_CourseSubject_ReturnsCourseSubject()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToCheck = 1;
            var idOfSubjectToCheck = 1;
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var course2 = new Course { Id = 2, Name = "TestCourse2", BeginDate = new DateTime(2020, 2, 2), CourseTypeId = 1, Limit = 20, Price = 200 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 2, SubjectId = 2, SequenceNumber = 1 };
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddRangeAsync(course1, course2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.SaveChangesAsync();
            _repository = new CourseSubjectRepository(_dbContext);

            var result = await _repository.DeleteCourseSubject(courseSubject2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.CourseId);
            Assert.AreEqual(2, result.SubjectId);
            Assert.AreEqual(1, result.SequenceNumber);
            Assert.AreEqual(1, _dbContext.CourseSubjects.Count());
        }
    }
}