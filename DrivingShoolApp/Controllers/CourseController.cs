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
        private readonly ICourseSubjectService _courseSubjectService;

        public CourseController(ICourseService courseService, ICourseSubjectService courseSubjectService)
        {
            _courseService = courseService;
            _courseSubjectService = courseSubjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            ICollection<CourseGetDTO> courses;
            try
            {
                courses = await _courseService.GetCourses();
            }
            catch(NotFoundCourseException e)
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

        [HttpPost("courseId")]
        public async Task<IActionResult> PostCourseSubject(CourseSubjectPostDTO courseSubjectDetails)
        {
            CourseSubjectGetDTO addedCourseSubject;
            try
            {
                addedCourseSubject = await _courseSubjectService.PostCourseSubject(courseSubjectDetails);
            }
            catch (NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            catch (SubjectAlreadyAssignedToCourseException e)
            {
                return Conflict(e.ToJson());
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(PostCourse), addedCourseSubject);
        }
    }
}