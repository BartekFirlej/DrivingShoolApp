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

        public CustomerController(ICustomerService userService, IRegistrationService registrationService)
        {
            _customerService = userService;
            _registrationService = registrationService;
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

        [HttpPost]
        public async Task<IActionResult> PostCustomer(CustomerPostDTO customerDetails)
        {
            CustomerGetDTO addedCustomer;
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
            catch (ReferenceConstraintException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Dictionary<string, string> { { "reason", "This customer refers to something." } });
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