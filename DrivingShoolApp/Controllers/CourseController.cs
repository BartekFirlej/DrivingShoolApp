﻿using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("courses")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ICourseSubjectService _courseSubjectService;
        private readonly IRegistrationService _registrationService;

        public CourseController(ICourseService courseService, ICourseSubjectService courseSubjectService, IRegistrationService registrationService)
        {
            _courseService = courseService;
            _courseSubjectService = courseSubjectService;
            _registrationService = registrationService;
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
            catch (ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            catch(DateTimeException e)
            {
                return BadRequest(e.ToJson());
            }
            return CreatedAtAction(nameof(PostCourse), addedCourse);
        }

        [HttpPost("subject")]
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
            catch (TakenSequenceNumberException e)
            {
                return Conflict(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return CreatedAtAction(nameof(PostCourse), addedCourseSubject);
        }
    }
}