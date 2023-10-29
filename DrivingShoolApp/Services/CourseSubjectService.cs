using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using Microsoft.Identity.Client;

namespace DrivingSchoolApp.Services
{
    public interface ICourseSubjectService
    {
        public Task<ICollection<CourseSubjectGetDTO>> GetCoursesSubjects();
        public Task<CourseSubjectGetDTO> GetCourseSubject(int courseId, int subjectId);
        public Task<CourseSubjectGetDTO> PostCourseSubject(CourseSubjectPostDTO courseSubjectDetails);
    }
    public class CourseSubjectService : ICourseSubjectService
    {
        private readonly ICourseSubjectRepository _courseSubjectRepository;
        private readonly ICourseService _courseService;
        private readonly ISubjectService _subjectService;

        public CourseSubjectService(ICourseSubjectRepository coursSubjectRepository, ICourseService courseService, ISubjectService subjectService)
        {
            _courseSubjectRepository = coursSubjectRepository;
            _courseService = courseService;
            _subjectService = subjectService;
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
            var course = await _courseService.GetCourse(courseId);
            var subject = await _subjectService.GetSubject(subjectId);
            var courseSubject = await _courseSubjectRepository.GetCourseSubject(courseId, subjectId);
            if (courseSubject == null)
                throw new NotFoundCourseSubjectException(courseId, subjectId);
            return courseSubject;
        }

        public async Task<CourseSubjectGetDTO> PostCourseSubject(CourseSubjectPostDTO courseSubjectDetails)
        {
            if (courseSubjectDetails.SequenceNumber <= 0)
                throw new ValueMustBeGreaterThanZeroException("sequnce number");
            var course = await _courseService.GetCourse(courseSubjectDetails.CourseId);
            var subject = await _subjectService.GetSubject(courseSubjectDetails.SubjectId);
            var courseSubject = await _courseSubjectRepository.GetCourseSubject(courseSubjectDetails.CourseId, courseSubjectDetails.SubjectId);
            if (courseSubject != null)
                throw new SubjectAlreadyAssignedToCourseException(courseSubjectDetails.CourseId, courseSubjectDetails.SubjectId);
            if (await _courseSubjectRepository.TakenSeqNumber(courseSubjectDetails.CourseId, courseSubjectDetails.SequenceNumber))
                throw new TakenSequenceNumberException(courseSubjectDetails.CourseId, courseSubjectDetails.SequenceNumber);
            var addedSubjectService = await _courseSubjectRepository.PostCourseSubject(courseSubjectDetails);
            return await _courseSubjectRepository.GetCourseSubject(addedSubjectService.CourseId, courseSubjectDetails.SubjectId);
        }
    }
}
