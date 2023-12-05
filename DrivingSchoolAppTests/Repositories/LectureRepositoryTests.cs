using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolAppTests.Repositories
{
    [TestClass]
    public class LectureRepositoryTests
    {
        private DrivingSchoolDbContext _dbContext;
        private Fixture _fixture;
        private LectureRepository _repository;

        public LectureRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task Get_Lectures_ReturnsLectures()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var resultList = await _repository.GetLectures(1, 10);

            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.PagedItems.Count);
            Assert.AreEqual(1, resultList.PagedItems[0].Id);
            Assert.AreEqual(new DateTime(2023, 10, 10), resultList.PagedItems[0].LectureDate);
            Assert.AreEqual(1, resultList.PagedItems[0].LecturerId);
            Assert.AreEqual("TestName1", resultList.PagedItems[0].LecturerName);
            Assert.AreEqual(1, resultList.PagedItems[0].CourseId);
            Assert.AreEqual("TestCourse1", resultList.PagedItems[0].CourseName);
            Assert.AreEqual(1, resultList.PagedItems[0].SubjectId);
            Assert.AreEqual("TestSubject1", resultList.PagedItems[0].SubjectName);
            Assert.AreEqual(1, resultList.PagedItems[0].ClassroomNumber);
            Assert.AreEqual("TestCity1", resultList.PagedItems[0].City);
            Assert.AreEqual("TestStreet1", resultList.PagedItems[0].Street);
            Assert.AreEqual(10, resultList.PagedItems[0].Number);
            Assert.AreEqual(2, resultList.PagedItems[1].Id);
            Assert.AreEqual(new DateTime(2023, 11, 11), resultList.PagedItems[1].LectureDate);
            Assert.AreEqual(2, resultList.PagedItems[1].LecturerId);
            Assert.AreEqual("TestName2", resultList.PagedItems[1].LecturerName);
            Assert.AreEqual(1, resultList.PagedItems[1].CourseId);
            Assert.AreEqual("TestCourse1", resultList.PagedItems[1].CourseName);
            Assert.AreEqual(2, resultList.PagedItems[1].SubjectId);
            Assert.AreEqual("TestSubject2", resultList.PagedItems[1].SubjectName);
            Assert.AreEqual(1, resultList.PagedItems[1].ClassroomNumber);
            Assert.AreEqual("TestCity1", resultList.PagedItems[1].City);
            Assert.AreEqual("TestStreet1", resultList.PagedItems[1].Street);
            Assert.AreEqual(10, resultList.PagedItems[1].Number);
            Assert.AreEqual(1, resultList.PageIndex);
            Assert.AreEqual(10, resultList.PageSize);
            Assert.IsFalse(resultList.HasNextPage);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Lectures_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.GetLectures(1, 10);

            Assert.IsFalse(result.PagedItems.Any());
            Assert.IsFalse(result.HasNextPage);
            Assert.AreEqual(1, result.PageIndex);
            Assert.AreEqual(10, result.PageSize);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Lectures_PropagatesPageIndexMustBeGreaterThanZeroException()
        {
            _dbContext = new DrivingSchoolDbContext();
            _repository = new LectureRepository(_dbContext);

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await _repository.GetLectures(-1, 10));
            
            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Lecture_ReturnsLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLectureToFind = 2;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.GetLecture(idOfLectureToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(new DateTime(2023, 11, 11), result.LectureDate);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual("TestName2", result.LecturerName);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual("TestCourse1", result.CourseName);
            Assert.AreEqual(2, result.SubjectId);
            Assert.AreEqual("TestSubject2", result.SubjectName);
            Assert.AreEqual(1, result.ClassroomNumber);
            Assert.AreEqual("TestCity1", result.City);
            Assert.AreEqual("TestStreet1", result.Street);
            Assert.AreEqual(10, result.Number);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_Lecture_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLectureToFind = 3;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.GetLecture(idOfLectureToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseLectureSubject_ReturnsLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToFind = 1;
            var idOfSubjectToFind = 2;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.CheckLectureAtCourseAboutSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(new DateTime(2023, 11, 11), result.LectureDate);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual(idOfCourseToFind, result.CourseId);
            Assert.AreEqual(idOfSubjectToFind, result.SubjectId);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Get_CourseLectureSubject_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfCourseToFind = 2;
            var idOfSubjectToFind = 2;
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.CheckLectureAtCourseAboutSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Post_Lecture_ReturnsAddedLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddAsync(lecture1);
            await _dbContext.SaveChangesAsync();
            var lectureToAdd = new LectureRequestDTO { ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            _repository = new LectureRepository(_dbContext);

            var addedLecture = await _repository.PostLecture(lectureToAdd);

            var retrievedLecture = await _repository.GetLecture(addedLecture.Id);
            Assert.IsNotNull(addedLecture);
            Assert.IsNotNull(retrievedLecture);
            Assert.AreEqual(addedLecture.Id, retrievedLecture.Id);
            Assert.AreEqual(lectureToAdd.ClassroomId, addedLecture.ClassroomId);
            Assert.AreEqual(lectureToAdd.SubjectId, addedLecture.SubjectId);
            Assert.AreEqual(lectureToAdd.CourseId, addedLecture.CourseId);
            Assert.AreEqual(lectureToAdd.LecturerId, addedLecture.LecturerId);
            Assert.AreEqual(lectureToAdd.LectureDate, addedLecture.LectureDate);
            Assert.AreEqual(1, retrievedLecture.ClassroomNumber);
            Assert.AreEqual(lectureToAdd.SubjectId, retrievedLecture.SubjectId);
            Assert.AreEqual("TestSubject2", retrievedLecture.SubjectName);
            Assert.AreEqual(lectureToAdd.CourseId, retrievedLecture.CourseId);
            Assert.AreEqual("TestCourse1", retrievedLecture.CourseName);
            Assert.AreEqual(lectureToAdd.LecturerId, retrievedLecture.LecturerId);
            Assert.AreEqual("TestName2", retrievedLecture.LecturerName);
            Assert.AreEqual(lectureToAdd.LectureDate, retrievedLecture.LectureDate);
            Assert.AreEqual("TestCity1", retrievedLecture.City);
            Assert.AreEqual("TestStreet1", retrievedLecture.Street);
            Assert.AreEqual(10, retrievedLecture.Number);
            Assert.AreEqual(2, await _dbContext.Lectures.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Delete_Lecture_ReturnsLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.DeleteLecture(lecture2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(1, result.ClassroomId);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual(2, result.SubjectId);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual(new DateTime(2023, 11, 11), result.LectureDate);
            Assert.AreEqual(1, await _dbContext.Lectures.CountAsync());

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_Lecture_ReturnsLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLectureToCheck = 2;
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.CheckLecture(idOfLectureToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(1, result.ClassroomId);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual(2, result.SubjectId);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual(new DateTime(2023, 11, 11), result.LectureDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_Lecture_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLectureToCheck = 3;
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.CheckLecture(idOfLectureToCheck);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_LectureTracking_ReturnsLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLectureToCheck = 2;
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.CheckLectureTracking(idOfLectureToCheck);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(1, result.ClassroomId);
            Assert.AreEqual(1, result.CourseId);
            Assert.AreEqual(2, result.SubjectId);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual(new DateTime(2023, 11, 11), result.LectureDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Check_LectureTracking_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var idOfLectureToCheck = 3;
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.CheckLectureTracking(idOfLectureToCheck);

            Assert.IsNull(result);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task Update_Lecture_ReturnsLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 9, 9) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            var lecture1update = new LectureRequestDTO { ClassroomId = 2, CourseId = 2, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 10, 10) };
            await _dbContext.Lectures.AddRangeAsync(lecture1, lecture2);
            await _dbContext.SaveChangesAsync();
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.UpdateLecture(lecture1, lecture1update);

            var updatedLecture = await _repository.CheckLecture(lecture1.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(2, result.ClassroomId);
            Assert.AreEqual(2, result.CourseId);
            Assert.AreEqual(2, result.SubjectId);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual(new DateTime(2023, 10, 10), result.LectureDate);
            Assert.IsNotNull(updatedLecture);
            Assert.AreEqual(1, updatedLecture.Id);
            Assert.AreEqual(2, updatedLecture.ClassroomId);
            Assert.AreEqual(2, updatedLecture.CourseId);
            Assert.AreEqual(2, updatedLecture.SubjectId);
            Assert.AreEqual(2, updatedLecture.LecturerId);
            Assert.AreEqual(new DateTime(2023, 10, 10), updatedLecture.LectureDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task CheckLectureCourseSubject_ReturnsLecture()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddAsync(lecture1);
            await _dbContext.SaveChangesAsync();
            var idOfCourse = 1;
            var idOfSubject = 1;
            _repository = new LectureRepository(_dbContext);

            var lecture = await _repository.CheckLectureAtCourseAboutSubject(idOfCourse, idOfSubject);

            Assert.IsNotNull(lecture);
            Assert.AreEqual(1, lecture.Id);
            Assert.AreEqual(1, lecture.LecturerId);
            Assert.AreEqual(1, lecture.SubjectId);
            Assert.AreEqual(1, lecture.CourseId);
            Assert.AreEqual(1, lecture.ClassroomId);
            Assert.AreEqual(new DateTime(2023, 10, 10), lecture.LectureDate);

            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task CheckLectureCourseSubject_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            var address1 = new Address { Id = 1, City = "TestCity1", Number = 10, PostalCode = "22-222", Street = "TestStreet1" };
            var classroom1 = new Classroom { Id = 1, AddressId = 1, Number = 1, Size = 10 };
            var licenceCategory1 = new LicenceCategory { Id = 1, Name = "TestLicence" };
            var courseType1 = new CourseType { Id = 1, Name = "TestCourseType1", DrivingHours = 10, LectureHours = 10, MinimumAge = 18, LicenceCategoryId = 1 };
            var course1 = new Course { Id = 1, Name = "TestCourse1", BeginDate = new DateTime(2020, 1, 1), CourseTypeId = 1, Limit = 10, Price = 100 };
            var subject1 = new Subject { Id = 1, Name = "TestSubject1", Code = "TestCode1", Duration = 1 };
            var subject2 = new Subject { Id = 2, Name = "TestSubject2", Code = "TestCode2", Duration = 2 };
            var courseSubject1 = new CourseSubject { CourseId = 1, SubjectId = 1, SequenceNumber = 1 };
            var courseSubject2 = new CourseSubject { CourseId = 1, SubjectId = 2, SequenceNumber = 2 };
            var lecturer1 = new Lecturer { Id = 1, Name = "TestName1", SecondName = "TestSName1" };
            var lecturer2 = new Lecturer { Id = 2, Name = "TestName2", SecondName = "TestSName2" };
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseId = 1, SubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Classrooms.AddAsync(classroom1);
            await _dbContext.LicenceCategories.AddAsync(licenceCategory1);
            await _dbContext.CourseTypes.AddAsync(courseType1);
            await _dbContext.Courses.AddAsync(course1);
            await _dbContext.Subjects.AddRangeAsync(subject1, subject2);
            await _dbContext.CourseSubjects.AddRangeAsync(courseSubject1, courseSubject2);
            await _dbContext.Lecturers.AddRangeAsync(lecturer1, lecturer2);
            await _dbContext.Lectures.AddAsync(lecture1);
            await _dbContext.SaveChangesAsync();
            var idOfCourse = 2;
            var idOfSubject = 2;
            _repository = new LectureRepository(_dbContext);

            var lecture = await _repository.CheckLectureAtCourseAboutSubject(idOfCourse, idOfSubject);

            Assert.IsNull(lecture);

            await _dbContext.DisposeAsync();
        }
    }
}