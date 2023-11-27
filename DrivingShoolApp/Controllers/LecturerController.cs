using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Services;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("lecturers")]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;

        public LecturerController(ILecturerService lectureService)
        {
            _lecturerService = lectureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLecturers([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<LecturerGetDTO> lecturers;
            try
            {
                lecturers = await _lecturerService.GetLecturers(page, size);
            }
            catch (NotFoundLecturerException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(lecturers);
        }

        [HttpGet("{lecturerid}")]
        public async Task<IActionResult> GetLecturer(int lecturerid)
        {
            LecturerGetDTO lecturer;
            try
            {
                lecturer = await _lecturerService.GetLecturer(lecturerid);
            }
            catch (NotFoundLecturerException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(lecturer);
        }

        [HttpPost]
        public async Task<IActionResult> PostLecturer(LecturerPostDTO lecturerDetails)
        {
            var addedLecturer = await _lecturerService.PostLecturer(lecturerDetails);
            return CreatedAtAction(nameof(PostLecturer), addedLecturer);
        }

        [HttpDelete("{lecturerid}")]
        public async Task<IActionResult> DeleteLecturer(int lecturerId)
        {
            Lecturer deleted;
            try
            {
                deleted = await _lecturerService.DeleteLecturer(lecturerId);
            }
            catch (NotFoundLecturerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ReferenceConstraintException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "This lecturer refers to something." } });
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

        [HttpPut("{lecturerid}")]
        public async Task<IActionResult> UpdateAddress(int lecturerid, LecturerPostDTO lecturerUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Lecturer updatedLecturer;
            try
            {
                updatedLecturer = await _lecturerService.UpdateLecturer(lecturerid, lecturerUpdate);
            }
            catch (NotFoundLecturerException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(updatedLecturer);
        }
    }
}
