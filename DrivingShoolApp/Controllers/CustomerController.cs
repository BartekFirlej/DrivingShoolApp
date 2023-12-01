using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IRegistrationService _registrationService;
        private readonly ICustomerLectureService _customerLectureService;
        private readonly IDrivingLicenceService _drivingLicenceService;

        public CustomerController(ICustomerService userService, IRegistrationService registrationService, ICustomerLectureService customerLectureService, 
                                  IDrivingLicenceService drivingLicenceService)
        {
            _customerService = userService;
            _registrationService = registrationService;
            _customerLectureService = customerLectureService;
            _drivingLicenceService = drivingLicenceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<CustomerGetDTO> customers;
            try
            {
                customers = await _customerService.GetCustomers(page, size);
            }
            catch(NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(customers);
        }

        [HttpGet("{customerid}")]
        public async Task<IActionResult> GetCustomer(int customerid)
        {
            CustomerGetDTO customer;
            try
            {
                customer = await _customerService.GetCustomer(customerid);
            }
            catch(NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(customer);
        }

        [HttpGet("{customerid}/courses")]
        public async Task<IActionResult> GetCustomerRegistrations(int customerid, [FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "size")] int size = 10)
        {
            PagedList<RegistrationGetDTO> customerRegistrations; 
            try
            {
                customerRegistrations = await _registrationService.GetCustomerRegistrations(customerid, page, size);
            }
            catch(NotFoundRegistrationException e)
            {
                return NotFound(e.ToJson());
            }
            catch(NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch(ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(customerRegistrations);
        }

        [HttpGet("{customerid}/lectures")]
        public async Task<IActionResult> GetCustomerLectures(int customerid)
        {
            ICollection<CustomerLectureGetDTO> customerLectures;
            try
            {
                customerLectures = await _customerLectureService.GetCustomerLectures(customerid);
            }
            catch (NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundCustomerLectureException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(customerLectures);
        }

        [HttpGet("{customerid}/drivinglicences")]
        public async Task<IActionResult> GetCustomerDrivingLicences(int customerid)
        {
            ICollection<DrivingLicenceGetDTO> customerDrivingLicences;
            try
            {
                customerDrivingLicences = await _drivingLicenceService.GetCustomerDrivingLicences(customerid);
            }
            catch (NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (NotFoundDrivingLicenceException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(customerDrivingLicences);
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer(CustomerRequestDTO customerDetails)
        {
            CustomerResponseDTO addedCustomer;
            try
            {
                addedCustomer = await _customerService.PostCustomer(customerDetails);
            }
            catch(DateTimeException e)
            {
                return BadRequest(e.ToJson());
            }
            return CreatedAtAction(nameof(PostCustomer), addedCustomer);
        }

        [HttpDelete("{customerid}")]
        public async Task<IActionResult> DeleteCustomer(int customerid)
        {
            Customer deleted;
            try
            {
                deleted = await _customerService.DeleteCustomer(customerid);
            }
            catch (NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (ReferenceConstraintException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "This customer refers to something." } });
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

        [HttpPut("{customerid}")]
        public async Task<IActionResult> UpdateCustomer(int customerid, CustomerRequestDTO customerUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CustomerGetDTO updatedCustomer;
            try
            {
                updatedCustomer = await _customerService.UpdateCustomer(customerid, customerUpdate);
            }
            catch (NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch (DateTimeException e)
            {
                return BadRequest(e.ToJson());
            }
            return Ok(updatedCustomer);
        }
    }
}