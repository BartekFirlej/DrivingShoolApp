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
        public async Task<IActionResult> GetCourseTypes() {
            ICollection<CourseTypeGetDTO> courseTypes;
            try
            {
                courseTypes = await _courseTypeService.GetCourseTypes();
            }
            catch(NotFoundCourseTypesException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(courseTypes);
        }

        [HttpGet("{courseTypeId}")]
        public async Task<IActionResult> GetCourseType(int courseTypeId)
        {
            CourseTypeGetDTO courseType;
            try
            {
                courseType = await _courseTypeService.GetCourseType(courseTypeId);
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
            return CreatedAtAction(nameof(PostCourseType), addedCourseType);
        }
    }
}
