using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;
using Microsoft.AspNetCore.Mvc;

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
                return NotFound(e.ToJson());
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
    }
}
