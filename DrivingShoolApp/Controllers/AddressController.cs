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
    [Route("addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses([FromQuery(Name = "page")] int page=1, [FromQuery(Name = "size")] int size=10)
        {
            PagedList<AddressGetDTO> addresses;
            try
            {
                addresses = await _addressService.GetAddresses(page, size);
            }
            catch (NotFoundAddressException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(addresses);
        }

        [HttpGet("{addressid}")]
        public async Task<IActionResult> GetAddress(int addressid)
        {
            AddressGetDTO address;
            try
            {
                address = await _addressService.GetAddress(addressid);
            }
            catch (NotFoundAddressException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> PostAddress(AddressPostDTO addressDetails)
        {
            AddressGetDTO addedAddress;
            try
            {
                addedAddress = await _addressService.PostAddress(addressDetails);
            }
            catch (ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            catch(WrongPostalCodeFormatException e)
            {
                return BadRequest(e.ToJson());
            }
            return CreatedAtAction(nameof(PostAddress), addedAddress);
        }

        [HttpDelete("{addressid}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            Address deleted;
            try
            {
                deleted = await _addressService.DeleteAddress(addressId);
            }
            catch (NotFoundAddressException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ReferenceConstraintException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "This address refers to something." } });
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
