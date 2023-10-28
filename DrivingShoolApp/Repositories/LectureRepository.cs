using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ILectureRepository
    {
        public Task<ICollection<LectureGetDTO>> GetLectures();
        public Task<LectureGetDTO> GetLecture(int lectureId);
        public Task<Lecture> PostLecture(LecturePostDTO lectureDetails);
        public Task<Lecture> GetCourseLectureSubject(int courseId, int subjectId);
    }
    public class LectureRepository : ILectureRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public LectureRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<LectureGetDTO>> GetLectures()
        {
            return await _dbContext.Lectures
                            .Include(l => l.Lecturer)
                            .Include(l => l.Classroom)
                            .Include(l => l.Classroom.Address)
                            .Include(l => l.CourseSubjects)
                            .Include(l => l.CourseSubjects.Subject)
                            .Include(l => l.CourseSubjects.Course)
                            .Select(l => new LectureGetDTO
                            {
                                Id = l.Id,
                                LectureDate = l.LectureDate,
                                CourseId = l.CourseSubjectsCourseId,
                                CourseName = l.CourseSubjects.Course.Name,
                                SubjectId = l.CourseSubjectsSubjectId,
                                SubjectName = l.CourseSubjects.Subject.Name,
                                LecturerId = l.LecturerId,
                                LecturerName = l.Lecturer.Name,
                                City = l.Classroom.Address.City,
                                Street = l.Classroom.Address.Street,
                                Number = l.Classroom.Address.Number,
                                ClassroomNumber = l.Classroom.Address.Number
                            }).ToListAsync();
        }

        public async Task<LectureGetDTO> GetLecture(int lectureId)
        {
            return await _dbContext.Lectures
                .Include(l => l.Lecturer)
                .Include(l => l.Classroom)
                .Include(l => l.Classroom.Address)
                .Include(l => l.CourseSubjects)
                .Include(l => l.CourseSubjects.Subject)
                .Include(l => l.CourseSubjects.Course)
                .Where(l => l.Id == lectureId)
                .Select(l => new LectureGetDTO
                {
                    Id = l.Id,
                    LectureDate = l.LectureDate,
                    CourseId = l.CourseSubjectsCourseId,
                    CourseName = l.CourseSubjects.Course.Name,
                    SubjectId = l.CourseSubjectsSubjectId,
                    SubjectName = l.CourseSubjects.Subject.Name,
                    LecturerId = l.LecturerId,
                    LecturerName = l.Lecturer.Name,
                    City = l.Classroom.Address.City,
                    Street = l.Classroom.Address.Street,
                    Number = l.Classroom.Address.Number,
                    ClassroomNumber = l.Classroom.Address.Number
                }).FirstOrDefaultAsync();
        }

        public async Task<Lecture> PostLecture(LecturePostDTO lectureDetails)
        {
            var lectureToAdd = new Lecture
            {
                LectureDate = lectureDetails.LectureDate,
                ClassroomId = lectureDetails.ClassroomId,
                LecturerId = lectureDetails.LecturerId,
                CourseSubjectsSubjectId = lectureDetails.SubjectId,
                CourseSubjectsCourseId = lectureDetails.CourseId
            };
            await _dbContext.Lectures.AddAsync(lectureToAdd);
            await _dbContext.SaveChangesAsync();
            return lectureToAdd;
        }

        public async Task<Lecture> GetCourseLectureSubject(int courseId, int subjectId)
        {
            return await _dbContext.Lectures.FirstOrDefaultAsync(l => l.CourseSubjectsCourseId == courseId && l.CourseSubjectsSubjectId == subjectId);
        }
    }
}
