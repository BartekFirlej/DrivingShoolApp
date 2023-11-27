using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;

namespace DrivingSchoolApp.Services
{
    public interface ICourseService
    {
        public Task<PagedList<CourseGetDTO>> GetCourses(int page, int size);
        public Task<CourseGetDTO> GetCourse(int courseId);
        public Task<CourseGetDTO> PostCourse(CoursePostDTO courseDetails);
        public Task<int> GetCourseAssignedPeopleCount(int courseId);
        public Task<Course> CheckCourse(int courseId);
        public Task<Course> DeleteCourse(int courseId);
        public Task<CourseGetDTO> UpdateCourse(int courseId, CoursePostDTO courseUpdate);

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

        public async Task<PagedList<CourseGetDTO>> GetCourses(int page, int size)
        {
            var courses = await _courseRepository.GetCourses(page, size);
            if(!courses.PagedItems.Any())
                throw new NotFoundCourseException();
            return courses;
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
            var course = await CheckCourse(courseId);
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
            await _courseTypeService.CheckCourseType(courseDetails.CourseTypeId);
            var addedCourse = await _courseRepository.PostCourse(courseDetails);
            return await _courseRepository.GetCourse(addedCourse.Id);
        }

        public async Task<Course> CheckCourse(int courseId)
        {
            var course = await _courseRepository.CheckCourse(courseId);
            if (course == null)
                throw new NotFoundCourseException(courseId);
            return course;
        }

        public async Task<Course> DeleteCourse(int courseId)
        {
            var courseToDelete = await CheckCourse(courseId);
            return await _courseRepository.DeleteCourse(courseToDelete);
        }

        public async Task<CourseGetDTO> UpdateCourse(int courseId, CoursePostDTO courseUpdate)
        {
            if (courseUpdate.Price <= 0)
                throw new ValueMustBeGreaterThanZeroException("Price");
            if (courseUpdate.BeginDate == DateTime.MinValue)
                throw new DateTimeException("begin date");
            if (courseUpdate.Limit <= 0)
                throw new ValueMustBeGreaterThanZeroException("Limit");
            await _courseTypeService.CheckCourseType(courseUpdate.CourseTypeId);
            await _courseRepository.UpdateCourse(courseId, courseUpdate);
            return await _courseRepository.GetCourse(courseId);
        }
    }
}
