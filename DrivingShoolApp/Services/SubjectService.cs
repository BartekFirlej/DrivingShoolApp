using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ISubjectService
    {
        public Task<PagedList<SubjectGetDTO>> GetSubjects(int page, int size);
        public Task<SubjectGetDTO> GetSubject(int subjectId);
        public Task<SubjectResponseDTO> PostSubject(SubjectRequestDTO subjectDetails);
        public Task<Subject> CheckSubject(int subjectId);
        public Task<Subject> CheckSubjectTracking(int subjectId);   
        public Task<Subject> DeleteSubject(int subjectId);
        public Task<SubjectResponseDTO> UpdateSubject(int subjectId, SubjectRequestDTO subjectUpdate);

    }
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectService(ISubjectRepository subjectRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
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

        public async Task<SubjectResponseDTO> PostSubject(SubjectRequestDTO subjectDetails)
        {
            if (subjectDetails.Duration <= 0)
                throw new ValueMustBeGreaterThanZeroException("duration");
            var addedSubject = await _subjectRepository.PostSubject(subjectDetails);
            return _mapper.Map<SubjectResponseDTO>(addedSubject);
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

        public async Task<Subject> CheckSubjectTracking(int subjectId)
        {
            var subject = await _subjectRepository.CheckSubjectTracking(subjectId);
            if (subject == null)
                throw new NotFoundSubjectException(subjectId);
            return subject;
        }

        public async Task<SubjectResponseDTO> UpdateSubject(int subjectId, SubjectRequestDTO subjectUpdate)
        {
            var subjectToUpdate = await CheckSubjectTracking(subjectId);
            if (subjectUpdate.Duration <= 0)
                throw new ValueMustBeGreaterThanZeroException("duration");
            var updatedSubject = await _subjectRepository.UpdateSubject(subjectToUpdate, subjectUpdate);
            return _mapper.Map<SubjectResponseDTO>(updatedSubject);
        }
    }
}
