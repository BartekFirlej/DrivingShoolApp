using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("courses")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            ICollection<CourseGetDTO> courses;
            try
            {
                courses = await _courseService.GetCourses();
            }
            catch(NotFoundCoursesException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(courses);
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourse(int courseId)
        {
            CourseGetDTO course;
            try
            {
                course = await _courseService.GetCourse(courseId);
            }
            catch(NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> PostCourse(CoursePostDTO courseDetails)
        {
            CourseGetDTO addedCourse;
            try
            {
                addedCourse = await _courseService.PostCourse(courseDetails);
            }
            catch (NotFoundCourseTypeException e)
            {
                return NotFound(e.ToJson());
            }
            return CreatedAtAction(nameof(PostCourse), addedCourse);
        }
    }
}