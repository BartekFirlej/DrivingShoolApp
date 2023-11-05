using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Services;

namespace DrivingSchoolApp.Repositories
{
    public interface IRegistrationService
    {
        public Task<ICollection<RegistrationGetDTO>> GetRegistrations();
        public Task<ICollection<RegistrationGetDTO>> GetCourseRegistrations(int courseId);
        public Task<ICollection<RegistrationGetDTO>> GetCustomerRegistrations(int customerId);
        public Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId);
        public Task<RegistrationGetDTO> PostRegistration(RegistrationPostDTO registrationDetails);
    }
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ICustomerService _customerService;
        private readonly ICourseService _courseService;
        private readonly IDateTimeHelper _dateTimeHelperService;

        public RegistrationService(IRegistrationRepository registrationRepository, ICustomerService customerService, ICourseService courseService, IDateTimeHelper dateTimeHelperService)
        {
            _registrationRepository = registrationRepository;
            _customerService = customerService;
            _courseService = courseService;
            _dateTimeHelperService = dateTimeHelperService;
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
            var course = await _courseService.GetCourse(courseId);
            var registration = await _registrationRepository.GetCourseRegistrations(courseId);
            if (!registration.Any())
                throw new NotFoundRegistrationException();
            return registration;
        }

        public async Task<ICollection<RegistrationGetDTO>> GetCustomerRegistrations(int customerId)
        {
            var customer = await _customerService.GetCustomer(customerId);
            var registrations = await _registrationRepository.GetCustomerRegistrations(customerId);
            if (!registrations.Any())
                throw new NotFoundRegistrationException();
            return registrations;
        }

        public async Task<RegistrationGetDTO> GetRegistration(int customerId, int courseId)
        {
            var customer = await _customerService.GetCustomer(customerId);
            var course = await _courseService.GetCourse(courseId);
            var registration = await _registrationRepository.GetRegistration(customerId, courseId);
            if (registration == null)
                throw new NotFoundRegistrationException(customerId, courseId);
            return registration;
        }
        public async Task<RegistrationGetDTO> PostRegistration(RegistrationPostDTO registrationDetails)
        {
            var customer = await _customerService.GetCustomer(registrationDetails.CustomerId);
            var course = await _courseService.GetCourse(registrationDetails.CourseId);
            var assignedPeopleCount = await _courseService.GetCourseAssignedPeopleCount(registrationDetails.CourseId);
            if (assignedPeopleCount >= course.Limit)
                throw new AssignLimitReachedException(course.Id);
            var registration = await _registrationRepository.GetRegistration(registrationDetails.CustomerId, registrationDetails.CourseId);
            if (registration != null)
                throw new CustomerAlreadyAssignedToCourseException(registrationDetails.CustomerId, registrationDetails.CourseId);
            var meetAgeRequirement = _customerService.CheckCustomerAgeRequirement(customer.BirthDate, course.CourseType.MinimumAge, _dateTimeHelperService.GetDateTimeNow());
            if (meetAgeRequirement == false)
                throw new CustomerDoesntMeetRequirementsException(customer.Id);
            var createdRegistration = await _registrationRepository.PostRegistration(registrationDetails, _dateTimeHelperService.GetDateTimeNow());
            return await _registrationRepository.GetRegistration(createdRegistration.CustomerId, createdRegistration.CourseId);
        }
    }
}
