using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ISubjectService
    {
        public Task<ICollection<SubjectGetDTO>> GetSubjects();
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

        public async Task<ICollection<SubjectGetDTO>> GetSubjects()
        {
            var subjects = await _subjectRepository.GetSubjects();
            if (!subjects.Any())
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
            var addedSubject = await _subjectRepository.PostSubject(subjectDetails);
            return await _subjectRepository.GetSubject(addedSubject.Id);
        }
    }
}
