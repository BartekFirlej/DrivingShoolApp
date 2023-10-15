﻿using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolApp.Services
{
    public interface ICourseService
    {
        public Task<ICollection<CourseGetDTO>> GetCourses();
        public Task<ICollection<CourseWithUsersGetDTO>> GetCoursesWithUsers();
        public Task<CourseGetDTO> GetCourse(int courseId);
        public Task<CourseGetDTO> PostCourse(CoursePostDTO courseDetails);

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
                throw new NotFoundCoursesException();
            return courses;
        }

        public Task<ICollection<CourseWithUsersGetDTO>> GetCoursesWithUsers()
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

        public async Task<CourseGetDTO> PostCourse(CoursePostDTO courseDetails)
        {
            await _courseTypeService.GetCourseType(courseDetails.CourseTypeId);
            var addedCourse = await _courseRepository.PostCourse(courseDetails);
            return await _courseRepository.GetCourse(addedCourse.Id);
        }
    }
}
