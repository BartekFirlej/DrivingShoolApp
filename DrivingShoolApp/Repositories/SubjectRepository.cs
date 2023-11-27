using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ISubjectRepository
    {
        public Task<PagedList<SubjectGetDTO>> GetSubjects(int page, int size);
        public Task<SubjectGetDTO> GetSubject(int subjectId);
        public Task<Subject> PostSubject(SubjectPostDTO subjectDetails);
        public Task<Subject> CheckSubject(int subjectId);
        public Task<Subject> DeleteSubject(Subject subjectToDelete);
        public Task<Subject> UpdateSubject(int subjectId, SubjectPostDTO subjectUpdate);
    }
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public SubjectRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<SubjectGetDTO>> GetSubjects(int page, int size)
        {
            return await PagedList<SubjectGetDTO>.Create(
                _dbContext.Subjects
                      .AsNoTracking()
                      .Select(s => new SubjectGetDTO
                      {
                          Id = s.Id,
                          Name = s.Name,
                          Code = s.Code,
                          Duration = s.Duration
                      })
                      .OrderBy(s => s.Id),
                page, size);
        }

        public async Task<SubjectGetDTO> GetSubject(int subjectId)
        {
            return await _dbContext.Subjects
                            .AsNoTracking()
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

        public async Task<Subject> CheckSubject(int subjectId)
        {
            return await _dbContext.Subjects
                            .AsNoTracking()
                            .Where(s => s.Id == subjectId)
                            .FirstOrDefaultAsync();
        }

        public async Task<Subject> DeleteSubject(Subject subjectToDelete)
        {
            var deletedSubject = _dbContext.Subjects.Remove(subjectToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedSubject.Entity;
        }

        public async Task<Subject> UpdateSubject(int subjectId, SubjectPostDTO subjectUpdate)
        {
            var subject = await _dbContext.Subjects
                            .Where(s => s.Id == subjectId)
                            .FirstOrDefaultAsync();
            subject.Duration = subjectUpdate.Duration;
            subject.Name = subjectUpdate.Name;
            subject.Code = subjectUpdate.Code;
            await _dbContext.SaveChangesAsync();
            return subject;
        }
    }
}
