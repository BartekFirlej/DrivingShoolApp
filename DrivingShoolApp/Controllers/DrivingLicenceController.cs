using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Exceptions;

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
        public async Task<IActionResult> GetDrivingLicences()
        {
            ICollection<DrivingLicenceGetDTO> drivingLicences; 
            try
            {
                drivingLicences = await _drivingLicenceService.GetDrivingLicences();
            }
            catch(NotFoundDrivingLicenceException e)
            {
                return NotFound(e.ToJson());
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
    }
}
