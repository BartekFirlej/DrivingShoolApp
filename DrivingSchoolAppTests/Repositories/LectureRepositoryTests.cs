using AutoFixture;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore;
using DrivingSchoolApp.Models;

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
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
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

            var result = await _repository.GetLectures();

            var resultList = result.ToList();
            Assert.IsNotNull(resultList);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual(new DateTime(2023, 10, 10), resultList[0].LectureDate);
            Assert.AreEqual(1, resultList[0].LecturerId);
            Assert.AreEqual("TestName1", resultList[0].LecturerName);
            Assert.AreEqual(1, resultList[0].CourseId);
            Assert.AreEqual("TestCourse1", resultList[0].CourseName);
            Assert.AreEqual(1, resultList[0].SubjectId);
            Assert.AreEqual("TestSubject1", resultList[0].SubjectName);
            Assert.AreEqual(1, resultList[0].ClassroomNumber);
            Assert.AreEqual("TestCity1", resultList[0].City);
            Assert.AreEqual("TestStreet1", resultList[0].Street);
            Assert.AreEqual(10, resultList[0].Number);
            Assert.AreEqual(2, resultList[1].Id);
            Assert.AreEqual(new DateTime(2023, 11, 11), resultList[1].LectureDate);
            Assert.AreEqual(2, resultList[1].LecturerId);
            Assert.AreEqual("TestName2", resultList[1].LecturerName);
            Assert.AreEqual(1, resultList[1].CourseId);
            Assert.AreEqual("TestCourse1", resultList[1].CourseName);
            Assert.AreEqual(2, resultList[1].SubjectId);
            Assert.AreEqual("TestSubject2", resultList[1].SubjectName);
            Assert.AreEqual(1, resultList[1].ClassroomNumber);
            Assert.AreEqual("TestCity1", resultList[1].City);
            Assert.AreEqual("TestStreet1", resultList[1].Street);
            Assert.AreEqual(10, resultList[1].Number);
        }

        [TestMethod]
        public async Task Get_Lectures_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<DrivingSchoolDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DrivingSchoolDbContext(options);
            _repository = new LectureRepository(_dbContext);

            var result = await _repository.GetLectures();

            Assert.IsFalse(result.Any());
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
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
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
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
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
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
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

            var result = await _repository.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(new DateTime(2023, 11, 11), result.LectureDate);
            Assert.AreEqual(2, result.LecturerId);
            Assert.AreEqual(idOfCourseToFind, result.CourseSubjectsCourseId);
            Assert.AreEqual(idOfSubjectToFind, result.CourseSubjectsSubjectId);
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
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
            var lecture2 = new Lecture { Id = 2, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
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

            var result = await _repository.GetCourseLectureSubject(idOfCourseToFind, idOfSubjectToFind);

            Assert.IsNull(result);
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
            var lecture1 = new Lecture { Id = 1, ClassroomId = 1, CourseSubjectsCourseId = 1, CourseSubjectsSubjectId = 1, LecturerId = 1, LectureDate = new DateTime(2023, 10, 10) };
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
            var lectureToAdd = new LecturePostDTO { ClassroomId = 1, CourseId = 1, SubjectId = 2, LecturerId = 2, LectureDate = new DateTime(2023, 11, 11) };
            _repository = new LectureRepository(_dbContext);

            var addedLecture = await _repository.PostLecture(lectureToAdd);

            var retrievedLecture = await _repository.GetLecture(addedLecture.Id);
            Assert.IsNotNull(addedLecture);
            Assert.IsNotNull(retrievedLecture);
            Assert.AreEqual(addedLecture.Id, retrievedLecture.Id);
            Assert.AreEqual(lectureToAdd.ClassroomId, addedLecture.ClassroomId);
            Assert.AreEqual(lectureToAdd.SubjectId, addedLecture.CourseSubjectsSubjectId);
            Assert.AreEqual(lectureToAdd.CourseId, addedLecture.CourseSubjectsCourseId);
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
        }
    }
}