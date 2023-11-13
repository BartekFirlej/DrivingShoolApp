using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using Microsoft.AspNetCore.Mvc;

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
    }
}
