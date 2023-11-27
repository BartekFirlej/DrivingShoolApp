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
    [Route("subjects")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetSubjects([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<SubjectGetDTO> subjects;
            try
            {
                subjects = await _subjectService.GetSubjects(page, size);
            }
            catch (NotFoundSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(subjects);
        }

        [HttpGet("{subjectid}")]
        public async Task<IActionResult> GetSubject(int subjectid)
        {
            SubjectGetDTO subject;
            try
            {
                subject = await _subjectService.GetSubject(subjectid);
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
            return CreatedAtAction(nameof(PostSubject),subject);
        }

        [HttpDelete("{subjectid}")]
        public async Task<IActionResult> DeleteSubject(int subjectid)
        {
            Subject deleted;
            try
            {
                deleted = await _subjectService.DeleteSubject(subjectid);
            }
            catch (NotFoundSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ReferenceConstraintException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "This subject refers to something." } });
            }
            catch(DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "Something is wrong with your request or database." } });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "Something gone wrong." } });
            }
            return NoContent();
        }

        [HttpPut("{subjectid}")]
        public async Task<IActionResult> UpdateSubject(int subjectid, SubjectPostDTO subjectUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SubjectGetDTO updatedSubject;
            try
            {
                updatedSubject = await _subjectService.UpdateSubject(subjectid, subjectUpdate);
            }
            catch (NotFoundSubjectException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(updatedSubject);
        }
    }
}
