using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("subject")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            ICollection<SubjectGetDTO> subjects;
            try
            {
                subjects = await _subjectService.GetSubjects();
            }
            catch (NotFoundSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(subjects);
        }

        [HttpGet("{subjectId}")]
        public async Task<IActionResult> GetSubject(int subjectId)
        {
            SubjectGetDTO subject;
            try
            {
                subject = await _subjectService.GetSubject(subjectId);
            }
            catch (NotFoundSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(subject);
        }

        [HttpPost]
        public async Task<IActionResult> PostSubject(SubjectPostDTO subjectDetails)
        {
            SubjectGetDTO subject;
            try
            {
                subject = await _subjectService.PostSubject(subjectDetails);
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(subject);
        }
    }
}
