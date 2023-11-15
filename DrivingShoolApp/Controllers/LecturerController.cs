using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{lecturerId}")]
        public async Task<IActionResult> GetLecturer(int lecturerId)
        {
            LecturerGetDTO lecturer;
            try
            {
                lecturer = await _lecturerService.GetLecturer(lecturerId);
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
    }
}
