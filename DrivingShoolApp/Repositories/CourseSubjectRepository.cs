using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DrivingSchoolApp.Repositories
{
    public interface ICourseSubjectRepository
    {
        public Task<ICollection<CourseSubjectGetDTO>> GetCoursesSubjects();
        public Task<CourseSubjectGetDTO> GetCourseSubject(int courseId, int subjectId);
        public Task<bool> TakenSeqNumber(int courseId, int seqNumber);
        public Task<CourseSubject> PostCourseSubject(CourseSubjectPostDTO courseSubjectDetails);
    }
    public class CourseSubjectRepository : ICourseSubjectRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public CourseSubjectRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<CourseSubjectGetDTO>> GetCoursesSubjects()
        {
            /*return await _dbContext.CourseSubjects
                                .Include(c => c.Course)
                                .Include(c => c.Subject)
                                .GroupBy(c => new { c.CourseId, c.Course.Name })
                                .Select(c => new CourseSubjectGetDTO
                                {
                                    CourseId = c.FirstOrDefault().CourseId,
                                    CourseName = c.FirstOrDefault().Course.Name,
                                    CourseSubjectsSequence = new CourseSubjectSequenceGetDTO
                                    {
                                        Subject = new SubjectGetDTO
                                        {
                                            Id = c.FirstOrDefault().SubjectId,
                                            Name = c.FirstOrDefault().Subject.Name,
                                            Code = c.FirstOrDefault().Subject.Code,
                                            Duration = c.FirstOrDefault().Subject.Duration
                                        },
                                        Sequence = c.FirstOrDefault().SequenceNumber
                                    }
                                }).ToListAsync();*/
            return await _dbContext.CourseSubjects
                                   .Include(s => s.Course)
                                   .Include(s => s.Subject)
                                   .Select(s => new CourseSubjectGetDTO
                                   {
                                       CourseId = s.CourseId,
                                       CourseName = s.Course.Name,
                                       SubjectId = s.SubjectId,
                                       SubjectName = s.Subject.Name,
                                       Duration = s.Subject.Duration
                                   }).ToListAsync();
        }

        public async Task<CourseSubjectGetDTO> GetCourseSubject(int courseId, int subjectId)
        {
            /*return await _dbContext.CourseSubjects
                    .Include(c => c.Course)
                    .Include(c => c.Subject)
                    .Where(c => c.CourseId == courseId)
                    .GroupBy(c => new { c.CourseId, c.Course.Name })
                    .Select(c => new CourseSubjectGetDTO
                    {
                        CourseId = c.FirstOrDefault().CourseId,
                        CourseName = c.FirstOrDefault().Course.Name,
                        CourseSubjectsSequence = c.Select(c => new CourseSubjectSequenceGetDTO
                        {
                            Subject = c.Select(c=> new SubjectGetDTO
                            {
                                Id = c.FirstOrDefault().SubjectId,
                                Name = c.FirstOrDefault().Subject.Name,
                                Code = c.FirstOrDefault().Subject.Code,
                                Duration = c.FirstOrDefault().Subject.Duration
                            }),
                            Sequence = c.FirstOrDefault().SequenceNumber
                        }).ToListAsync()
                    }).FirstOrDefaultAsync();*/
            return await _dbContext.CourseSubjects
                                   .Include(s => s.Course)
                                   .Include(s => s.Subject)
                                   .Where(s => s.CourseId == courseId && s.SubjectId == subjectId)
                                   .Select(s => new CourseSubjectGetDTO
                                   {
                                       CourseId = s.CourseId,
                                       CourseName = s.Course.Name,
                                       SubjectId = s.SubjectId,
                                       SubjectName = s.Subject.Name,
                                       Duration = s.Subject.Duration
                                   }).FirstOrDefaultAsync();
        }

        public async Task<CourseSubject> PostCourseSubject(CourseSubjectPostDTO courseSubjectDetails)
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
            var courseSubject = await _dbContext.CourseSubjects.SingleOrDefaultAsync(p => p.CourseId == courseId && p.SequenceNumber == seqNumber);
            if (courseSubject == null)
                return false;
            return true;
        }
    }
    
}
