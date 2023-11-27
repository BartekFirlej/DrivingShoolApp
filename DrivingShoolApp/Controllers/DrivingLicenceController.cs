using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("drivinglicences")]
    public class DrivingLicenceController : ControllerBase
    {
        private readonly IDrivingLicenceService _drivingLicenceService;

        public DrivingLicenceController(IDrivingLicenceService drivingLicenceService)
        {
            _drivingLicenceService = drivingLicenceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDrivingLicences([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<DrivingLicenceGetDTO> drivingLicences;
            try
            {
                drivingLicences = await _drivingLicenceService.GetDrivingLicences(page, size);
            }
            catch(NotFoundDrivingLicenceException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(drivingLicences);
        }

        [HttpGet("{drivinglicenceid}")]
        public async Task<IActionResult> GetDrivingLicence(int drivinglicenceid)
        {
            DrivingLicenceGetDTO drivingLicence;
            try
            {
                drivingLicence = await _drivingLicenceService.GetDrivingLicence(drivinglicenceid);
            }
            catch(NotFoundDrivingLicenceException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(drivingLicence);
        }

        [HttpPost]
        public async Task<IActionResult> PostDrivingLicence(DrivingLicencePostDTO drivingLicenceDetails)
        {
            DrivingLicenceGetDTO addedDrivingLicence;
            try
            {
                addedDrivingLicence = await _drivingLicenceService.PostDrivingLicence(drivingLicenceDetails);
            }
            catch(NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch(NotFoundLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            catch(CustomerDoesntMeetRequirementsException e)
            {
                return Conflict(e.ToJson());
            }
            catch(DateTimeException e)
            {
                return BadRequest(e.ToJson());
            }
            return CreatedAtAction(nameof(PostDrivingLicence), addedDrivingLicence);
        }

        [HttpDelete("{drivinglicenceid}")]
        public async Task<IActionResult> DeleteDrivingLicence(int drivinglicenceid)
        {
            try
            {
                await _drivingLicenceService.DeleteDrivingLicence(drivinglicenceid);
            }
            catch (NotFoundDrivingLicenceException e)
            {
                return NotFound(e.ToJson());
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

        [HttpPut("{drivinglicenceid}")]
        public async Task<IActionResult> UpdateDrivingLicence(int drivinglicenceid, DrivingLicencePostDTO drivingLicenceUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DrivingLicenceGetDTO updatedDrivingLicence;
            try
            {
                updatedDrivingLicence = await _drivingLicenceService.UpdateDrivingLicence(drivinglicenceid, drivingLicenceUpdate);
            }
            catch (NotFoundDrivingLicenceException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundLicenceCategoryException e)
            {
                return NotFound(e.ToJson());
            }
            catch (CustomerDoesntMeetRequirementsException e)
            {
                return Conflict(e.ToJson());
            }
            catch (DateTimeException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(updatedDrivingLicence);
        }
    }
}
