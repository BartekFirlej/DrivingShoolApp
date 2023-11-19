using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Expressions;

namespace DrivingSchoolApp.Services
{
    public interface ISubjectService
    {
        public Task<PagedList<SubjectGetDTO>> GetSubjects(int page, int size);
        public Task<SubjectGetDTO> GetSubject(int subjectId);
        public Task<SubjectGetDTO> PostSubject(SubjectPostDTO subjectDetails);
        public Task<Subject> CheckSubject(int subjectId);
        public Task<Subject> DeleteSubject(int subjectId);

    }
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<PagedList<SubjectGetDTO>> GetSubjects(int page, int size)
        {
            var subjects = await _subjectRepository.GetSubjects(page, size);
            if (!subjects.PagedItems.Any())
                throw new NotFoundSubjectException();
            return subjects;
        }

        public async Task<SubjectGetDTO> GetSubject(int subjectId)
        {
            var subject = await _subjectRepository.GetSubject(subjectId);
            if(subject == null)
                throw new NotFoundSubjectException(subjectId);
            return subject;
        }

        public async Task<SubjectGetDTO> PostSubject(SubjectPostDTO subjectDetails)
        {
            if (subjectDetails.Duration <= 0)
                throw new ValueMustBeGreaterThanZeroException("duration");
            var addedSubject = await _subjectRepository.PostSubject(subjectDetails);
            return await _subjectRepository.GetSubject(addedSubject.Id);
        }

        public async Task<Subject> DeleteSubject(int subjectId)
        {
            var subjectToDelete = await CheckSubject(subjectId);
            return await _subjectRepository.DeleteSubject(subjectToDelete);
        }

        public async Task<Subject> CheckSubject(int subjectId)
        {
            var subject = await _subjectRepository.CheckSubject(subjectId);
            if (subject == null)
                throw new NotFoundSubjectException(subjectId);
            return subject;
        }
    }
}
