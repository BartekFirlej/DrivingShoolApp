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
    [Route("classrooms")]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomService _classroomService;

        public ClassroomController(IClassroomService classroomService)
        {
            _classroomService = classroomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClassrooms([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<ClassroomGetDTO> classrooms;
            try
            {
                classrooms = await _classroomService.GetClassrooms(page, size);
            }
            catch (NotFoundClassroomException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(classrooms);
        }

        [HttpGet("{classroomid}")]
        public async Task<IActionResult> GetClassroom(int classroomid)
        {
            ClassroomGetDTO classroom;
            try
            {
                classroom = await _classroomService.GetClassroom(classroomid);
            }
            catch (NotFoundClassroomException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(classroom);
        }

        [HttpPost]
        public async Task<IActionResult> PostClassroom(ClassroomPostDTO classroomDetails)
        {
            ClassroomGetDTO addedClassroom;
            try
            {
                addedClassroom = await _classroomService.PostClassroom(classroomDetails);
            }
            catch (NotFoundAddressException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return CreatedAtAction(nameof(PostClassroom), addedClassroom);
        }

        [HttpDelete("{classroomid}")]
        public async Task<IActionResult> DeleteClassroom(int classroomId)
        {
            Classroom deleted;
            try
            {
                deleted = await _classroomService.DeleteClassroom(classroomId);
            }
            catch (NotFoundClassroomException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ReferenceConstraintException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "This classroom refers to something." } });
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
