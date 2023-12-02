using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ICourseSubjectRepository
    {
        public Task<ICollection<CourseSubjectGetDTO>> GetCoursesSubjects();
        public Task<CourseSubjectGetDTO> GetCourseSubject(int courseId, int subjectId);
        public Task<CourseSubject> CheckCourseSubject(int courseId, int subjectId);
        public Task<CourseSubjectsGetDTO> GetCourseSubjects(int courseId);
        public Task<bool> TakenSeqNumber(int courseId, int seqNumber);
        public Task<CourseSubject> PostCourseSubject(CourseSubjectRequestDTO courseSubjectDetails);
        public Task<CourseSubject> DeleteCourseSubject(CourseSubject courseSubjectToDelete);
    }
    public class CourseSubjectRepository : ICourseSubjectRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public CourseSubjectRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CourseSubject> CheckCourseSubject(int courseId, int subjectId)
        {
            return await _dbContext.CourseSubjects
                        .AsNoTracking()
                        .Where(s => s.CourseId == courseId && s.SubjectId == subjectId)
                        .FirstOrDefaultAsync();
        }

        public async Task<CourseSubject> DeleteCourseSubject(CourseSubject courseSubjectToDelete)
        {
            var deletedCourseSubject = _dbContext.CourseSubjects.Remove(courseSubjectToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedCourseSubject.Entity;
        }

        public async Task<ICollection<CourseSubjectGetDTO>> GetCoursesSubjects()
        {
            return await _dbContext.CourseSubjects
                                   .AsNoTracking()
                                   .Include(s => s.Course)
                                   .Include(s => s.Subject)
                                   .Select(s => new CourseSubjectGetDTO
                                   {
                                       CourseId = s.CourseId,
                                       CourseName = s.Course.Name,
                                       SubjectId = s.SubjectId,
                                       SubjectName = s.Subject.Name,
                                       Duration = s.Subject.Duration,
                                       SequenceNumber = s.SequenceNumber
                                   }).ToListAsync();
        }

        public async Task<CourseSubjectGetDTO> GetCourseSubject(int courseId, int subjectId)
        {
            return await _dbContext.CourseSubjects
                                   .AsNoTracking()
                                   .Include(s => s.Course)
                                   .Include(s => s.Subject)
                                   .Where(s => s.CourseId == courseId && s.SubjectId == subjectId)
                                   .Select(s => new CourseSubjectGetDTO
                                   {
                                       CourseId = s.CourseId,
                                       CourseName = s.Course.Name,
                                       SubjectId = s.SubjectId,
                                       SubjectName = s.Subject.Name,
                                       Duration = s.Subject.Duration,
                                       SequenceNumber = s.SequenceNumber
                                   }).FirstOrDefaultAsync();
        }

        public async Task<CourseSubjectsGetDTO> GetCourseSubjects(int courseId)
        {
            return await _dbContext.CourseSubjects
                    .AsNoTracking()
                    .Include(s => s.Course)
                    .Include(s => s.Course.CourseType)
                    .Include(s => s.Subject)
                    .Where(s => s.CourseId == courseId)
                    .GroupBy(s => new
                    {
                        s.CourseId,
                        s.Course.BeginDate,
                        s.Course.Limit,
                        s.Course.Name,
                        s.Course.Price,
                        s.Course.CourseTypeId,
                        CourseTypeName = s.Course.CourseType.Name
                    })
                    .Select(group => new CourseSubjectsGetDTO
                    {
                        Id = group.Key.CourseId,
                        BeginDate = group.Key.BeginDate,
                        Limit = group.Key.Limit,
                        Name = group.Key.Name,
                        Price = group.Key.Price,
                        CourseTypeId = group.Key.CourseTypeId,
                        CourseTypeName = group.Key.CourseTypeName,
                        CourseSubjects = group.Select(s => new CourseSubjectSequenceGetDTO
                        {
                            Subject = new SubjectGetDTO
                            {
                                Id = s.SubjectId,
                                Name = s.Subject.Name,
                                Code = s.Subject.Code,
                                Duration = s.Subject.Duration
                            },
                            Sequence = s.SequenceNumber
                        }).OrderBy(s => s.Sequence).ToList()
                    })
                    .FirstOrDefaultAsync();
        }

        public async Task<CourseSubject> PostCourseSubject(CourseSubjectRequestDTO courseSubjectDetails)
        {
            var courseSubjectToAdd = new CourseSubject
            {
                CourseId = courseSubjectDetails.CourseId,
                SubjectId = courseSubjectDetails.SubjectId,
                SequenceNumber = courseSubjectDetails.SequenceNumber
            };
            await _dbContext.CourseSubjects.AddAsync(courseSubjectToAdd);
            await _dbContext.SaveChangesAsync();
            return courseSubjectToAdd;
        }

        public async Task<bool> TakenSeqNumber(int courseId, int seqNumber)
        {
            var courseSubject = await _dbContext.CourseSubjects.AsNoTracking().SingleOrDefaultAsync(p => p.CourseId == courseId && p.SequenceNumber == seqNumber);
            if (courseSubject == null)
                return false;
            return true;
        }
    }
    
}
