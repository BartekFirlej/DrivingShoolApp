using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;

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
        public async Task<IActionResult> GetCustomers()
        {
            ICollection<CustomerGetDTO> customers;
            try
            {
                customers = await _customerService.GetCustomers();
            }
            catch(NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
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
        public async Task<IActionResult> GetCustomerRegistrations(int customerid)
        {
            ICollection<RegistrationGetDTO> customerRegistrations; 
            try
            {
                customerRegistrations = await _registrationService.GetCustomerRegistrations(customerid);
            }
            catch(NotFoundRegistrationException e)
            {
                return NotFound(e.ToJson());
            }
            catch(NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(customerRegistrations);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerPostDTO customerDetails)
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
            return CreatedAtAction(nameof(AddCustomer), addedCustomer);
        }

        [HttpPost("course")]
        public async Task<IActionResult> RegisterCustomerForCourse(RegistrationPostDTO registrationDetails)
        {
            RegistrationGetDTO customerRegistration;
            try
            {
                customerRegistration = await _registrationService.PostRegistration(registrationDetails);
            }
            catch(NotFoundCustomerException e)
            {
                return NotFound(e.ToJson());
            }
            catch(NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            catch(CustomerAlreadyAssignedToCourseException e)
            {
                return Conflict(e.ToJson());
            }
            catch(CustomerDoesntMeetRequirementsException e)
            {
                return Conflict(e.ToJson());
            }
            catch(AssignLimitReachedException e)
            {
                return Conflict(e.ToJson());
            }
            return CreatedAtAction(nameof(RegisterCustomerForCourse), customerRegistration);
        }
    }
}