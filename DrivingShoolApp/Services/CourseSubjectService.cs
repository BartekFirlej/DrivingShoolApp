using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using Microsoft.Identity.Client;

namespace DrivingSchoolApp.Services
{
    public interface ICourseSubjectService
    {
        public Task<ICollection<CourseSubjectGetDTO>> GetCourseSubjects();
        public Task<CourseSubjectGetDTO> GetCourseSubjects(int courseId, int subjectId);
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

        public async Task<ICollection<CourseSubjectGetDTO>> GetCourseSubjects()
        {
            var courseSubjects = await _courseSubjectRepository.GetCourseSubjects();
            if (!courseSubjects.Any())
                throw new NotFoundCourseSubjectsException();
            return courseSubjects;
        }

        public async Task<CourseSubjectGetDTO> GetCourseSubjects(int courseId, int subjectId)
        {
            var courseSubject = await _courseSubjectRepository.GetCourseSubject(courseId, subjectId);
            if (courseSubject == null)
                throw new NotFoundCourseSubjectException(courseId, subjectId);
            return courseSubject;
        }

        public async Task<CourseSubjectGetDTO> PostCourseSubject(CourseSubjectPostDTO courseSubjectDetails)
        {
            var course = await _courseService.GetCourse(courseSubjectDetails.CourseId);
            var subject = await _subjectService.GetSubject(courseSubjectDetails.SubjectId);
            var addedSubjectService = await _courseSubjectRepository.PostCourseSubject(courseSubjectDetails);
            return await _courseSubjectRepository.GetCourseSubject(addedSubjectService.CourseId, courseSubjectDetails.SubjectId);
        }
    }
}
