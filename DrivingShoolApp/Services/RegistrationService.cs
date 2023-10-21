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
        private readonly ICustomerService _customerService;
        private readonly ICourseService _courseService;

        public RegistrationService(IRegistrationRepository registrationRepository, ICustomerService customerService, ICourseService courseService)
        {
            _registrationRepository = registrationRepository;
            _customerService = customerService;
            _courseService = courseService;
        }

        public async Task<ICollection<RegistrationGetDTO>> GetRegistrations()
        {
            var registrations = await _registrationRepository.GetRegistrations();
            if (!registrations.Any())
                throw new NotFoundRegistrationException();
            return registrations;
        }

        public async Task<ICollection<RegistrationGetDTO>> GetCourseRegistrations(int courseId)
        {
            var registration = await _registrationRepository.GetCourseRegistrations(courseId);
            if (!registration.Any())
                throw new NotFoundRegistrationException();
            return registration;
        }

        public async Task<ICollection<RegistrationGetDTO>> GetUserRegistrations(int userId)
        {
            await _customerService.GetCustomer(userId);
            var registrations = await _registrationRepository.GetUserRegistrations(userId);
            if (!registrations.Any())
                throw new NotFoundRegistrationException();
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
            var customer = await _customerService.GetCustomer(registrationDetails.UserId);
            var course = await _courseService.GetCourse(registrationDetails.CourseId);
            var registration = await _registrationRepository.GetRegistration(registrationDetails.UserId, registrationDetails.CourseId);
            if (registration != null)
                throw new CustomerAlreadyAssignedToCourseException(registrationDetails.UserId, registrationDetails.CourseId);
            var meetAgeRequirement = _customerService.CheckCustomerAgeRequirement(customer.BirthDate, course.CourseType.MinimumAge, DateTime.Now);
            if (meetAgeRequirement == false)
                throw new CustomerDoesntMeetRequirementsException(customer.Id);
            var createdRegistration = await _registrationRepository.PostRegistration(registrationDetails);
            return await _registrationRepository.GetRegistration(createdRegistration.CustomerId, createdRegistration.CourseId);
        }
    }
}
