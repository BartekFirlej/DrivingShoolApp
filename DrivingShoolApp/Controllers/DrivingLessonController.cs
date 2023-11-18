using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("drivinglessons")]
    public class DrivingLessonController : ControllerBase
    {
        private readonly IDrivingLessonService _drivingLessonService;

        public DrivingLessonController(IDrivingLessonService drivingLessonService)
        {
            _drivingLessonService = drivingLessonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDrivingLessons([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<DrivingLessonGetDTO> drivingLessons;
            try
            {
                drivingLessons = await _drivingLessonService.GetDrivingLessons(page, size);
            }
            catch (NotFoundDrivingLessonException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            catch (DbException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(drivingLessons);
        }

        [HttpGet("{drivinglessonid}")]
        public async Task<IActionResult> GetDrivingLesson(int drivinglessonid)
        {
            DrivingLessonGetDTO drivingLesson;
            try
            {
                drivingLesson = await _drivingLessonService.GetDrivingLesson(drivinglessonid);
            }
            catch (NotFoundDrivingLessonException e)
            {
                return NotFound(e.ToJson());
            }
            catch (DbException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(drivingLesson);
        }

        [HttpPost]
        public async Task<IActionResult> PostDrivingLesson(DrivingLessonPostDTO drivingLessonDetails)
        {
            DrivingLessonGetDTO addedDrivingLesson;
            try
            {
                addedDrivingLesson = await _drivingLessonService.PostDrivingLesson(drivingLessonDetails);
            }
            catch (NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundLecturerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundAddressException e)
            {
                return NotFound(e.ToJson());
            }
            catch(NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            catch(DateTimeException e)
            {
                return BadRequest(e.ToJson());
            }
            catch (DbException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(PostDrivingLesson), addedDrivingLesson);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDrivingLesson(int drivinglessonid)
        {
            try
            {
                await _drivingLessonService.DeleteDrivingLesson(drivinglessonid);
            }
            catch (NotFoundDrivingLessonException e)
            {
                return NotFound(e.ToJson());
            }
            catch(DbException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
    }
}
