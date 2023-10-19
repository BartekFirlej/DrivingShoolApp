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
        public async Task<IActionResult> GetAddresses()
        {
            ICollection<AddressGetDTO> addresses;
            try
            {
                addresses = await _addressService.GetAddresses();
            }
            catch (NotFoundAddressException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(addresses);
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddress(int addressId)
        {
            AddressGetDTO address;
            try
            {
                address = await _addressService.GetAddress(addressId);
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
            catch (NotFoundCourseTypeException e)
            {
                return NotFound(e.ToJson());
            }
            return CreatedAtAction(nameof(PostAddress), addedAddress);
        }
    }
}
