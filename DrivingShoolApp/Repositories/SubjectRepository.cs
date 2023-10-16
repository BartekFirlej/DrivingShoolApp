using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ISubjectRepository
    {
        public Task<ICollection<SubjectGetDTO>> GetSubjects();
        public Task<SubjectGetDTO> GetSubject(int subjectId);
        public Task<Subject> PostSubject(SubjectPostDTO subjectDetails);
    }
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public SubjectRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<SubjectGetDTO>> GetSubjects()
        {
            return await _dbContext.Subjects
                            .Select(s => new SubjectGetDTO
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Code = s.Code,
                                Duration = s.Duration
                            }).ToListAsync();
        }

        public async Task<SubjectGetDTO> GetSubject(int subjectId)
        {
            return await _dbContext.Subjects
                            .Where(s => s.Id == subjectId)
                            .Select(s => new SubjectGetDTO
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Code = s.Code,
                                Duration = s.Duration
                            }).FirstOrDefaultAsync();
        }

        public async Task<Subject> PostSubject(SubjectPostDTO subjectDetails)
        {
            var SubjectToAdd = new Subject
            {
                Name = subjectDetails.Name,
                Code = subjectDetails.Code,
                Duration = subjectDetails.Duration
            };
            await _dbContext.Subjects.AddAsync(SubjectToAdd);
            await _dbContext.SaveChangesAsync();
            return SubjectToAdd;
        }
    }
}
