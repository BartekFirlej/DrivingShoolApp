using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ICourseSubjectService
    {
        public Task<ICollection<CourseSubjectGetDTO>> GetCoursesSubjects();
        public Task<CourseSubjectGetDTO> GetCourseSubject(int courseId, int subjectId);
        public Task<CourseSubjectsGetDTO> GetCourseSubjects(int courseId);
        public Task<CourseSubject> CheckCourseSubject(int courseId, int subjectId);
        public Task<CourseSubjectResponseDTO> PostCourseSubject(CourseSubjectRequestDTO courseSubjectDetails);
        public Task<CourseSubject> DeleteCourseSubject(int courseId, int subjectId);
    }
    public class CourseSubjectService : ICourseSubjectService
    {
        private readonly ICourseSubjectRepository _courseSubjectRepository;
        private readonly ICourseService _courseService;
        private readonly ISubjectService _subjectService;
        private readonly IMapper _mapper;

        public CourseSubjectService(ICourseSubjectRepository coursSubjectRepository, ICourseService courseService, ISubjectService subjectService, IMapper mapper)
        {
            _courseSubjectRepository = coursSubjectRepository;
            _courseService = courseService;
            _subjectService = subjectService;
            _mapper = mapper;
        }

        public async Task<ICollection<CourseSubjectGetDTO>> GetCoursesSubjects()
        {
            var courseSubjects = await _courseSubjectRepository.GetCoursesSubjects();
            if (!courseSubjects.Any())
                throw new NotFoundCourseSubjectException();
            return courseSubjects;
        }

        public async Task<CourseSubjectGetDTO> GetCourseSubject(int courseId, int subjectId)
        {
            var course = await _courseService.CheckCourse(courseId);
            var subject = await _subjectService.CheckSubject(subjectId);
            var courseSubject = await _courseSubjectRepository.GetCourseSubject(courseId, subjectId);
            if (courseSubject == null)
                throw new NotFoundCourseSubjectException(courseId, subjectId);
            return courseSubject;
        }

        public async Task<CourseSubjectsGetDTO> GetCourseSubjects(int courseId)
        {
            var course = await _courseService.CheckCourse(courseId);
            var courseSubjects = await _courseSubjectRepository.GetCourseSubjects(courseId);
            if (courseSubjects == null)
                throw new NotFoundCourseSubjectException(courseId);
            return courseSubjects;
        }

        public async Task<CourseSubjectResponseDTO> PostCourseSubject(CourseSubjectRequestDTO courseSubjectDetails)
        {
            if (courseSubjectDetails.SequenceNumber <= 0)
                throw new ValueMustBeGreaterThanZeroException("sequnce number");
            var course = await _courseService.CheckCourse(courseSubjectDetails.CourseId);
            var subject = await _subjectService.CheckSubject(courseSubjectDetails.SubjectId);
            var courseSubject = await _courseSubjectRepository.CheckCourseSubject(courseSubjectDetails.CourseId, courseSubjectDetails.SubjectId);
            if (courseSubject != null)
                throw new SubjectAlreadyAssignedToCourseException(courseSubjectDetails.CourseId, courseSubjectDetails.SubjectId);
            if (await _courseSubjectRepository.TakenSeqNumber(courseSubjectDetails.CourseId, courseSubjectDetails.SequenceNumber))
                throw new TakenSequenceNumberException(courseSubjectDetails.CourseId, courseSubjectDetails.SequenceNumber);
            var addedCourseSubject = await _courseSubjectRepository.PostCourseSubject(courseSubjectDetails);
            return _mapper.Map<CourseSubjectResponseDTO>(addedCourseSubject);
        }
        public async Task<CourseSubject> DeleteCourseSubject(int courseId, int subjectId)
        {
            var courseSubjectToDelete = await CheckCourseSubject(courseId, subjectId);
            return await _courseSubjectRepository.DeleteCourseSubject(courseSubjectToDelete);
        }

        public async Task<CourseSubject> CheckCourseSubject(int courseId, int subjectId)
        {
            var course = await _courseService.CheckCourse(courseId);
            var subject = await _subjectService.CheckSubject(subjectId);
            var courseSubject = await _courseSubjectRepository.CheckCourseSubject(courseId, subjectId);
            if (courseSubject == null)
                throw new NotFoundCourseSubjectException(courseId, subjectId);
            return courseSubject;
        }
    }
}
