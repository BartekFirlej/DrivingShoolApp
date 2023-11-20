using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ILectureRepository
    {
        public Task<PagedList<LectureGetDTO>> GetLectures(int page, int size);
        public Task<LectureGetDTO> GetLecture(int lectureId);
        public Task<Lecture> PostLecture(LecturePostDTO lectureDetails);
        public Task<Lecture> GetCourseLectureSubject(int courseId, int subjectId);
        public Task<Lecture> CheckLecture(int lectureId);
        public Task<Lecture> DeleteLecture(Lecture lectureToDelete);
    }
    public class LectureRepository : ILectureRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public LectureRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<LectureGetDTO>> GetLectures(int page, int size)
        {
            return await PagedList<LectureGetDTO>.Create(
                            _dbContext.Lectures
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
                                ClassroomNumber = l.Classroom.Number
                            }).OrderBy(l => l.Id),
                            page, size);
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
                    ClassroomNumber = l.Classroom.Number
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

        public async Task<Lecture> CheckLecture(int lectureId)
        {
            return await _dbContext.Lectures
               .Where(l => l.Id == lectureId)
               .AsNoTracking()
               .FirstOrDefaultAsync();
        }

        public async Task<Lecture> DeleteLecture(Lecture lectureToDelete)
        {
            var deletedLecture = _dbContext.Lectures.Remove(lectureToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedLecture.Entity;
        }
    }
}
