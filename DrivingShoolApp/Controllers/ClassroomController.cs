using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetClassrooms()
        {
            ICollection<ClassroomGetDTO> classrooms;
            try
            {
                classrooms = await _classroomService.GetClassrooms();
            }
            catch (NotFoundClassroomException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(classrooms);
        }

        [HttpGet("{clasroomId}")]
        public async Task<IActionResult> GetClassroom(int classroomId)
        {
            ClassroomGetDTO classroom;
            try
            {
                classroom = await _classroomService.GetClassroom(classroomId);
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
            return CreatedAtAction(nameof(PostClassroom), addedClassroom);
        }
    }
}
