using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Models;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("coursetypes")]
    public class CourseTypeController : ControllerBase
    {
        private readonly ICourseTypeService _courseTypeService;

        public CourseTypeController(ICourseTypeService courseTypeService)
        {
            _courseTypeService = courseTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseTypes([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10) {
            PagedList<CourseTypeGetDTO> courseTypes;
            try
            {
                courseTypes = await _courseTypeService.GetCourseTypes(page, size);
            }
            catch(NotFoundCourseTypeException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(courseTypes);
        }

        [HttpGet("{coursetypeid}")]
        public async Task<IActionResult> GetCourseType(int coursetypeid)
        {
            CourseTypeGetDTO courseType;
            try
            {
                courseType = await _courseTypeService.GetCourseType(coursetypeid);
            }
            catch(NotFoundCourseTypeException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(courseType);
        }

        [HttpPost]
        public async Task<IActionResult> PostCourseType(CourseTypePostDTO courseTypeDetails)
        {
            CourseTypeGetDTO addedCourseType;
            try
            {
                addedCourseType = await _courseTypeService.PostCourseType(courseTypeDetails);
            }
            catch (NotFoundLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return CreatedAtAction(nameof(PostCourseType), addedCourseType);
        }

        [HttpDelete("{coursetypeid}")]
        public async Task<IActionResult> DeleteCourseType(int coursetypeid)
        {
            CourseType deleted;
            try
            {
                deleted = await _courseTypeService.DeleteCourseType(coursetypeid);
            }
            catch (NotFoundCourseTypeException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ReferenceConstraintException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "This course type refers to something." } });
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "Something is wrong with your request or database." } });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "Something gone wrong." } });
            }
            return NoContent();
        }
    }
}
