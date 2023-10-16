using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;

namespace DrivingSchoolApp.Repositories
{
    public interface IRegistrationService
    {
        public Task<ICollection<RegistrationGetDTO>> GetRegistrations();
        public Task<ICollection<RegistrationGetDTO>> GetCourseRegistrations(int courseId);
        public Task<ICollection<RegistrationGetDTO>> GetUserRegistrations(int userId);
        public Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId);
        public Task<RegistrationGetDTO> PostRegistration(RegistrationPostDTO registrationDetails);
    }
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ICustomerService _userService;
        private readonly ICourseService _courseService;

        public RegistrationService(IRegistrationRepository registrationRepository, ICustomerService userService, ICourseService courseService)
        {
            _registrationRepository = registrationRepository;
            _userService = userService;
            _courseService = courseService;
        }

        public async Task<ICollection<RegistrationGetDTO>> GetRegistrations()
        {
            var registrations = await _registrationRepository.GetRegistrations();
            if (!registrations.Any())
                throw new NotFoundRegistrationsException();
            return registrations;
        }

        public async Task<ICollection<RegistrationGetDTO>> GetCourseRegistrations(int courseId)
        {
            var registration = await _registrationRepository.GetCourseRegistrations(courseId);
            if (!registration.Any())
                throw new NotFoundRegistrationsException();
            return registration;
        }

        public async Task<ICollection<RegistrationGetDTO>> GetUserRegistrations(int userId)
        {
            await _userService.GetCustomer(userId);
            var registrations = await _registrationRepository.GetUserRegistrations(userId);
            if (!registrations.Any())
                throw new NotFoundRegistrationsException();
            return registrations;
        }

        public async Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId)
        {
            var registration = await _registrationRepository.GetRegistration(customerId, courseId);
            if (registration == null)
                throw new NotFoundRegistrationException(customerId, courseId);
            return registration;
        }
        public async Task<RegistrationGetDTO> PostRegistration(RegistrationPostDTO registrationDetails)
        {
            await _userService.GetCustomer(registrationDetails.UserId);
            await _courseService.GetCourse(registrationDetails.CourseId);
            var createdRegistration = await _registrationRepository.PostRegistration(registrationDetails);
            return await _registrationRepository.GetRegistration(createdRegistration.CustomerId, createdRegistration.CourseId);
        }
    }
}
