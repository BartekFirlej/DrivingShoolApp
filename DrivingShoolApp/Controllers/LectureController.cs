using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("lectures")]
    public class LectureController : ControllerBase
    {
        private readonly ILectureService _lectureService;
        private readonly ICustomerLectureService _customerLectureService;
        public LectureController(ILectureService lectureService, ICustomerLectureService customerLectureService)
        {
            _lectureService = lectureService;
            _customerLectureService = customerLectureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLectures([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<LectureGetDTO> lectures;
            try
            {
                lectures = await _lectureService.GetLectures(page, size);
            }
            catch (NotFoundLectureException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
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

        [HttpGet("{lectureid}/customers")]
        public async Task<IActionResult> GetCustomersLecture(int lectureid)
        {
            ICollection<CustomerLectureGetDTO> customersLecture;
            try
            {
                customersLecture = await _customerLectureService.GetCustomersLecture(lectureid);
            }
            catch (NotFoundCustomersLectureException e)
            {
                return NotFound(e.ToJson());
            }
            catch(NotFoundLectureException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(customersLecture);
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
            catch (NotFoundClassroomException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundCourseSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundLecturerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (DateTimeException e)
            {
                return BadRequest(e.ToJson());
            }
            catch (SubjectAlreadyConductedLectureException e)
            {
                return Conflict(e.ToJson());
            }
            return CreatedAtAction(nameof(PostLecture), addedLecture);
        }

        [HttpPost("customer")]
        public async Task<IActionResult> PostCustomerLecture(CustomerLecturePostDTO customerLectureDetails)
        {
            CustomerLectureGetDTO addedCustomerLecture;
            try
            {
                addedCustomerLecture = await _customerLectureService.PostCustomerLecture(customerLectureDetails);
            }
            catch (NotFoundLectureException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (CustomerAlreadyAssignedToLectureException e)
            {
                return Conflict(e.ToJson());
            }
            catch(NotFoundRegistrationException e)
            {
                return NotFound(e.ToJson());
            }
            return CreatedAtAction(nameof(PostCustomerLecture), customerLectureDetails);
        }
    }
}
