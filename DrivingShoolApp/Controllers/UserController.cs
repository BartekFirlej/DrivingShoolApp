using DrivingSchoolApp.Services;
using DrivingSchoolApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolApp.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRegistrationService _registrationService;

        public UserController(IUserService userService, IRegistrationService registrationService)
        {
            _userService = userService;
            _registrationService = registrationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            ICollection<UserGetDTO> users;
            try
            {
                users = await _userService.GetUsers();
            }
            catch(NotFoundUsersException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(users);
        }

        [HttpGet("{userid}")]
        public async Task<IActionResult> GetUser(int userid)
        {
            UserGetDTO user;
            try
            {
                user = await _userService.GetUser(userid);
            }
            catch(NotFoundUserException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(user);
        }

        [HttpGet("{userid}/course")]
        public async Task<IActionResult> GetUserRegistrations(int userid)
        {
            ICollection<RegistrationGetDTO> userRegistrations; 
            try
            {
                userRegistrations = await _registrationService.GetUserRegistrations(userid);
            }
            catch(NotFoundUserRegistrationsException e)
            {
                return NotFound(e.ToJson());
            }
            return Ok(userRegistrations);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserPostDTO userDetails)
        {
            var addedUser = await _userService.AddUser(userDetails);
            return CreatedAtAction(nameof(AddUser), addedUser);
        }

        [HttpPost("course")]
        public async Task<IActionResult> RegisterUserForCourse(RegistrationPostDTO registrationDetails)
        {
            RegistrationGetDTO userRegistration;
            try
            {
                userRegistration = await _registrationService.PostRegistration(registrationDetails);
            }
            catch(NotFoundUserException e)
            {
                return NotFound(e.ToJson());
            }
            catch(NotFoundCourseException e)
            {
                return NotFound(e.ToJson());
            }
            return CreatedAtAction(nameof(RegisterUserForCourse), userRegistration);
        }
    }
}