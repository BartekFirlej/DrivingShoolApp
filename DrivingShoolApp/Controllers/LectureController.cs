using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("lecture")]
    public class LectureController : ControllerBase
    {
        private readonly ILectureService _lectureService;
        public LectureController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLectures()
        {
            ICollection<LectureGetDTO> lectures;
            try
            {
                lectures = await _lectureService.GetLectures();
            }
            catch (NotFoundLecturesException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(lectures);
        }

        [HttpGet("{lectureid}")]
        public async Task<IActionResult> GetLecture(int lectureid)
        {
            LectureGetDTO lecture;
            try
            {
                lecture = await _lectureService.GetLecture(lectureid);
            }
            catch (NotFoundLectureException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(lecture);
        }

        [HttpPost]
        public async Task<IActionResult> PostLecture(LecturePostDTO lectureDetails)
        {
            LectureGetDTO addedLecture;
            try
            {
                addedLecture = await _lectureService.PostLecture(lectureDetails);
            }
            catch (NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundAddressException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundCourseSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            return CreatedAtAction(nameof(PostLecture), addedLecture);
        }
    }
}
