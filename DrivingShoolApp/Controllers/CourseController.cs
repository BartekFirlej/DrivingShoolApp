﻿using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Models;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetCourses([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<CourseGetDTO> courses;
            try
            {
                courses = await _courseService.GetCourses(page, size);
            }
            catch(NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(courses);
        }

        [HttpGet("{courseid}")]
        public async Task<IActionResult> GetCourse(int courseid)
        {
            CourseGetDTO course;
            try
            {
                course = await _courseService.GetCourse(courseid);
            }
            catch(NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(course);
        }

        [HttpGet("{courseid}/customers")]
        public async Task<IActionResult> GetCourseRegistrations(int courseid, [FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<RegistrationGetDTO> courseRegistrations;
            try
            {
                courseRegistrations = await _registrationService.GetCourseRegistrations(courseid, page, size);
            }
            catch (NotFoundRegistrationException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(courseRegistrations);
        }

        [HttpGet("{courseid}/subjects")]
        public async Task<IActionResult> GetCourseSubjects(int courseid)
        {
            CourseSubjectsGetDTO courseSubjects;
            try
            {
                courseSubjects = await _courseSubjectService.GetCourseSubjects(courseid);
            }
            catch (NotFoundCourseSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(courseSubjects);
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

        [HttpPost("subjects")]
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

        [HttpDelete("{courseid}")]
        public async Task<IActionResult> DeleteCourse(int courseid)
        {
            Course deleted;
            try
            {
                deleted = await _courseService.DeleteCourse(courseid);
            }
            catch (NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ReferenceConstraintException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "This course refers to something." } });
            }
            catch (DbUpdateException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "Something is wrong with your request or database." } });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "Something gone wrong." } });
            }
            return NoContent();
        }
    }
}