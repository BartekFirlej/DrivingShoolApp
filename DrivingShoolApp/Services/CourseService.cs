using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolApp.Services
{
    public interface ICourseService
    {
        public Task<ICollection<CourseGetDTO>> GetCourses();
        public Task<ICollection<CourseWithCustomersGetDTO>> GetCoursesWithUsers();
        public Task<CourseGetDTO> GetCourse(int courseId);
        public Task<CourseGetDTO> PostCourse(CoursePostDTO courseDetails);
        public Task<int> GetCourseAssignedPeopleCount(int courseId);

    }
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTypeService _courseTypeService;

        public CourseService(ICourseRepository courseRepository, ICourseTypeService courseTypeService)
        {
            _courseRepository = courseRepository;
            _courseTypeService = courseTypeService;
        }

        public async Task<ICollection<CourseGetDTO>> GetCourses()
        {
            var courses = await _courseRepository.GetCourses();
            if(!courses.Any())
                throw new NotFoundCourseException();
            return courses;
        }

        public Task<ICollection<CourseWithCustomersGetDTO>> GetCoursesWithUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<CourseGetDTO> GetCourse(int courseId)
        {
            var course = await _courseRepository.GetCourse(courseId);
            if(course == null)
                throw new NotFoundCourseException(courseId);
            return course;
        }

        public async Task<int> GetCourseAssignedPeopleCount(int courseId)
        {
            var course = await _courseRepository.GetCourse(courseId);
            if(course == null)
                throw new NotFoundCourseException(courseId);
            return await _courseRepository.GetCourseAssignedPeopleCount(courseId);
        }

        public async Task<CourseGetDTO> PostCourse(CoursePostDTO courseDetails)
        {
            if(courseDetails.Price <= 0)
                throw new ValueMustBeGreaterThanZeroException("Price");
            if (courseDetails.BeginDate == DateTime.MinValue)
                throw new DateTimeException("begin date");
            if (courseDetails.Limit <= 0)
                throw new ValueMustBeGreaterThanZeroException("Limit");
            await _courseTypeService.GetCourseType(courseDetails.CourseTypeId);
            var addedCourse = await _courseRepository.PostCourse(courseDetails);
            return await _courseRepository.GetCourse(addedCourse.Id);
        }
    }
}
