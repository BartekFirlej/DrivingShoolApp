using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("drivinglesson")]
    public class DrivingLessonController : ControllerBase
    {
        private readonly IDrivingLessonService _drivingLessonService;

        public DrivingLessonController(IDrivingLessonService drivingLessonService)
        {
            _drivingLessonService = drivingLessonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDrivingLessons()
        {
            ICollection<DrivingLessonGetDTO> drivingLessons;
            try
            {
                drivingLessons = await _drivingLessonService.GetDrivingLessons();
            }
            catch (NotFoundDrivingLessonException e)
            {
                return NotFound(e.ToJson());
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
            return CreatedAtAction(nameof(PostDrivingLesson), addedDrivingLesson);
        }
    }
}
