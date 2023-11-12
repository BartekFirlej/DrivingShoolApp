using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ISubjectService
    {
        public Task<PagedList<SubjectGetDTO>> GetSubjects(int page, int size);
        public Task<SubjectGetDTO> GetSubject(int subjectId);
        public Task<SubjectGetDTO> PostSubject(SubjectPostDTO subjectDetails);

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
    }
}
